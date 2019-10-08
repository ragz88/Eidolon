/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ThirdPersonEngine.Runtime;
using ThirdPersonEngine.Utility;
using Object = UnityEngine.Object;

namespace ThirdPersonEngine.Editor
{
    public sealed class PropertiesItem : ManagerItem
    {
        private static Type[] propertiesTypes;
        private static List<Object> propertiesObjects;
        private static string[] propertiesNames;
        private static int selectedPropertyType;
        private static int lastSelectedPropertyType;

        public override string GetDisplayName()
        {
            return "Properties";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.Properties;
        }

        public override void OnInitializeProperties()
        {
            propertiesTypes = GetAllProperties();
            propertiesNames = new string[propertiesTypes.Length];
            for (int i = 0, length = propertiesTypes.Length; i < length; i++)
            {
                propertiesNames[i] = propertiesTypes[i].Name.AddSpaces();
            }
            propertiesObjects = RSEditorInternal.FindAssetsByType(propertiesTypes[selectedPropertyType]);
        }

        public override void OnPropertiesGUI()
        {
            GUILayout.Space(10);
            selectedPropertyType = EditorGUILayout.Popup("Type", selectedPropertyType, propertiesNames);
            GUILayout.Space(5);

            if (selectedPropertyType != lastSelectedPropertyType)
            {
                OnInitializeProperties();
                lastSelectedPropertyType = selectedPropertyType;
            }

            if (propertiesObjects != null && propertiesObjects.Count > 0)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Space(3);

                for (int i = 0; i < propertiesObjects.Count; i++)
                {
                    if (i >= propertiesObjects.Count)
                    {
                        break;
                    }

                    Object propertyObject = propertiesObjects[i];
                    if (propertyObject == null)
                    {
                        OnInitializeProperties();
                        continue;
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(3);
                    GUILayout.Label(propertyObject.name, EditorStyles.boldLabel);

                    GUI.color = new Color32(70, 150, 255, 255);
                    if (GUILayout.Button("Open", "ButtonLeft", GUILayout.Width(70)))
                    {
                        Selection.activeObject = propertyObject;
                    }

#if UNITY_EDITOR_OSX
                    EditorGUI.BeginDisabledGroup(true);
#endif

                    GUI.color = new Color32(70, 220, 70, 255);
                    if (GUILayout.Button("Duplicate", "ButtonMid", GUILayout.Width(70)))
                    {
                        string path = AssetDatabase.GetAssetPath(propertyObject);
                        string name = propertyObject.name;
                        path = path.Replace(propertyObject.name + ".asset", "");
                        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(propertyObject), path + name + ".asset");
                        AssetDatabase.Refresh();
                        OnInitializeProperties();
                    }

#if UNITY_EDITOR_OSX
                    EditorGUI.EndDisabledGroup();
#endif

                    GUI.color = new Color32(255, 55, 55, 255);
                    if (GUILayout.Button("Delete", "ButtonRight", GUILayout.Width(70)))
                    {
                        if (RSDisplayDialogs.Confirmation("Are you really want to delete \"" + propertyObject.name + "\" asset?"))
                        {
                            string path = AssetDatabase.GetAssetPath(propertyObject);
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.Refresh();
                            OnInitializeProperties();
                            break;
                        }
                    }
                    GUI.color = Color.white;
                    GUILayout.Space(3);
                    GUILayout.EndHorizontal();

                    if (propertiesObjects.Count > 1 && i < propertiesObjects.Count - 1)
                    {
                        GUILayout.Space(3);
                        RSEditor.HorizontalLine();
                        GUILayout.Space(3);
                    }
                    else
                    {
                        GUILayout.Space(3);
                    }
                }
                GUILayout.Space(3);
                GUILayout.EndVertical();
            }
            else
            {
                RSEditorHelpBoxMessages.Message(propertiesNames[selectedPropertyType].AddSpaces() + " not found...\n" + "Create new property or reload properties", MessageType.Warning);
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Create New", "ButtonLeft", GUILayout.Width(105.5f)))
            {
                string path = EditorUtility.SaveFilePanelInProject("Create new Property", string.Format("New {0}", propertiesTypes[selectedPropertyType].Name).AddSpaces(), "", "");
                if (!string.IsNullOrEmpty(path))
                {
                    string name = System.IO.Path.GetFileName(path);
                    path = path.Replace(name, "");
                    ScriptableObjectUtility.CreateAsset(propertiesTypes[selectedPropertyType], path, name);
                    OnInitializeProperties();
                }
            }
            if (GUILayout.Button(" Refresh Assets ", "ButtonRight", GUILayout.Width(105.5f)))
            {
                OnInitializeProperties();
            }
            GUILayout.Space(3.5f);
            GUILayout.EndHorizontal();
        }

        public static Type[] GetAllProperties()
        {
            Type type = typeof(IProperties<>);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)).ToArray();
        }
    }
}