/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Collections;
using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [System.Serializable]
    public class CameraFOVSystem : ICameraFOV
    {

        [SerializeField] private float increaseValue = 70.0f;
        [SerializeField] private float defaultValue = 60.0f;
        [SerializeField] private float increaseSpeed = 5.0f;
        [SerializeField] private float decreaseSpeed = 5.0f;

        private Camera camera;
        private IController controller;
        private ICoroutineCallbacks coroutineCallbacks;
        private IEnumerator processing;
        private bool state;

        /// <summary>
        /// Initialize Camera field or view system.
        /// </summary>
        public virtual void Initialize(Camera camera, IController controller, ICoroutineCallbacks coroutineCallbacks)
        {
            this.camera = camera;
            this.controller = controller;
            this.coroutineCallbacks = coroutineCallbacks;
        }

        public virtual void Update()
        {
            if (controller.IsSprinting() && state)
                Start();
            else if (!controller.IsSprinting() && !state)
                Stop();
        }

        /// <summary>
        /// Start field of view system.
        /// </summary>
        public virtual void Start()
        {
            if (processing != null)
            {
                coroutineCallbacks.Stop(processing);
            }
            processing = Processing(increaseValue, increaseSpeed);
            coroutineCallbacks.Start(processing);
            state = false;
        }

        /// <summary>
        /// Stop field of view system.
        /// </summary>
        public virtual void Stop()
        {
            if (processing != null)
            {
                coroutineCallbacks.Stop(processing);
            }
            processing = Processing(defaultValue, decreaseSpeed);
            coroutineCallbacks.Start(processing);
            state = true;
        }

        /// <summary>
        /// Field of view system processing.
        /// </summary>
        protected virtual IEnumerator Processing(float targetFOV, float speed)
        {
            while (true)
            {
                camera.fieldOfView = Mathf.SmoothStep(camera.fieldOfView, targetFOV, speed * Time.deltaTime);
                if (TPEMathf.Approximately(camera.fieldOfView, targetFOV, 0.03f))
                {
                    processing = null;
                    break;
                }
                yield return null;
            }
        }

        public float GetIncreaseValue()
        {
            return increaseValue;
        }

        public void SetIncreaseValue(float value)
        {
            increaseValue = value;
        }

        public float GetDefaultValue()
        {
            return defaultValue;
        }

        public void SetDefaultValue(float value)
        {
            defaultValue = value;
        }

        public float GetIncreaseSpeed()
        {
            return increaseSpeed;
        }

        public void SetIncreaseSpeed(float value)
        {
            increaseSpeed = value;
        }

        public float GetDecreaseSpeed()
        {
            return decreaseSpeed;
        }

        public void SetDecreaseSpeed(float value)
        {
            decreaseSpeed = value;
        }

        public IController GetController()
        {
            return controller;
        }

        public void SetController(IController value)
        {
            controller = value;
        }

        public ICoroutineCallbacks GetCoroutineCallbacks()
        {
            return coroutineCallbacks;
        }

        public void SetCoroutineCallbacks(ICoroutineCallbacks value)
        {
            coroutineCallbacks = value;
        }
    }
}