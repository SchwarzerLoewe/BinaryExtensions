using System.Numerics;
using System.Runtime.InteropServices;

namespace System.IO.BinaryExtensions
{
    public static class Extensions
    {
        public static T ReadStruct<T>(this BinaryReader br)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            T oReturn;

            try
            {
                br.BaseStream.Read(buffer, 0, buffer.Length);

                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                oReturn = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                handle.Free();

                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Write<T>(this BinaryWriter bw, T value)
            where T : struct
        {
            try
            {
                // This function copys the structure data into a byte[]
                var buffer = new byte[Marshal.SizeOf(value)]; //Set the buffer ot the correct size

                GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                //Allocate the buffer to memory and pin it so that GC cannot use the space (Disable GC)
                Marshal.StructureToPtr(value, h.AddrOfPinnedObject(), false);
                // copy the struct into int byte[] mem alloc 
                h.Free(); //Allow GC to do its job

                bw.Write(buffer); // return the byte[] . After all thats why we are here right.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Write(this BinaryWriter bw, MemoryStream value)
        {
            bw.Write(value.Length);
            bw.Write(value.ToArray());
        }
        public static MemoryStream ReadMemoryStream(this BinaryReader br)
        {
            var c = br.ReadInt32();

            return new MemoryStream(br.ReadBytes(c));
        }

        public static void Write(this BinaryWriter bw, Guid value)
        {
            bw.Write(value.ToByteArray().Length);
            bw.Write(value.ToByteArray());
        }
        public static Guid ReadGuid(this BinaryReader br)
        {
            var c = br.ReadInt32();
            return new Guid(br.ReadBytes(c));
        }

        public static void Write(this BinaryWriter bw, TimeSpan value)
        {
            bw.Write(value.Milliseconds);
        }
        public static TimeSpan ReadTimespan(this BinaryReader br)
        {
            return TimeSpan.FromMilliseconds(br.ReadInt32());
        }

        public static void Write(this BinaryWriter bw, DateTime value)
        {
            bw.Write(value.ToBinary());
        }
        public static DateTime ReadDateTime(this BinaryReader br)
        {
            return DateTime.FromBinary(br.ReadInt64());
        }

        public static void Write(this BinaryWriter bw, Version value)
        {
            bw.Write(value.Major);
            bw.Write(value.Minor);
            bw.Write(value.Revision);
            bw.Write(value.Build);
        }
        public static Version Read(this BinaryReader br)
        {
            return new Version(br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16());
        }

        public static void Write(this BinaryWriter bw, BigInteger bi)
        {
            var buff = bi.ToByteArray();

            bw.Write(buff.Length);
            bw.Write(buff);
        }
        public static BigInteger ReadBigInteger(this BinaryReader br)
        {
            var c = br.ReadInt32();
            var buff = br.ReadBytes(c);

            return new BigInteger(buff);
        }

        public static void Write(this BinaryWriter bw, BigDecimal bi)
        {
            bw.Write(bi.Mantissa);
            bw.Write(bi.Exponent);
        }
        public static BigDecimal ReadBigDecimal(this BinaryReader br)
        {
            var man = br.ReadBigInteger();
            var exp = br.ReadInt32();

            return new BigDecimal(man, exp);
        }

        public static void Write(this BinaryWriter bw, Complex bi)
        {
            bw.Write(bi.Real);
            bw.Write(bi.Imaginary);
        }
        public static Complex ReadComplex(this BinaryReader br)
        {
            var real = br.ReadDouble();
            var imaginary = br.ReadDouble();
            
            return new Complex(real, imaginary);
        }
    }
}