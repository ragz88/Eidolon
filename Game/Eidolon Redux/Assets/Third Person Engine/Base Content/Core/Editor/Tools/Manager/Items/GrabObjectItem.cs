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
    public class GrabObjectItem : ManagerItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Name = new GUIContent("Name", "Prefab name.");
            public readonly static GUIContent Prefab = new GUIContent("Prefab");
        }

        private string name;
        private GameObject prefab;
        private GrabObject grabObject;
        private IEditorDelay editorDelay;

        public override void OnInitializeProperties()
        {
            name = "Grab Object";
            editorDelay = new EditorDelay(0.25f);
        }

        public override void OnPropertiesGUI()
        {
            bool nameConditions = string.IsNullOrEmpty(name);
            name = EditorGUILayout.TextField(ContentProperties.Name, name);
            if (nameConditions)
            {
                RSEditorHelpBoxMessages.Message("Name field cannot be empty!", MessageType.Error);
            }
            prefab = RSEditor.ObjectField<GameObject>(ContentProperties.Prefab, prefab, true);

            EditorGUI.BeginDisabledGroup(nameConditions || prefab == null);
            if (RSEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                grabObject = CreateGrabObject(prefab);
            }
            EditorGUI.EndDisabledGroup();
            if (grabObject != null && editorDelay.WaitForSeconds())
            {
                if (RSDisplayDialogs.Message("Create Successful", "Grab Object was created on scene!\nSetup Grab Object components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = grabObject.gameObject;
                }
                grabObject = null;
            }
        }

        public override string GetDisplayName()
        {
            return "Grab Object";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.General;
        }

        public GrabObject CreateGrabObject(GameObject prefab)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            gameObject.name = name;
            gameObject.layer = LayerMask.NameToLayer(LNC.GRABBABLE_OBJECT);

            GrabObject grabObject = RSEditorInternal.AddComponent<GrabObject>(gameObject);
            MeshCollider meshCollider = RSEditorInternal.AddComponent<MeshCollider>(gameObject);
            Rigidbody rigidbody = RSEditorInternal.AddComponent<Rigidbody>(gameObject);

            RSEditorInternal.MoveComponentBottom<GrabObject>(gameObject.transform);
            RSEditorInternal.MoveComponentBottom<MeshCollider>(gameObject.transform);
            RSEditorInternal.MoveComponentBottom<Rigidbody>(gameObject.transform);

            GameObject leftHandIKTarget = new GameObject("LEFT_HAND_IK_TARGET");
            GameObject rightHandIKTarget = new GameObject("RIGHT_HAND_IK_TARGET");

            leftHandIKTarget.transform.SetParent(gameObject.transform);
            rightHandIKTarget.transform.SetParent(gameObject.transform);

            leftHandIKTarget.transform.localPosition = new Vector3(-0.7f, 0.0f, 0.0f);
            rightHandIKTarget.transform.localPosition = new Vector3(0.7f, 0.0f, 0.0f);

            grabObject.SetLeftHandIKTarget(leftHandIKTarget.transform);
            grabObject.SetRightHandIKTarget(rightHandIKTarget.transform);

            meshCollider.convex = true;

            return grabObject;
        }
    }
}