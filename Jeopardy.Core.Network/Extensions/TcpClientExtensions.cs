using Jeopardy.Core.Serialization;
using System.Net.Sockets;

namespace Jeopardy.Core.Network.Extensions
{
    public static class TcpClientExtensions
    {
        public static async Task<T> ReceiveDataAsync<T>(this TcpClient tcpClient)
        {
            var bytes = new byte[8192];
            NetworkStream stream = tcpClient.GetStream();
            var recievedTotal = -sizeof(int);
            var dataSize = 0;

            using var ms = new MemoryStream();
            do
            {
                var receivedPerCycle = await stream.ReadAsync(bytes);
                if (receivedPerCycle > 0)
                {
                    if (dataSize == 0) //possible crash if recieved less than 4 bytes at first read
                    {
                        dataSize = (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
                        ms.Write(bytes, 4, receivedPerCycle - sizeof(int));
                    }
                    else
                    {
                        ms.Write(bytes, 0, receivedPerCycle);
                    }

                    recievedTotal += receivedPerCycle;
                }
            }
            while (recievedTotal != dataSize /*&& stream.DataAvailable*/);

            return BinarySerializer.Deserialize<T>(ms.ToArray());
        }

        public static async Task<bool> SendDataAsync<T>(this TcpClient tcpClient, T data)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                var usefulData = BinarySerializer.Serialize(data);
                var dataSize = new byte[4]
                {
                    (byte)((usefulData.Length >> 24) & 0xff),
                    (byte)((usefulData.Length >> 16) & 0xff),
                    (byte)((usefulData.Length >> 8) & 0xff),
                    (byte)(usefulData.Length & 0xff)
                };
                await stream.WriteAsync(dataSize.Concat(usefulData).ToArray());
                return true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            return false;
        }
    }
}
