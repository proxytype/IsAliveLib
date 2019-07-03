using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public IsAlivePayload check(IPAddress address, int port, NETWORK_PROTOCOL protocol)
        {
            IsAlivePayload payload = new IsAlivePayload();
            payload.host = address;
            payload.port = port;
            payload.protocol = protocol;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            switch (protocol)
            {
                case NETWORK_PROTOCOL.TCP:
                    isAliveTCP(ref payload);
                    break;
                case NETWORK_PROTOCOL.UDP:
                    isAliveUDP(ref payload);
                    break;
                case NETWORK_PROTOCOL.ICMP:
                    isAliveICMP(ref payload);
                    break;
                default:
                    payload.errorMessage = "Unsupported Protocol";
                    break;
            }

            watch.Stop();

            payload.totalTime = watch.Elapsed;

            return payload;
        }

        private void isAliveTCP(ref IsAlivePayload payload)
        {
            TcpClient myclient = new TcpClient();

            myclient.SendTimeout = _timeOut;
            myclient.ReceiveTimeout = _timeOut;

            try
            {
                if (!myclient.ConnectAsync(payload.host, payload.port).Wait(_timeOut)) {
                    payload.errorMessage = "Timeout";
                }
                else
                {
                    payload.success = true;
                }
                
            }
            catch (Exception ex)
            {
                payload.errorMessage = ex.ToString();
            }
            finally
            {
                myclient.Close();
            }

        }

        private void isAliveUDP(ref IsAlivePayload payload)
        {

            UdpClient myclient = new UdpClient();

            try
            {
                myclient.Client.SendTimeout = _timeOut;
                myclient.Client.ReceiveTimeout = _timeOut;

                myclient.Connect(payload.host, payload.port);
                Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there");

                int count = myclient.Send(sendBytes, sendBytes.Length);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                
                Byte[] receiveBytes = myclient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                if (returnData.Length != 0)
                {
                    payload.success = true;
                }
                else {
                    payload.errorMessage = "Package Sent No Message Received";
                }
            }
            catch (Exception ex)
            {
                payload.errorMessage = ex.ToString();
            }
            finally
            {
                myclient.Close();
            }

        }

        private void isAliveICMP(ref IsAlivePayload payload)
        {
            Ping myping = new Ping();
            try
            {
                PingReply replay = myping.Send(payload.host, 2000);

                if (replay == null)
                {
                    payload.errorMessage = "Replay Empty";
                }
                else if (replay.Status == IPStatus.Success)
                {
                    payload.success = true;
                }
                else
                {
                    payload.errorMessage = replay.Status.ToString();
                }

            }
            catch (Exception ex)
            {
                payload.errorMessage = ex.ToString();
            }

        }
    }
}
