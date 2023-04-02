using System;
using System.Collections.Generic;
using System.Linq;
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
            return null;
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

        private static IEnumerable<byte> GetBitsByByte(byte _byte)
        {
            return null;
        }
    }
}
