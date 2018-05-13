using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System;
using System.IO.Ports;
using System.Threading;

namespace Netduino.IP.LinkLayers
{
    public partial class CC3100
    {
        /// <summary>
        ///     CC3100 Socket Information
        /// </summary>
        public class CC3100SocketInfo
        {
            public UInt32 RemoteIPAddress;
            public UInt16 RemoteIPPort;
            public Int32 SocketHandle;
            public SocketAddressFamily SocketAddressFamily;
            public SocketSocketType SocketSocketType;
            // the following are used to buffer RX data
            public byte[] SocketReceiveBuffer = null;
            public Int32 SocketReceiveBufferFirstAvailableIndex = 0;
            public object SocketReceiveBufferLockObject = new object();

            /// <summary>
            ///     Constructor for a CC3100SocketInfo object.
            /// </summary>
            /// <param name="socketHandle">Handle for the socket.</param>
            /// <param name="addressFamily">Socket address family (IPV4, IPV6 etc).  Currently only IPV4 is supported.</param>
            /// <param name="socketType">Socket type (Stream or datagram).</param>
            public CC3100SocketInfo(Int32 socketHandle, SocketAddressFamily addressFamily, SocketSocketType socketType)
            {
                this.SocketHandle = socketHandle;
                this.SocketAddressFamily = addressFamily;
                this.SocketSocketType = socketType;
                this.RemoteIPAddress = 0; // default
                this.RemoteIPPort = 0; // default
            }
        }
    }
}