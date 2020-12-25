using NUnit.Framework;

namespace Mirror.TransformSyncing.Tests
{
    public class BitWriterTest
    {
        [Test]
        public void CanWrite32Bits([Random(0, uint.MaxValue, 100)]uint inValue)
        {
            var netWriter = new NetworkWriter();
            var writer = new BitWriter(netWriter);

            writer.Write(inValue, 32);

            writer.Flush();

            var netReader = new NetworkReader(netWriter.ToArray());
            var reader = new BitReader(netReader);

            var outValue = reader.Read(32);

            Assert.That(outValue, Is.EqualTo(inValue));
        }

        const uint max = (1u << 10) - 1u;
        const int CanWrite3MultipleValues_RandomCount = 10;
        [Test]
        public void CanWrite3MultipleValues(
            [Random(0, max, CanWrite3MultipleValues_RandomCount)] uint inValue1,
            [Random(0, max, CanWrite3MultipleValues_RandomCount)] uint inValue2,
            [Random(0, max, CanWrite3MultipleValues_RandomCount)] uint inValue3
        )
        {
            var netWriter = new NetworkWriter();
            var writer = new BitWriter(netWriter);

            writer.Write(inValue1, 10);
            writer.Write(inValue2, 10);
            writer.Write(inValue3, 10);

            writer.Flush();

            var netReader = new NetworkReader(netWriter.ToArray());
            var reader = new BitReader(netReader);

            var outValue1 = reader.Read(10);
            var outValue2 = reader.Read(10);
            var outValue3 = reader.Read(10);

            Assert.That(outValue1, Is.EqualTo(inValue1));
            Assert.That(outValue2, Is.EqualTo(inValue2));
            Assert.That(outValue3, Is.EqualTo(inValue3));
        }

        const int CanWrite8MultipleValues_RandomCount = 3;
        [Test]
        public void CanWrite8MultipleValues(
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue1,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue2,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue3,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue4,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue5,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue6,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue7,
            [Random(0, max, CanWrite8MultipleValues_RandomCount)] uint inValue8
        )
        {
            var netWriter = new NetworkWriter();
            var writer = new BitWriter(netWriter);

            writer.Write(inValue1, 10);
            writer.Write(inValue2, 10);
            writer.Write(inValue3, 10);
            writer.Write(inValue4, 10);
            writer.Write(inValue5, 10);
            writer.Write(inValue6, 10);
            writer.Write(inValue7, 10);
            writer.Write(inValue8, 10);

            writer.Flush();

            var netReader = new NetworkReader(netWriter.ToArray());
            var reader = new BitReader(netReader);

            var outValue1 = reader.Read(10);
            var outValue2 = reader.Read(10);
            var outValue3 = reader.Read(10);
            var outValue4 = reader.Read(10);
            var outValue5 = reader.Read(10);
            var outValue6 = reader.Read(10);
            var outValue7 = reader.Read(10);
            var outValue8 = reader.Read(10);

            Assert.That(outValue1, Is.EqualTo(inValue1));
            Assert.That(outValue2, Is.EqualTo(inValue2));
            Assert.That(outValue3, Is.EqualTo(inValue3));
            Assert.That(outValue4, Is.EqualTo(inValue4));
            Assert.That(outValue5, Is.EqualTo(inValue5));
            Assert.That(outValue6, Is.EqualTo(inValue6));
            Assert.That(outValue7, Is.EqualTo(inValue7));
            Assert.That(outValue8, Is.EqualTo(inValue8));
        }
    }
}
