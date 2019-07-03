using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace IsAliveLib
{
    public class IsAlive
    {
        public enum NETWORK_PROTOCOL
        {
            TCP,
            UDP,
            ICMP
        }

        public const int DEFAULT_TIME_OUT = 30;

        int _timeOut = DEFAULT_TIME_OUT;

        public IsAlive() { }

        public IsAlive(int timeOut)
        {
            _timeOut = timeOut;
        }


        public bool check(IPAddress address, int port, NETWORK_PROTOCOL protocol)
        {
            switch (protocol)
            {
                case NETWORK_PROTOCOL.TCP:
                    return isAliveTCP(address, port);
                case NETWORK_PROTOCOL.UDP:
                    return isAliveUDP(address, port);
                case NETWORK_PROTOCOL.ICMP:
                    return isAliveICMP(address);
                default:
                    return false;
            }
        }

        private bool isAliveTCP(IPAddress address, int port)
        {
            TcpClient myclient = new TcpClient();

            myclient.SendTimeout = _timeOut;
            myclient.ReceiveTimeout = _timeOut;

            try
            {
                myclient.Connect(address, port);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                myclient.Close();
            }

        }

        private bool isAliveUDP(IPAddress ip, int port)
        {

            UdpClient myclient = new UdpClient();

            try
            {
                myclient.Client.SendTimeout = _timeOut;
                myclient.Client.ReceiveTimeout = _timeOut;

                myclient.Connect(ip, port);
                Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there");

                int count = myclient.Send(sendBytes, sendBytes.Length);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                
                Byte[] receiveBytes = myclient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                if (returnData.Length != 0)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                myclient.Close();
            }

        }

        private bool isAliveICMP(IPAddress address)
        {
            Ping myping = new Ping();
            try
            {
                PingReply replay = myping.Send(address, 2000);

                if (replay == null)
                {
                    return false;
                }
                else if (replay.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
