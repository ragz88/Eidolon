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
    public abstract class ManagerItem
    {
        private int id = -1;
        private float sideLevel;
        private bool foldoutIsActive;
        private RSEditorGUIProperties editorGUIProperties;

        /// <summary>
        /// Item name in Manager window.
        /// </summary>
        public abstract string GetDisplayName();

        /// <summary>
        /// Section in Manager window.
        /// </summary>
        public abstract ManagerSection GetSection();

        /// <summary>
        /// Get item ID.
        /// </summary>
        public int GetID()
        {
            return id;
        }

        /// <summary>
        /// Set item ID.
        /// </summary>
        public void SetID(int value)
        {
            id = value;
        }

        /// <summary>
        /// Get position in Manager window.
        /// </summary>
        public virtual int GetPriority()
        {
            return 999;
        }

        /// <summary>
        /// Called once when Manager is open.
        /// </summary>
        public virtual void OnManagerOpen()
        {

        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        public virtual void OnManagerEnable()
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
        /// Called when the Manager window gets keyboard or mouse focus.
        /// </summary>
        public virtual void OnManagerFocus()
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
        /// Called once when Manager window is closed.
        /// </summary>
        public virtual void OnManagerClosed()
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
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            GUI.Label(labelRect, content, RSEditorStyles.LargeCenteredGrayLabel);

            GUILayout.Space(30);
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
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderGroupColor() : RSEditorGUIProperties.DefaultHeaderGroupColor;
            GUI.Label(headerRect, GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            GUI.Label(labelRect, content, RSEditorStyles.LargeCenteredGrayLabel);

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

        private Vector2 size;

        public Vector2 Size { get => size; set => size = value; }
    }
}