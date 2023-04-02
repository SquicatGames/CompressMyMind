using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.DA.Core.Common.Helpers
{
    /// <summary>
    /// Вспомогательные методы для работы с коллекциами атомарных элементов сжатия
    /// </summary>
    internal static class CompressionAtomHelper
    {
        public static IEnumerable<byte> GetBytesByCompressionAtoms(IEnumerable<CompressionAtom> atoms)
        {
            List<byte> bytes = new List<byte>
            {
                0,
                0
            };

            int bitCount = 0;
            byte[] bitBuffer = new byte[8];

            foreach (CompressionAtom atom in atoms)
            {
                int codeValue = atom.Code;

                for (int i = 0; i < atom.CodeLen; i++)
                {
                    if(bitCount == 8)
                    {
                        bytes.Add(
                            GetByteByBits(bitBuffer));

                        bitCount = 0;
                    }

                    bitBuffer[bitCount] = (byte)(codeValue % 2);
                    codeValue /= 2;
                    bitCount++;
                }
            }

            bytes.Add(
                GetByteByBits(bitBuffer));

            bytes[0] = (byte)(bytes.Count() % 256);
            bytes[1] = (byte)(bytes.Count() / 256);

            return bytes;
        }

        public static IEnumerable<CompressionAtom> GetCompressionAtomsByBytes(IEnumerable<byte> bytes)
        {
            var result = new List<CompressionAtom>();

            var semibytes = QuantifyBytesBySemibytes(bytes)
                .ToArray();

            int index = 0;

            while(index <  semibytes.Length)
            {
                if (semibytes[index]<15)
                {
                    result.Add(new CompressionAtom
                    {
                        Code = semibytes[index],
                        CodeLen = 4
                    });

                    index++;
                }
                else
                {
                    index++;

                    result.Add(new CompressionAtom
                    {
                        Code = (15 << 8) + semibytes[index] * 16 + semibytes[index + 1],
                        CodeLen = 12
                    });

                    index += 2;
                }
            }

            return result;
        }

        public static Dictionary<CompressionAtom, byte> ReflectByteMap(Dictionary<byte, CompressionAtom> map)
        {
            var result = new Dictionary<CompressionAtom, byte>();
            foreach(var pair in map)
            {
                result.Add(
                    pair.Value, 
                    pair.Key);
            }

            return result;
        }

        private static byte GetByteByBits(IEnumerable<byte> bits)
        {
            byte result = 0;
            foreach(var bit in bits)
            {
                result = (byte)(result * 2 + bit);
            }

            return result;
        }

        private static IEnumerable<byte> QuantifyBytesBySemibytes(
            IEnumerable<byte> source)
        {
            var result = new List<byte>();

            Dictionary<byte, byte> mapReversedSemibitValue = new Dictionary<byte, byte>
            {
                { (byte)0, (byte)0 },
                { (byte)8, (byte)1 },
                { (byte)4, (byte)2 },
                { (byte)12, (byte)3 },
                { (byte)2, (byte)4 },
                { (byte)10, (byte)5 },
                { (byte)6, (byte)6 },
                { (byte)14, (byte)7 },
                { (byte)1, (byte)8 },
                { (byte)9, (byte)9 },
                { (byte)5, (byte)10 },
                { (byte)13, (byte)11 },
                { (byte)3, (byte)12 },
                { (byte)11, (byte)13 },
                { (byte)7, (byte)14 },
                { (byte)15, (byte)15 },
            };

            foreach(var _byte in source)
            {
                result.Add(mapReversedSemibitValue[(byte)(_byte / 16)]);
                result.Add(mapReversedSemibitValue[(byte)(_byte % 16)]);
            }

            return result;
        }
    }
}
