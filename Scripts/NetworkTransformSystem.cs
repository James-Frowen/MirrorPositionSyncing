using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PositionSyncing
{
    public class NetworkTransformSystem : MonoBehaviour
    {
        // todo replace singleton with scriptable object
        public static NetworkTransformSystem Instance { get; private set; }

        //public int maxMessageSize = 1000;

        readonly Dictionary<uint, IHasPosition> behaviours = new Dictionary<uint, IHasPosition>();

        [Header("Position Compression")]
        [SerializeField] bool compressPosition = true;


        internal void AddBehaviour(IHasPosition behaviour)
        {
            behaviours.Add(behaviour.Id, behaviour);
        }

        internal void RemoveBehaviour(IHasPosition behaviour)
        {
            behaviours.Remove(behaviour.Id);
        }

        [SerializeField] Vector3 min = Vector3.one * -100;
        [SerializeField] Vector3 max = Vector3.one * -100;
        [SerializeField] float precision = 0.01f;


        [Header("Debug And Gizmo")]
        [SerializeField] private bool drawGizmo;
        [SerializeField] private Color gizmoColor;
        [Tooltip("readonly")]
        [SerializeField] private int _bitCount;
        [Tooltip("readonly")]
        [SerializeField] private int _byteCount;

        [NonSerialized]
        public PositionCompression compression;

        private void OnValidate()
        {
            compression = new PositionCompression(min, max, precision);
            _bitCount = compression.bitCount;
            _byteCount = Mathf.CeilToInt(_bitCount);
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
            NetworkPositionMessage msg;
            using (PooledNetworkWriter writer = NetworkWriterPool.GetWriter())
            {
                foreach (KeyValuePair<uint, IHasPosition> kvp in behaviours)
                {
                    uint id = kvp.Key;
                    Vector3 position = kvp.Value.Position;

                    writer.WritePackedUInt32(id);

                    if (compressPosition)
                    {
                        compression.Compress(writer, position);
                    }
                    else
                    {
                        writer.WriteVector3(position);
                    }
                }

                msg = new NetworkPositionMessage
                {
                    bytes = writer.ToArraySegment()
                };
            }

            NetworkServer.SendToAll(msg);
        }



        /// <summary>
        /// Position from client to server
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void ServerHandleNetworkPositionMessage(NetworkConnection conn, NetworkPositionSingleMessage msg)
        {
            uint id = msg.id;
            Vector3 position = msg.position;

            if (behaviours.TryGetValue(id, out IHasPosition behaviour))
            {
                behaviour.SetPositionServer(position);
            }
        }


        private void ClientHandleNetworkPositionMessage(NetworkConnection conn, NetworkPositionMessage msg)
        {
            int count = msg.bytes.Count;
            using (PooledNetworkReader reader = NetworkReaderPool.GetReader(msg.bytes))
            {
                int i = 0;
                while (i < count)
                {
                    uint id = reader.ReadPackedUInt32();
                    Vector3 position = compressPosition
                        ? compression.Decompress(reader)
                        : reader.ReadVector3();

                    if (behaviours.TryGetValue(id, out IHasPosition behaviour))
                    {
                        behaviour.SetPositionClient(position);
                    }
                }
                Debug.Assert(i == count, "should have read exact amount");
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmo) { return; }
            Gizmos.color = gizmoColor;
            Bounds bounds = default;
            bounds.min = new Vector3(0, -20, 0);
            bounds.max = new Vector3(500, 40, 500);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
#endif
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

        void SetPositionServer(Vector3 pos);
        void SetPositionClient(Vector3 pos);
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
