/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public class HipsDamageHandler : MonoBehaviour
    {
        private CharacterHealth characterHealth;
        private Rigidbody hipsRigidbody;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            hipsRigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Initialize HipRigidbodyHandler component.
        /// </summary>
        public virtual void Initialize(CharacterHealth characterHealth)
        {
            this.characterHealth = characterHealth;
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (hipsRigidbody != null && !hipsRigidbody.isKinematic)
            {
                characterHealth.VelocityDamageProcessing(other.relativeVelocity);
            }
        }
    }
}