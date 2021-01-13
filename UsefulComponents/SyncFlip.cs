using Mirror;
using UnityEngine;

namespace JamesFrowen.Mirror.UsefulComponents 
{
    public class SyncFlip : NetworkBehaviour
    {
        public SpriteRenderer target;

        private void OnValidate()
        {
            if (this.target == null)
            {
                this.target = this.GetComponent<SpriteRenderer>();
            }
            if (this.target == null)
            {
                Debug.LogError("SyncFlip did not have a target, please set one");
            }
        }

        [SyncVar(hook = nameof(OnFlipX))]
        bool flipX;

        [SyncVar(hook = nameof(OnFlipY))]
        bool flipY;

        void OnFlipX(bool _oldValue, bool value)
        {
            // ignore syncvar if owner (this component is client authority)
            if (this.hasAuthority) { return; }

            this.target.flipX = value;
        }

        void OnFlipY(bool _oldValue, bool value)
        {
            // ignore syncvar if owner (this component is client authority)
            if (this.hasAuthority) { return; }

            this.target.flipY = value;
        }

        public void SetFlipX(bool value)
        {
            this.target.flipX = value;

            if (this.isServer)
            {
                this.flipX = value;
            }
            else if (this.hasAuthority)
            {
                this.CmdSetFlipX(value);
            }
        }

        public void SetFlipY(bool value)
        {
            this.target.flipY = value;

            if (this.isServer)
            {
                this.flipY = value;
            }
            else if (this.hasAuthority)
            {
                this.CmdSetFlipY(value);
            }
        }

        [Command]
        private void CmdSetFlipX(bool value)
        {
            // set syncvar on server
            this.flipX = value;
            this.target.flipX = value;
        }

        [Command]
        private void CmdSetFlipY(bool value)
        {
            // set syncvar on server
            this.flipY = value;
            this.target.flipY = value;
        }
    }
}