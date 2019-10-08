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
    [CustomEditor(typeof(TPController))]
    public sealed class TPControllerEditor : RSEditor<TPController>
    {
        internal new static class ContentProperties
        {
            // Base properties content
            public readonly static GUIContent Movement = new GUIContent("Base", "Base properties.");
            public readonly static GUIContent LocomotionType = new GUIContent("Locomotion Type", "Controller locomotion type.");
            public readonly static GUIContent SprintDirection = new GUIContent("Sprint Direction", "Controller sprint direction conditions.");
            public readonly static GUIContent FreeRotationSpeed = new GUIContent("Rotation Speed", "Character rotation speed, while active free locomotion type.");
            public readonly static GUIContent StrafeRotationSpeed = new GUIContent("Rotation Speed", "Character rotation speed, while active strafe locomotion type.");
            public readonly static GUIContent RootMotion = new GUIContent("Root Motion", "Root motion for character.");
            public readonly static GUIContent KeepDirection = new GUIContent("Keep Direction", "Keep character direction by camera direction.");
            public readonly static GUIContent FreeSpeed = new GUIContent("Speed", "Speed properties while active free locomotion type.");
            public readonly static GUIContent FreeCrouchSpeed = new GUIContent("Crouch Speed", "Crouch speed properties while active free locomotion type.");
            public readonly static GUIContent StrafeSpeed = new GUIContent("Speed", "Speed properties while active strafe locomotion type.");
            public readonly static GUIContent StrafeCrouchSpeed = new GUIContent("Crouch Speed", "Crouch speed properties while active strafe locomotion type.");
            public readonly static GUIContent Walk = new GUIContent("Walk", "Walk speed.");
            public readonly static GUIContent Run = new GUIContent("Run", "Run speed.");
            public readonly static GUIContent Sprint = new GUIContent("Sprint", "Sprint speed.");

            // On wall stop properties content
            public readonly static GUIContent OnWallStopProperties = new GUIContent("On Wall Stop Properties");
            public readonly static GUIContent CheckRange = new GUIContent("Check Range", "At what range from the character to check the presence of the wall.");
            public readonly static GUIContent Angle = new GUIContent("Angle", "At what angle relative to the wall the character can't move.");
            public readonly static GUIContent WallLayer = new GUIContent("Wall Layer", "Wall layer mask.");
            public readonly static GUIContent CheckConditions = new GUIContent("Check Conditions", "Check on wall stop conditions?\nTrue: System is active.\nFalse: System not active.");

            // Crouch properties content
            public readonly static GUIContent Crouch = new GUIContent("Crouch", "Crouch properties.");
            public readonly static GUIContent CrouchAmplitude = new GUIContent("Amplitude", "Crouching amplitude.\nFormula:(CH = H-A).\nWhere: CH -Crouch character height, H-Character height, A-Crouch amplitude.\nExample: 1.8(H) - 0.67(A) = 1.13 (CH)");
            public readonly static GUIContent CrouchRate = new GUIContent("Rate", "The rate of change in the height of the Collider.");
            public readonly static GUIContent CrouchSprint = new GUIContent("Allow Sprint", "Allow character sprint while crouching.");

            // Air properties content
            public readonly static GUIContent Air = new GUIContent("Air", "Air/Jump properties.");
            public readonly static GUIContent AirControl = new GUIContent("Air Control", "Allow control character while in air.");
            public readonly static GUIContent JumpTimer = new GUIContent("Timer", "How long will be active jumping state after jumping.");
            public readonly static GUIContent JumpForwardImpulse = new GUIContent("Forward Impulse", "Character forward jump impulse.");
            public readonly static GUIContent ControllSpeed = new GUIContent("Controll Speed", "Air controll speed.");
            public readonly static GUIContent JumpHeight = new GUIContent("Height", "Jump height.");

            // Step properties content
            public readonly static GUIContent Step = new GUIContent("Step", "Step properties.");
            public readonly static GUIContent StepOffsetEnd = new GUIContent("Offset End", "Character step offset end.");
            public readonly static GUIContent StepOffsetStart = new GUIContent("Offset Start", "Character step offset start.");
            public readonly static GUIContent StepSmooth = new GUIContent("Smooth", "Character step smooth.");

            // Sliding properties contnet
            public readonly static GUIContent Sliding = new GUIContent("Sliding", "Sliding properties.");
            public readonly static GUIContent SlopeLimit = new GUIContent("Slope Limit", "Slope limit for start sliding.");
            public readonly static GUIContent SlidingSpeed = new GUIContent("Sliding Speed", "Character speed while sliding.");

            // Ground properties content
            public readonly static GUIContent Ground = new GUIContent("Ground", "Ground properties.");
            public readonly static GUIContent ExtraGravity = new GUIContent("Extra Gravity", "[Extra Gravity tooltip]");
            public readonly static GUIContent GroundLayer = new GUIContent("Ground Layer", "Layer for detect ground.");
            public readonly static GUIContent GroundDistance = new GUIContent("Distance", "Ground min/max distance.");
            public readonly static GUIContent GroundMinDistance = new GUIContent("Min Limit", "Ground min distance.");
            public readonly static GUIContent GroundMaxDistance = new GUIContent("Max Limit", "Ground max distance.");
        }

        private bool[] foldouts;
        private float maxGroundDistance = 1;
        private float minGroundDistance = -1;
        private bool editGroundDistanceValues = false;

        public override string GetHeaderName()
        {
            return "Third Person Controller";
        }

        public override void OnInitializeProperties()
        {
            foldouts = new bool[3];
            maxGroundDistance = GetMaxGroundDistance();
            minGroundDistance = GetMinGroundDistance();
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Movement);
            instance.SetLocomotionType((LocomotionType) EditorGUILayout.EnumPopup(ContentProperties.LocomotionType, instance.GetLocomotionType()));

            switch (instance.GetLocomotionType())
            {
                case LocomotionType.Free:
                    instance.SetSprintDirection(TPController.SprintDirection.Forward);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.EnumPopup(ContentProperties.SprintDirection, instance.GetSprintDirection());
                    EditorGUI.EndDisabledGroup();
                    instance.SetFreeRotationSpeed(EditorGUILayout.FloatField(ContentProperties.FreeRotationSpeed, instance.GetFreeRotationSpeed()));
                    DrawDoubleBoolGroup();
                    IncreaseIndentLevel();
                    BeginFoldoutGroup(ref foldouts[0], ContentProperties.FreeSpeed);
                    if (foldouts[0])
                    {
                        TPController.Speed freeSpeed = instance.GetFreeSpeed();
                        freeSpeed.SetWalk(EditorGUILayout.FloatField(ContentProperties.Walk, freeSpeed.GetWalk()));
                        freeSpeed.SetRun(EditorGUILayout.FloatField(ContentProperties.Run, freeSpeed.GetRun()));
                        freeSpeed.SetSprint(EditorGUILayout.FloatField(ContentProperties.Sprint, freeSpeed.GetSprint()));
                        instance.SetFreeSpeed(freeSpeed);
                    }
                    EndFoldoutGroup();
                    DecreaseIndentLevel();
                    break;

                case LocomotionType.Strafe:
                    instance.SetSprintDirection((TPController.SprintDirection) EditorGUILayout.EnumPopup(ContentProperties.SprintDirection, instance.GetSprintDirection()));
                    instance.SetStrafeRotationSpeed(EditorGUILayout.FloatField(ContentProperties.StrafeRotationSpeed, instance.GetStrafeRotationSpeed()));
                    DrawDoubleBoolGroup();
                    IncreaseIndentLevel();
                    BeginFoldoutGroup(ref foldouts[0], ContentProperties.StrafeSpeed);
                    if (foldouts[0])
                    {
                        TPController.Speed strafeSpeed = instance.GetStrafeSpeed();
                        strafeSpeed.SetWalk(EditorGUILayout.FloatField(ContentProperties.Walk, strafeSpeed.GetWalk()));
                        strafeSpeed.SetRun(EditorGUILayout.FloatField(ContentProperties.Run, strafeSpeed.GetRun()));
                        strafeSpeed.SetSprint(EditorGUILayout.FloatField(ContentProperties.Sprint, strafeSpeed.GetSprint()));
                        instance.SetStrafeSpeed(strafeSpeed);
                    }
                    EndFoldoutGroup();
                    DecreaseIndentLevel();
                    break;
            }
            IncreaseIndentLevel();
            BeginFoldoutGroup(ref foldouts[2], ContentProperties.OnWallStopProperties);
            if (foldouts[2])
            {
                TPController.OnWallStopProperties onWallStopProperties = instance.GetOnWallStopProperties();
                onWallStopProperties.SetCheckRange(EditorGUILayout.FloatField(ContentProperties.CheckRange, onWallStopProperties.GetCheckRange()));
                onWallStopProperties.SetAngle(EditorGUILayout.Slider(ContentProperties.Angle, onWallStopProperties.GetAngle(), 0.0f, 180.0f));
                LayerMask mask = EditorGUILayout.MaskField(ContentProperties.WallLayer, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(onWallStopProperties.GetWallLayer()), InternalEditorUtility.layers);
                onWallStopProperties.SetWallLayer(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask));
                instance.SetOnWallStopProperties(onWallStopProperties);
                onWallStopProperties.CheckConditions(EditorGUILayout.Toggle(ContentProperties.CheckConditions, onWallStopProperties.CheckConditions()));
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();
            EndGroup();

            BeginGroup(ContentProperties.Crouch);
            instance.SetCrouchAmplitude(EditorGUILayout.FloatField(ContentProperties.CrouchAmplitude, instance.GetCrouchAmplitude()));
            instance.SetCrouchRate(EditorGUILayout.FloatField(ContentProperties.CrouchRate, instance.GetCrouchRate()));
            instance.CrouchSprint(EditorGUILayout.Toggle(ContentProperties.CrouchSprint, instance.CrouchSprint()));

            IncreaseIndentLevel();
            BeginFoldoutGroup(ref foldouts[1], ContentProperties.FreeCrouchSpeed);
            if (foldouts[1])
            {
                TPController.Speed freeCrouchSpeed = instance.GetFreeCrouchSpeed();
                freeCrouchSpeed.SetWalk(EditorGUILayout.FloatField(ContentProperties.Walk, freeCrouchSpeed.GetWalk()));
                freeCrouchSpeed.SetRun(EditorGUILayout.FloatField(ContentProperties.Run, freeCrouchSpeed.GetRun()));
                freeCrouchSpeed.SetSprint(EditorGUILayout.FloatField(ContentProperties.Sprint, freeCrouchSpeed.GetSprint()));
                instance.SetFreeCrouchSpeed(freeCrouchSpeed);
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();

            EndGroup();

            BeginGroup(ContentProperties.Air);
            instance.AirControl(EditorGUILayout.Toggle(ContentProperties.AirControl, instance.AirControl()));
            instance.SetJumpForwardImpulse(EditorGUILayout.FloatField(instance.AirControl() ? ContentProperties.ControllSpeed : ContentProperties.JumpForwardImpulse, instance.GetJumpForwardImpulse()));
            instance.SetJumpTimer(EditorGUILayout.FloatField(ContentProperties.JumpTimer, instance.GetJumpTimer()));
            instance.SetJumpHeight(EditorGUILayout.FloatField(ContentProperties.JumpHeight, instance.GetJumpHeight()));
            EndGroup();

            BeginGroup(ContentProperties.Step);
            instance.SetStepOffsetEnd(EditorGUILayout.FloatField(ContentProperties.StepOffsetEnd, instance.GetStepOffsetEnd()));
            instance.SetStepOffsetStart(EditorGUILayout.FloatField(ContentProperties.StepOffsetStart, instance.GetStepOffsetStart()));
            instance.SetStepSmooth(EditorGUILayout.FloatField(ContentProperties.StepSmooth, instance.GetStepSmooth()));
            EndGroup();

            BeginGroup(ContentProperties.Sliding);
            instance.SetSlidingSpeed(EditorGUILayout.FloatField(ContentProperties.SlidingSpeed, instance.GetSlidingSpeed()));
            instance.SetSlopeLimit(EditorGUILayout.FloatField(ContentProperties.SlopeLimit, instance.GetSlopeLimit()));
            EndGroup();

            BeginGroup(ContentProperties.Ground);
            instance.SetExtraGravity(EditorGUILayout.FloatField(ContentProperties.ExtraGravity, instance.GetExtraGravity()));
            LayerMask groundLayer = EditorGUILayout.MaskField(ContentProperties.GroundLayer, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(instance.GetGroundLayer()), InternalEditorUtility.layers);
            instance.SetGroundLayer(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(groundLayer));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ContentProperties.GroundDistance, EditorStyles.label))
            {
                editGroundDistanceValues = !editGroundDistanceValues;
            }
            GUILayout.Space(EditorGUIUtility.labelWidth - 53);
            float min = instance.GetGroundMinDistance();
            float max = instance.GetGroundMaxDistance();
            min = EditorGUILayout.FloatField(min, GUILayout.Width(33));
            EditorGUILayout.MinMaxSlider(ref min, ref max, minGroundDistance, maxGroundDistance);
            max = EditorGUILayout.FloatField(max, GUILayout.Width(33));
            instance.SetGroundMinDistance(min);
            instance.SetGroundMaxDistance(max);
            GUILayout.EndHorizontal();
            if (editGroundDistanceValues)
            {
                minGroundDistance = EditorGUILayout.FloatField(ContentProperties.GroundMinDistance, minGroundDistance);
                maxGroundDistance = EditorGUILayout.FloatField(ContentProperties.GroundMaxDistance, max);
                if (RSEditor.MiniButton("Apply", "Right", GUILayout.Width(60)))
                {
                    editGroundDistanceValues = false;
                }
                GUILayout.Space(3);
            }
            EndGroup();
        }

        private void DrawDoubleBoolGroup()
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, ContentProperties.RootMotion, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, ContentProperties.KeepDirection, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width = 13;
            leftToggleValueRect.height = 13;
            leftToggleValueRect.x = leftToggleValueGroupRect.x + leftToggleValueGroupRect.width / 2 - leftToggleValueRect.width;
            leftToggleValueRect.y += 4;
            instance.RootMotion(EditorGUI.Toggle(leftToggleValueRect, instance.RootMotion()));

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width = 13;
            rightToggleValueRect.height = 13;
            rightToggleValueRect.x = rightToggleValueGroupRect.x + rightToggleValueGroupRect.width / 2 - rightToggleValueRect.width;
            rightToggleValueRect.y += 4;
            instance.KeepDirection(EditorGUI.Toggle(rightToggleValueRect, instance.KeepDirection()));
        }

        private float GetMaxGroundDistance()
        {
            if (instance.GetGroundMaxDistance() == 0)
                return 1;
            return Mathf.Ceil(instance.GetGroundMaxDistance());
        }

        private float GetMinGroundDistance()
        {
            if (instance.GetGroundMinDistance() == 0)
                return -1;
            return Mathf.Floor(instance.GetGroundMinDistance());
        }
    }
}