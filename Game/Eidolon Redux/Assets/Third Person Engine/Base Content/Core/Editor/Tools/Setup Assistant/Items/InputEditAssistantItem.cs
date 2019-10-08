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
    public class InputEditAssistantItem : AssistantItem
    {
        private const string TNC_PATH = RSEditorPaths.BASE_CONTENT_FOLDER_PATH + "/Core/Runtime/Name Convention/INC.cs";

        public override string GetDisplayName()
        {
            return "Edit Input";
        }

        public override AssastantSelection GetSection()
        {
            return AssastantSelection.Optional;
        }

        public override int IsReady()
        {
            return 0;
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnPropertiesGUI()
        {
            GUILayout.Label("You can change default inputs. To do this, go to the INC file. The default location of the file \"Third Person Engine > Core > Runtime > Name Convention > INC\"", RSEditorStyles.CenteredLabelWrap);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(" Edit INC ", GUILayout.Width(150)))
            {
                if (System.IO.File.Exists(TNC_PATH))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(TNC_PATH, 15);
                }
                else
                {
                    RSDisplayDialogs.Message("Input Name Convention", "INC file not found, may be location changed try find it manually from Project window by Search > INC");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    }
}