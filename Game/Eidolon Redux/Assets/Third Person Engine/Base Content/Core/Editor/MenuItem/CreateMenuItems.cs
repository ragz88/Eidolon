/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using ThirdPersonEngine.Runtime;
using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
	public class UCreateMenuItems
	{
		[MenuItem(RSEditorPaths.MENUITEM_DEFAULT_PATH + "Create/Respawn Zone", false, 21)]
		public static void CreateRespawnZone()
		{
			GameObject respawnZone = new GameObject("Respawn Zone");
			respawnZone.isStatic = true;
			
			RespawnManager respawnManager = respawnZone.AddComponent<RespawnManager>();
			Transform player = RSEditorInternal.FindPlayer();
			if (player != null)
			{
				respawnManager.SetCharacter(player);
			}
		}

		[MenuItem(RSEditorPaths.MENUITEM_DEFAULT_PATH + "Create/Event Trigger", false, 22)]
		public static void CreateEventTrigger()
		{
			GameObject eventTrigger = new GameObject("Event Trigger");
			eventTrigger.isStatic = true;
			
			EventTrigger respawnManager = eventTrigger.AddComponent<EventTrigger>();
			BoxCollider boxCollider = eventTrigger.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}
	}
}