/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public partial class SetupAssistant : EditorWindow
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Title = new GUIContent("Setup Assistant", "Setup Assistant title.");
        }

        private TreeViewState treeViewState;
        private AssistantTreeView treeView;
        private SearchField searchField;
        private AssistantItem[] items;
        private bool itemsInitialize = false;
        private int lastSelectedID;
        private int currentItemIndex;
        private int lastItemIndex;
        private bool initializeFont;

        private Vector2 scrollPosItems;
        private Vector2 scrollPosEditor;

        /// <summary>
        /// Open Setup Assistent window.
        /// </summary>
        [MenuItem(RSEditorPaths.MENUITEM_DEFAULT_PATH + "Setup Assistent", false, 91)]
        public static void Open()
        {
            SetupAssistant window = GetWindow<SetupAssistant>();
            window.titleContent = ContentProperties.Title;
            window.maxSize = new Vector2(600, 400);
            window.minSize = new Vector2(300, 200);
            window.ShowAuxWindow();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        protected virtual void OnFocus()
        {
            items = ReflectionHelper.GetItems();

            InitializeTreeView();

            OnManagerOpenOverride();
            OnInitializeOverride();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        protected virtual void OnGUI()
        {
            GUILayout.BeginVertical();
            DrawSearch();

            if (items != null && items.Length > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                DrawTreeView();
                GUILayout.EndVertical();

                RSEditor.VerticalLine(Screen.height, 1.5f);

                GUILayout.BeginVertical(GUILayout.MinWidth(300), GUILayout.MaxWidth(Screen.width + 20));
                scrollPosEditor = GUILayout.BeginScrollView(scrollPosEditor);
                DrawItemGUI(treeViewState.lastClickedID);
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Space(5);
                GUILayout.Label("Setups is empty...", RSEditorStyles.GroupHeaderLabel);
                GUILayout.FlexibleSpace();
            }

            GUILayout.Label(GUIContent.none, "ToolbarButton");
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            items[currentItemIndex].OnAssistantClosed();
        }

        /// <summary>
        /// OnInspectorUpdate is called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        protected virtual void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Initialize TreeView and SearchField.
        /// </summary>
        protected virtual void InitializeTreeView()
        {
            if (treeViewState == null)
                treeViewState = new TreeViewState();
            treeView = new AssistantTreeView(treeViewState, items);
            treeView.ExpandAll();
            searchField = new SearchField();
            searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
        }

        /// <summary>
        /// Draw TreeView layout.
        /// </summary>
        protected virtual void DrawTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(100, 300, 0, Screen.height);
            treeView.OnGUI(rect);
        }

        /// <summary>
        /// Draw TreeView search layout.
        /// </summary>
        protected virtual void DrawSearch()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Space(100);
            GUILayout.FlexibleSpace();
            treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw UItem GUI methods in UManager window.
        /// </summary>
        protected virtual void DrawItemGUI(int selectedID)
        {
            if (selectedID < 0)
            {
                GUILayout.Label(GUIContent.none);
                return;
            }

            if (lastSelectedID != selectedID)
            {
                for (int i = 0, length = items.Length; i < length; i++)
                {
                    if (items[i].GetID() == selectedID)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            if (items[j].GetID() == lastSelectedID)
                            {
                                items[j].OnDisable();
                            }
                        }
                        items[i].OnSelect();
                        lastSelectedID = selectedID;
                        currentItemIndex = i;
                    }
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginVertical();
                items[currentItemIndex].OnMainGUI();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Called once when Setup Assistant is open.
        /// </summary>
        protected virtual void OnManagerOpenOverride()
        {
            if (!itemsInitialize)
            {
                for (int i = 0, length = items.Length; i < length; i++)
                {
                    items[i].OnAssistantOpen();
                }
                itemsInitialize = true;
            }
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base AssistantItem properties.
        /// </summary>
        protected virtual void OnInitializeOverride()
        {
            for (int i = 0, length = items.Length; i < length; i++)
            {
                items[i].OnInitialize();
            }
        }
        /// <summary>
        /// Draw project setup complete status bar.
        /// </summary>
        protected virtual void DrawProjectSetupStatusBar(AssistantItem[] items)
        {
            if (items == null && items.Length == 0)
            {
                return;
            }
            int maxCount = items.Length;
            int nonReadyCount = items.Where(t => t.IsReady() == -1).ToArray().Length;

            float value = Mathf.InverseLerp(maxCount, 0, nonReadyCount);

            GUILayout.BeginHorizontal();
            GUILayout.Space(3);
            Rect rect = GUILayoutUtility.GetRect(1, 18);
            EditorGUI.ProgressBar(rect, value, "");
            string valueLabel = value > 0 ? (value * 100).ToString("#") + "%" : 0 + "%";
            EditorGUI.LabelField(rect, "Complete: " + valueLabel, RSEditorStyles.CenteredBoldLabel);
            GUILayout.Space(3);
            GUILayout.EndHorizontal();
        }
    }
}