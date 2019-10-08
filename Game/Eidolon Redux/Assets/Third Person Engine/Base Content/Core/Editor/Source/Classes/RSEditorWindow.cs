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
    public class RSEditorWindow : EditorWindow
    {
        private float sideLevel;
        private bool foldoutIsActive;
        private RSEditorGUIProperties editorGUIProperties;

        public virtual void BeginGroup(GUIContent content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetGroupColor() : RSEditorGUIProperties.DefaultGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(GetHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, RSEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        public virtual void BeginFoldoutGroup(ref bool foldout, GUIContent content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBodyFoldoutGroup() : RSEditorGUIProperties.DefaultFoldoutGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderFoldoutGroupColor() : RSEditorGUIProperties.DefaultHeaderFoldoutGroupColor;
            GUI.Label(GetHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldout ? 7 : 1);

            foldoutIsActive = foldout;
        }

        public virtual void BeginGroup(string content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetGroupColor() : RSEditorGUIProperties.DefaultGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, RSEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        public virtual void BeginFoldoutGroup(ref bool foldout, string content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            foldoutIsActive = foldout;

            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBodyFoldoutGroup() : RSEditorGUIProperties.DefaultFoldoutGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderFoldoutGroupColor() : RSEditorGUIProperties.DefaultHeaderFoldoutGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 7 : 1);

        }

        /// <summary>
        /// End box GUI.
        /// </summary>
        public virtual void EndGroup()
        {
            GUILayout.Space(5);

            GUILayout.EndVertical();
            GUILayout.Space(this.sideLevel);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// End box GUI.
        /// </summary>
        public virtual void EndFoldoutGroup()
        {
            GUILayout.Space(foldoutIsActive ? 5 : 0);

            GUILayout.EndVertical();
            GUILayout.Space(this.sideLevel);
            GUILayout.EndHorizontal();
        }

        public static Rect GetHeaderRect()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.x -= 4;
            rect.y -= 3;
            rect.width += 8;
            rect.height += 25;
            return rect;
        }

        public RSEditorGUIProperties GetEditorGUIProperties()
        {
            return editorGUIProperties;
        }

        public void SetEditorGUIProperties(RSEditorGUIProperties value)
        {
            editorGUIProperties = value;
        }
    }
}