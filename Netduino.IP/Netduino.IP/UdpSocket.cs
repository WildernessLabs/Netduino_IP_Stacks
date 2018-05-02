using System;
using System.Collections;
using System.Threading;

namespace Netduino.IP
{
    internal class UdpSocket : Socket  
    {
        IPv4Layer _ipv4Layer;

        // fixed buffer for UDP header
        internal const int UDP_HEADER_LENGTH = 8;
        const int UDP_PSEUDO_HEADER_LENGTH = 12;
        byte[] _udpHeaderBuffer = new byte[UDP_HEADER_LENGTH];
        object _udpHeaderBufferLockObject = new object();
        // fixed buffer for UDP pseudo header
        byte[] _udpPseudoHeaderBuffer = new byte[UDP_PSEUDO_HEADER_LENGTH];
        object _udpPseudoHeaderBufferLockObject = new object();

        bool _sourceIpAddressAndPortAssigned = false;

        protected byte[][] _checksumBufferArray = new byte[3][];
        protected int[] _checksumOffsetArray = new int[3];
        protected int[] _checksumCountArray = new int[3];
        /* NOTE: _checksum... objects are sync-locked by the inherited class if they are used in multiple functions */

        protected byte[][] _bufferArray = new byte[2][];
        protected int[] _indexArray = new int[2];
        protected int[] _countArray = new int[2];

        /* TODO: consider using a pool of global ReceivedPacketBuffers instead of creating a single buffer per socket */
        internal class ReceivedPacketBuffer
        {
            public UInt32 SourceIPAddress;
            public UInt16 SourceIPPort;
            public byte[] Buffer;
            public Int32 BufferBytesFilled;
            public bool IsEmpty;
            //public object LockObject;
        }
//        ReceivedPacketBuffer _receivedPacketBuffer = new ReceivedPacketBuffer();
        internal class Datagrams
        {
            private const int MAX_DATAGRAMS = 8;
            public object LockObject;
            private readonly ArrayList _datagrams;

            private UInt16 _preferredBuffersize = 1500;
            public UInt16 PreferredBufferSize
            {
                get { return (_preferredBuffersize); }
                set
                {
                    if (value < UDP_HEADER_LENGTH)
                    {
                        throw new ArgumentOutOfRangeException("PreferredBufferSize");
                    }

                    _preferredBuffersize = value;
                }
            }

            public int Count
            {
                get { return (_datagrams.Count); }
            }

            public bool CanAcceptDatagram
            {
                get { return (_datagrams.Count < MAX_DATAGRAMS); }
            }

            public bool IsEmpty
            {
                get { return (_datagrams.Count == 0); }
            }

            public void Add(ReceivedPacketBuffer packet)
            {
                if (_datagrams.Count == MAX_DATAGRAMS)
                {
                    throw new InvalidOperationException("UDP datagram queue is full");
                }
                _datagrams.Add(packet);
            }

            public ReceivedPacketBuffer Get()
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("UDP datagram queue is empty.");
                }
                ReceivedPacketBuffer packet = (ReceivedPacketBuffer)_datagrams[0];
                _datagrams.RemoveAt(0);
                return (packet);
            }

            public ReceivedPacketBuffer Peek()
            {
                if (IsEmpty)

                {
                    throw new InvalidOperationException("UDP datagram queue is empty.");
                }
                return((ReceivedPacketBuffer)_datagrams[0]);
            }

