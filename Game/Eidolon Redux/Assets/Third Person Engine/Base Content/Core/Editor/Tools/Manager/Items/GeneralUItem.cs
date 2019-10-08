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
    public sealed class GeneralUItem : ManagerItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent BaseOptions = new GUIContent("Base Options");
            public readonly static GUIContent VerificationProject = new GUIContent("Check Project (Current Session)", "Verification project settings on errors each compile.");
            public readonly static GUIContent PropertiesNull = new GUIContent("Editor Properties not found...\nCheck Editor Properties asset contatins in Resources/" + RSEditorResourcesHelper.PROPETIES_PATH);

            public readonly static GUIContent GUIColor = new GUIContent("Inspector GUI Colors");
            public readonly static GUIContent HeaderColor = new GUIContent("Header Color");
            public readonly static GUIContent BackgroundColor = new GUIContent("Backgronud Color");
            public readonly static GUIContent BodyColor = new GUIContent("Body Color");
            public readonly static GUIContent GroupHeaderColor = new GUIContent("Group Header Color");
            public readonly static GUIContent GroupColor = new GUIContent("Group Color");

            public readonly static GUIContent HeaderFoldoutGroupColor = new GUIContent("Header Foldout Color");
            public readonly static GUIContent FoldoutGroupColor = new GUIContent("Foldout Body Color");
        }

        private RSEditorProperties editorProperties;
        private RSEditorGUIProperties editorGUIProperties;
        private Texture2D returnIcon;

        public override string GetDisplayName()
        {
            return "General";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.General;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnInitializeProperties()
        {
            editorProperties = RSEditorResourcesHelper.GetEditorProperties();
            editorGUIProperties = RSEditorResourcesHelper.GetEditorGUIProperties();
            returnIcon = RSEditorResourcesHelper.GetIcon("Return");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnPropertiesGUI()
        {
            if (editorProperties != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.VerificationProject, GUILayout.Width(200));
                editorProperties.VerificationEachCompile(EditorGUILayout.Toggle(editorProperties.VerificationEachCompile()));
                GUILayout.EndHorizontal();
                if (GUI.changed)
                    EditorUtility.SetDirty(editorProperties);
            }
            else
            {
                GUILayout.Label(ContentProperties.PropertiesNull);
            }

            if (editorGUIProperties != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.HeaderColor, GUILayout.Width(125));
                editorGUIProperties.SetHeaderColor(EditorGUILayout.ColorField(editorGUIProperties.GetHeaderColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetHeaderColor(RSEditorGUIProperties.DefaultHeaderColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.BackgroundColor, GUILayout.Width(125));
                editorGUIProperties.SetBackgroundColor(EditorGUILayout.ColorField(editorGUIProperties.GetBackgroundColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetBackgroundColor(RSEditorGUIProperties.DefaultBackgroundColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.BodyColor, GUILayout.Width(125));
                editorGUIProperties.SetBodyColor(EditorGUILayout.ColorField(editorGUIProperties.GetBodyColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetBodyColor(RSEditorGUIProperties.DefaultBodyColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.GroupHeaderColor, GUILayout.Width(125));
                editorGUIProperties.SetHeaderGroupColor(EditorGUILayout.ColorField(editorGUIProperties.GetHeaderGroupColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetHeaderGroupColor(RSEditorGUIProperties.DefaultHeaderGroupColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.GroupColor, GUILayout.Width(125));
                editorGUIProperties.SetGroupColor(EditorGUILayout.ColorField(editorGUIProperties.GetGroupColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetGroupColor(RSEditorGUIProperties.DefaultGroupColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.HeaderFoldoutGroupColor, GUILayout.Width(125));
                editorGUIProperties.SetHeaderFoldoutGroupColor(EditorGUILayout.ColorField(editorGUIProperties.GetHeaderFoldoutGroupColor()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetHeaderFoldoutGroupColor(RSEditorGUIProperties.DefaultHeaderFoldoutGroupColor);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(ContentProperties.GroupColor, GUILayout.Width(125));
                editorGUIProperties.SetBodyFoldoutGroup(EditorGUILayout.ColorField(editorGUIProperties.GetBodyFoldoutGroup()));
                if (GUILayout.Button(returnIcon, GUILayout.Width(30), GUILayout.Height(16)))
                {
                    editorGUIProperties.SetBodyFoldoutGroup(RSEditorGUIProperties.DefaultFoldoutGroupColor);
                }
                GUILayout.EndHorizontal();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(editorGUIProperties);
                }

            }
            else
            {
                RSEditorHelpBoxMessages.Error("Editor GUI Properties not found.", "Create UEditorGUIProperties asset from " + RSEditorPaths.MENUITEM_EDITOR_PATH + "Editor GUI Properties and move it in Third Person Engine/Resources/Editor/Properties folder.");
            }
        }
    }
}