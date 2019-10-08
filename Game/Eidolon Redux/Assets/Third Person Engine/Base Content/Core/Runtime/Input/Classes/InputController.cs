/* ====================================================================
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
	/// <summary>
	/// Input Controller interface, base interface for all controllers used in ThirdPersonEngine.Runtime.
	/// Contains main functions for implementation.
	/// </summary>
	public abstract class InputController : Singleton<InputController>
	{
		/// <summary>
		/// Returns the value of the virtual axis identified by axisName.
		/// The value will be in the range -1...1 for keyboard and joystick input.
		/// </summary>
		/// <param name="axisName">Axis identified</param>
		/// <returns>Axis float value.</returns>
		public abstract float GetAxis(string axisName);

		/// <summary>
		/// Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
		/// The value will be in the range -1...1 for keyboard and joystick input. 
		/// Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
		/// </summary>
		/// /// <param name="axisName">Axis identified</param>
		/// <returns>Axis float value.</returns>
		public abstract float GetAxisRaw(string axisName);

		/// <summary>
		/// Returns true during the frame the user pressed down the virtual button identified by buttonName.
		/// </summary>
		/// <param name="butoonName">Button identified</param>
		/// <returns>bool True when an axis has been pressed and not released.</returns>
		public abstract bool GetButtonDown(string butoonName);

		/// <summary>
		/// Returns true while the virtual button identified by buttonName is held down.
		/// </summary>
		/// <param name="buttonName">Button identified</param>
		/// <returns>bool True when an axis has been held down and not released.</returns>
		public abstract bool GetButton(string buttonName);

		/// <summary>
		/// Returns true the first frame the user releases the virtual button identified by buttonName.
		/// </summary>
		/// <param name="buttonName">Button identified</param>
		/// <returns>bool True when an axis has been released</returns>
		public abstract bool GetButtonUp(string buttonName);
	}
}