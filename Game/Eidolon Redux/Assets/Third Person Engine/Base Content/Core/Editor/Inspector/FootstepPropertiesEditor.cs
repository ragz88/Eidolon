/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using ThirdPersonEngine.Runtime;
using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    [CustomEditor(typeof(FootstepProperties))]
    [CanEditMultipleObjects]
    public class FootstepPropertiesEditor : RSEditor<FootstepProperties>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Properties = new GUIContent("Properties");
            public readonly static GUIContent PhysicsMaterial = new GUIContent("Physics Material", "Physics Material of surface.");
            public readonly static GUIContent Texture = new GUIContent("Texture", "Texture of surface.");
            public readonly static GUIContent JumpSounds = new GUIContent("Jump Sounds", "Jump sounds array.");
            public readonly static GUIContent StepSounds = new GUIContent("Step Sounds", "Step sounds array.");
            public readonly static GUIContent LandSounds = new GUIContent("Land Sounds", "Land sounds array.");
        }

        private SerializedProperty sp_properties;
        private List<bool> propertiesFoldouts;

        public override void OnInitializeProperties()
        {
            sp_properties = serializedObject.FindProperty("properties");
            propertiesFoldouts = CreatePropertiesFoldouts();
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Properties);
            IncreaseIndentLevel();
            for (int i = 0, length = sp_properties.arraySize; i < length; i++)
            {
                bool foldout = propertiesFoldouts[i];
                SerializedProperty sp_footstepProperty = sp_properties.GetArrayElementAtIndex(i);
                SerializedProperty sp_physicsMaterial = sp_footstepProperty.FindPropertyRelative("physicMaterial");
                SerializedProperty sp_texture = sp_footstepProperty.FindPropertyRelative("texture");
                SerializedProperty sp_stepSounds = sp_footstepProperty.FindPropertyRelative("stepSounds");
                SerializedProperty sp_jumpSounds = sp_footstepProperty.FindPropertyRelative("jumpSounds");
                SerializedProperty sp_landSounds = sp_footstepProperty.FindPropertyRelative("landSounds");
                BeginFoldoutGroup(ref foldout, string.Format("Property {0}", i + 1));
                if (foldout)
                {
                    EditorGUILayout.PropertyField(sp_physicsMaterial, ContentProperties.PhysicsMaterial, true);
                    EditorGUILayout.PropertyField(sp_texture, ContentProperties.Texture, true);
                    EditorGUILayout.PropertyField(sp_stepSounds, ContentProperties.StepSounds, true);
                    EditorGUILayout.PropertyField(sp_jumpSounds, ContentProperties.JumpSounds, true);
                    EditorGUILayout.PropertyField(sp_landSounds, ContentProperties.LandSounds, true);
                    if (RSEditor.Button("Remove", "Right", GUILayout.Width(70)))
                    {
                        sp_properties.DeleteArrayElementAtIndex(i);
                        propertiesFoldouts.RemoveAt(i);
                        break;
                    }
                }
                EndFoldoutGroup();
                propertiesFoldouts[i] = foldout;
            }
            DecreaseIndentLevel();

            GUILayout.Space(sp_properties.arraySize > 0 ? 5 : 0);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Property", GUI.skin.GetStyle("ButtonLeft"), GUILayout.Width(150)))
            {
                sp_properties.arraySize++;
                propertiesFoldouts.Add(false);
            }
            if (GUILayout.Button("Remove All Properties", GUI.skin.GetStyle("ButtonRight"), GUILayout.Width(150)))
            {
                sp_properties.ClearArray();
                propertiesFoldouts.Clear();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EndGroup();
        }

        private List<bool> CreatePropertiesFoldouts()
        {
            List<bool> list = new List<bool>();
            for (int i = 0, length = sp_properties.arraySize; i < length; i++)
            {
                list.Add(false);
            }
            return list;
        }
    }
}