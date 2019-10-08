/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
	/// <summary>
	/// Interface describing the architecture of the health
	/// </summary>
	public interface IHealth
	{
		/// <summary>
		/// Get health point.
		/// </summary>
		int GetHealth();

		/// <summary>
		/// Set health point.
		/// </summary>
		void SetHealth(int value);

		/// <summary>
		/// Get max health point.
		/// </summary>
		/// <returns>MaxHealth</returns>
		int GetMaxHealth();

		/// <summary>
		/// Set max health point.
		/// </summary>
		/// <returns>MaxHealth</returns>
		void SetMaxHealth(int value);

		/// <summary>
		/// Health state.
		/// </summary>
		/// <returns>IsAlive</returns>
		bool IsAlive();

		/// <summary>
		/// Take damage.
		/// </summary>
		/// <param name="damage"></param>
		void TakeDamage(int damage);
	}
}