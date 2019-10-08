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

namespace ThirdPersonEngine.Editor
{
    [CustomEditor(typeof(CharacterIKSystem))]
    public sealed class CharacterIKSystemEditor : RSEditor<CharacterIKSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseContent = new GUIContent("Base Settings");
            public readonly static GUIContent IKIsActive = new GUIContent("IK Is Active", "IK active state.");
            public readonly static GUIContent InteractiveIKBone = new GUIContent("Interactive IK Bones", "[Only in Editor] Display bones and IK interactively at the scene window.");

            // Foot IK content
            public readonly static GUIContent FootIK = new GUIContent("Foot IK");
            public readonly static GUIContent LeftFoot = new GUIContent("Left Foot", "Left foot bone.");
            public readonly static GUIContent RightFoot = new GUIContent("Right Foot", "Right foot bone.");
            public readonly static GUIContent GroundLayer = new GUIContent("Ground Layer", "Layer to detect ground by foot ray.");
            public readonly static GUIContent FootOffset = new GUIContent("Foot Offset", "Foot offset distance between ground and foot.");
            public readonly static GUIContent DeltaAmplifier = new GUIContent("Delta Amplifier", "Delta amplifier for character weight.");
            public readonly static GUIContent ColliderSmooth = new GUIContent("Collider Smooth", "Collider height change speed.");
            public readonly static GUIContent ProcessFootRotation = new GUIContent("Foot Rotation", "Process foot rotation by surface normal.");

            // Upper body IK content
            public readonly static GUIContent UpperBodyIK = new GUIContent("Upper Body IK");
            public readonly static GUIContent LookTarget = new GUIContent("Look Target", "Look target for character height.");
            public readonly static GUIContent LookWeight = new GUIContent("Weight", "Upped body IK weight.");
            public readonly static GUIContent BodyWeight = new GUIContent("Body Weight", "Body IK weight.");
            public readonly static GUIContent HeadWeight = new GUIContent("Head Weight", "Head IK weight.");
            public readonly static GUIContent EyesWeight = new GUIContent("Eyes Weight", "Eyes IK weight.");
            public readonly static GUIContent ClampWeight = new GUIContent("Clamp Weight", "Clamp IK weight.");

