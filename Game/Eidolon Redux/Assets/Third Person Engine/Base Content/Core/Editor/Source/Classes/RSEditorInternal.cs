/* ==================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public static class RSEditorInternal
    {
        /// <summary>
        /// Find first person player with Player tag in scene.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag(TNC.PLAYER);
            if (player != null)
                return player.transform;
            return null;
        }

        public static T FindComponent<T>(Transform transform) where T : Component
        {
            if (transform == null)
                return null;

            T t = transform.GetComponentInChildren<T>();
            if (t != null)
                return t;
            return null;
        }

        public static T AddComponent<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
                component = gameObject.AddComponent<T>();
            return component;
        }

        public static Component AddComponent(System.Type type, GameObject gameObject)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
                component = gameObject.AddComponent(type);
            return component;
        }

        /// <summary>
        /// Move component with type T to top in inspector.
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void MoveComponentTop<T>(Transform target) where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(targetComponent);
            }
        }

        /// <summary>
        /// Move component with type T to bottom in inspector.
        /// </summary>
        public static void MoveComponentBottom<T>(Transform target) where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(targetComponent);
            }
        }

        /// <summary>
        /// Move component with type T to bottom in inspector.
        /// </summary>
        public static void MoveComponentBottom(System.Type type, Transform target)
        {
            Component[] components = target.GetComponents<Component>();
            
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetType() == type)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(components[index]);
            }
        }

        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static List<Object> FindAssetsByType(System.Type type)
        {
            List<Object> assets = new List<Object>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// Return all animation clips from transform animator.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static AnimationClip[] GetAllClips(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.runtimeAnimatorController.animationClips;
        }

        /// <summary>
        /// Return all animator parameter names.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAnimatorParameterNames(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters.Select(n => n.name).ToArray();
        }

        /// <summary>
        /// Return all animator parameters.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.AnimatorControllerParameter[] GetAnimatorParameters(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters;
        }

        /// <summary>
        /// Get all inputs that contains in project
        /// </summary>
        public static string[] GetAllInputs()
        {
            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset") [0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axesArray = obj.FindProperty("m_Axes");
            List<string> inputs = new List<string>();
            for (int i = 0; i < axesArray.arraySize; ++i)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);
                string name = axis.FindPropertyRelative("m_Name").stringValue;
                inputs.Add(name);
            }
            return inputs.ToArray();
        }

        /// <summary>
        /// Verify project inputs and return missing require Third Person Engine inputs. 
        /// </summary>
        /// <param name="type">[Axes], [Buttons], [All]</param>
        /// <returns>Axes / Buttons / All</returns>
        public static string[] GetMissingInput(string type)
        {
            List<string> missingAxes = new List<string>();
            List<string> incAxes = new List<string>();
            switch (type)
            {
                case "Axes":
                    incAxes.AddRange(INC.Axes);
                    break;
                case "Buttons":
                    incAxes.AddRange(INC.Buttons);
                    break;
                case "All":
                    incAxes.AddRange(INC.Axes);
                    incAxes.AddRange(INC.Buttons);
                    break;
                default:
                    return null;
            }

            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset") [0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axesArray = obj.FindProperty("m_Axes");
            List<string> axesArrayList = new List<string>();
            for (int i = 0; i < axesArray.arraySize; ++i)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);
                string name = axis.FindPropertyRelative("m_Name").stringValue;
                axesArrayList.Add(name);
            }
            for (int i = 0; i < incAxes.Count; i++)
            {
                bool contains = axesArrayList.Any(t => t == incAxes[i]);
                if (contains)
                    continue;
                else
                    missingAxes.Add(incAxes[i]);
            }
            return missingAxes.ToArray();
        }

        /// <summary>
        /// Verify project tags and return missing require Third Person Engine tags. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingTags()
        {
            string[] editorTags = InternalEditorUtility.tags;
            string[] uTags = TNC.Tags;
            List<string> missingTags = new List<string>();
            for (int i = 0; i < uTags.Length; i++)
            {
                bool contain = false;
                for (int j = 0; j < editorTags.Length; j++)
                {
                    if (uTags[i] == editorTags[j])
                    {
                        contain = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!contain)
                {
                    missingTags.Add(uTags[i]);
                }
            }
            return missingTags.ToArray();
        }

        /// <summary>
        /// Verify project layers and return missing require Third Person Engine layers. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingLayers()
        {
            string[] editorLayers = InternalEditorUtility.layers;
            string[] requireLayers = LNC.Layers;
            List<string> missingLayers = new List<string>();
            for (int i = 0; i < requireLayers.Length; i++)
            {
                bool contain = editorLayers.Any(t => t == requireLayers[i]);
                if (contain)
                    continue;
                else
                    missingLayers.Add(requireLayers[i]);
            }
            return missingLayers.ToArray();
        }

        /// <summary>
        /// Auto add all require Third Person Engine missing tags in project settings.
        /// </summary>
        public static void AddMissingTags()
        {
            string[] missingTags = GetMissingTags();
            if (missingTags != null && missingTags.Length == 0)
            {
                RSDisplayDialogs.Message("Tags", "Your project has all the necessary tags!");
                return;
            }
            else if (missingTags == null)
            {
                return;
            }

            for (int i = 0; i < missingTags.Length; i++)
            {
                InternalEditorUtility.AddTag(missingTags[i]);
            }
        }

        /// <summary>
        /// Add spaces to this line
        /// 
        ///     Note: Spaces are added only between  capital letters.
        /// </summary>
        /// <returns>Return string</returns>
        public static string AddSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static IEnumerable<string> GetNamespacesInAssembly(Assembly assembly)
        {
            System.Type[] types = assembly.GetTypes();

            return types.Select(t => t.Namespace)
                .Distinct()
                .Where(n => n != null);
        }

        public static bool ContatinsNamespace(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] namespaces = GetNamespacesInAssembly(assembly).ToArray();
            for (int i = 0, length = namespaces.Length; i < length; i++)
            {
                if(namespaces[i] == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}