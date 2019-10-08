/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

namespace ThirdPersonEngine.Editor
{
    public interface IEditorDelay
    {
        /// <summary>
        /// Apply delay.
        /// </summary>
        bool WaitForSeconds();
        
        /// <summary>
        /// Apply new delay.
        /// </summary>
        bool WaitForSeconds(float delay);

        /// <summary>
        /// Return delay value.
        /// </summary>
        /// <returns></returns>
        float GetDelay();

        /// <summary>
        /// Set delay value.
        /// </summary>
        /// <param name="delay"></param>
        void SetDelay(float delay);
    }
}