using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class NetworkTransformSystem : MonoBehaviour
    {
        // todo replace singleton with scriptable object
        public static NetworkTransformSystem Instance { get; private set; }

        //public int maxMessageSize = 1000;

        Dictionary<uint, IHasPosition> behaviours;

        [Header("Position Compression")]
        [SerializeField] bool compressPosition = true;
        [SerializeField] Vector3 min = Vector3.one * -100;
        [SerializeField] Vector3 max = Vector3.one * -100;
        [SerializeField] float precision = 0.01f;


        [Header("Debug And Gizmo")]
        [SerializeField] private bool drawGizmo;
        [SerializeField] private Color gizmoColor;
        [Tooltip("readonly")]
        [SerializeField] private int bitCount;
        [Tooltip("readonly")]
        [SerializeField] private int byteCount;

        [NonSerialized]
        public PositionCompression compression;

        private void OnValidate()
        {
            compression = new PositionCompression(min, max, precision);
            bitCount = compression.bitCount;
            byteCount = Mathf.CeilToInt(bitCount);
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            compression = new PositionCompression(min, max, precision);
        }
        public void RegisterHandlers()
        {
            if (NetworkClient.active)
            {
                NetworkClient.RegisterHandler<NetworkPositionMessage>(ClientHandleNetworkPositionMessage);
            }

            if (NetworkServer.active)
            {
                NetworkServer.RegisterHandler<NetworkPositionSingleMessage>(ServerHandleNetworkPositionMessage);
            }
        }

        public void UnregisterHandlers()
        {
            if (NetworkClient.active)
            {
                NetworkClient.UnregisterHandler<NetworkPositionMessage>();
            }

            if (NetworkServer.active)
            {
                NetworkServer.UnregisterHandler<NetworkPositionSingleMessage>();
            }
        }

        //public class VisibleGroup
        //{
        //    public List<NetworkConnection> connections;
        //    public IHasPosition hasPosition;
        //}

        //void SendUpdate(VisibleGroup group)
        //{

        //}

        void SendUpdateToAll()
        {
            // message size = (1/2 + 12) * n + 4;
            NetworkPositionMessage msg;
            using (PooledNetworkWriter writer = NetworkWriterPool.GetWriter())
            {
                foreach (KeyValuePair<uint, IHasPosition> kvp in behaviours)
                {
                    uint id = kvp.Key;
                    IHasPosition behaviour = kvp.Value;
                    Vector3 position = behaviour.Position;

                    writer.WritePackedUInt32(id);
                    // todo compress
                    writer.WriteVector3(position);
                }

                msg = new NetworkPositionMessage
                {
                    bytes = writer.ToArraySegment()
                };
            }

            NetworkServer.SendToAll(msg);
        }



        private void ServerHandleNetworkPositionMessage(NetworkConnection arg1, NetworkPositionSingleMessage arg2)
        {
            throw new NotImplementedException();
        }


        private void ClientHandleNetworkPositionMessage(NetworkConnection conn, NetworkPositionMessage msg)
        {
            throw new NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo) { return; }
            Gizmos.color = gizmoColor;
            Bounds bounds = default;
            bounds.min = new Vector3(0, -20, 0);
            bounds.max = new Vector3(500, 40, 500);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    public interface IHasPosition
    {
        /// <summary>
        /// Position of object
        /// <para>Could be localposition or world position writer doesn't care</para>
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Normally NetId, but could be a 
        /// </summary>
        uint Id { get; }
    }
    public struct NetworkPositionMessage : NetworkMessage
    {
        public ArraySegment<byte> bytes;
    }
    public struct NetworkPositionSingleMessage : NetworkMessage
    {
        public uint id;
        public Vector3 position;
    }
    public static class PositionMessageWriter
    {
        public static void WritePositionMessage(this NetworkWriter writer, NetworkPositionMessage msg)
        {
            int count = msg.bytes.Count;
            writer.WriteUInt16((ushort)count);
            writer.WriteBytes(msg.bytes.Array, msg.bytes.Offset, count);
        }
        public static NetworkPositionMessage ReadPositionMessage(this NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            ArraySegment<byte> bytes = reader.ReadBytesSegment(count);

            return new NetworkPositionMessage
            {
                bytes = bytes
            };
        }

        public static void WriteNetworkPositionSingleMessage(this NetworkWriter writer, NetworkPositionSingleMessage msg)
        {
            writer.WritePackedUInt32(msg.id);
            PositionCompression compression = NetworkTransformSystem.Instance.compression;
            compression.Compress(writer, msg.position);
        }
        public static NetworkPositionSingleMessage ReadNetworkPositionSingleMessage(this NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            ArraySegment<byte> bytes = reader.ReadBytesSegment(count);

            uint id = reader.ReadPackedUInt32();
            PositionCompression compression = NetworkTransformSystem.Instance.compression;
            Vector3 pos = compression.Decompress(reader);

            return new NetworkPositionSingleMessage
            {
                id = id,
                position = pos
            };
        }
    }
}
