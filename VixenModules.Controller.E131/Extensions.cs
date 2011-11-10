namespace VixenModules.Controller.E131
{
    using System;
    using System.Text;

    public static class Extensions
    {
        public static int TryParseInt32(this string value, int defaultInt32)
        {
            int converted;
            if (!Int32.TryParse(value, out converted))
            {
                converted = defaultInt32;
            }

            return converted;
        }

        internal static Guid BufferToGuid(byte[] bfr, int offset)
        {
            var valBytes = new byte[16];
            Guid val;

            Array.Copy(bfr, offset, valBytes, 0, valBytes.Length);

            val = new Guid(valBytes);

            return val;
        }

        internal static string BfrToString(byte[] bfr, int offset, int length)
        {
            var val = new UTF8Encoding();

            return val.GetString(bfr, offset, length);
        }

        internal static ushort BfrToUInt16Swapped(byte[] bfr, int offset)
        {
            return (UInt16)((bfr[offset] << 8) | bfr[offset + 1]);
        }

        internal static uint BfrToUInt32Swapped(byte[] bfr, int offset)
        {
            return (((UInt32)bfr[offset]) << 24) | (((UInt32)bfr[offset + 1]) << 16) | (((UInt32)bfr[offset + 2]) << 8)
                   | (bfr[offset + 3]);
        }

        internal static void GuidToBfr(Guid value, byte[] bfr, int offset)
        {
            var valBytes = value.ToByteArray();

            Array.Copy(valBytes, 0, bfr, offset, valBytes.Length);
        }

        internal static void StringToBfr(string value, byte[] bfr, int offset, int length)
        {
            var val = new UTF8Encoding();
            byte[] valBytes;

            valBytes = val.GetBytes(value);

            if (valBytes.Length >= length)
            {
                Array.Copy(valBytes, 0, bfr, offset, length);
            }
            else
            {
                Array.Copy(valBytes, 0, bfr, offset, valBytes.Length);

                offset += valBytes.Length;
                length -= valBytes.Length;

                while (length-- > 0)
                {
                    bfr[offset++] = 0;
                }
            }
        }

        internal static void UInt16ToBfrSwapped(ushort value, byte[] bfr, int offset)
        {
            bfr[offset] = (byte)((value & 0xff00) >> 8);
            bfr[offset + 1] = (byte)(value & 0x00ff);
        }

        internal static void UInt32ToBfrSwapped(uint value, byte[] bfr, int offset)
        {
            bfr[offset] = (byte)((value & 0xff000000) >> 24);
            bfr[offset + 1] = (byte)((value & 0x00ff0000) >> 16);
            bfr[offset + 2] = (byte)((value & 0x0000ff00) >> 8);
            bfr[offset + 3] = (byte)(value & 0x000000ff);
        }
    }
}