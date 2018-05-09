//-----------------------------------------------------------------------------
//
//    ** DO NOT EDIT THIS FILE! **
//    This file was generated by a tool
//    re-running the tool will overwrite this file.
//
//-----------------------------------------------------------------------------


#ifndef _NETDUINO_IP_H_
#define _NETDUINO_IP_H_

#include <TinyCLR_Interop.h>
struct Library_Netduino_IP_Netduino_IP_ArpResolver
{
    static const int FIELD___arpFrameBuffer = 1;
    static const int FIELD___arpFrameBufferLock = 2;
    static const int FIELD___currentArpRequestProtocolAddress = 3;
    static const int FIELD___currentArpRequestPhysicalAddress = 4;
    static const int FIELD___simultaneousArpRequestLock = 5;
    static const int FIELD___currentArpRequestAnsweredEvent = 6;
    static const int FIELD___bufferArray = 7;
    static const int FIELD___indexArray = 8;
    static const int FIELD___countArray = 9;
    static const int FIELD___ipv4ProtocolAddress = 10;
    static const int FIELD___arpCache = 11;
    static const int FIELD___arpCacheLock = 12;
    static const int FIELD___sendArpGenericInBackgroundThread = 13;
    static const int FIELD___sendArpGenericInBackgroundEvent = 14;
    static const int FIELD___sendArpGenericInBackgroundQueue = 15;
    static const int FIELD___cleanupArpCacheTimer = 16;
    static const int FIELD___ethernetInterface = 17;
    static const int FIELD___isDisposed = 18;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_ArpResolver__ArpCacheEntry
{
    static const int FIELD__PhysicalAddress = 1;
    static const int FIELD__TimeoutTicks = 2;
    static const int FIELD__LastUsedTicks = 3;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_ArpResolver__ArpGenericData
{
    static const int FIELD__DestinationEthernetAddress = 1;
    static const int FIELD__ArpOperaton = 2;
    static const int FIELD__TargetProtocolAddress = 3;
    static const int FIELD__TargetPhysicalAddress = 4;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DHCPv4Client
{
    static const int FIELD___randomGenerator = 1;
    static const int FIELD___isDhcpDnsConfigEnabled = 2;
    static const int FIELD___isDhcpIpConfigEnabled = 3;
    static const int FIELD___linkState = 4;
    static const int FIELD___dhcpServerAddress = 5;
    static const int FIELD___ipConfigIPAddress = 6;
    static const int FIELD___ipConfigSubnetMask = 7;
    static const int FIELD___ipConfigGatewayAddress = 8;
    static const int FIELD___leaseExpirationTimeInMachineTicks = 9;
    static const int FIELD___leaseRenewalTimeInMachineTicks = 10;
    static const int FIELD___leaseRebindingTimeInMachineTicks = 11;
    static const int FIELD___leaseUpdateTimer = 12;
    static const int FIELD__IpConfigChanged = 13;
    static const int FIELD__DnsConfigChanged = 14;
    static const int FIELD___dhcpStateMachineThread = 15;
    static const int FIELD___dhcpStateMachineEvent = 16;
    static const int FIELD___dhcpStateMachineState = 17;
    static const int FIELD___ipv4Layer = 18;
    static const int FIELD___physicalAddress = 19;
    static const int FIELD___isDisposed = 20;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_Socket
{
    static const int FIELD___handle = 1;
    static const int FIELD___isDisposed = 2;
    static const int FIELD___srcIPAddress = 3;
    static const int FIELD___srcIPPort = 4;
    static const int FIELD___destIPAddress = 5;
    static const int FIELD___destIPPort = 6;
    static const int FIELD___transmitTimeoutInMilliseconds = 7;
    static const int FIELD___receiveTimeoutInMilliseconds = 8;
    static const int FIELD___protocolType = 9;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_TcpSocket
{
    static const int FIELD___tcpHandler = 10;
    static const int FIELD___sourceIpAddressAndPortAssigned = 11;
    static const int FIELD___randomGenerator = 12;
    static const int FIELD___receiveWindowLeftEdge = 13;
    static const int FIELD___receiveWindowRightEdge = 14;
    static const int FIELD___receiveBufferSize = 15;
    static const int FIELD___transmitWindowLeftEdge = 16;
    static const int FIELD___transmitWindowRightEdge = 17;
    static const int FIELD___transmitWindowMaximumSegmentSize = 18;
    static const int FIELD___transmitWindowSentButNotAcknowledgedMarker = 19;
    static const int FIELD___transmitBufferSize = 20;
    static const int FIELD___receiveBuffer = 21;
    static const int FIELD___receiveBufferFirstBytePos = 22;
    static const int FIELD___receiveBufferNextBytePos = 23;
    static const int FIELD___receiveBufferLockObject = 24;
    static const int FIELD___receiveBufferSpaceFilledEvent = 25;
    static const int FIELD___transmitBuffer = 26;
    static const int FIELD___transmitBufferFirstBytePos = 27;
    static const int FIELD___transmitBufferNextBytePos = 28;
    static const int FIELD___transmitBufferLockObject = 29;
    static const int FIELD___transmitBufferSpaceFreedEvent = 30;
    static const int FIELD___tcpStateMachineThread = 31;
    static const int FIELD___tcpStateMachine_ActionRequiredEvent = 32;
    static const int FIELD___tcpStateMachine_CurrentState = 33;
    static const int FIELD___tcpStateMachine_SendAckRequired = 34;
    static const int FIELD___currentOutgoingTransmission = 35;
    static const int FIELD___currentOutgoingTransmissionLock = 36;
    static const int FIELD___tcpStateMachine_CalculatedRetryTimeoutInMilliseconds = 37;
    static const int FIELD___incomingConnectionSockets = 38;
    static const int FIELD___incomingConnectionBacklog = 39;
    static const int FIELD___connectionState = 40;
    static const int FIELD___outgoingConnectionCompleteEvent = 41;
    static const int FIELD___outgoingConnectionClosedEvent = 42;
    static const int FIELD___acceptSocketWaitHandle = 43;
    static const int FIELD__ConnectionComplete = 44;
    static const int FIELD__ConnectionAborted = 45;
    static const int FIELD___sendFunctionSynchronizationLockObject = 46;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_UdpSocket
{
    static const int FIELD___ipv4Layer = 10;
    static const int FIELD___udpHeaderBuffer = 11;
    static const int FIELD___udpHeaderBufferLockObject = 12;
    static const int FIELD___udpPseudoHeaderBuffer = 13;
    static const int FIELD___udpPseudoHeaderBufferLockObject = 14;
    static const int FIELD___sourceIpAddressAndPortAssigned = 15;
    static const int FIELD___checksumBufferArray = 16;
    static const int FIELD___checksumOffsetArray = 17;
    static const int FIELD___checksumCountArray = 18;
    static const int FIELD___bufferArray = 19;
    static const int FIELD___indexArray = 20;
    static const int FIELD___countArray = 21;
    static const int FIELD___datagrams = 22;
    static const int FIELD___receivedPacketBufferFilledEvent = 23;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_UdpSocket__Datagrams
{
    static const int FIELD__LockObject = 1;
    static const int FIELD___datagrams = 2;
    static const int FIELD___preferredBuffersize = 3;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DHCPv4Client__DhcpOffer
{
    static const int FIELD__IPAddress = 1;
    static const int FIELD__SubnetMask = 2;
    static const int FIELD__GatewayAddress = 3;
    static const int FIELD__DnsAddresses = 4;
    static const int FIELD__ServerIdentifier = 5;
    static const int FIELD__LeaseExpirationTimeInSeconds = 6;
    static const int FIELD__LeaseRenewalTimeInSeconds = 7;
    static const int FIELD__LeaseRebindingTimeInSeconds = 8;
    static const int FIELD__TransactionID = 9;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DHCPv4Client__DhcpOption
{
    static const int FIELD__Code = 1;
    static const int FIELD__Value = 2;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DHCPv4Client__DhcpOptionsBlockRange
{
    static const int FIELD__BeginOffset = 1;
    static const int FIELD__EndOffset = 2;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DnsResolver
{
    static const int FIELD___ipv4Layer = 1;
    static const int FIELD___isDisposed = 2;
    static const int FIELD___nextTransactionID = 3;
    static const int FIELD___dnsCache = 4;
    static const int FIELD___dnsCacheLock = 5;
    static const int FIELD___cleanupDnsCacheTimer = 6;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DnsResolver__DnsCacheEntry
{
    static const int FIELD__Name = 1;
    static const int FIELD__CanonicalName = 2;
    static const int FIELD__IpAddresses = 3;
    static const int FIELD__ExpirationTicks = 4;
    static const int FIELD__LastUsedTicks = 5;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DnsResolver__DnsResourceRecord
{
    static const int FIELD__Name = 1;
    static const int FIELD__RecordType = 2;
    static const int FIELD__TimeToLive = 3;
    static const int FIELD__Data = 4;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_DnsResolver__DnsResponse
{
    static const int FIELD__ResponseCode = 1;
    static const int FIELD__QueryType = 2;
    static const int FIELD__QueryName = 3;
    static const int FIELD__AnswerRecords = 4;
    static const int FIELD__AuthorityRecords = 5;
    static const int FIELD__AdditionalInformationRecords = 6;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_ICMPv4Handler
{
    static const int FIELD___icmpFrameBuffer = 1;
    static const int FIELD___icmpFrameBufferLock = 2;
    static const int FIELD___bufferArray = 3;
    static const int FIELD___indexArray = 4;
    static const int FIELD___countArray = 5;
    static const int FIELD___checksumBufferArray = 6;
    static const int FIELD___checksumOffsetArray = 7;
    static const int FIELD___checksumCountArray = 8;
    static const int FIELD___checksumLock = 9;
    static const int FIELD___ipv4Layer = 10;
    static const int FIELD___isDisposed = 11;
    static const int FIELD___sendIcmpMessagesInBackgroundThread = 12;
    static const int FIELD___sendIcmpMessagesInBackgroundEvent = 13;
    static const int FIELD___sendIcmpMessagesInBackgroundQueue = 14;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_ICMPv4Handler__IcmpMessage
{
    static const int FIELD__DestinationIPAddress = 1;
    static const int FIELD__IcmpMessageType = 2;
    static const int FIELD__IcmpMessageCode = 3;
    static const int FIELD__RestOfHeader = 4;
    static const int FIELD__Data = 5;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_Application
{
    static const int FIELD_STATIC___applicationStartThread = 0;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_EthernetInterface
{
    static const int FIELD__IPv4PacketReceived = 1;
    static const int FIELD__ARPFrameReceived = 2;
    static const int FIELD___ethernetHeaderBuffer = 3;
    static const int FIELD___cachedPhysicalAddress = 4;
    static const int FIELD___ethernetHeaderBufferWaitHandle = 5;
    static const int FIELD___bufferArray = 6;
    static const int FIELD___indexArray = 7;
    static const int FIELD___countArray = 8;
    static const int FIELD___linkLayer = 9;
    static const int FIELD___isDisposed = 10;
    static const int FIELD__LinkStateChanged = 11;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_IPv4Layer
{
    static const int FIELD_STATIC___sockets = 1;

    static const int FIELD___ethernetInterface = 1;
    static const int FIELD___isDisposed = 2;
    static const int FIELD___arpResolver = 3;
    static const int FIELD___dhcpv4Client = 4;
    static const int FIELD___icmpv4Handler = 5;
    static const int FIELD___dnsResolver = 6;
    static const int FIELD___tcpHandler = 7;
    static const int FIELD___handlesInUseBitmask = 8;
    static const int FIELD___handlesInUseBitmaskLockObject = 9;
    static const int FIELD___socketHandleReservedFreed = 10;
    static const int FIELD___socketHandleUserFreed = 11;
    static const int FIELD___ipv4configIPAddress = 12;
    static const int FIELD___ipv4configSubnetMask = 13;
    static const int FIELD___ipv4configGatewayAddress = 14;
    static const int FIELD___ipv4configDnsServerAddresses = 15;
    static const int FIELD___loopbackSourceIPAddress = 16;
    static const int FIELD___loopbackDestinationIPAddress = 17;
    static const int FIELD___loopbackProtocol = 18;
    static const int FIELD___loopbackBuffer = 19;
    static const int FIELD___loopbackBufferCount = 20;
    static const int FIELD___loopbackBufferInUse = 21;
    static const int FIELD___loopbackBufferFreedEvent = 22;
    static const int FIELD___loopbackBufferLockObject = 23;
    static const int FIELD___loopbackThread = 24;
    static const int FIELD___loopbackBufferFilledEvent = 25;
    static const int FIELD___receivedPacketBuffers = 26;
    static const int FIELD___ipv4HeaderBuffer = 27;
    static const int FIELD___ipv4HeaderBufferLockObject = 28;
    static const int FIELD___bufferArray = 29;
    static const int FIELD___indexArray = 30;
    static const int FIELD___countArray = 31;
    static const int FIELD___nextDatagramID = 32;
    static const int FIELD___nextDatagramIDLockObject = 33;
    static const int FIELD___nextEphemeralPort = 34;
    static const int FIELD___nextEphemeralPortLockObject = 35;
    static const int FIELD__LinkStateChanged = 36;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_SocketsInterface
{
    static const int FIELD_STATIC___initializeMethodSyncObject = 2;
    static const int FIELD_STATIC___isInitialized = 3;
    static const int FIELD_STATIC___ethernetInterface = 4;
    static const int FIELD_STATIC___ipv4Layer = 5;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_TcpHandler
{
    static const int FIELD___isDisposed = 1;
    static const int FIELD___ipv4Layer = 2;
    static const int FIELD___tcpHeaderBuffer = 3;
    static const int FIELD___tcpHeaderBufferLockObject = 4;
    static const int FIELD___tcpPseudoHeaderBuffer = 5;
    static const int FIELD___tcpPseudoHeaderBufferLockObject = 6;
    static const int FIELD___checksumBufferArray = 7;
    static const int FIELD___checksumOffsetArray = 8;
    static const int FIELD___checksumCountArray = 9;
    static const int FIELD___bufferArray = 10;
    static const int FIELD___indexArray = 11;
    static const int FIELD___countArray = 12;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_Utility
{
    static const int FIELD_STATIC___singleBuffer = 6;
    static const int FIELD_STATIC___singleOffset = 7;
    static const int FIELD_STATIC___singleCount = 8;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_IPv4Layer__ReceivedPacketBuffer
{
    static const int FIELD__SourceIPAddress = 1;
    static const int FIELD__DestinationIPAddress = 2;
    static const int FIELD__Identification = 3;
    static const int FIELD__Buffer = 4;
    static const int FIELD__ActualBufferLength = 5;
    static const int FIELD__Holes = 6;
    static const int FIELD__TimeoutInMachineTicks = 7;
    static const int FIELD__IsEmpty = 8;
    static const int FIELD__LockObject = 9;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_UdpSocket__ReceivedPacketBuffer
{
    static const int FIELD__SourceIPAddress = 1;
    static const int FIELD__SourceIPPort = 2;
    static const int FIELD__Buffer = 3;
    static const int FIELD__BufferBytesFilled = 4;
    static const int FIELD__IsEmpty = 5;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_IPv4Layer__ReceivedPacketBufferHoles
{
    static const int FIELD__FirstIndex = 1;
    static const int FIELD__LastIndex = 2;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_TcpHandler__TcpOption
{
    static const int FIELD__Kind = 1;
    static const int FIELD__Data = 2;


    //--//

};

struct Library_Netduino_IP_Netduino_IP_TcpSocket__TransmissionDetails
{
    static const int FIELD__TransmissionType = 1;
    static const int FIELD__TransmissionAttemptsCounter = 2;
    static const int FIELD__CurrentRetryTimeoutInMilliseconds = 3;
    static const int FIELD__CurrentRetryTimeoutInMachineTicks = 4;
    static const int FIELD__MaximumTimeoutInMachineTicks = 5;


    //--//

};



extern const CLR_RT_NativeAssemblyData g_CLR_AssemblyNative_Netduino_IP;

#endif  //_NETDUINO_IP_H_