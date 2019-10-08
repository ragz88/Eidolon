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
using UnityEditorInternal;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    [CustomEditor(typeof(AdaptiveRagdollSystem))]
    public sealed class AdaptiveRagdollSystemEditor : RSEditor<AdaptiveRagdollSystem>
    {
        internal new static class ContentProperties
        {
            // Ragdoll properties contents
            public readonly static GUIContent RagdollProperties = new GUIContent("Ragdoll Properties");
            public readonly static GUIContent RelativeVelocity = new GUIContent("Relative Velocity Limit", "Limits of the relative linear velocity of the two colliding objects for the character to start ragdoll system.");
            public readonly static GUIContent StandDelay = new GUIContent("Stand Delay", "Delay before character stand. after ragdoll is played and wait in stable position.");
            public readonly static GUIContent ComponentsToDisable = new GUIContent("Components To Disable", "Components array which will disable while ragdoll is active.");

            // Animation properties contents
            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent BellyStateName = new GUIContent("Belly State", "Animator state name to get up from belly.");
            public readonly static GUIContent BackStateName = new GUIContent("Back State", "Animator state name to get up from back.");
            public readonly static GUIContent BellyTime = new GUIContent("Belly Time", "Animation time.");
            public readonly static GUIContent BackTime = new GUIContent("Back Time", "Animation time.");

            public readonly static GUIContent Time = new GUIContent("Time", "Animation time (lenght).");
            public readonly static GUIContent Path = new GUIContent("Path", "Animation path in animator controller.");

            //Camera properties contents
            public readonly static GUIContent CameraProperties = new GUIContent("Camera Properties");
            public readonly static GUIContent CameraSystem = new GUIContent("Camera System", "Camera system instance.");
            public readonly static GUIContent Target = new GUIContent("Target", "Target while ragdoll is active.");
        }

        private bool componentsToDisableFoldout;
        private ReorderableList rl_componentsToDisable;

        public override void OnInitializeProperties()
        {
            SerializedProperty sp_componentsToDisable = serializedObject.FindProperty("componentsToDisable");
            rl_componentsToDisable = new ReorderableList(serializedObject, sp_componentsToDisable, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, "Components List");
                    },
                    drawElementCallback = (rect, index, isFocused, isActive) =>
                    {
                        SerializedProperty sp_Component = sp_componentsToDisable.GetArrayElementAtIndex(index);
                        string name = string.Format("Component {0}", index + 1);
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + 1.5f, 75, EditorGUIUtility.singleLineHeight), name);
                        EditorGUI.PropertyField(new Rect(rect.x + 85, rect.y + 1.5f, rect.width - 85, EditorGUIUtility.singleLineHeight), sp_Component, GUIContent.none);
                    }
            };
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.RagdollProperties);
            instance.SetRelativeVelocityLimit(EditorGUILayout.FloatField(ContentProperties.RelativeVelocity, instance.GetRelativeVelocitLimit()));
            instance.SetStandDelay(EditorGUILayout.FloatField(ContentProperties.StandDelay, instance.GetStandDelay()));
            IncreaseIndentLevel();
            BeginFoldoutGroup(ref componentsToDisableFoldout, ContentProperties.ComponentsToDisable);
            if (componentsToDisableFoldout)
            {
                DecreaseIndentLevel();
                rl_componentsToDisable.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();
            EndGroup();

            BeginGroup(ContentProperties.CameraProperties);
            instance.SetCameraSystem(ObjectField<TPCamera>(ContentProperties.CameraSystem, instance.GetCameraSystem(), true));
            instance.SetCameraTarget(ObjectField<Transform>(ContentProperties.Target, instance.GetCameraTarget(), true));
            EndGroup();

            BeginGroup(ContentProperties.AnimationProperties);
            instance.SetAnimationGetUpFromBelly(EditorGUILayout.TextField(ContentProperties.BellyStateName, instance.GetAnimationGetUpFromBelly()));
            instance.SetBellyStandTime(EditorGUILayout.FloatField(ContentProperties.BellyTime, instance.GetBellyStandTime()));
            instance.SetAnimationGetUpFromBack(EditorGUILayout.TextField(ContentProperties.BackStateName, instance.GetAnimationGetUpFromBack()));
            instance.SetBackStandTime(EditorGUILayout.FloatField(ContentProperties.BackTime, instance.GetBackStandTime()));
            EndGroup();
        }
    }
}