            public Datagrams()
            {
                _datagrams = new ArrayList();
                PreferredBufferSize = 1500;
                LockObject = new object();
            }
        }

        private Datagrams _datagrams;
        AutoResetEvent _receivedPacketBufferFilledEvent = new AutoResetEvent(false);
        const Int32 RECEIVE_BUFFER_MIN_SIZE = 536; /* 536 bytes is the minimum receive buffer size allowed by UDP spec */

        public UdpSocket(IPv4Layer ipv4Layer, int handle)
            : base(handle)
        {
            // save a reference to our IPv4Layer; we'll use this to send IPv4 frames
            _ipv4Layer = ipv4Layer;

            base._protocolType = IPv4Layer.ProtocolType.Udp;
            _datagrams = new Datagrams();
            //InitializeReceivedPacketBuffer(_receivedPacketBuffer);
        }

        public override void Dispose()
        {
            base.Dispose();

            _ipv4Layer = null;

            _udpHeaderBuffer = null;
            _udpHeaderBufferLockObject = null;

            _udpPseudoHeaderBuffer = null;
            _udpPseudoHeaderBufferLockObject = null;

            _checksumBufferArray = null;
            _checksumOffsetArray = null;
            _checksumCountArray = null;

            _bufferArray = null;
            _indexArray = null;
            _countArray = null;
        }

        public override void Bind(UInt32 ipAddress, UInt16 ipPort)
        {
            if (_sourceIpAddressAndPortAssigned)
                throw Utility.NewSocketException(SocketError.IsConnected); /* is this correct; should we throw an exception if we already have an IP address/port assigned? */

            _sourceIpAddressAndPortAssigned = true;

            // if ipAddress is IP_ADDRESS_ANY, then change it to our actual ipAddress.
            if (ipAddress == IP_ADDRESS_ANY)
                ipAddress = _ipv4Layer.IPAddress;

            if (ipPort == IP_PORT_ANY)
                ipPort = _ipv4Layer.GetNextEphemeralPortNumber(IPv4Layer.ProtocolType.Udp) /* next available ephemeral port # */;

            // verify that this source IP address is correct
            if (ipAddress != _ipv4Layer.IPAddress)
                throw Utility.NewSocketException(SocketError.AddressNotAvailable); 

            /* ensure that no other UdpSockets are bound to this address/port */
            for (int iSocketHandle = 0; iSocketHandle < IPv4Layer.MAX_SIMULTANEOUS_SOCKETS; iSocketHandle++)
            {
                Socket socket = _ipv4Layer.GetSocket(iSocketHandle);
                if (socket != null && 
                    socket.ProtocolType == IPv4Layer.ProtocolType.Udp &&
                    socket.SourceIPAddress == ipAddress && 
                    socket.SourceIPPort == ipPort)
                {
                    throw Utility.NewSocketException(SocketError.AddressAlreadyInUse); 
                }
            }

            _srcIPAddress = ipAddress;
            _srcIPPort = ipPort;
        }

        public override void Connect(UInt32 ipAddress, UInt16 ipPort)
        {
            if (!_sourceIpAddressAndPortAssigned)
                Bind(IP_ADDRESS_ANY, IP_PORT_ANY);

            // UDP is connectionless, so the Connect function sets the default destination IP address and port values.
            _destIPAddress = ipAddress;
            _destIPPort = ipPort;
        }

        public override bool Poll(int mode, int microSeconds)
        {
            switch (mode)
            {
                case SELECT_MODE_READ:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - if Listen has been called and a connection is pending
                     *   - if data is available for reading
                     *   - if the connection has been closed, reset, or terminated
                     * otherwise return false */
                    {
                        /* TODO: check if listen has been called and a connection is pending */
                        //return true;

                        if (_datagrams.Count > 0)
                            return true;

                        /* TODO: check if connection has been closed, reset, or terminated */
                        //return true;

                        /* TODO: only check _isDisposed if the connection hasn't been closed/reset/terminated; those other cases should return TRUE */
                        if (_isDisposed) return false;

                        // in all other circumstances, return false.
                        return false;
                    }
                case SELECT_MODE_WRITE:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - processing a Connect and the connection has succeeded
                     *   - if data can be sent
                     * otherwise return false */
                    {
                        if (_isDisposed) return false;

                        if ((_destIPAddress != 0) && (_destIPPort != 0))
                            return true;
                        else
                            return false;
                    }
                case SELECT_MODE_ERROR:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - if processing a Connect that does not block--and the connection has failed
                     *   - if OutOfBandInline is not set and out-of-band data is available 
                     * otherwise return false */
                    {
                        if (_isDisposed) return false;

                        return false;
                    }
                default:
                    {
                        // the following line should never be executed
                        return false;
                    }
            }
        }

        public override int Send(byte[] buffer, Int32 offset, Int32 count, Int32 flags, Int64 timeoutInMachineTicks)
        {
            // make sure that a default destination IPEndpoint has been configured.
            if ((_destIPAddress == IP_ADDRESS_ANY) || (_destIPPort == IP_PORT_ANY))
                throw Utility.NewSocketException(SocketError.NotConnected); /* "must specify destination IP address and port" */

            // send to the default destination ipAddress/ipPort (as specified when Connect was called)
            return SendTo(buffer, offset, count, flags, timeoutInMachineTicks, _destIPAddress, _destIPPort);
        }

        /* TODO: we can't lock permanently while waiting for another UDP packet to be sent; get rid of the _udpHeaderBufferLock "lock that could stay locked forever" */
        public override Int32 SendTo(byte[] buffer, Int32 offset, Int32 count, Int32 flags, Int64 timeoutInMachineTicks, UInt32 ipAddress, UInt16 ipPort)
        {
            if (!_sourceIpAddressAndPortAssigned)
                Bind(IP_ADDRESS_ANY, IP_PORT_ANY);

            lock (_udpHeaderBufferLockObject)
            {
                // UDP header: 8 bytes
                _udpHeaderBuffer[0] = (byte)((_srcIPPort >> 8) & 0xFF);
                _udpHeaderBuffer[1] = (byte)(_srcIPPort & 0xFF);
                _udpHeaderBuffer[2] = (byte)((ipPort >> 8) & 0xFF);
                _udpHeaderBuffer[3] = (byte)(ipPort & 0xFF);
                UInt16 udpLength = (UInt16)(UDP_HEADER_LENGTH + count);
                _udpHeaderBuffer[4] = (byte)((udpLength >> 8) & 0xFF);
                _udpHeaderBuffer[5] = (byte)(udpLength & 0xFF);
                _udpHeaderBuffer[6] = 0; // for now, no checksum; we'll populate this when we calculate checksum over the pseudo-header + header + data + 16-bit-alignment-if-necessary-zero-pad-byte
                _udpHeaderBuffer[7] = 0; // checksum (continued)

                UInt16 checksum;
                lock (_udpPseudoHeaderBufferLockObject)
                {
                    // create temporary pseudo header
                    _udpPseudoHeaderBuffer[0] = (byte)((_srcIPAddress >> 24) & 0xFF);
                    _udpPseudoHeaderBuffer[1] = (byte)((_srcIPAddress >> 16) & 0xFF);
                    _udpPseudoHeaderBuffer[2] = (byte)((_srcIPAddress >> 8) & 0xFF);
                    _udpPseudoHeaderBuffer[3] = (byte)(_srcIPAddress & 0xFF);
                    _udpPseudoHeaderBuffer[4] = (byte)((ipAddress >> 24) & 0xFF);
                    _udpPseudoHeaderBuffer[5] = (byte)((ipAddress >> 16) & 0xFF);
                    _udpPseudoHeaderBuffer[6] = (byte)((ipAddress >> 8) & 0xFF);
                    _udpPseudoHeaderBuffer[7] = (byte)(ipAddress & 0xFF);
                    _udpPseudoHeaderBuffer[8] = 0; // ZERO
                    _udpPseudoHeaderBuffer[9] = (byte)IPv4Layer.ProtocolType.Udp; // Protocol Number
                    _udpPseudoHeaderBuffer[10] = (byte)((udpLength >> 8) & 0xFF);
                    _udpPseudoHeaderBuffer[11] = (byte)(udpLength & 0xFF);

                    // calculate checksum over entire pseudo-header, UDP header and data
                    _checksumBufferArray[0] = _udpPseudoHeaderBuffer;
                    _checksumOffsetArray[0] = 0;
                    _checksumCountArray[0] = UDP_PSEUDO_HEADER_LENGTH;
                    _checksumBufferArray[1] = _udpHeaderBuffer;
                    _checksumOffsetArray[1] = 0;
                    _checksumCountArray[1] = UDP_HEADER_LENGTH;
                    _checksumBufferArray[2] = buffer;
                    _checksumOffsetArray[2] = offset;
                    _checksumCountArray[2] = count;
                    checksum = Utility.CalculateInternetChecksum(_checksumBufferArray, _checksumOffsetArray, _checksumCountArray); /* NOTE: this function will append a pad byte if necessary for 16-bit alignment before calculation */
                }

                // insert checksum into UDP header
                _udpHeaderBuffer[6] = (byte)((checksum >> 8) & 0xFF);
                _udpHeaderBuffer[7] = (byte)(checksum & 0xFF);

                // queue up our buffer arrays
                _bufferArray[0] = _udpHeaderBuffer;
                _indexArray[0] = 0;
                _countArray[0] = UDP_HEADER_LENGTH;
                _bufferArray[1] = buffer;
                _indexArray[1] = offset;
                _countArray[1] = count;

                /* TODO: deal with our flags and timeout_ms; we shouldn't just be sending while blocking -- and ignoring the flags */
                _ipv4Layer.Send((byte)IPv4Layer.ProtocolType.Udp, _srcIPAddress, ipAddress, _bufferArray, _indexArray, _countArray, timeoutInMachineTicks);
            }

            /* TODO: return actual # of bytes sent, if different; this may be caused by a limit on UDP datagram size, timeout while sending data, etc. */
            return count;
        }

        public override Int32 Receive(byte[] buf, Int32 offset, Int32 count, Int32 flags, Int64 timeoutInMachineTicks)
        {
            UInt32 ipAddress;
            UInt16 ipPort;
            return ReceiveFrom(buf, offset, count, flags, timeoutInMachineTicks, out ipAddress, out ipPort);
        }

        public override Int32 ReceiveFrom(byte[] buf, Int32 offset, Int32 count, Int32 flags, Int64 timeoutInMachineTicks, out UInt32 ipAddress, out UInt16 ipPort)
        {
            if (_datagrams.IsEmpty)
            {
                Int32 waitTimeout = (Int32)((timeoutInMachineTicks != Int64.MaxValue) ? System.Math.Max((timeoutInMachineTicks - Microsoft.SPOT.Hardware.Utility.GetMachineTime().Ticks) / System.TimeSpan.TicksPerMillisecond, 0) : System.Threading.Timeout.Infinite);
                if (!_receivedPacketBufferFilledEvent.WaitOne(waitTimeout, false))
                {
                    /* TODO: do we need to throw a timeout exception here, or just return zero bytes? */
                    ipAddress = 0;
                    ipPort = 0;
                    return 0;
                }
            }

            int bytesRead;
            lock (_datagrams.LockObject)
            {
                ReceivedPacketBuffer packet = _datagrams.Get();

                bytesRead = Math.Min(count, packet.BufferBytesFilled);
                Array.Copy(packet.Buffer, 0, buf, offset, bytesRead);
                ipAddress = packet.SourceIPAddress;
                ipPort = packet.SourceIPPort;

                // now empty our datagram buffer
//                InitializeReceivedPacketBuffer(_receivedPacketBuffer);
            }

            return bytesRead;
        }

        internal override void OnPacketReceived(UInt32 sourceIPAddress, UInt32 destinationIPAddress, byte[] buffer, Int32 index, Int32 count)
        {
            lock (_datagrams.LockObject)
            {
                if (count < UDP_HEADER_LENGTH)
                    return;

                /* if we do not have enough room for the incoming frame, discard it */
                if (!_datagrams.CanAcceptDatagram)
                    return;

                UInt16 packetHeaderChecksum = (UInt16)((((UInt16)buffer[index + 6]) << 8) + buffer[index + 7]);

                // if the incoming packet includes a checksum then calculate a checksum to verify packet integrity.
                // NOTE: UDP checksums are mandatory for inclusion on our outgoing datagrams, but are technically optional for incoming IPv4 UDP datagrams
                if (packetHeaderChecksum != 0x0000)
                {
                    UInt16 calculateChecksum;
                    lock (_udpPseudoHeaderBufferLockObject)
                    {
                        // create temporary pseudo header
                        _udpPseudoHeaderBuffer[0] = (byte)((sourceIPAddress >> 24) & 0xFF);
                        _udpPseudoHeaderBuffer[1] = (byte)((sourceIPAddress >> 16) & 0xFF);
                        _udpPseudoHeaderBuffer[2] = (byte)((sourceIPAddress >> 8) & 0xFF);
                        _udpPseudoHeaderBuffer[3] = (byte)(sourceIPAddress & 0xFF);
                        _udpPseudoHeaderBuffer[4] = (byte)((destinationIPAddress >> 24) & 0xFF);
                        _udpPseudoHeaderBuffer[5] = (byte)((destinationIPAddress >> 16) & 0xFF);
                        _udpPseudoHeaderBuffer[6] = (byte)((destinationIPAddress >> 8) & 0xFF);
                        _udpPseudoHeaderBuffer[7] = (byte)(destinationIPAddress & 0xFF);
                        _udpPseudoHeaderBuffer[8] = 0; // ZERO
                        _udpPseudoHeaderBuffer[9] = (byte)IPv4Layer.ProtocolType.Udp; // Protocol Number
                        _udpPseudoHeaderBuffer[10] = (byte)((count >> 8) & 0xFF);
                        _udpPseudoHeaderBuffer[11] = (byte)(count & 0xFF);

                        // calculate checksum over entire pseudo-header, UDP header and data
                        _checksumBufferArray[0] = _udpPseudoHeaderBuffer;
                        _checksumOffsetArray[0] = 0;
                        _checksumCountArray[0] = UDP_PSEUDO_HEADER_LENGTH;
                        _checksumBufferArray[1] = buffer;
                        _checksumOffsetArray[1] = index;
                        _checksumCountArray[1] = count;
                        calculateChecksum = Utility.CalculateInternetChecksum(_checksumBufferArray, _checksumOffsetArray, _checksumCountArray, 2);
                    }
                    if (calculateChecksum != 0x0000)
                        return; // drop packet
                }

                UInt16 sourceIPPort = (UInt16)((((UInt16)buffer[index + 0]) << 8) + buffer[index + 1]);
                ReceivedPacketBuffer packetBuffer = new ReceivedPacketBuffer();
                packetBuffer.Buffer = new byte[_datagrams.PreferredBufferSize];
                //
                //  The code below throws away some data if the amount of data to copy is greater than
                //  the preferred buffer size.
                //
                //  TODO: Should be change this behaviour to throw an exception?
                //
                int amountToCopy = Math.Min(_datagrams.PreferredBufferSize, count - UDP_HEADER_LENGTH);
                Array.Copy(buffer, index + UDP_HEADER_LENGTH, packetBuffer.Buffer, 0, amountToCopy);
                packetBuffer.BufferBytesFilled = amountToCopy;

                packetBuffer.SourceIPAddress = sourceIPAddress;
                packetBuffer.SourceIPPort = sourceIPPort;

                packetBuffer.IsEmpty = false;
                _datagrams.Add(packetBuffer);
                _receivedPacketBufferFilledEvent.Set();
            }
        }

        //internal void InitializeReceivedPacketBuffer(ReceivedPacketBuffer buffer)
        //{
        //    buffer.SourceIPAddress = 0;
        //    buffer.SourceIPPort = 0;
            
        //    if (buffer.Buffer == null)
        //        buffer.Buffer = new byte[_datagrams.PreferredBufferSize];
        //    buffer.BufferBytesFilled = 0;

        //    buffer.IsEmpty = true;
        //    if (buffer.LockObject == null)
        //        buffer.LockObject = new object();
        //}

        /// <summary>
        ///     Get or set the size of the buffers for new UDP packets.
        /// </summary>
        internal override UInt16 ReceiveBufferSize
        {
            get
            {
                lock (_datagrams.LockObject)
                {
                    return _datagrams.PreferredBufferSize;
                }
            }
            set
            {
                value = (UInt16) Math.Max(value, RECEIVE_BUFFER_MIN_SIZE);

                lock (_datagrams.LockObject)
                {
                    _datagrams.PreferredBufferSize = value;
                    //
                    //  TODO: Loop through all of the datagrams received so far and resize the buffers?
                    //
                    //  This was the behaviour of the SL code (see below).
                    //

                    // if our new receive buffer is smaller than our old receive buffer, we will truncate any remaining data
                    /* NOTE: an alternate potential behavior would be to block new data reception until enough data gets retrieved by the caller that we can shrink the buffer...
                     *       or to only allow this operation before sockets are opened...
                     *       or to choose the larger of the requested size and the currently-used size
                     *       or to temporarily choose the larger of the requested size and the currently-used size...and then reduce the buffer size ASAP */

                    // copy existing receive buffer to new receive buffer
                    //Int32 bytesToCopy = System.Math.Min(_receivedPacketBuffer.BufferBytesFilled, newReceiveBuffer.Length);
                    //Array.Copy(_receivedPacketBuffer.Buffer, newReceiveBuffer, bytesToCopy);

                    //_receivedPacketBuffer.Buffer = newReceiveBuffer;
                    //_receivedPacketBuffer.BufferBytesFilled = bytesToCopy;
                }
            }
        }

        /// <summary>
        ///     Get the number of bytes in the first datagram in the queue.
        /// </summary>
        /// <returns>Number of bytes in the first datagram ready to be read or 0 if the queue is empty.</returns>
        public override Int32 GetBytesToRead()
        {
            lock (_datagrams.LockObject)
            {
                int result = _datagrams.IsEmpty ? 0 : ((ReceivedPacketBuffer) _datagrams.Peek()).BufferBytesFilled;

                return (result);
            }
        }
    }
}
