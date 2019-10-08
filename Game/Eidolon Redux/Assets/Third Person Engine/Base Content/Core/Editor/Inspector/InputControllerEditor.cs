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
    [CustomEditor(typeof(InputController), true)]
    public sealed class InputControllerEditor : RSEditor<InputController>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Information = new GUIContent("Information", "Information about current input controller.");
        }

        private string inputControllerName;

        public override string GetHeaderName()
        {
            inputControllerName = instance.GetType().Name;
            inputControllerName = inputControllerName.AddSpaces();
            return inputControllerName;
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Information);
            RSEditorHelpBoxMessages.Message(string.Format("{0} {1}.", "The current character processed by", inputControllerName));
            EndGroup();
        }
    }
}