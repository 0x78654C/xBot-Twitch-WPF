using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class Network
    {

        /// <summary>
        /// Verifies if IP is up or not
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>verifies if IP is up or not</returns>

        public static bool pingH(string ip)
        {
            bool pingable = false;
            Ping pinger = null;
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(ip);
                pingable = reply.Status == IPStatus.Success;

            }
            catch (PingException p)
            {
                CLog.LogWriteError("Core - pingH: " + p.ToString());
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }

            }
            return pingable;

        }
        /// <summary>
        /// Checking internet connection with Google DNS 8.8.8.8
        /// </summary>
        /// <returns></returns>
        public static bool inetCK()
        {
            if (pingH("8.8.8.8"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///Check TCP connection for a specific address/hostname and port
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns>bool</returns>
        public static bool portCheck(string address,int port)
        {
            var connect = new TcpClient(address, port);
            if (connect.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
