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
    [CustomEditor(typeof(TPCamera))]
    public sealed class TPCameraEditor : RSEditor<TPCamera>
    {
        internal new static class ContentProperties
        {
            // Base properties
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Controller = new GUIContent("Target Controller", "The controller followed by the camera.");
            public readonly static GUIContent SmoothCameraRotation = new GUIContent("Rotation Smooth", "Camera rotation smooth value.");
            public readonly static GUIContent CullingLayer = new GUIContent("Culling Layer", "Camera culling layer for detect obstacle.");
            public readonly static GUIContent CullingSmooth = new GUIContent("Culling Smooth", "Culling handle smooth.");
            public readonly static GUIContent RightOffset = new GUIContent("Right Offset", "Camera right offset.");
            public readonly static GUIContent DefaultDistance = new GUIContent("Default Distance", "Default camera distance.");
            public readonly static GUIContent DistanceLimit = new GUIContent("Distance Limit", "Limits camera distance.");
            public readonly static GUIContent MinDistance = new GUIContent("Min Distance");
            public readonly static GUIContent MaxDistance = new GUIContent("Max Distance");
            public readonly static GUIContent Height = new GUIContent("Height", "Camera height (Y axis).");
            public readonly static GUIContent FollowSpeed = new GUIContent("Follow Speed", "The speed of the camera following the controller.");
            public readonly static GUIContent SensitivityByX = new GUIContent("Sensitivity X", "Sensitivity by X axis.");
            public readonly static GUIContent SensitivityByY = new GUIContent("Sensitivity Y", "Sensitivity by Y axis.");
            public readonly static GUIContent LimitByY = new GUIContent("Limit Y", "Limits by Y axis.");

            // Scrolling properties
            public readonly static GUIContent ScrollingProperties = new GUIContent("Scrolling Properties");
            public readonly static GUIContent UseScrolling = new GUIContent("Use Scrolling");
            public readonly static GUIContent ScrollSensitivity = new GUIContent("Sensitivity", "Scroll sensitivity.");

            // Field of view system properties
            public readonly static GUIContent FOVSystem = new GUIContent("Field Of View System");
            public readonly static GUIContent IncreaseValue = new GUIContent("Increase Value", "Field of view value when character is sprint.");
            public readonly static GUIContent DefaultValue = new GUIContent("Default Value", "Default field of view value.");
            public readonly static GUIContent IncreaseSpeed = new GUIContent("Increase Speed", "Field of view increase speed.");
            public readonly static GUIContent DecreaseSpeed = new GUIContent("Decrease Speed", "Return on default field of view value speed.");
        }

        private float maxDistance = 1;
        private float minDistance = -1;
        private bool editDistanceLimit = false;

        public override string GetHeaderName()
        {
            return "Third Person Camera";
        }

        public override void OnInitializeProperties()
        {
            maxDistance = GetMaxDistance();
            minDistance = GetMinDistance();

            if (instance.GetTargetController() == null)
            {
                TPController controller = GameObject.FindObjectOfType<TPController>();
                if (controller != null)
                {
                    instance.SetTargetController(controller);
                }
            }
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetTargetController(ObjectField<TPController>(ContentProperties.Controller, instance.GetTargetController(), true));
            instance.SetSmoothCameraRotation(EditorGUILayout.FloatField(ContentProperties.SmoothCameraRotation, instance.GetSmoothCameraRotation()));
            LayerMask cullingLayer = EditorGUILayout.MaskField(ContentProperties.CullingLayer, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(instance.GetCullingLayer()), InternalEditorUtility.layers);
            instance.SetCullingLayer(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(cullingLayer));
            instance.SetCullingSmooth(EditorGUILayout.FloatField(ContentProperties.CullingSmooth, instance.GetCullingSmooth()));
            instance.SetRightOffset(EditorGUILayout.FloatField(ContentProperties.RightOffset, instance.GetRightOffset()));
            instance.SetDefaultDistance(EditorGUILayout.FloatField(ContentProperties.DefaultDistance, instance.GetDefaultDistance()));
            DistanceLimitLayout();
            instance.SetHeight(EditorGUILayout.FloatField(ContentProperties.Height, instance.GetHeight()));
            instance.SetFollowSpeed(EditorGUILayout.FloatField(ContentProperties.FollowSpeed, instance.GetFollowSpeed()));
            instance.SetMouseSensitivityX(EditorGUILayout.FloatField(ContentProperties.SensitivityByX, instance.GetMouseSensitivityX()));
            instance.SetMouseSensitivityY(EditorGUILayout.FloatField(ContentProperties.SensitivityByY, instance.GetMouseSensitivityY()));
            YLimitLayout();
            EndGroup();

            BeginGroup(ContentProperties.ScrollingProperties);
            instance.SetScrollSensitivity(EditorGUILayout.FloatField(ContentProperties.ScrollSensitivity, instance.GetScrollSensitivity()));
            instance.UseScroll(EditorGUILayout.Toggle(ContentProperties.UseScrolling, instance.UseScroll()));
            EndGroup();

            BeginGroup(ContentProperties.FOVSystem);
            instance.GetCameraFOVSystem().SetIncreaseValue(EditorGUILayout.Slider(ContentProperties.IncreaseValue, instance.GetCameraFOVSystem().GetIncreaseValue(), 0.0f, 179.0f));
            instance.GetCameraFOVSystem().SetDefaultValue(EditorGUILayout.Slider(ContentProperties.DefaultValue, instance.GetCameraFOVSystem().GetDefaultValue(), 0.0f, 179.0f));
            instance.GetCameraFOVSystem().SetIncreaseSpeed(EditorGUILayout.FloatField(ContentProperties.IncreaseSpeed, instance.GetCameraFOVSystem().GetIncreaseSpeed()));
            instance.GetCameraFOVSystem().SetDecreaseSpeed(EditorGUILayout.FloatField(ContentProperties.DecreaseSpeed, instance.GetCameraFOVSystem().GetDecreaseSpeed()));
            EndGroup();

        }

        private void YLimitLayout()
        {
            float min = instance.GetMinLimitY();
            float max = instance.GetMaxLimitY();
            MinMaxSlider(ContentProperties.LimitByY, ref min, ref max, -360, 360);
            instance.SetMinLimitY(min);
            instance.SetMaxLimitY(max);
        }

        private void DistanceLimitLayout()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ContentProperties.DistanceLimit, EditorStyles.label))
            {
                editDistanceLimit = !editDistanceLimit;
            }
            GUILayout.Space(EditorGUIUtility.labelWidth - 80);
            float min = instance.GetMinDistance();
            float max = instance.GetMaxDistance();
            min = EditorGUILayout.FloatField(min, GUILayout.Width(33));
            EditorGUILayout.MinMaxSlider(ref min, ref max, minDistance, maxDistance);
            max = EditorGUILayout.FloatField(max, GUILayout.Width(33));
            instance.SetMinDistance(min);
            instance.SetMaxDistance(max);
            GUILayout.EndHorizontal();
            if (editDistanceLimit)
            {
                minDistance = EditorGUILayout.FloatField(ContentProperties.MinDistance, minDistance);
                maxDistance = EditorGUILayout.FloatField(ContentProperties.MaxDistance, maxDistance);
                if (RSEditor.MiniButton("Apply", "Right", GUILayout.Width(60)))
                {
                    editDistanceLimit = false;
                }
                GUILayout.Space(3);
            }
        }

        private float GetMaxDistance()
        {
            if (instance.GetMaxDistance() == 0)
                return 1;
            return Mathf.Ceil(instance.GetMaxDistance());
        }

        private float GetMinDistance()
        {
            if (instance.GetMinDistance() == 0)
                return -1;
            return Mathf.Floor(instance.GetMinDistance());
        }
    }
}