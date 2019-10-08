/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public class AboutWindow : RSEditorWindow
    {
        /// <summary>
        /// Open About window.
        /// </summary>
        [MenuItem("Third Person Engine/About", false, 0)]
        public static void Open()
        {
            AboutWindow window = ScriptableObject.CreateInstance(typeof(AboutWindow)) as AboutWindow;
            window.titleContent = new GUIContent("About Third Person Engine");
            window.maxSize = new Vector2(475, 227);
            window.minSize = new Vector2(475, 227);
            window.ShowUtility();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        protected virtual void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            GUILayout.BeginVertical();
            BeginGroup(ThirdPersonEngineInfo.NAME);
            DefaultLabelContent("Product:", ThirdPersonEngineInfo.NAME);
            DefaultLabelContent("Release:", ThirdPersonEngineInfo.RELEASE);
            DefaultLabelContent("Publisher:", ThirdPersonEngineInfo.PUBLISHER);
            DefaultLabelContent("Author:", ThirdPersonEngineInfo.AUTHOR);
            DefaultLabelContent("Version:", ThirdPersonEngineInfo.VERSION);
            GUILayout.Label(ThirdPersonEngineInfo.COPYRIGHT, RSEditorStyles.BoldLabel);

            GUILayout.Space(20);

            OpenSupportButton("For get full informations about " + ThirdPersonEngineInfo.NAME + " see - ", "Documentation", UserHelper.OpenDocumentation);
            OpenSupportButton("To keep abreast of all the new news, follow us on - ", "Twitter", UserHelper.OFFICIAL_TWITTER_URL);
            OpenSupportButton("If you have any questions you can ask them in the - ", "Official Thread", UserHelper.OFFICIAL_THREAD_URL);
            SelectableContent("Official support - ", UserHelper.OFFICIAL_EMAIL);
            EndGroup();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private static void DefaultLabelContent(string title, string message = "", float space = 3.0f, float width = 65)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(1));
            GUILayout.Label(title, RSEditorStyles.BoldLabel, GUILayout.Width(width));
            GUILayout.Space(space);
            GUILayout.Label(message, RSEditorStyles.BoldLabel);
            GUILayout.EndHorizontal();
            GUILayout.Space(2.5f);
        }

        public static void SelectableContent(string content, string selectableContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, RSEditorStyles.BoldLabel, GUILayout.Height(17));
            EditorGUILayout.SelectableLabel(selectableContent, RSEditorStyles.LinkLabelBold, GUILayout.Height(17));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void OpenSupportButton(string content, string buttonContent, string url)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, RSEditorStyles.BoldLabel, GUILayout.Height(17));
            if (GUILayout.Button(buttonContent, RSEditorStyles.LinkLabelBold, GUILayout.Height(17)))
                Application.OpenURL(url);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void OpenSupportButton(string content, string buttonContent, Action buttonAction)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, RSEditorStyles.BoldLabel, GUILayout.Height(17));
            if (GUILayout.Button(buttonContent, RSEditorStyles.LinkLabelBold, GUILayout.Height(17)))
                buttonAction.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}