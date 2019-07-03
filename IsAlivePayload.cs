using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IsAliveLib
{
    public class IsAlivePayload
    {
        public bool success { get; set; } = false;
        public string errorMessage { get; set; } = String.Empty;
        public TimeSpan totalTime { get; set; }
        public IsAlive.NETWORK_PROTOCOL protocol { get; set; }
        public IPAddress host { get; set; }
        public int port { get; set; }
    }
}
