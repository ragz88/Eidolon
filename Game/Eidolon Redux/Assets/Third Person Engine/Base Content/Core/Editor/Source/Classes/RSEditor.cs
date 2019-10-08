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
    /// <summary>
    /// Third Person Engine editor class.
    /// </summary>
    public class RSEditor : UnityEditor.Editor
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent GenerateIDButton = new GUIContent("", "Generate a new ID for weapon.");
            public readonly static GUIContent SearchButton = new GUIContent("", "Search required element for this field.");
            public readonly static GUIContent ListButton = new GUIContent("", "Show list of available elements fron this field.");
            public readonly static GUIContent GenerateButton = new GUIContent("", "Generate new required element for this field.");
        }

        private float sideLevel;
        private bool foldoutIsActive;
        private RSEditorGUIProperties editorGUIProperties;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            editorGUIProperties = RSEditorResourcesHelper.GetEditorGUIProperties();
            OnInitializeProperties();
        }

        /// <summary>
        /// Custom Inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BeginBackground();
            DrawHeaderGUI(GetHeaderName());
            BeginBody();
            OnBaseGUI();
            EndBody();
            EndBackground();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public virtual void OnInitializeProperties()
        {
            // Some initialize here.
        }

        /// <summary>
        /// Base inspector GUI.
        /// </summary>
        public virtual void OnBaseGUI()
        {
            BeginGroup("Properties");
            base.OnInspectorGUI();
            EndGroup();
        }

        public virtual string GetHeaderName()
        {
            const string EDITOR = "Editor";
            string name = GetType().Name;
            if (name.Contains(EDITOR))
            {
                name = name.Remove(name.IndexOf(EDITOR), EDITOR.Length);
            }
            name = name.AddSpaces();
            return name;
        }

        public virtual void DrawHeaderGUI(string content)
        {
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderColor() : RSEditorGUIProperties.DefaultHeaderColor;
            GUILayout.BeginVertical(GUI.skin.button);
            GUILayout.Space(2);
            GUILayout.Label(content, RSEditorStyles.CenteredBoldLabel);
            GUILayout.Space(2);
            GUILayout.EndVertical();
        }

        public virtual void DrawHeaderGUI(GUIContent content)
        {
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetHeaderColor() : RSEditorGUIProperties.DefaultHeaderColor;
            GUILayout.BeginVertical(GUI.skin.button);
            GUILayout.Space(2);
            GUILayout.Label(content, RSEditorStyles.CenteredBoldLabel);
            GUILayout.Space(2);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Begin background GUI.
        /// </summary>
        public virtual void BeginBackground()
        {
            GUILayout.Space(7);
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBackgroundColor() : RSEditorGUIProperties.DefaultBackgroundColor;
            GUILayout.BeginVertical(GUI.skin.window, GUILayout.Height(1));
            GUI.color = Color.white;
        }

        /// <summary>
        /// End background GUI.
        /// </summary>
        public virtual void EndBackground()
        {
            GUILayout.EndVertical();
            GUILayout.Space(7);
        }

        /// <summary>
        /// Begin box GUI.
        /// </summary>
        public virtual void BeginBody()
        {
            GUI.color = editorGUIProperties != null ? editorGUIProperties.GetBodyColor() : RSEditorGUIProperties.DefaultBodyColor;
            GUILayout.BeginVertical(GUI.skin.button);
            GUI.color = Color.white;
            GUILayout.Space(7);
        }

        /// <summary>
        /// End box GUI.
        /// </summary>
        public virtual void EndBody()
        {
            GUILayout.Space(7);
            GUILayout.EndVertical();
        }

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

        /// <summary>
        /// Increase current window editor GUI indent level.
        /// </summary>
        public static void IncreaseIndentLevel(int level = 1)
        {
            EditorGUI.indentLevel += level;
        }

        /// <summary>
        /// Decrease current window editor GUI indent level.
        /// </summary>
        public static void DecreaseIndentLevel(int level = 1)
        {
            EditorGUI.indentLevel -= level;
        }

        /// <summary>
        /// Layout object field.
        /// </summary>
        public static T ObjectField<T>(GUIContent content, T value, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object
        {
            return (T) EditorGUILayout.ObjectField(content, value, typeof(T), allowSceneObjects, options);
        }

        /// <summary>
        /// Layout object field.
        /// </summary>
        public static T ObjectField<T>(string content, T value, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object
        {
            return (T) EditorGUILayout.ObjectField(content, value, typeof(T), allowSceneObjects, options);
        }

        /// <summary>
        /// Layout object field.
        /// </summary>
        public static T ObjectField<T>(Rect rect, GUIContent content, T value, bool allowSceneObjects) where T : Object
        {
            return (T) EditorGUI.ObjectField(rect, content, value, typeof(T), allowSceneObjects);
        }

        /// <summary>
        /// Layout object field.
        /// </summary>
        public static T ObjectField<T>(Rect rect, string content, T value, bool allowSceneObjects) where T : Object
        {
            return (T) EditorGUI.ObjectField(rect, content, value, typeof(T), allowSceneObjects);
        }

        public static void MinMaxSlider(GUIContent content, ref float min, ref float max, float minLimit, float maxLimit)
        {
            GUILayout.BeginHorizontal();
            if (EditorGUI.indentLevel > 0)
            {
                GUILayout.Space(15);
            }
            GUILayout.Label(content);
            float width = EditorGUI.indentLevel > 0 ? EditorGUIUtility.labelWidth - 125 : EditorGUIUtility.labelWidth - 44;
            GUILayout.Space(width);
            float fieldWidth = EditorGUI.indentLevel > 0 ? 50 : 33;
            min = EditorGUILayout.FloatField(min, GUILayout.Width(fieldWidth));
            EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
            max = EditorGUILayout.FloatField(max, GUILayout.Width(fieldWidth));
            GUILayout.EndHorizontal();
        }

        public static void MinMaxSlider(string content, ref float min, ref float max, float minLimit, float maxLimit)
        {
            GUILayout.BeginHorizontal();
            if (EditorGUI.indentLevel > 0)
            {
                GUILayout.Space(15);
            }
            GUILayout.Label(content);
            float width = EditorGUI.indentLevel > 0 ? EditorGUIUtility.labelWidth - 81 : EditorGUIUtility.labelWidth - 41;
            GUILayout.Space(width);
            float fieldWidth = EditorGUI.indentLevel > 0 ? 53 : 33;
            min = EditorGUILayout.FloatField(min, GUILayout.Width(fieldWidth));
            EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
            max = EditorGUILayout.FloatField(max, GUILayout.Width(fieldWidth));
            GUILayout.EndHorizontal();
        }

        public static void MinMaxSlider(GUIContent content, ref int min, ref int max, int minLimit, int maxLimit)
        {
            float fmin = min;
            float fmax = max;
            GUILayout.BeginHorizontal();
            if (EditorGUI.indentLevel > 0)
            {
                GUILayout.Space(15);
            }
            GUILayout.Label(content);
            float width = EditorGUI.indentLevel > 0 ? EditorGUIUtility.labelWidth - 81 : EditorGUIUtility.labelWidth - 41;
            GUILayout.Space(width);
            float fieldWidth = EditorGUI.indentLevel > 0 ? 53 : 33;
            min = EditorGUILayout.IntField(min, GUILayout.Width(fieldWidth));
            EditorGUILayout.MinMaxSlider(ref fmin, ref fmax, minLimit, maxLimit);
            max = EditorGUILayout.IntField(max, GUILayout.Width(fieldWidth));
            GUILayout.EndHorizontal();
        }

        public static void MinMaxSlider(string content, ref int min, ref int max, int minLimit, int maxLimit)
        {
            float fmin = min;
            float fmax = max;
            GUILayout.BeginHorizontal();
            if (EditorGUI.indentLevel > 0)
            {
                GUILayout.Space(15);
            }
            GUILayout.Label(content);
            float width = EditorGUI.indentLevel > 0 ? EditorGUIUtility.labelWidth - 81 : EditorGUIUtility.labelWidth - 41;
            GUILayout.Space(width);
            float fieldWidth = EditorGUI.indentLevel > 0 ? 53 : 33;
            min = EditorGUILayout.IntField(min, GUILayout.Width(fieldWidth));
            EditorGUILayout.MinMaxSlider(ref fmin, ref fmax, minLimit, maxLimit);
            max = EditorGUILayout.IntField(max, GUILayout.Width(fieldWidth));
            GUILayout.EndHorizontal();
        }

        public static void TransformGroupField(ref Transform leftValue, ref Transform rightValue, GUIContent leftValueContent, GUIContent rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width -= 12;
            leftToggleValueRect.height = 15;
            leftToggleValueRect.x += 7;
            leftToggleValueRect.y += 4;
            leftValue = ObjectField<Transform>(leftToggleValueRect, GUIContent.none, leftValue, true);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width -= 12;
            rightToggleValueRect.height = 15;
            rightToggleValueRect.x += 7;
            rightToggleValueRect.y += 4;
            rightValue = ObjectField<Transform>(rightToggleValueRect, GUIContent.none, rightValue, true);
        }

        public static void TransformGroupField(ref Transform leftValue, ref Transform rightValue, string leftValueContent, string rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width -= 12;
            leftToggleValueRect.height = 15;
            leftToggleValueRect.x += 7;
            leftToggleValueRect.y += 4;
            leftValue = ObjectField<Transform>(leftToggleValueRect, GUIContent.none, leftValue, true);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width -= 12;
            rightToggleValueRect.height = 15;
            rightToggleValueRect.x += 7;
            rightToggleValueRect.y += 4;
            rightValue = ObjectField<Transform>(rightToggleValueRect, GUIContent.none, rightValue, true);
        }

        public static void BoolGroupField(ref bool leftValue, ref bool rightValue, GUIContent leftValueContent, GUIContent rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width = 13;
            leftToggleValueRect.height = 13;
            leftToggleValueRect.x = leftToggleValueGroupRect.x + leftToggleValueGroupRect.width / 2 - leftToggleValueRect.width;
            leftToggleValueRect.y += 4;
            leftValue = EditorGUI.Toggle(leftToggleValueRect, leftValue);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width = 13;
            rightToggleValueRect.height = 13;
            rightToggleValueRect.x = rightToggleValueGroupRect.x + rightToggleValueGroupRect.width / 2 - rightToggleValueRect.width;
            rightToggleValueRect.y += 4;
            rightValue = EditorGUI.Toggle(rightToggleValueRect, rightValue);
        }

        public static void BoolGroupField(ref bool leftValue, ref bool rightValue, string leftValueContent, string rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, RSEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, RSEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width = 13;
            leftToggleValueRect.height = 13;
            leftToggleValueRect.x = leftToggleValueGroupRect.x + leftToggleValueGroupRect.width / 2 - leftToggleValueRect.width;
            leftToggleValueRect.y += 4;
            leftValue = EditorGUI.Toggle(leftToggleValueRect, leftValue);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width = 13;
            rightToggleValueRect.height = 13;
            rightToggleValueRect.x = rightToggleValueGroupRect.x + rightToggleValueGroupRect.width / 2 - rightToggleValueRect.width;
            rightToggleValueRect.y += 4;
            rightValue = EditorGUI.Toggle(rightToggleValueRect, rightValue);
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenFloatField(GUIContent valueContent, GUIContent toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.FloatField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenFloatField(string valueContent, string toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.FloatField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedFloatField(GUIContent valueContent, GUIContent toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedFloatField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedFloatField(string valueContent, string toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedFloatField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDoubleField(GUIContent valueContent, GUIContent toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDoubleField(string valueContent, string toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedDoubleField(GUIContent valueContent, GUIContent toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedDoubleField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedDoubleField(string valueContent, string toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenIntField(GUIContent valueContent, GUIContent toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenIntField(string valueContent, string toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedIntField(GUIContent valueContent, GUIContent toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedIntField(string valueContent, string toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void TextHiddenField(GUIContent valueContent, GUIContent toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.TextField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void TextHiddenField(string valueContent, string toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.TextField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedTextField(GUIContent valueContent, GUIContent toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedTextField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenDelayedTextField(string valueContent, string toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedTextField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenColorField(GUIContent valueContent, GUIContent toggleContent, ref Color value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.ColorField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenColorField(string valueContent, string toggleContent, ref Color value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.ColorField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector2Field(GUIContent valueContent, GUIContent toggleContent, ref Vector2 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector2Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector2Field(string valueContent, string toggleContent, ref Vector2 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector2Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector3Field(GUIContent valueContent, GUIContent toggleContent, ref Vector3 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector3Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector3Field(string valueContent, string toggleContent, ref Vector3 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector3Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector4Field(GUIContent valueContent, GUIContent toggleContent, ref Vector4 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector4Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenVector4Field(string valueContent, string toggleContent, ref Vector4 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector4Field(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenCurveField(GUIContent valueContent, GUIContent toggleContent, ref AnimationCurve value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.CurveField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        public static void HiddenCurveField(string valueContent, string toggleContent, ref AnimationCurve value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.CurveField(valueContent, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Hiddel by toggle deafult object field.
        /// </summary>
        public static void ObjectHiddenField<T>(string valueContent, string toggleContent, ref T value, ref bool toggle) where T : Object
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!toggle);
                value = (T) EditorGUILayout.ObjectField(valueContent, value, typeof(T), true);
                EditorGUI.EndDisabledGroup();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Hiddel by toggle deafult object field.
        /// </summary>
        public static void ObjectHiddenField<T>(GUIContent valueContent, GUIContent toggleContent, ref T value, ref bool toggle) where T : Object
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!toggle);
                value = (T) EditorGUILayout.ObjectField(valueContent, value, typeof(T), true);
                EditorGUI.EndDisabledGroup();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="height"></param>
        public static void HorizontalLine(float separate = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, separate);
            rect.x -= 3;
            rect.width += 7;
            GUI.Label(rect, GUIContent.none, GUI.skin.GetStyle("IN Title"));
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void VerticalLine(float height, float width = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.height = height;
            rect.width = width;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void VerticalDividingLine(float height, Color color, float width = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.height = height;
            rect.width = width;
            EditorGUI.DrawRect(rect, color);
        }

        public static void Notification(string message, Rect notificationBackgroungRect, Rect notificationTextRect)
        {
            GUI.Label(notificationBackgroungRect, "", GUI.skin.GetStyle("NotificationBackground"));
            GUI.Label(notificationTextRect, message, RSEditorStyles.WhiteLargeBoldLabel);
        }

        public static void Notification(string message)
        {
            Rect notificationBackgroungRect = GUILayoutUtility.GetLastRect();
            Rect notificationTextRect = GUILayoutUtility.GetLastRect();

            notificationBackgroungRect.y -= 40;
            notificationBackgroungRect.height = 58.5f;

            notificationTextRect.y -= 43;
            notificationTextRect.height = 60;

            GUI.Label(notificationBackgroungRect, "", GUI.skin.GetStyle("NotificationBackground"));
            GUI.Label(notificationTextRect, message, RSEditorStyles.WhiteLargeBoldLabel);
        }

        public static bool MiniButton(GUIContent content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("MiniButton"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool Button(GUIContent content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("Button"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool LargeButton(GUIContent content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("LargeButton"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool MiniButton(string content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("MiniButton"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool Button(string content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("Button"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool LargeButton(string content, string side = "Right", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, GUI.skin.GetStyle("LargeButton"), options))
            {
                return true;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return false;
        }

        public static bool SearchButton()
        {
            Texture2D icon = RSEditorResourcesHelper.GetIcon("Search");
            GUILayout.BeginVertical(GUILayout.Width(20));
            if (GUILayout.Button(icon != null ? ContentProperties.SearchButton : new GUIContent("S", ContentProperties.SearchButton.tooltip), GUILayout.Width(21), GUILayout.Height(18)))
            {
                return true;
            }
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.width = 15;
            rect.height = 15;
            rect.y -= 19f;
            rect.x += 7.5f;
            if (icon != null)
                GUI.DrawTexture(rect, icon);
            GUILayout.EndVertical();
            return false;
        }

        public static bool GenerateIDButton()
        {
            Texture2D icon = RSEditorResourcesHelper.GetIcon("ID");
            GUILayout.BeginVertical(GUILayout.Width(20));
            if (GUILayout.Button(icon != null ? ContentProperties.GenerateIDButton : new GUIContent("G", ContentProperties.GenerateIDButton.tooltip), GUILayout.Width(21), GUILayout.Height(18)))
            {
                return true;
            }
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.width = 15;
            rect.height = 15;
            rect.y -= 19f;
            rect.x += 7.5f;
            if (icon != null)
                GUI.DrawTexture(rect, icon);
            GUILayout.EndVertical();
            return false;
        }

        public static bool ListButton()
        {
            Texture2D icon = RSEditorResourcesHelper.GetIcon("List");
            GUILayout.BeginVertical(GUILayout.Width(20));
            if (GUILayout.Button(icon != null ? ContentProperties.ListButton : new GUIContent("L", ContentProperties.ListButton.tooltip), GUILayout.Width(21), GUILayout.Height(18)))
            {
                return true;
            }
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.width = 14;
            rect.height = 14;
            rect.y -= 19f;
            rect.x += 7.5f;
            if (icon != null)
                GUI.DrawTexture(rect, icon);
            GUILayout.EndVertical();
            return false;
        }

        public static bool GenerateButton()
        {
            Texture2D icon = RSEditorResourcesHelper.GetIcon("Generate");
            GUILayout.BeginVertical(GUILayout.Width(20));
            if (GUILayout.Button(icon != null ? ContentProperties.GenerateButton : new GUIContent("G", ContentProperties.GenerateButton.tooltip), GUILayout.Width(21), GUILayout.Height(18)))
            {
                return true;
            }
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.width = 14;
            rect.height = 14;
            rect.y -= 19f;
            rect.x += 7.5f;
            if (icon != null)
                GUI.DrawTexture(rect, icon);
            GUILayout.EndVertical();
            return false;
        }

        public static bool ToggleButton(GUIContent content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(GUI.skin.button);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : GUI.skin.button, options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool MiniToggleButton(GUIContent content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(EditorStyles.miniButton);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : EditorStyles.miniButton, options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool LargeToggleButton(GUIContent content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle("LargeButton");
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : GUI.skin.GetStyle("LargeButton"), options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool ToggleButton(string content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(GUI.skin.button);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : GUI.skin.button, options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool MiniToggleButton(string content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(EditorStyles.miniButton);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : EditorStyles.miniButton, options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool LargeToggleButton(string content, bool value, string side = "Right", params GUILayoutOption[] options)
        {
            GUIStyle pressedButtonStyle = new GUIStyle("LargeButton");
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            GUILayout.BeginHorizontal();
            if (side == "Right" || side == "Middle")
                GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, value ? pressedButtonStyle : GUI.skin.GetStyle("LargeButton"), options))
            {
                value = !value;
            }
            if (side == "Left" || side == "Middle")
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool ToggleButton(string content, bool value, GUIStyle style)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(style);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            if (GUILayout.Button(content, value ? pressedButtonStyle : style))
            {
                value = !value;
            }
            return value;
        }

        public static bool ToggleButton(GUIContent content, bool value, GUIStyle style)
        {
            GUIStyle pressedButtonStyle = new GUIStyle(style);
            pressedButtonStyle.normal.background = pressedButtonStyle.active.background;
            if (GUILayout.Button(content, value ? pressedButtonStyle : style))
            {
                value = !value;
            }
            return value;
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
    }
}