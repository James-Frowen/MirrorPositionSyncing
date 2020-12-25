using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace Mirror.TransformSyncing
{
    public class BitWriter
    {
        private const int ScratchSize = 32;
        readonly NetworkWriter writer;

        ulong scratch;
        int scratch_bits;

        public BitWriter(NetworkWriter writer)
        {
            this.writer = writer;
        }

        public void Write(uint value, int bits)
        {
            if (bits > ScratchSize)
            {
                throw new System.ArgumentException($"bits must be less than {ScratchSize}");
            }

            var mask = (1ul << bits) - 1;
            ulong longValue = value & mask;
            scratch |= (longValue << scratch_bits);
            scratch_bits += bits;

            if (scratch_bits >= ScratchSize)
            {
                var toWrite = (uint)scratch;
                writer.WriteBlittable(toWrite);

                scratch >>= ScratchSize;
                scratch_bits -= ScratchSize;
            }
        }
        public void Flush()
        {
            if (scratch_bits > 24)
            {
                var toWrite = (uint)scratch;
                writer.WriteBlittable(toWrite);
            }
            else if (scratch_bits > 16)
            {
                // todo only write 3 bytes
                var toWrite = (uint)scratch;
                writer.WriteBlittable(toWrite);
            }
            else if (scratch_bits > 8)
            {
                var toWrite = (ushort)scratch;
                writer.WriteBlittable(toWrite);
            }
            else if (scratch_bits > 0)
            {
                var toWrite = (byte)scratch;
                writer.WriteBlittable(toWrite);
            }
        }
    }
    public class BitReader
    {
        private const int ScratchSize = 32;
        readonly NetworkReader reader;

        ulong scratch;
        int scratch_bits;
        int full_scratches;
        int scratches_extra_bits;

        public BitReader(NetworkReader reader)
        {
            this.reader = reader;
            var total_bytes = reader.Length;
            full_scratches = total_bytes / 4;
            scratches_extra_bits = total_bytes - full_scratches * 4;
        }

        public uint Read(int bits)
        {
            if (bits > ScratchSize)
            {
                throw new System.ArgumentException($"bits must be less than {ScratchSize}");
            }

            if (scratch_bits < ScratchSize)
            {
                uint newBits= readScratch();
                scratch >>= ScratchSize;
                scratch |= ((ulong)newBits) << ScratchSize;
                scratch_bits += ScratchSize;
            }

            ulong mask = (1ul << bits) - 1;
            mask <<= scratch_bits;
            var value = (scratch & mask) >> scratch_bits;
            scratch_bits += bits;

            return (uint)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint readScratch()
        {
            uint newBits;
            if (full_scratches > 0)
            {
                newBits = reader.ReadBlittable<uint>();
                full_scratches--;
            }
            else
            {
                if (scratches_extra_bits > 24)
                {
                    newBits = reader.Read<uint>();
                }
                else if (scratches_extra_bits > 16)
                {
                    // todo only write 3 bytes
                    newBits = reader.Read<uint>();
                }
                else if (scratches_extra_bits > 8)
                {
                    newBits = reader.Read<ushort>();
                }
                else
                {
                    newBits = reader.Read<byte>();
                }
            }

            return newBits;
        }
    }
}
