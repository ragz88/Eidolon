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
    public abstract class AssistantItem
    {
        private int id = -1;

        private float sideLevel;
        private bool foldoutIsActive;
        private RSEditorGUIProperties editorGUIProperties;

        /// <summary>
        /// Index in specific section in Assistant window.
        /// </summary>
        public int GetID()
        {
            return id;
        }

        public void SetID(int value)
        {
            id = value;
        }

        public virtual int GetPosition()
        {
            return 999;
        }

        /// <summary>
        /// Item name in Assistant window.
        /// </summary>
        public abstract string GetDisplayName();

        /// <summary>
        /// Return current setup item is ready state.
        /// </summary>
        public abstract int IsReady();

        /// <summary>
        /// Section in Assistant window.
        /// </summary>
        public abstract AssastantSelection GetSection();

        /// <summary>
        /// Called once when Assistent is open.
        /// </summary>
        public virtual void OnAssistantOpen()
        {

        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base AssistantItem properties.
        /// </summary>
        public virtual void OnInitialize()
        {
            editorGUIProperties = RSEditorResourcesHelper.GetEditorGUIProperties();
            OnInitializeProperties();
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// </summary>
        public virtual void OnInitializeProperties()
        {

        }

        /// <summary>
        /// Called when this element is selected.
        /// </summary>
        public virtual void OnSelect()
        {

        }

        /// <summary>
        /// OnMainGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public virtual void OnMainGUI()
        {
            BeginMainGroup(GetDisplayName());
            OnPropertiesGUI();
            EndGroup();
        }

        /// <summary>
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public virtual void OnPropertiesGUI()
        {

        }

        /// <summary>
        /// Called when the current item is changed to another.
        /// </summary>
        public virtual void OnDisable()
        {

        }

        /// <summary>
        /// Called once when Assistent window is closed.
        /// </summary>
        public virtual void OnAssistantClosed()
        {

        }

        public virtual void BeginGroup(GUIContent content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetGroupColor() : RSEditorGUIProperties.DefaultGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = RSEditor.GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(RSEditor.GetHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, RSEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        public virtual void BeginMainGroup(GUIContent content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetGroupColor() : RSEditorGUIProperties.DefaultGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = RSEditor.GetHeaderRect();
            headerRect.height += 5;
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            GUI.Label(labelRect, content, RSEditorStyles.LargeCenteredGrayLabel);

            GUILayout.Space(35);
        }

        public virtual void BeginFoldoutGroup(ref bool foldout, GUIContent content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBodyFoldoutGroup() : RSEditorGUIProperties.DefaultFoldoutGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = RSEditor.GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderFoldoutGroupColor() : RSEditorGUIProperties.DefaultHeaderFoldoutGroupColor;
            GUI.Label(RSEditor.GetHeaderRect(), GUIContent.none, EditorStyles.helpBox);

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

            Rect headerRect = RSEditor.GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, RSEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        public virtual void BeginMainGroup(string content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetGroupColor() : RSEditorGUIProperties.DefaultGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = RSEditor.GetHeaderRect();
            headerRect.height += 5;
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            GUI.Label(labelRect, content, RSEditorStyles.LargeCenteredGrayLabel);

            GUILayout.Space(35);
        }

        public virtual void BeginFoldoutGroup(ref bool foldout, string content, float sideLevel = 0)
        {
            this.sideLevel = sideLevel;
            foldoutIsActive = foldout;

            GUILayout.BeginHorizontal();

            GUILayout.Space(this.sideLevel);

            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBodyFoldoutGroup() : RSEditorGUIProperties.DefaultFoldoutGroupColor;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = RSEditor.GetHeaderRect();
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderFoldoutGroupColor() : RSEditorGUIProperties.DefaultHeaderFoldoutGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 7 : 1);

        }

        public virtual void EndGroup()
        {
            GUILayout.Space(5);

            GUILayout.EndVertical();
            GUILayout.Space(this.sideLevel);
            GUILayout.EndHorizontal();
        }

        public virtual void EndFoldoutGroup()
        {
            GUILayout.Space(foldoutIsActive ? 5 : 0);

            GUILayout.EndVertical();
            GUILayout.Space(this.sideLevel);
            GUILayout.EndHorizontal();
        }
    }
}