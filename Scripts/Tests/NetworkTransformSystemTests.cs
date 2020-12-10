using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Mirror.PositionSyncing.Tests
{
    public class NetworkTransformSystemTests
    {
        static IEnumerable CompressesAndDecompressesCases()
        {
            yield return new TestCaseData(new Vector3(0, 0, 0), new Vector3(100, 100, 100), 0.01f, new Vector3(0, 0, 0));
            yield return new TestCaseData(new Vector3(0, 0, 0), new Vector3(100, 100, 100), 0.01f, new Vector3(20, 20, 20));
            yield return new TestCaseData(new Vector3(0, 0, 0), new Vector3(100, 100, 100), 0.01f, new Vector3(50, 50, 50));
            yield return new TestCaseData(new Vector3(0, 0, 0), new Vector3(100, 100, 100), 0.01f, new Vector3(100, 100, 100));
        }


        [Test]
        [TestCaseSource(nameof(CompressesAndDecompressesCases))]
        public void CanSync1Object(Vector3 min, Vector3 max, float precision, Vector3 inValue)
        {
            NetworkTransformSystem system = new NetworkTransformSystem();
            system.compression = new PositionCompression(min, max, precision);

            IHasPosition hasPos = Substitute.For<IHasPosition>();
            hasPos.Position.Returns(inValue);
            hasPos.Id.Returns(1u);

            system.AddBehaviour(hasPos);

            NetworkPositionMessage msg = system.CreateSendToAllMessage();

            Assert.That(msg.bytes.Count, Is.EqualTo(1 + Mathf.CeilToInt(system.compression.bitCount / 8f)));

            system.ClientHandleNetworkPositionMessage(null, msg);

            hasPos.Received(1).SetPositionClient(Arg.Is<Vector3>(v => Vector3AlmostEqual(v, inValue, precision)));
        }

        bool Vector3AlmostEqual(Vector3 actual, Vector3 expected, float precision)
        {
            return FloatAlmostEqual(actual.x, expected.x, precision)
                && FloatAlmostEqual(actual.y, expected.y, precision)
                && FloatAlmostEqual(actual.z, expected.z, precision);
        }

        bool FloatAlmostEqual(float actual, float expected, float precision)
        {
            float minAllowed = expected + precision;
            float maxnAllowed = expected - precision;

            return minAllowed < actual && actual < maxnAllowed;
        }
    }
}
