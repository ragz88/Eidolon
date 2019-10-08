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
    [CustomEditor(typeof(GrabObject))]
    [CanEditMultipleObjects]
    public class GrabObjectEditor : RSEditor<GrabObject>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Anchor = new GUIContent("Anchor", "The position of the axis around which the body swings. The position is defined in local space.");
            public readonly static GUIContent Axis = new GUIContent("Axis", "The direction of the axis around which the body swings. The direction is defined in local space.");
            public readonly static GUIContent ConnectedAnchor = new GUIContent("Connected Anchor", "Optional reference to the Rigidbody that the joint is dependent upon. If not set, the joint connects to the world.");
            public readonly static GUIContent JointSpring = new GUIContent("Joint Spring");
            public readonly static GUIContent Spring = new GUIContent("Spring", "The force the object asserts to move into the position.");
            public readonly static GUIContent Damper = new GUIContent("Spring", "The higher this value, the more the object will slow down.");
            public readonly static GUIContent TargetPosition = new GUIContent("Target Position", "Target angle of the spring. The spring pulls towards this angle measured in degrees.");
            public readonly static GUIContent LeftHandIKTarget = new GUIContent("Left Hand", "Left Hand IK Target");
            public readonly static GUIContent RightHandIKTarget = new GUIContent("Right Hand", "Right Hand IK Target");
            public readonly static GUIContent Debug = new GUIContent("Debug");
            public readonly static GUIContent DrawConnectedAnchorGUI = new GUIContent("Connected Anchor GUI");
            public readonly static GUIContent DrawHandsIKGUI = new GUIContent("Hands IK GUI");
        }

        private bool jointSpringFoldout;
        private bool drawGUIFoldout;
        private bool drawConnectedAnchorGUI = true;
        private bool drawHandsIKGUI = true;

        protected virtual void OnSceneGUI()
        {
            if (drawConnectedAnchorGUI)
                ConntecterAnchorGUI();
            if (drawHandsIKGUI)
                IKHandsGUI();
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetAnchor(EditorGUILayout.Vector3Field(ContentProperties.Anchor, instance.GetAnchor()));
            instance.SetAxis(EditorGUILayout.Vector3Field(ContentProperties.Axis, instance.GetAxis()));
            instance.SetConnectedAnchor(EditorGUILayout.Vector3Field(ContentProperties.ConnectedAnchor, instance.GetConnectedAnchor()));
            IncreaseIndentLevel();
            BeginFoldoutGroup(ref jointSpringFoldout, ContentProperties.JointSpring);
            if (jointSpringFoldout)
            {
                JointSpring jointSpring = instance.GetJointSpring();
                jointSpring.spring = EditorGUILayout.FloatField(ContentProperties.Spring, jointSpring.spring);
                jointSpring.damper = EditorGUILayout.FloatField(ContentProperties.Damper, jointSpring.damper);
                jointSpring.targetPosition = EditorGUILayout.FloatField(ContentProperties.TargetPosition, jointSpring.targetPosition);
                instance.SetJointSpring(jointSpring);
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref drawGUIFoldout, ContentProperties.Debug);
            if (drawGUIFoldout)
            {
                drawConnectedAnchorGUI = EditorGUILayout.Toggle(ContentProperties.DrawConnectedAnchorGUI, drawConnectedAnchorGUI);
                drawHandsIKGUI = EditorGUILayout.Toggle(ContentProperties.DrawHandsIKGUI, drawHandsIKGUI);
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();
            Transform leftHandIKTarget = instance.GetLeftHandIKTarget();
            Transform rightHandIKTarget = instance.GetRightHandIKTarget();
            TransformGroupField(ref leftHandIKTarget, ref rightHandIKTarget, ContentProperties.LeftHandIKTarget, ContentProperties.RightHandIKTarget);
            instance.SetLeftHandIKTarget(leftHandIKTarget);
            instance.SetRightHandIKTarget(rightHandIKTarget);
            EndGroup();
        }

        protected virtual void IKHandsGUI()
        {
            Transform leftHandIKTarget = instance.GetLeftHandIKTarget();
            Transform rightHandIKTarget = instance.GetRightHandIKTarget();

            const float CUBE_SIZE = 0.075f;
            const float ARROW_SIZE = 0.1f;
            const string LEFT_IK_TARGET_LABEL = "L IK";
            const string RIGHT_IK_TARGET_LABEL = "R IK";

            Color cubeColor = new Color(0.06f, 0.5f, 0.9f, 1.0f);
            Color arrowColor = Color.white;
            Color labelColor = Color.white;
            GUIStyle labelStyle = new GUIStyle(EditorStyles.helpBox);
            labelStyle.fontStyle = FontStyle.Bold;
            if (leftHandIKTarget != null)
            {
                Handles.color = cubeColor;
                Handles.CubeHandleCap(0, leftHandIKTarget.position, leftHandIKTarget.rotation, CUBE_SIZE, EventType.Repaint);
                leftHandIKTarget.position = Handles.PositionHandle(leftHandIKTarget.position, Quaternion.identity);
                leftHandIKTarget.rotation = Handles.RotationHandle(leftHandIKTarget.rotation, leftHandIKTarget.position);
                Handles.color = arrowColor;
                Handles.ArrowHandleCap(0, leftHandIKTarget.position, leftHandIKTarget.rotation, ARROW_SIZE, EventType.Repaint);
                GUI.color = labelColor;
                Handles.Label(leftHandIKTarget.position, LEFT_IK_TARGET_LABEL, labelStyle);
            }

            if (rightHandIKTarget != null)
            {
                Handles.color = cubeColor;
                Handles.CubeHandleCap(0, rightHandIKTarget.position, rightHandIKTarget.rotation, CUBE_SIZE, EventType.Repaint);
                rightHandIKTarget.position = Handles.PositionHandle(rightHandIKTarget.position, Quaternion.identity);
                rightHandIKTarget.rotation = Handles.RotationHandle(rightHandIKTarget.rotation, rightHandIKTarget.position);
                Handles.color = arrowColor;
                Handles.ArrowHandleCap(0, rightHandIKTarget.position, rightHandIKTarget.rotation, ARROW_SIZE, EventType.Repaint);
                GUI.color = labelColor;
                Handles.Label(rightHandIKTarget.position, RIGHT_IK_TARGET_LABEL, labelStyle);
            }
        }

        protected virtual void ConntecterAnchorGUI()
        {
            const float DISC_SIZE = 0.1f;
            Vector3 objectPosition = instance.transform.position;
            Vector3 discPosition = objectPosition - instance.GetConnectedAnchor();
            Vector3 forwardDirection = objectPosition - (Vector3.forward * instance.GetConnectedAnchor().z);

            Handles.color = Color.black;
            Handles.DrawWireDisc(discPosition, Vector3.up, DISC_SIZE);
            Handles.color = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            Handles.DrawSolidDisc(discPosition, Vector3.up, DISC_SIZE);
            Handles.color = Color.black;
            Handles.DrawLine(objectPosition, discPosition);
            Handles.DrawLine(objectPosition, forwardDirection);
            Handles.DrawLine(forwardDirection, discPosition);
        }
    }
}