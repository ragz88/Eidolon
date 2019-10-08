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
using UnityEditorInternal;
using UnityEngine;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using System;

namespace ThirdPersonEngine.Editor
{
    public class PlayerManagerItem : ManagerItem
    {
        protected enum FootstepSystem
        {
            None,
            Simple,
            Advanced
        }

        protected enum RagdollSystem
        {
            Adaptive
        }

        [Serializable]
        protected struct Settings
        {
            [Serializable]
            public struct CharacterComponent
            {
                public string name;
                public Type type;
                public Type[] requireComponents;
                public bool isActive;

                public CharacterComponent(string name, Type type, Type[] requireComponents, bool isActive)
                {
                    this.name = name;
                    this.type = type;
                    this.requireComponents = requireComponents;
                    this.isActive = isActive;
                }
            }

            public string name;
            public GameObject body;
            public AnimatorController animatorController;
            public Avatar avatar;
            public bool autoCreateCamera;
            public FootstepSystem footstepSystem;
            public CharacterComponent[] characterComponents;
            public RagdollSystem ragdollSystem;

            public Settings(string name, GameObject body, AnimatorController animatorController, Avatar avatar, bool autoCreateCamera, FootstepSystem footstepSystem, CharacterComponent[] characterComponents, RagdollSystem ragdollSystem)
            {
                this.name = name;
                this.body = body;
                this.animatorController = animatorController;
                this.avatar = avatar;
                this.autoCreateCamera = autoCreateCamera;
                this.footstepSystem = footstepSystem;
                this.characterComponents = characterComponents;
                this.ragdollSystem = ragdollSystem;
            }

            public readonly static Settings Default = new Settings("New Player", null, null, null, true, FootstepSystem.Simple, GetDefaultSystems(), RagdollSystem.Adaptive);

            public static CharacterComponent[] GetDefaultSystems()
            {
                CharacterComponent[] characterComponents = new CharacterComponent[]
                {
                    new CharacterComponent("Health System", typeof(CharacterHealth), null, true),
                    new CharacterComponent("IK System", typeof(CharacterIKSystem), null, true),
                    new CharacterComponent("Grab System", typeof(CharacterGrabSystem), null, true)
                };
                return characterComponents;

            }
        }

        public override string GetDisplayName() { return "Player"; }

        public override ManagerSection GetSection() { return ManagerSection.General; }

        public override int GetPriority() { return 0; }

        private GameObject player;
        private Settings settings;
        private ReorderableList reorderableList;
        private string[] options;
        private int selectedOption;
        private int lastSelectionOption;
        private EditorDelay delay;

