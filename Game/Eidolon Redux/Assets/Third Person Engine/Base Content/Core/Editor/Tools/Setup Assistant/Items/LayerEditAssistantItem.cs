/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public class LayerEditAssistantItem : AssistantItem
    {
        private const string LNC_PATH = RSEditorPaths.BASE_CONTENT_FOLDER_PATH + "/Core/Runtime/Name Convention/LNC.cs";

        public override string GetDisplayName()
        {
            return "Edit Layers";
        }

        public override int IsReady()
        {
            return 0;
        }

        public override AssastantSelection GetSection()
        {
            return AssastantSelection.Optional;
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            GUILayout.Label("Edit layers", RSEditorStyles.GroupHeaderLabel);
            GUILayout.Space(10);
            GUILayout.Label("You can change default layers. To do this, go to the LNC file. The default location of the file \"Third Person Engine > Core > Runtime > Name Convention > LNC\"", RSEditorStyles.CenteredLabelWrap);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Edit LNC ", GUILayout.Width(150)))
            {
                if (System.IO.File.Exists(LNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(LNC_PATH, 15);
                }
                else
                {
                    RSDisplayDialogs.Message("Layers Name Convention", "LNC file not found, may be location changed try find it manually from Project window by Search > [LNC]");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    }
}