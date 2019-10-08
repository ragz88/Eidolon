/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonEngine.Runtime
{
    public class EventTrigger : MonoBehaviour
    {
        public enum DestroyTrigger
        {
            None,
            AfterEnter,
            AfterStaySpecificTime,
            AfterExit,
        }

        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private DestroyTrigger destroyTrigger;
        [SerializeField] private float stayTime;
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerStay;
        [SerializeField] private UnityEvent onTriggerExit;

        public float storedTime;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!TargetLayerCondition(other))
            {
                return;
            }
            storedTime = Time.time;
            onTriggerEnter.Invoke();
            if (DestroyTriggerCompare(DestroyTrigger.AfterEnter))
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerStay(Collider other)
        {
            if (!TargetLayerCondition(other))
            {
                return;
            }
            onTriggerStay.Invoke();
            if (DestroyTriggerCompare(DestroyTrigger.AfterStaySpecificTime) && Timeout())
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        protected virtual void OnTriggerExit(Collider other)
        {
            if (!TargetLayerCondition(other))
            {
                return;
            }
            storedTime = -1;
            onTriggerExit.Invoke();
            if (DestroyTriggerCompare(DestroyTrigger.AfterExit))
            {
                Destroy(gameObject);
            }
        }

        private bool TargetLayerCondition(Collider other)
        {
            return targetLayers.value == 1 << other.gameObject.layer;
        }

        public bool Timeout()
        {
            return storedTime > -1 && Time.time - storedTime >= stayTime;
        }

        public bool DestroyTriggerCompare(DestroyTrigger destroyTrigger)
        {
            return this.destroyTrigger == destroyTrigger;
        }

        public LayerMask GetTargetLayers()
        {
            return targetLayers;
        }

        public void SetTargetLayers(LayerMask value)
        {
            targetLayers = value;
        }

        public DestroyTrigger GetDestroyTrigger()
        {
            return destroyTrigger;
        }

        public void SetDestroyTrigger(DestroyTrigger value)
        {
            destroyTrigger = value;
        }

        public float GetStayTime()
        {
            return stayTime;
        }

        public void SetStayTime(float value)
        {
            stayTime = value >= 0 ? value : 0;
        }

        public UnityEvent GetOnTriggerEnter()
        {
            return onTriggerEnter;
        }

        public void SetOnTriggerEnter(UnityEvent value)
        {
            onTriggerEnter = value;
        }

        public UnityEvent GetOnTriggerStay()
        {
            return onTriggerStay;
        }

        public void SetOnTriggerStay(UnityEvent value)
        {
            onTriggerStay = value;
        }

        public UnityEvent GetOnTriggerExit()
        {
            return onTriggerExit;
        }

        public void SetOnTriggerExit(UnityEvent value)
        {
            onTriggerExit = value;
        }
    }
}