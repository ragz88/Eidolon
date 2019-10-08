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
    /// Third Person Engine generic editor class.
    /// </summary>
    public class RSEditor<T> : RSEditor where T : Object
    {
        protected T instance;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            instance = (T) target as T;
            base.OnEnable();
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
            if (instance != null)
                OnBaseGUI();
            else
                ReInitInstanceGUI();
            EndBody();
            EndBackground();
            SaveInstanceProperties();
            serializedObject.ApplyModifiedProperties();
        }

        public override string GetHeaderName()
        {
            const string EDITOR = "Editor";
            string name = instance.GetType().Name;
            if (name.Contains(EDITOR))
            {
                name = name.Remove(name.IndexOf(EDITOR), EDITOR.Length);
            }
            name = name.AddSpaces();
            return name;
        }

        /// <summary>
        /// Reinitialize instance.
        /// </summary>
        public void ReInitInstanceGUI()
        {
            RSEditorHelpBoxMessages.Error("Instance not loaded...", "Try Reinitialize instance by [Reinitialize] button.", true);
            if (Button("Reinitialize"))
            {
                OnEnable();
                Repaint();
            }
        }

        /// <summary>
        /// Save dirty instance properties.
        /// </summary>
        protected virtual void SaveInstanceProperties()
        {
            if (GUI.changed && instance != null)
            {
                EditorUtility.SetDirty(instance);
            }
        }
    }
}