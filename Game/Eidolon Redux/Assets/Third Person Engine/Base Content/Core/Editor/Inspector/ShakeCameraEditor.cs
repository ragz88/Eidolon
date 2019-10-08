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
    [CustomEditor(typeof(ShakeCamera))]
    [CanEditMultipleObjects]
    public class ShakeCameraEditor : RSEditor<ShakeCamera>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Information = new GUIContent("Information", "Information about current component.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Information);
            RSEditorHelpBoxMessages.Message("Read-only component.");
            EndGroup();
        }
    }
}