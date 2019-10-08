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
    [CustomEditor(typeof(Platform))]
    [CanEditMultipleObjects]
    public class PlatformEditor : RSEditor<Platform>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent BasePosition = new GUIContent("Base Position", "Base platform position.");
            public readonly static GUIContent TargetPosition = new GUIContent("Target Position", "Target platform position.");
            public readonly static GUIContent Speed = new GUIContent("Speed", "Change platform position speed.");
        }

        public override void OnInitializeProperties()
        {
            if(instance.GetBasePosition() == Vector3.zero)
            {
                instance.SetBasePosition(instance.transform.position);
            }

            if(instance.GetTargetPosition() == Vector3.zero)
            {
                instance.SetTargetPosition(instance.transform.position + (Vector3.up * 2));
            }
        }

        public virtual void OnSceneGUI()
        {
            const float CUBE_SIZE = 0.1f;

            Vector3 basePosition = instance.GetBasePosition();
            Vector3 targetPosition = instance.GetTargetPosition();
            Vector3 currentTargetPosition = instance.GetCurrentTargetPosition();

            Color staticCubeColor = new Color(0.06f, 0.5f, 0.9f, 0.75f);
            Color dynamicCubeColor = new Color(0.9f, 0.06f, 0.06f, 0.75f);
            Color lineColor = Color.white;
            
            Handles.color = staticCubeColor;
            Handles.CubeHandleCap(0, basePosition, Quaternion.identity, CUBE_SIZE, EventType.Repaint);
            Handles.CubeHandleCap(0, targetPosition, Quaternion.identity, CUBE_SIZE, EventType.Repaint);

            basePosition = Handles.PositionHandle(basePosition, Quaternion.identity);
            targetPosition = Handles.PositionHandle(targetPosition, Quaternion.identity);

            instance.SetBasePosition(basePosition);
            instance.SetTargetPosition(targetPosition);

            Handles.color = dynamicCubeColor;
            Handles.CubeHandleCap(0, currentTargetPosition, Quaternion.identity, CUBE_SIZE, EventType.Repaint);

            Handles.color = lineColor;
            Handles.DrawLine(basePosition, targetPosition);
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetBasePosition(EditorGUILayout.Vector3Field(ContentProperties.BasePosition, instance.GetBasePosition()));
            instance.SetTargetPosition(EditorGUILayout.Vector3Field(ContentProperties.TargetPosition, instance.GetTargetPosition()));
            instance.SetSpeed(EditorGUILayout.Slider(ContentProperties.Speed, instance.GetSpeed(), 0.0f, 10.0f));
            EndGroup();
        }
    }
}