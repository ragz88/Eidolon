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
    public static class RSEditorResourcesHelper
    {
        public const string PROPETIES_PATH = "Editor/Properties/";
        public const string ICONS_PATH = "Editor/Icons/";

        public static RSEditorProperties GetEditorProperties()
        {
            RSEditorProperties editorProperties = null;
            if (editorProperties == null)
                editorProperties = Resources.Load(PROPETIES_PATH + "Editor Properties") as RSEditorProperties;
            return editorProperties;
        }

        public static RSEditorGUIProperties GetEditorGUIProperties()
        {
            RSEditorGUIProperties editorGUIProperties = null;
            if (editorGUIProperties == null)
                editorGUIProperties = Resources.Load(PROPETIES_PATH + "Editor GUI Properties") as RSEditorGUIProperties;
            return editorGUIProperties;
        }

        public static Texture2D GetIcon(string iconName)
        {
            return (Texture2D) Resources.Load(ICONS_PATH + iconName) as Texture2D;
        }
    }
}