            // Hands IK content
            public readonly static GUIContent HandsIK = new GUIContent("Hands IK");
            public readonly static GUIContent LeftHandTarget = new GUIContent("Left Hand Target", "Left hand target transform.");
            public readonly static GUIContent RightHandTarget = new GUIContent("Right Hand Target", "Right hand target transform.");
            public readonly static GUIContent HandIKSmooth = new GUIContent("Smooth", "Hand IK smooth.");
        }

        private bool interactiveIKBone = true;
        private bool footFoldout;
        private bool upperBodyFoldout;
        private bool handsFoldout;

        public override string GetHeaderName()
        {
            return "Character IK System";
        }

        public void OnSceneGUI()
        {
            if (interactiveIKBone)
            {
                Animator animator = instance.GetComponent<Animator>();

                const float sphereSize = 0.05f;
                const float sphereLineSize = 0.025f;
                Color sphereColor = instance.IKIsActive() ? new Color(0.06f, 0.5f, 0.9f, 0.75f) : new Color(0.9f, 0.06f, 0.06f, 0.75f);
                Color sphereLineColor = new Color(1, 1, 1, 1.0f);
                Transform[] boneIK = new Transform[6]
                {
                    animator.GetBoneTransform(HumanBodyBones.Head),
                    animator.GetBoneTransform(HumanBodyBones.Spine),
                    animator.GetBoneTransform(HumanBodyBones.LeftHand),
                    animator.GetBoneTransform(HumanBodyBones.RightHand),
                    animator.GetBoneTransform(HumanBodyBones.LeftFoot),
                    animator.GetBoneTransform(HumanBodyBones.RightFoot)
                };

                for (int i = 0, length = boneIK.Length; i < length; i++)
                {
                    Transform bone = boneIK[i];
                    if (bone != null)
                    {
                        Handles.color = sphereColor;
                        Handles.SphereHandleCap(0, bone.position, Quaternion.identity, sphereSize, EventType.Repaint);
                        Handles.color = sphereLineColor;
                        Handles.CircleHandleCap(0, bone.position, Quaternion.LookRotation(Vector3.up), sphereLineSize, EventType.Repaint);
                        Handles.CircleHandleCap(0, bone.position, Quaternion.LookRotation(Vector3.right), sphereLineSize, EventType.Repaint);
                        Handles.CircleHandleCap(0, bone.position, Quaternion.LookRotation(Vector3.forward), sphereLineSize, EventType.Repaint);
                    }
                }

                Transform rootNode = animator.GetBoneTransform(HumanBodyBones.Hips);
                if (rootNode != null)
                {
                    Transform[] childNodes = rootNode.GetComponentsInChildren<Transform>();
                    Handles.color = new Color(0.06f, 0.5f, 0.9f, 0.75f);
                    for (int i = 0; i < childNodes.Length; i++)
                    {
                        Transform child = childNodes[i];
                        Handles.DrawLine(child.position, child.parent.position);
                        Handles.SphereHandleCap(1, child.position, child.rotation, 0.01f, EventType.Repaint);
                    }
                }

            }
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseContent);
            instance.IKIsActive(EditorGUILayout.Toggle(ContentProperties.IKIsActive, instance.IKIsActive()));
            interactiveIKBone = EditorGUILayout.Toggle(ContentProperties.InteractiveIKBone, interactiveIKBone);
            IncreaseIndentLevel();

            BeginFoldoutGroup(ref footFoldout, ContentProperties.FootIK);
            if (footFoldout)
            {
                if (instance.GetLeftFoot() != null)
                {
                    instance.SetLeftFoot(ObjectField<Transform>(ContentProperties.LeftFoot, instance.GetLeftFoot(), true));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    instance.SetLeftFoot(ObjectField<Transform>(ContentProperties.LeftFoot, instance.GetLeftFoot(), true));
                    if (RSEditor.SearchButton())
                    {
                        Animator animator = instance.GetComponent<Animator>();
                        if (animator != null)
                        {
                            Transform leftFootBone = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
                            if (leftFootBone != null)
                            {
                                instance.SetLeftFoot(leftFootBone);
                            }
                            else
                            {
                                RSDisplayDialogs.Message("Left Foot", "Left foot bone not found.\nTry to find it manually.");
                            }
                        }
                        else
                        {
                            RSDisplayDialogs.Message("Error", "Animator not found in current character, add Animator component and try again.");
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                if (instance.GetRightFoot() != null)
                {
                    instance.SetRightFoot(ObjectField<Transform>(ContentProperties.RightFoot, instance.GetRightFoot(), true));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    instance.SetRightFoot(ObjectField<Transform>(ContentProperties.RightFoot, instance.GetRightFoot(), true));
                    if (RSEditor.SearchButton())
                    {
                        Animator animator = instance.GetComponent<Animator>();
                        if (animator != null)
                        {
                            Transform rightFootBone = animator.GetBoneTransform(HumanBodyBones.RightFoot);
                            if (rightFootBone != null)
                            {
                                instance.SetRightFoot(rightFootBone);
                            }
                            else
                            {
                                RSDisplayDialogs.Message("Right Foot", "Right foot bone not found.\nTry to find it manually.");
                            }
                        }
                        else
                        {
                            RSDisplayDialogs.Message("Error", "Animator not found in current character, add Animator component and try again.");
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                instance.SetFootOffset(EditorGUILayout.Slider(ContentProperties.FootOffset, instance.GetFootOffset(), 0.0f, 1.0f));
                instance.SetDeltaAmplifier(EditorGUILayout.FloatField(ContentProperties.DeltaAmplifier, instance.GetDeltaAmplifier()));
                instance.SetColliderSmooth(EditorGUILayout.FloatField(ContentProperties.ColliderSmooth, instance.GetColliderSmooth()));
                instance.ProcessFootRotation(EditorGUILayout.Toggle(ContentProperties.ProcessFootRotation, instance.ProcessFootRotation()));
                LayerMask groundLayer = EditorGUILayout.MaskField(ContentProperties.GroundLayer, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(instance.GetGroundLayer()), InternalEditorUtility.layers);
                instance.SetGroundLayer(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(groundLayer));
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref upperBodyFoldout, ContentProperties.UpperBodyIK);
            if (upperBodyFoldout)
            {
                instance.SetLookTarget(ObjectField<Transform>(ContentProperties.LookTarget, instance.GetLookTarget(), true));
                instance.SetLookWeight(EditorGUILayout.Slider(ContentProperties.LookWeight, instance.GetLookWeight(), 0.0f, 1.0f));
                instance.SetBodyWeight(EditorGUILayout.Slider(ContentProperties.BodyWeight, instance.GetBodyWeight(), 0.0f, 1.0f));
                instance.SetHeadWeight(EditorGUILayout.Slider(ContentProperties.HeadWeight, instance.GetHeadWeight(), 0.0f, 1.0f));
                instance.SetEyesWeight(EditorGUILayout.Slider(ContentProperties.EyesWeight, instance.GetEyesWeight(), 0.0f, 1.0f));
                instance.SetClampWeight(EditorGUILayout.Slider(ContentProperties.ClampWeight, instance.GetClampWeight(), 0.0f, 1.0f));
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref handsFoldout, ContentProperties.HandsIK);
            if (handsFoldout)
            {
                instance.SetLeftHandTarget(ObjectField<Transform>(ContentProperties.LeftHandTarget, instance.GetLeftHandTarget(), true));
                instance.SetRightHandTarget(ObjectField<Transform>(ContentProperties.RightHandTarget, instance.GetRightHandTarget(), true));
                instance.SetHandIKSmooth(EditorGUILayout.FloatField(ContentProperties.HandIKSmooth, instance.GetHandIKSmooth()));
            }
            EndFoldoutGroup();

            DecreaseIndentLevel();
            EndGroup();
        }
    }
}