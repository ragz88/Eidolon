/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public class InputSetupAssistantItem : AssistantItem
    {
        public override string GetDisplayName()
        {
            return "Setup Input";
        }

        public override AssastantSelection GetSection()
        {
            return AssastantSelection.Important;
        }

        public override int IsReady()
        {
            string[] missingInputs = RSEditorInternal.GetMissingInput("All");
            if (missingInputs == null && missingInputs.Length == 0)
            {
                return 1;
            }
            else if (missingInputs != null && missingInputs.Length > 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            string[] missingAxes = RSEditorInternal.GetMissingInput("Axes");
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses special axes that must be declared in the project settings.", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses the following axes:", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", INC.Axes) + "]", RSEditorStyles.CenteredBoldLabelWrap);

            if (missingAxes != null && missingAxes.Length > 0)
            {
                GUILayout.Label("Add the following axes:", RSEditorStyles.LabelWrap);
                GUILayout.Space(5);
                GUILayout.Label("[" + string.Join(", ", missingAxes) + "]", RSEditorStyles.CenteredBoldLabelWrap);
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(" Add Manually "))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
#endif
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingAxes != null && missingAxes.Length == 0) || missingAxes == null)
            {
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Axes setup complite!", GUILayout.Width(150))) { }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            RSEditor.HorizontalLine();
            GUILayout.Space(10);

            string[] missingButtons = RSEditorInternal.GetMissingInput("Buttons");
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses special buttons that must be declared in the project settings.", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses the following buttons:", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", INC.Buttons) + "]", RSEditorStyles.CenteredBoldLabelWrap);

            if (missingButtons != null && missingButtons.Length > 0)
            {
                GUILayout.Label("Add the following buttons:", RSEditorStyles.LabelWrap);
                GUILayout.Space(5);
                EditorGUILayout.SelectableLabel("[" + string.Join(", ", missingButtons) + "]", RSEditorStyles.CenteredBoldLabelWrap);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(" Add Manually ", GUILayout.Width(150)))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
#endif
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingButtons != null && missingButtons.Length == 0) || missingButtons == null)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Button setup complite!", GUILayout.Width(150))) { }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
    }
}