# IsAlive
Introducing a versatile library for monitoring remote network services, capable of checking connectivity across ports and IP addresses. This robust solution supports three key protocols: ICMP for ping functionality, TCP, and UDP (requiring data reception for confirmation). This library empowers you to efficiently monitor the health and connectivity of remote network services, ensuring a reliable and comprehensive approach to network monitoring. Integrate seamlessly to enhance your application's capabilities in assessing and maintaining remote service connections.

![alt text](https://github.com/proxytype/IsAliveLib/blob/master/isAlive1.gif)

Usage:

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
     Console.WriteLine("Response - " + payload.host.ToString() 
     + ":" + payload.port.ToString() + " " + payload.protocol.ToString() + " ->" + payload.success.ToString());
}
```
