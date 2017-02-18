//INSTANT C# NOTE: Formerly VB project-level imports:

namespace BigLamp.Extensions.ByteArray
{
    public static class ByteArrayExtensions
    {

        /// <summary>
        /// Orders byte array backwards.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] FlipByteArray(this byte[] bytes)
        {
            var flippedBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                flippedBytes[bytes.Length - i - 1] = bytes[i];
            }
            return flippedBytes;
        }

    }
}
