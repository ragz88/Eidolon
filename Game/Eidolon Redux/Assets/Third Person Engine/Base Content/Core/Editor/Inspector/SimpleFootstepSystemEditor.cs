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
    [CustomEditor(typeof(SimpleFootstepSystem))]
    public sealed class SimpleFootstepSystemEditor : RSEditor<SimpleFootstepSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Base = new GUIContent("Base Settings");
            public readonly static GUIContent FootstepProperties = new GUIContent("Footstep Properties", "Footstep properties asset.");
            public readonly static GUIContent StepInterval = new GUIContent("Step Interval", "After each step will be played footstep sound.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Base);
            instance.SetFootstepProperties(ObjectField<FootstepProperties>(ContentProperties.FootstepProperties, instance.GetFootstepProperties(), true));
            if (instance.GetFootstepProperties() == null)
            {
                RSEditorHelpBoxMessages.Tip("Footstep sounds will not be played.", "For create Footstep Properties asset press right mouse button on Project window and select Create > Third Person Engine > Player > Footstep Properties.");
            }
            instance.SetStepInterval(EditorGUILayout.FloatField(ContentProperties.StepInterval, instance.GetStepInterval()));
            EndGroup();
        }
    }
}