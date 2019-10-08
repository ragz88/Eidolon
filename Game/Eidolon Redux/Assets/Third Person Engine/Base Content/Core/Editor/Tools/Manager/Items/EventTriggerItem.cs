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
    public class EventTriggerItem : ManagerItem
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Name = new GUIContent("Name", "Event Trigger name.");
            public readonly static GUIContent TriggerShape = new GUIContent("Trigger Shape");
            public readonly static GUIContent Mesh = new GUIContent("Mesh");
        }

        private enum TriggerShape
        {
            Box,
            Sphere,
            Capsule,
            Custom
        }

        private string name;
        private TriggerShape triggerShape;
        private Mesh mesh;
        private EventTrigger eventTrigger;
        private IEditorDelay editorDelay;

        public override void OnInitializeProperties()
        {
            name = "Event Trigger";
            triggerShape = TriggerShape.Box;
            editorDelay = new EditorDelay(0.25f);
        }

        public override void OnPropertiesGUI()
        {
            name = EditorGUILayout.TextField(ContentProperties.Name, name);
            if (string.IsNullOrEmpty(name))
            {
                RSEditorHelpBoxMessages.Message("Name field cannot be empty!");
            }
            triggerShape = (TriggerShape) EditorGUILayout.EnumPopup(ContentProperties.TriggerShape, triggerShape);
            if (triggerShape == TriggerShape.Custom)
            {
                mesh = RSEditor.ObjectField<Mesh>(ContentProperties.Mesh, mesh, true);
            }

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(name) || (triggerShape == TriggerShape.Custom && mesh == null));
            if (RSEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                eventTrigger = CreateEventTrigger();
            }
            if (eventTrigger != null && editorDelay.WaitForSeconds())
            {
                if (RSDisplayDialogs.Message("Create Successful", "Event Triggger was created on scene!\nSetup Event Triggger components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = eventTrigger.gameObject;
                }
                eventTrigger = null;
            }
            EditorGUI.EndDisabledGroup();
        }

        private EventTrigger CreateEventTrigger()
        {
            GameObject eventTriggerObject = new GameObject(name);
            eventTriggerObject.isStatic = true;

            EventTrigger eventTrigger = RSEditorInternal.AddComponent<EventTrigger>(eventTriggerObject);
            Collider collider = null;
            MeshFilter meshFilter = null;

            switch (triggerShape)
            {
                case TriggerShape.Box:
                    collider = RSEditorInternal.AddComponent<BoxCollider>(eventTriggerObject);
                    break;
                case TriggerShape.Sphere:
                    collider = RSEditorInternal.AddComponent<SphereCollider>(eventTriggerObject);
                    break;
                case TriggerShape.Capsule:
                    collider = RSEditorInternal.AddComponent<CapsuleCollider>(eventTriggerObject);
                    break;
                case TriggerShape.Custom:
                    meshFilter = RSEditorInternal.AddComponent<MeshFilter>(eventTriggerObject);
                    MeshCollider meshCollider = RSEditorInternal.AddComponent<MeshCollider>(eventTriggerObject);
                    meshCollider.convex = true;
                    meshCollider.sharedMesh = mesh;
                    collider = meshCollider;
                    break;
            }

            if (meshFilter != null)
            {
                meshFilter.sharedMesh = mesh;
                RSEditorInternal.MoveComponentBottom<MeshFilter>(eventTriggerObject.transform);
                MeshRenderer meshRenderer = RSEditorInternal.AddComponent<MeshRenderer>(eventTriggerObject);
                meshRenderer.enabled = false;
                RSEditorInternal.MoveComponentBottom<MeshRenderer>(eventTriggerObject.transform);
            }

            RSEditorInternal.MoveComponentBottom<EventTrigger>(eventTriggerObject.transform);

            if (collider != null)
            {
                collider.isTrigger = true;
                RSEditorInternal.MoveComponentBottom<Collider>(eventTriggerObject.transform);
            }

            return eventTrigger;
        }

        public override string GetDisplayName()
        {
            return "Event Trigger";
        }

        public override ManagerSection GetSection()
        {
            return ManagerSection.General;
        }
    }
}