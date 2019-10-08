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
    public interface ICameraFOV
    {
        /// <summary>
        /// Initialize Camera field or view system.
        /// </summary>
        void Initialize(Camera camera, IController controller, ICoroutineCallbacks coroutineCallbacks);

        /// <summary>
        /// Start field of view system.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop field of view system.
        /// </summary>
        void Stop();
    }
}