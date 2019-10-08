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
    [CustomEditor(typeof(EventTrigger))]
    [CanEditMultipleObjects]
    public class EventTriggerEditor : RSEditor<EventTrigger>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties", "Trigget target.");
            public readonly static GUIContent TargetLayers = new GUIContent("Target", "Target layers.");
            public readonly static GUIContent DestroyTrigger = new GUIContent("Destroy Trigger", "Destroy trigger state.");
            public readonly static GUIContent StayTime = new GUIContent("Stay Time", "Destroy trigger after stay specific time in trigger");
            public readonly static GUIContent Events = new GUIContent("Events");
        }

        private SerializedProperty onTriggerEnter;
        private SerializedProperty onTriggerStay;
        private SerializedProperty onTriggerExit;
        private bool eventsFoldouts;

        public override string GetHeaderName()
        {
            return "Event Trigger";
        }


        public override void OnInitializeProperties()
        {
            onTriggerEnter = serializedObject.FindProperty("onTriggerEnter");
            onTriggerStay = serializedObject.FindProperty("onTriggerStay");
            onTriggerExit = serializedObject.FindProperty("onTriggerExit");

        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            LayerMask targetLLayerMask = EditorGUILayout.MaskField(ContentProperties.TargetLayers, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(instance.GetTargetLayers()), InternalEditorUtility.layers);
            instance.SetTargetLayers(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(targetLLayerMask));
            instance.SetDestroyTrigger((EventTrigger.DestroyTrigger) EditorGUILayout.EnumPopup(ContentProperties.DestroyTrigger, instance.GetDestroyTrigger()));
            if (instance.DestroyTriggerCompare(EventTrigger.DestroyTrigger.AfterStaySpecificTime))
            {
                instance.SetStayTime(EditorGUILayout.FloatField(ContentProperties.StayTime, instance.GetStayTime()));
            }
            IncreaseIndentLevel();
            BeginFoldoutGroup(ref eventsFoldouts, ContentProperties.Events);
            if (eventsFoldouts)
            {
                if (onTriggerEnter != null)
                {
                    EditorGUI.BeginDisabledGroup(instance.DestroyTriggerCompare(EventTrigger.DestroyTrigger.AfterExit));
                    EditorGUILayout.PropertyField(onTriggerEnter);
                    EditorGUI.EndDisabledGroup();
                }
                if (onTriggerStay != null)
                {
                    EditorGUI.BeginDisabledGroup(instance.DestroyTriggerCompare(EventTrigger.DestroyTrigger.AfterEnter));
                    EditorGUILayout.PropertyField(onTriggerStay);
                    EditorGUI.EndDisabledGroup();

                }
                if (onTriggerExit != null)
                {
                    EditorGUI.BeginDisabledGroup(instance.DestroyTriggerCompare(EventTrigger.DestroyTrigger.AfterEnter));
                    EditorGUILayout.PropertyField(onTriggerExit);
                    EditorGUI.EndDisabledGroup();
                }
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();
            EndGroup();
        }
    }
}