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
	public interface IRagdoll
	{
		/// <summary>
		/// Check if you hit character. Works as Physics.Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
		/// </summary>
		/// <returns>True if hit occured</returns>
		bool Raycast(Ray ray, out RaycastHit hit, float distance);

		/// <summary>
		/// Adding extra move
		/// </summary>
		/// <param name="move">direction and magnitude</param>
		void AddExtraMove(Vector3 move);

        /// <summary>
        /// If character is ragdolled, the value is True. Otherwise False
        /// </summary>
        bool IsRagdolled();

        /// <summary>
        /// If character is ragdolled, the value is True. Otherwise False
        /// </summary>
        void IsRagdolled(bool value);
        
        /// <summary>
        /// Get ragdoll velocity.
        /// </summary>
        Vector3 GetVelocity();
        
        /// <summary>
        /// Character hips transform.
        /// </summary>
        Transform GetHipsTransform();
    }
}

