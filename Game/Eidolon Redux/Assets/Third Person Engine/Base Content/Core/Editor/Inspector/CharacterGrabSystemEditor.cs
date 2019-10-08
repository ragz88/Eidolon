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
    [CustomEditor(typeof(CharacterGrabSystem))]
    public class CharacterGrabSystemEditor : RSEditor<CharacterGrabSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseSettings = new GUIContent("Base Settings");
            public readonly static GUIContent Camera = new GUIContent("Camera", "Character camera.");
            public readonly static GUIContent DetectionRange = new GUIContent("Detection Range", "Ray range from camera.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseSettings);
            instance.SetCharacterCamera(ObjectField<Transform>(ContentProperties.Camera, instance.GetCharacterCamera(), true));
            instance.SetDetectionRange(EditorGUILayout.FloatField(ContentProperties.DetectionRange, instance.GetDetectionRange()));
            EndGroup();
        }
    }
}