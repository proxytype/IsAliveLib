# IsAlive
Simple library for checking network services are availability on remote host, support ICMP (Ping), TCP, UDP

![alt text](https://github.com/proxytype/IsAliveLib/blob/master/isAlive1.gif)

This Library can help monitor remote network services by checking connectivity of ports and ip address, it's support 3 main protocols: ICMP as ping, TCP and UDP (must receive some data for confirmation).

```c#
static void Main(string[] args)
        {

            IsAlive isAlive = new IsAlive(3000);

            IPAddress address = IPAddress.Parse("8.8.8.8");

            IsAlivePayload payload = isAlive.check(address, 0, IsAlive.NETWORK_PROTOCOL.ICMP);
            print(payload);

            payload = isAlive.check(address, 80, IsAlive.NETWORK_PROTOCOL.TCP);
            print(payload);

            payload = isAlive.check(address, 80, IsAlive.NETWORK_PROTOCOL.UDP);
            print(payload);

            Console.ReadLine();
        }

        static void print(IsAlivePayload payload) {
            Console.WriteLine("Response - " + payload.host.ToString() + ":" + payload.port.ToString() + " " + payload.protocol.ToString() + " ->" + payload.success.ToString());
        }
```
