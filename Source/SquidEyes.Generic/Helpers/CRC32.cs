using System;
using System.Security.Cryptography;

namespace SquidEyes.Generic
{
    public abstract class CRC32 : HashAlgorithm
    {
        public CRC32()
            : base()
        {
            this.HashSizeValue = 32;
        }

        public uint CRC32Hash { get; protected set; }

        new public static CRC32 Create()
        {
            return new CRC32Managed();
        }

        public static CRC32 Create(uint polynomial)
        {
            return new CRC32Managed(polynomial);
        }

        new public static CRC32 Create(string hashName)
        {
            throw new NotImplementedException();
        }
    }

    public class CRC32Managed : CRC32
    {
        private uint[] crc32Table = new uint[256];

        private uint crc32Result;

        public CRC32Managed()
            : this(0xEDB88320)
        {
        }

        public CRC32Managed(uint polynomial)
            : base()
        {
            for (uint i = 0; i < 256; i++)
            {
                uint crc32 = i;

                for (int j = 8; j > 0; j--)
                {
                    if ((crc32 & 1) == 1)
                        crc32 = (crc32 >> 1) ^ polynomial;
                    else
                        crc32 >>= 1;
                }

                crc32Table[i] = crc32;
            }
        }

        public override bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        public override bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        public override void Initialize()
        {
            this.crc32Result = 0xFFFFFFFF;
        }

        protected override void HashCore(byte[] array, int start, int size)
        {
            int end = start + size;

            for (int i = start; i < end; i++)
            {
                this.crc32Result = (this.crc32Result >> 8) ^
                    this.crc32Table[array[i] ^ (this.crc32Result & 0x000000FF)];
            }
        }

        protected override byte[] HashFinal()
        {
            this.crc32Result = ~this.crc32Result;

            this.CRC32Hash = this.crc32Result;

            this.HashValue = BitConverter.GetBytes(this.crc32Result);

            return this.HashValue;
        }
    }
}