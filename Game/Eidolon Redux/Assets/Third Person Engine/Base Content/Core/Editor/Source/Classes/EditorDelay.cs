/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public sealed class EditorDelay : IEditorDelay
    {
        private float delay;
        private float savedTime;

        /// <summary>
        /// EditorDelay constructor.
        /// </summary>
        public EditorDelay()
        {
            this.delay = 0.0f;
            savedTime = -1.0f;
        }

        /// <summary>
        /// EditorDelay constructor.
        /// </summary>
        /// <param name="delay"></param>
        public EditorDelay(float delay)
        {
            this.delay = delay;
            savedTime = -1.0f;
        }

        /// <summary>
        /// Apply delay.
        /// </summary>
        public bool WaitForSeconds()
        {
            if (savedTime == -1.0f)
            {
                savedTime = Time.realtimeSinceStartup;
            }
            else if (Time.realtimeSinceStartup - savedTime >= delay)
            {
                savedTime = -1.0f;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Apply new delay.
        /// </summary>
        /// <param name="delay"></param>
        public bool WaitForSeconds(float delay)
        {
            if (savedTime == -1.0f)
            {
                savedTime = Time.realtimeSinceStartup;
            }
            else if (Time.realtimeSinceStartup - savedTime >= delay)
            {
                savedTime = -1.0f;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return delay value.
        /// </summary>
        /// <returns></returns>
        public float GetDelay()
        {
            return delay;
        }

        /// <summary>
        /// Set delay value.
        /// </summary>
        /// <param name="delay"></param>
        public void SetDelay(float delay)
        {
            this.delay = delay;
        }
    }
}