        public override void OnInitializeProperties()
        {
            settings = Settings.Default;
            reorderableList = new ReorderableList(settings.characterComponents, typeof(Settings.CharacterComponent), false, false, false, false)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + 1.5f, rect.width, rect.height), "Character Components");
                    },

                    drawElementCallback = (rect, index, isFocus, isActive) =>
                    {
                        Settings.CharacterComponent component = settings.characterComponents[index];
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight), component.name);
                        component.isActive = EditorGUI.Toggle(new Rect(rect.width, rect.y + 1.5f, 15, EditorGUIUtility.singleLineHeight), component.isActive);
                        settings.characterComponents[index] = component;
                    }
            };
            options = new string[2] { "Create", "Ragdoll Helper" };
            delay = new EditorDelay(0.25f);
        }

        public override void OnPropertiesGUI()
        {
            selectedOption = GUILayout.Toolbar(selectedOption, options, RSEditorStyles.ToolbarButton);
            if (selectedOption != lastSelectionOption)
            {
                settings = Settings.Default;
                lastSelectionOption = selectedOption;
            }
            GUILayout.Space(5);
            switch (selectedOption)
            {
                case 0:
                    CreateGUILayout();
                    break;
                case 1:
                    RagdollHelperGUILayout();
                    break;
            }
        }

        public virtual void CreateGUILayout()
        {
            settings.name = EditorGUILayout.TextField("Name", settings.name);
            if (string.IsNullOrEmpty(settings.name))
            {
                EditorGUILayout.HelpBox("Name field cannot be empty!", MessageType.Error);
            }

            settings.body = RSEditor.ObjectField<GameObject>("Body", settings.body, true);
            if (settings.body == null)
            {
                EditorGUILayout.HelpBox("Body cannot be empty!", MessageType.Error);
            }

            settings.animatorController = RSEditor.ObjectField<AnimatorController>("Animator Controller", settings.animatorController, true);
            if (settings.animatorController == null)
            {
                EditorGUILayout.HelpBox("Animator Controller cannot be empty!", MessageType.Error);
            }
            settings.avatar = RSEditor.ObjectField<Avatar>("Avatar", settings.avatar, true);

            settings.autoCreateCamera = EditorGUILayout.Toggle("Auto Create Camera", settings.autoCreateCamera);
            settings.ragdollSystem = (RagdollSystem) EditorGUILayout.EnumPopup("Ragdoll System", settings.ragdollSystem);
            settings.footstepSystem = (FootstepSystem) EditorGUILayout.EnumPopup("Footstep System", settings.footstepSystem);
            if (reorderableList != null)
                reorderableList.DoLayoutList();
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(settings.name) || settings.body == null || settings.animatorController == null);
            if (RSEditor.Button("Create", "Right", GUILayout.Width(70)))
            {
                player = CreatePlayer(settings);
            }
            if (player != null && delay.WaitForSeconds())
            {
                if (RSDisplayDialogs.Message("Create Successful", "Player was created on scene!\nSetup Player components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = player;
                }
                player = null;
            }
            EditorGUI.EndDisabledGroup();
        }

        public virtual void RagdollHelperGUILayout()
        {
            settings.body = RSEditor.ObjectField<GameObject>("Player", settings.body, true);
            if (settings.body == null)
            {
                EditorGUILayout.HelpBox("Add player!", MessageType.Error);
            }

            Collider[] colliders = null;
            if (settings.body != null)
            {
                colliders = settings.body.GetComponentsInChildren<Collider>(true);
                RSEditorHelpBoxMessages.Message(string.Format("Finded {0} colliders in body", colliders.Length));
            }
            EditorGUI.BeginDisabledGroup(settings.body == null || colliders != null && colliders.Length == 0);
            if (RSEditor.Button("Optimize", "Right", GUILayout.Width(70)))
            {
                for (int i = 0, length = colliders.Length; i < length; i++)
                {
                    GameObject bounds = colliders[i].gameObject;
                    if (settings.body == bounds)
                    {
                        continue;
                    }
                    bounds.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                if (RSDisplayDialogs.Message("Optimize Successful", "Player ragdoll was optimize!\nSetup Player components before start play.", "Select", "Ok"))
                {
                    Selection.activeGameObject = settings.body;
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        protected virtual GameObject CreatePlayer(Settings settings)
        {
            GameObject player = GameObject.Instantiate(settings.body, Vector3.zero, Quaternion.identity);
            player.name = settings.name;
            player.tag = TNC.PLAYER;
            player.layer = LayerMask.NameToLayer(LNC.PLAYER);

            Animator animator = RSEditorInternal.AddComponent<Animator>(player);
            TPController controller = RSEditorInternal.AddComponent<TPController>(player);

            SimpleFootstepSystem footstepSystem = null;
            switch (settings.footstepSystem)
            {
                case FootstepSystem.Simple:
                    footstepSystem = RSEditorInternal.AddComponent<SimpleFootstepSystem>(player);
                    break;
                case FootstepSystem.Advanced:
                    footstepSystem = RSEditorInternal.AddComponent<AdvancedFootstepSystem>(player);
                    break;
            }

            StandaloneController standaloneController = RSEditorInternal.AddComponent<StandaloneController>(player);
            Rigidbody rigidbody = RSEditorInternal.AddComponent<Rigidbody>(player);
            CapsuleCollider capsuleCollider = RSEditorInternal.AddComponent<CapsuleCollider>(player);
            AudioSource audioSource = RSEditorInternal.AddComponent<AudioSource>(player);
            AdaptiveRagdollSystem adaptiveRagdollSystem = RSEditorInternal.AddComponent<AdaptiveRagdollSystem>(player);

            RSEditorInternal.MoveComponentBottom<Animator>(player.transform);
            RSEditorInternal.MoveComponentBottom<TPController>(player.transform);

            for (int i = 0, length = settings.characterComponents.Length; i < length; i++)
            {
                Settings.CharacterComponent component = settings.characterComponents[i];
                if (!component.isActive)
                {
                    continue;
                }
                RSEditorInternal.AddComponent(component.type, player);
                RSEditorInternal.MoveComponentBottom(component.type, player.transform);
                for (int j = 0; component.requireComponents != null && j < component.requireComponents.Length; j++)
                {
                    Type requireComponent = component.requireComponents[j];
                    RSEditorInternal.AddComponent(requireComponent, player);
                    RSEditorInternal.MoveComponentBottom(requireComponent, player.transform);
                }
            }

            RSEditorInternal.MoveComponentBottom<AdaptiveRagdollSystem>(player.transform);
            if (settings.footstepSystem != FootstepSystem.None)
            {
                RSEditorInternal.MoveComponentBottom<SimpleFootstepSystem>(player.transform);
            }

            RSEditorInternal.MoveComponentBottom<StandaloneController>(player.transform);
            RSEditorInternal.MoveComponentBottom<Rigidbody>(player.transform);
            RSEditorInternal.MoveComponentBottom<CapsuleCollider>(player.transform);
            RSEditorInternal.MoveComponentBottom<AudioSource>(player.transform);

            // Setup components
            animator.runtimeAnimatorController = settings.animatorController;
            animator.avatar = settings.avatar;
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            rigidbody.mass = 60;
            rigidbody.freezeRotation = true;
            capsuleCollider.height = 1.8f;
            capsuleCollider.radius = 0.2f;
            capsuleCollider.center = new Vector3(0.0f, 0.9f, 0.0f);

            if (animator != null)
            {
                CharacterIKSystem characterIKSystem = player.GetComponent<CharacterIKSystem>();
                if (characterIKSystem != null)
                {
                    characterIKSystem.SetLeftFoot(animator.GetBoneTransform(HumanBodyBones.LeftFoot));
                    characterIKSystem.SetRightFoot(animator.GetBoneTransform(HumanBodyBones.RightFoot));
                }
            }

            if (settings.autoCreateCamera)
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

                cameraSystem.SetTargetController(controller);

                adaptiveRagdollSystem.SetCameraTarget(animator.GetBoneTransform(HumanBodyBones.Hips));
                adaptiveRagdollSystem.SetCameraSystem(cameraSystem);

                CharacterGrabSystem grabSystem = player.GetComponent<CharacterGrabSystem>();
                if (grabSystem != null)
                {
                    grabSystem.SetCharacterCamera(camera.transform);
                }

            }

            return player;
        }
    }
}