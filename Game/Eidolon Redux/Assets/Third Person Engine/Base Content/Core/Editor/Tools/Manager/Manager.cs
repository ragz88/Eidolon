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
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeViewExamples;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public partial class Manager : EditorWindow
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Title = new GUIContent("Manager", "Manager title.");
        }

        private TreeViewState treeViewState;
        private PMTreeView treeView;
        private SearchField searchField;
        private ManagerItem[] managerItems;
        private bool managerItemsInitialize = false;
        private int lastSelectedID;
        private int currentItemIndex;
        private int lastItemIndex;
        private bool initializeFont;

        private Vector2 scrollPosItems;
        private Vector2 scrollPosEditor;

        /// <summary>
        /// Open UManager window.
        /// </summary>
        [MenuItem(RSEditorPaths.MENUITEM_DEFAULT_PATH + "Manager", false, 91)]
        public static void Open()
        {
            Manager window = GetWindow<Manager>();
            Texture2D icon = RSEditorResourcesHelper.GetIcon("Window");
            window.titleContent = ContentProperties.Title;
            window.titleContent.image = icon;
            window.maxSize = new Vector2(800, 600);
            window.minSize = new Vector2(440, 300);
            window.Show();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            managerItems = ReflectionHelper.GetItems();

            InitializeTreeView();

            OnManagerOpenOverride();
            OnManagerEnableOverride();
        }
        
        /// <summary>
        /// Called when the window gets keyboard or mouse focus.
        /// </summary>
        protected virtual void OnFocus()
        {
            OnManagerFocusOverride();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        protected virtual void OnGUI()
        {
            
            GUILayout.BeginVertical();
            DrawSearch();

            if (managerItems != null && managerItems.Length > 0)
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
                GUILayout.Label("Manager items is empty...", RSEditorStyles.GroupHeaderLabel);
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
            if (managerItems == null || managerItems.Length == 0)
            {
                return;
            }

            managerItems[currentItemIndex].OnManagerClosed();
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
            treeView = new PMTreeView(treeViewState, managerItems);
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
            // Debug.Log(string.Format("Called: {0}", selectedID));
            if (selectedID < 0)
            {
                GUILayout.Label(GUIContent.none);
                return;
            }

            if (lastSelectedID != selectedID)
            {
                for (int i = 0, length = managerItems.Length; i < length; i++)
                {
                    if (managerItems[i].GetID() == selectedID)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            if (managerItems[j].GetID() == lastSelectedID)
                            {
                                managerItems[j].OnDisable();
                            }
                        }
                        managerItems[i].OnSelect();
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
                managerItems[currentItemIndex].OnMainGUI();
                managerItems[currentItemIndex].Size = position.center;
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Called once when Project manager is open.
        /// </summary>
        protected virtual void OnManagerOpenOverride()
        {
            if (!managerItemsInitialize)
            {
                for (int i = 0, length = managerItems.Length; i < length; i++)
                {
                    managerItems[i].OnManagerOpen();
                }
                managerItemsInitialize = true;
            }
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        protected virtual void OnManagerEnableOverride()
        {
            for (int i = 0, length = managerItems.Length; i < length; i++)
            {
                managerItems[i].OnManagerEnable();
            }
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        protected virtual void OnManagerFocusOverride()
        {
            for (int i = 0, length = managerItems.Length; i < length; i++)
            {
                managerItems[i].OnManagerFocus();
            }
        }
    }
}