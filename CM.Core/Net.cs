using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CM.Core
{
    public class Net
    {
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        public static string GetIpAddr()
        {
            var address = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return address.ToString();
        }
    }
}
