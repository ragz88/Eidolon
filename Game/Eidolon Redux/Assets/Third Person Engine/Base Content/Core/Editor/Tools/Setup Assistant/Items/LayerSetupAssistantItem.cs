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
    public class LayerSetupAssistantItem : AssistantItem
    {
        public override string GetDisplayName()
        {
            return "Setup Layers";
        }

        public override int IsReady()
        {
            return RSEditorInternal.GetMissingLayers().Length == 0 ? 1 : -1;
        }

        public override AssastantSelection GetSection()
        {
            return AssastantSelection.Important;
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            string[] missingLayers = RSEditorInternal.GetMissingLayers();
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses special layers that must be declared in the project settings.", RSEditorStyles.LabelWrap);
            GUILayout.Space(5);
            GUILayout.Label("Pay attention on layer number!", RSEditorStyles.CenteredBoldLabel);
            GUILayout.Space(5);
            GUILayout.Label(ThirdPersonEngineInfo.NAME + " uses the following layers:");
            GUILayout.Space(5);
            GUILayout.Label("[" + string.Join("], [", LNC.LayersWithIndex) + "]", RSEditorStyles.CenteredBoldLabelWrap);

            if (missingLayers != null && missingLayers.Length > 0)
            {
                GUILayout.Label("Add the following layers:");
                GUILayout.Space(5);
                EditorGUILayout.SelectableLabel("[" + string.Join(", ", missingLayers) + "]", RSEditorStyles.CenteredBoldLabelWrap);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(" Add Manually ", GUILayout.Width(150)))
                {
#if UNITY_2018_1_OR_NEWER
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
#else
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
#endif
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if ((missingLayers != null && missingLayers.Length == 0) || missingLayers == null)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Layer setup complite!", GUILayout.Width(150))) { }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
    }
}