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
    public class TagEditAssistantItem : AssistantItem
    {
        private const string TNC_PATH = RSEditorPaths.BASE_CONTENT_FOLDER_PATH + "/Core/Runtime/Name Convention/TNC.cs";

        public override string GetDisplayName()
        {
            return "Edit Tag";
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
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public override void OnPropertiesGUI()
        {
            GUILayout.Label("You can change default tags. To do this, go to the TNC file. The default location of the file \"Third Person Engine > Core > Runtime > Name Convention > TNC\"", RSEditorStyles.CenteredLabelWrap);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Edit TNC ", GUILayout.Width(150)))
            {
                if (System.IO.File.Exists(TNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(TNC_PATH, 15);
                }
                else
                {
                    RSDisplayDialogs.Message("Tag Name Convention", "TNC file not found, may be location changed try find it manually from Project window by Search > [TNC]");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    }
}