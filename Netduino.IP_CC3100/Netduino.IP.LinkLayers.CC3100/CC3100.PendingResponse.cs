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
        ///     Class used to hold a pending request.
        /// </summary>
        /// <remarks>
        ///     The system has a queue of pending requests.  This requires an object to hold
        ///     information about any requests (socket handle, data buffers, locks etc.).
        /// </remarks>
        class PendingResponse
        {
            public CC3100Opcode OpCode;
            public AutoResetEvent WaitHandle = new AutoResetEvent(false);
            public object ResponseData = null;
            public Int32 SocketHandle = -1; /* this is used on async responses which are socket-specific; -1 means "do not match socketHandle on response" */

            internal void SetResponse(object responseData)
            {
                this.ResponseData = responseData;
                this.WaitHandle.Set();
            }
        }
    }
}