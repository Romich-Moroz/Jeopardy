using BinaryPack;

namespace Jeopardy.Core.Files.Serialization
{
    public static class BinarySerializer<T> where T : class, new()
    {
        public static byte[] Serialize(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return BinaryConverter.Serialize(data);
        }

        public static void SerializeToFile(T data, string destinationPath)
        {
            File.WriteAllBytes(destinationPath, Serialize(data));
        }

        public static T Deserialize(byte[] bytes)
        {
            using MemoryStream? stream = new();

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return BinaryConverter.Deserialize<T>(stream);
        }

        public static T DeserializeFromFile(string sourcePath)
        {
            byte[]? bytes = File.ReadAllBytes(sourcePath);
            return Deserialize(bytes);
        }
    }
}
