﻿/* ====================================================================
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
	/// <summary>
	/// Base StandaloneController Controller in Third Person Engine, which used default Input manager.
	/// </summary>
	public class StandaloneController : InputController
	{
		/// <summary>
		/// Returns the value of the virtual axis identified by axisName.
		/// The value will be in the range -1...1 for keyboard and joystick input.
		/// </summary>
		/// <param name="axisName">Axis identified</param>
		/// <returns>float</returns>
		public override float GetAxis(string axisName)
		{
			return Input.GetAxis(axisName);
		}

		/// <summary>
		/// Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
		/// The value will be in the range -1...1 for keyboard and joystick input. 
		/// Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
		/// </summary>
		/// <param name="axisName">Axis identified</param>
		/// <returns>float</returns>
		public override float GetAxisRaw(string axisName)
		{
			return Input.GetAxisRaw(axisName);
		}

		/// <summary>
		/// Returns true while the virtual button identified by buttonName is held down.
		/// </summary>
		/// <param name="buttonName">Button identified</param>
		/// <returns>bool True when an axis has been held down and not released.</returns>
		public override bool GetButtonDown(string butoonName)
		{
			return Input.GetButtonDown(butoonName);
		}

		/// <summary>
		/// Returns true during the frame the user pressed down the virtual button identified by buttonName.
		/// </summary>
		/// <param name="butoonName">Button identified</param>
		/// <returns>bool True when an axis has been pressed and not released.</returns>
		public override bool GetButton(string buttonName)
		{
			return Input.GetButton(buttonName);
		}

		/// <summary>
		/// Returns true the first frame the user releases the virtual button identified by buttonName.
		/// </summary>
		/// <param name="buttonName">Button identified</param>
		/// <returns>bool True when an axis has been released</returns>
		public override bool GetButtonUp(string buttonName)
		{
			return Input.GetButtonUp(buttonName);
		}

		/// <summary>
		/// Cursor processing coroutine.
		/// </summary>
		protected virtual System.Collections.IEnumerator Start()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			while (true)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					Cursor.lockState = Cursor.visible ? CursorLockMode.Locked : CursorLockMode.None;
					Cursor.visible = !Cursor.visible;
				}
				yield return null;
			}
		}
	}
}