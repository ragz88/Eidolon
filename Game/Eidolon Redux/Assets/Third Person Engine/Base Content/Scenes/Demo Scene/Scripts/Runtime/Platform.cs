/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(Collider))]
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Vector3 basePosition = Vector3.zero;
        [SerializeField] private Vector3 targetPosition = Vector3.zero;
        [SerializeField] private float speed = 5.0f;

        private Vector3 currentTargetPosition;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            currentTargetPosition = basePosition;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, speed * Time.deltaTime);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TNC.PLAYER))
            {
                currentTargetPosition = targetPosition;
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TNC.PLAYER))
            {
                currentTargetPosition = basePosition;
            }
        }

        public Vector3 GetBasePosition()
        {
            return basePosition;
        }

        public void SetBasePosition(Vector3 value)
        {
            basePosition = value;
        }

        public Vector3 GetTargetPosition()
        {
            return targetPosition;
        }

        public void SetTargetPosition(Vector3 value)
        {
            targetPosition = value;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }

        public Vector3 GetCurrentTargetPosition()
        {
            return currentTargetPosition;
        }
    }
}