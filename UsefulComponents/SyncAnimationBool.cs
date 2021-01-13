using Mirror;
using UnityEngine;

namespace JamesFrowen.Mirror.UsefulComponents 
{
    public class SyncAnimationBool : NetworkBehaviour
    {
        [SerializeField] Animator target;
        [SerializeField] string parameterName;

        int parameterHash;

        private void OnValidate()
        {
            if (this.target == null)
            {
                this.target = this.GetComponent<Animator>();
            }
            if (this.target == null)
            {
                Debug.LogError("SyncFlip did not have a target, please set one");
            }
        }

        private void Awake()
        {
            this.parameterHash = Animator.StringToHash(this.parameterName);
        }

        [SyncVar(hook = nameof(OnChange))]
        bool value;

        void OnChange(bool _oldValue, bool newValue)
        {
            this.target.SetBool(this.parameterHash, newValue);
        }

        [Server]
        public void SetValue(bool newValue)
        {
            // do nothing if null
            if (this.target == null || this.target.runtimeAnimatorController == null) { return; }

            this.value = newValue;
            this.target.SetBool(this.parameterHash, newValue);
        }
    }
}