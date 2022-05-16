using ProtoBuf;

namespace Jeopardy.Core.Serialization
{
    public static class BinarySerializer
    {
        public static byte[] Serialize<T>(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using MemoryStream ms = new();
            Serializer.Serialize(ms, data);

            return ms.ToArray();
        }

        public static void SerializeToFile<T>(T data, string destinationPath) => File.WriteAllBytes(destinationPath, Serialize(data));

        public static T Deserialize<T>(byte[] bytes)
        {
            using MemoryStream? stream = new();

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return Serializer.Deserialize<T>(stream);
        }

        public static T DeserializeFromFile<T>(string sourcePath)
        {
            var bytes = File.ReadAllBytes(sourcePath);
            return Deserialize<T>(bytes);
        }
    }
}
