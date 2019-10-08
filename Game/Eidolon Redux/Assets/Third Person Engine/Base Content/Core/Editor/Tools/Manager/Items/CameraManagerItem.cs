/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using ThirdPersonEngine.Runtime;
using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    public class CameraManagerItem : ManagerItem
    {
        public override string GetDisplayName()
        {
            return "Camera";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.General;
        }

        [System.Serializable]
        protected struct Settings
        {
            public string name;
            public TPController controller;
        }

        private GameObject camera;
        private Settings settings;
        private EditorDelay delay;

        public override void OnInitializeProperties()
        {
            settings.name = "New Camera";
            delay = new EditorDelay(0.25f);
        }

        public override void OnPropertiesGUI()
        {
            settings.name = EditorGUILayout.TextField("Name", settings.name);
            if (string.IsNullOrEmpty(settings.name))
            {
                EditorGUILayout.HelpBox("Name field cannot be empty!", MessageType.Error);
            }

            settings.controller = RSEditor.ObjectField<TPController>("Controller", settings.controller, true);
            if (settings.controller == null)
            {
                EditorGUILayout.HelpBox("Controller empty!", MessageType.Warning);
            }

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(settings.name));
            if (RSEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                camera = CreateCamera(settings);
            }
            if (camera != null && delay.WaitForSeconds())
            {
                if (RSDisplayDialogs.Message("Create Successful", "Camera was created on scene!\nSetup Camera components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = camera;
                }
                camera = null;
            }
            EditorGUI.EndDisabledGroup();
        }

        protected virtual GameObject CreateCamera(Settings settings)
        {
            GameObject camera = new GameObject(string.Format("Camera [{0}]", settings.name));
            camera.tag = TNC.CAMERA;

            Camera cameraComponent = RSEditorInternal.AddComponent<Camera>(camera);
            FlareLayer flareLayer = RSEditorInternal.AddComponent<FlareLayer>(camera);
            AudioListener audioListener = RSEditorInternal.AddComponent<AudioListener>(camera);
            TPCamera cameraSystem = RSEditorInternal.AddComponent<TPCamera>(camera);

            RSEditorInternal.MoveComponentBottom<Camera>(camera.transform);
            RSEditorInternal.MoveComponentBottom<FlareLayer>(camera.transform);
            RSEditorInternal.MoveComponentBottom<AudioListener>(camera.transform);
            RSEditorInternal.MoveComponentBottom<TPCamera>(camera.transform);

            if (settings.controller != null)
            {
                cameraSystem.SetTargetController(settings.controller);
            }

            return camera;
        }
    }
}