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
    [CustomEditor(typeof(AdvancedFootstepSystem))]
    public sealed class AdvancedFootstepSystemEditor : RSEditor<AdvancedFootstepSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent StepSettings = new GUIContent("Step Properties");
            public readonly static GUIContent FootstepProperties = new GUIContent("Footstep Properties", "Footstep properties asset.");
            public readonly static GUIContent StepInterval = new GUIContent("Step Interval", "After each step will be played footstep sound.");

            public readonly static GUIContent FootPropertes = new GUIContent("Foot Properties");
            public readonly static GUIContent LeftFootPivot = new GUIContent("Left Foot Pivot", "Character left foot pivot.");
            public readonly static GUIContent RightFootPivot = new GUIContent("Right Foot Pivot", "Character right foot pivot.");
            public readonly static GUIContent RayRange = new GUIContent("Ray Range", "Ray range from foot to the ground.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.StepSettings);
            instance.SetFootstepProperties(ObjectField<FootstepProperties>(ContentProperties.FootstepProperties, instance.GetFootstepProperties(), true));
            if (instance.GetFootstepProperties() == null)
            {
                RSEditorHelpBoxMessages.Tip("Footstep sounds will not be played.", "For create Footstep Properties asset press right mouse button on Project window and select Create > Third Person Engine > Player > Footstep Properties.");
            }
            instance.SetStepInterval(EditorGUILayout.FloatField(ContentProperties.StepInterval, instance.GetStepInterval()));
            EndGroup();

            BeginGroup(ContentProperties.FootPropertes);
            DrawDoubleBoolGroup();
            instance.SetRayRange(EditorGUILayout.FloatField(ContentProperties.RayRange, instance.GetRayRange()));
            EndGroup();
        }

        private void DrawDoubleBoolGroup()
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, ContentProperties.LeftFootPivot, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, ContentProperties.RightFootPivot, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width -= 12;
            leftToggleValueRect.height = 15;
            leftToggleValueRect.x += 7;
            leftToggleValueRect.y += 4;
            instance.SetLeftFootPivot(ObjectField<Transform>(leftToggleValueRect, GUIContent.none, instance.GetLeftFootPivot(), true));

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width -= 12;
            rightToggleValueRect.height = 15;
            rightToggleValueRect.x += 7;
            rightToggleValueRect.y += 4;
            instance.SetRightFootPivot(ObjectField<Transform>(rightToggleValueRect, GUIContent.none, instance.GetRightFootPivot(), true));
        }
    }
}