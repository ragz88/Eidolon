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
    public class TagSetupAssistantItem : AssistantItem
    {
        public override string GetDisplayName()
        {
            return "Setup Tag";
        }

        public override int IsReady()
        {
            string[] missingTags = RSEditorInternal.GetMissingTags();
            if (missingTags == null && missingTags.Length == 0)
            {
                return 1;
            }
            else if (missingTags != null && missingTags.Length > 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public override AssastantSelection GetSection()
        {
            return AssastantSelection.Important;
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public override void OnPropertiesGUI()
        {
            string[] missingTags = RSEditorInternal.GetMissingTags();
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses special tags that must be declared in the project settings.", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses the following tags:", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join(", ", TNC.Tags) + "]", RSEditorStyles.CenteredBoldLabelWrap);

            if (missingTags != null && missingTags.Length > 0)
            {
                GUILayout.Label("Add the following tags:", RSEditorStyles.LabelWrap);
                GUILayout.Space(5);
                EditorGUILayout.SelectableLabel("[" + string.Join(", ", missingTags) + "]", RSEditorStyles.CenteredBoldLabelWrap);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(" Add Manually ", GUILayout.Width(150)))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tag and Layers");
#endif
                }

                if (GUILayout.Button(" Add Auto ", GUILayout.Width(150)))
                {
                    RSEditorInternal.AddMissingTags();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingTags != null && missingTags.Length == 0) || missingTags == null)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Tag setup complite!", GUILayout.Width(150))) { }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }

    }
}