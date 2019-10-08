/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IController))]
    public partial class AdaptiveRagdollSystem : MonoBehaviour, IRagdoll
    {
        public const float RAGDOLL_TO_MECANIM_BLEND_TIME = 0.5f;
        public const float AIR_SPEED = 5.0f;

        [SerializeField] private float relativeVelocityLimit = 10.0f;
        [SerializeField] private float standDelay = 1.5f;
        [SerializeField] private string getUpFromBellyStateName = "FromBelly";
        [SerializeField] private string getUpFromBackStateName = "FromBack";
        [SerializeField] private float bellyStandTime = 2.2f;
        [SerializeField] private float backStandTime = 2.8f;
        [SerializeField] private TPCamera cameraSystem;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Behaviour[] componentsToDisable;

        private bool isGrounded;
        private int getUpFromBellyStateHash;
        private int getUpFromBackStateHash;
        private float timeCache;
        private float ragdollingEndTime;
        private RagdollState ragdollState = RagdollState.Animated;
        private Transform hipsTransform;
        private Rigidbody hipsRigidbody;
        private Vector3 storedHipsPosition;
        private Vector3 storedHipsPositionPrivAnim;
        private Vector3 storedHipsPositionPrivBlend;

        private Animator animator;
        private Collider characterCollider;
        private IController controller;
        private List<RigidbodyComponent> rigidbodyComponents;
        private List<TransformComponent> transformComponents;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<IController>();
            characterCollider = GetComponent<Collider>();
            hipsTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
            hipsRigidbody = hipsTransform.GetComponent<Rigidbody>();

            rigidbodyComponents = new List<RigidbodyComponent>();
            transformComponents = new List<TransformComponent>();

            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            for (int i = 0, length = rigidbodies.Length; i < length; i++)
            {
                Rigidbody rigid = rigidbodies[i];
                if (rigid.transform == transform)
                    continue;

                RigidbodyComponent rigidCompontnt = new RigidbodyComponent(rigid);
                rigidbodyComponents.Add(rigidCompontnt);
            }

            Transform[] array = GetComponentsInChildren<Transform>();
            for (int i = 0, length = array.Length; i < length; i++)
            {
                Transform t = array[i];
                TransformComponent trComp = new TransformComponent(t);
                transformComponents.Add(trComp);
            }

            getUpFromBackStateHash = Animator.StringToHash(getUpFromBackStateName);
            getUpFromBellyStateHash = Animator.StringToHash(getUpFromBellyStateName);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            ActivateRagdollParts(false);
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (ragdollState == RagdollState.Animated &&
                Mathf.Abs(controller.GetVelocity().y) >= relativeVelocityLimit)
            {
                RagdollIn();
                RagdollOut();
            }
            else if (ragdollState == RagdollState.WaitStablePosition &&
                hipsRigidbody.velocity.magnitude < 0.1f &&
                timeCache > 0 &&
                Time.time - timeCache >= standDelay)
            {
                GetUp();
            }
        }

        /// <summary>
        /// LateUpdate is called after all Update functions have been called. 
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (ragdollState != RagdollState.Animated && !IsReadyToMove() && controller.Enabled())
            {
                controller.Enabled(false);
            }
            else if (ragdollState == RagdollState.Animated && IsReadyToMove() && !controller.Enabled())
            {
                controller.Enabled(true);
            }

            if (ragdollState != RagdollState.BlendToAnim)
            {
                return;
            }

            float ragdollBlendAmount = 1f - Mathf.InverseLerp(
                ragdollingEndTime,
                ragdollingEndTime + RAGDOLL_TO_MECANIM_BLEND_TIME,
                Time.time);

            if (storedHipsPositionPrivBlend != hipsTransform.position)
            {
                storedHipsPositionPrivAnim = hipsTransform.position;
            }
            storedHipsPositionPrivBlend = Vector3.Lerp(storedHipsPositionPrivAnim, storedHipsPosition, ragdollBlendAmount);
            hipsTransform.position = storedHipsPositionPrivBlend;

            for (int i = 0; i < transformComponents.Count; i++)
            {
                TransformComponent tc = transformComponents[i];
                if (tc.GetRotation() != tc.GetTransform().localRotation)
                {
                    tc.SetRotation(Quaternion.Slerp(tc.GetTransform().localRotation, tc.GetStoredRotation(), ragdollBlendAmount));
                    tc.GetTransform().localRotation = tc.GetRotation();
                }

                if (tc.GetPosition() != tc.GetTransform().localPosition)
                {
                    tc.SetPosition(Vector3.Slerp(tc.GetTransform().localPosition, tc.GetStoredPosition(), ragdollBlendAmount));
                    tc.GetTransform().localPosition = tc.GetPosition();
                }
            }

            if (Mathf.Abs(ragdollBlendAmount) < Mathf.Epsilon)
            {
                ragdollState = RagdollState.Animated;
            }
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (ragdollState == RagdollState.Animated && other.relativeVelocity.magnitude >= relativeVelocityLimit)
            {
                RagdollIn();
                RagdollOut();
            }
        }

        private bool IsReadyToMove()
        {
            return timeCache - Time.time <= 0;
        }

        /// <summary>
        /// Prevents jittering (as a result of applying joint limits) of bone and smoothly translate rigid from animated mode to ragdoll
        /// </summary>
        /// <param name="rigid"></param>
        /// <returns></returns>
        private static IEnumerator FixTransformAndEnableJoint(RigidbodyComponent joint)
        {
            if (joint.GetJoint() == null || !joint.GetJoint().autoConfigureConnectedAnchor)
                yield break;

            SoftJointLimit highTwistLimit = new SoftJointLimit();
            SoftJointLimit lowTwistLimit = new SoftJointLimit();
            SoftJointLimit swing1Limit = new SoftJointLimit();
            SoftJointLimit swing2Limit = new SoftJointLimit();

            SoftJointLimit curHighTwistLimit = highTwistLimit = joint.GetJoint().highTwistLimit;
            SoftJointLimit curLowTwistLimit = lowTwistLimit = joint.GetJoint().lowTwistLimit;
            SoftJointLimit curSwing1Limit = swing1Limit = joint.GetJoint().swing1Limit;
            SoftJointLimit curSwing2Limit = swing2Limit = joint.GetJoint().swing2Limit;

            float aTime = 0.3f;
            Vector3 startConPosition = joint.GetJoint().connectedBody.transform.InverseTransformVector(joint.GetJoint().transform.position - joint.GetJoint().connectedBody.transform.position);

            joint.GetJoint().autoConfigureConnectedAnchor = false;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Vector3 newConPosition = Vector3.Lerp(startConPosition, joint.GetConnectedAnchorDefault(), t);
                joint.GetJoint().connectedAnchor = newConPosition;

                curHighTwistLimit.limit = Mathf.Lerp(177, highTwistLimit.limit, t);
                curLowTwistLimit.limit = Mathf.Lerp(-177, lowTwistLimit.limit, t);
                curSwing1Limit.limit = Mathf.Lerp(177, swing1Limit.limit, t);
                curSwing2Limit.limit = Mathf.Lerp(177, swing2Limit.limit, t);

                joint.GetJoint().highTwistLimit = curHighTwistLimit;
                joint.GetJoint().lowTwistLimit = curLowTwistLimit;
                joint.GetJoint().swing1Limit = curSwing1Limit;
                joint.GetJoint().swing2Limit = curSwing2Limit;

                yield return null;
            }
            joint.GetJoint().connectedAnchor = joint.GetConnectedAnchorDefault();
            yield return new WaitForFixedUpdate();
            joint.GetJoint().autoConfigureConnectedAnchor = true;

            joint.GetJoint().highTwistLimit = highTwistLimit;
            joint.GetJoint().lowTwistLimit = lowTwistLimit;
            joint.GetJoint().swing1Limit = swing1Limit;
            joint.GetJoint().swing2Limit = swing2Limit;
        }

        /// <summary>
        /// Ragdoll character
        /// </summary>
        protected virtual void RagdollIn()
        {
            Vector3 characterVelocity = controller.GetVelocity();
            timeCache = Time.time;
            AutoUpdateTarget();
            CharacterColliderEnabled(false);
            ComponentsToDisableState(false);
            ActivateRagdollParts(true);
            animator.enabled = false;
            ragdollState = RagdollState.Ragdolled;
            ApplyVelocity(characterVelocity);
        }

        /// <summary>
        /// Smoothly translate to animator's bone positions when character stops falling
        /// </summary>
        protected virtual void RagdollOut()
        {
            if (ragdollState == RagdollState.Ragdolled)
            {
                ragdollState = RagdollState.WaitStablePosition;
            }
        }

        protected virtual void GetUp()
        {
            AutoUpdateTarget();
            CharacterColliderEnabled(true);
            ragdollingEndTime = Time.time;
            animator.enabled = true;
            ragdollState = RagdollState.BlendToAnim;
            storedHipsPositionPrivAnim = Vector3.zero;
            storedHipsPositionPrivBlend = Vector3.zero;

            storedHipsPosition = hipsTransform.position;

            Vector3 shiftPos = hipsTransform.position - transform.position;
            shiftPos.y = GetDistanceToFloor(shiftPos.y);

            MoveNodeWithoutChildren(shiftPos);

            for (int i = 0; i < transformComponents.Count; i++)
            {
                TransformComponent tc = transformComponents[i];
                tc.SetStoredRotation(tc.GetTransform().localRotation);
                tc.SetRotation(tc.GetTransform().localRotation);

                tc.SetStoredPosition(tc.GetTransform().localPosition);
                tc.SetPosition(tc.GetTransform().localPosition);
            }

            if (CheckIfLieOnBack())
            {
                animator.CrossFadeInFixedTime(getUpFromBackStateHash, 0, 0);
                timeCache = Time.time + backStandTime;
            }
            else
            {
                animator.CrossFadeInFixedTime(getUpFromBellyStateHash, 0);
                timeCache = Time.time + bellyStandTime;
            }
            ActivateRagdollParts(false);
            ComponentsToDisableState(true);
        }

        protected float GetDistanceToFloor(float currentY)
        {
            RaycastHit[] hits = Physics.RaycastAll(new Ray(hipsTransform.position, Vector3.down));
            float distFromFloor = float.MinValue;

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (!hit.transform.IsChildOf(transform))
                {
                    distFromFloor = Mathf.Max(distFromFloor, hit.point.y);
                }
            }

            if (Mathf.Abs(distFromFloor - float.MinValue) > Mathf.Epsilon)
                currentY = distFromFloor - transform.position.y;

            return currentY;
        }

        protected void MoveNodeWithoutChildren(Vector3 shiftPos)
        {
            Vector3 ragdollDirection = GetRagdollDirection();

            hipsTransform.position -= shiftPos;
            transform.position += shiftPos;

            Vector3 forward = transform.forward;
            transform.rotation = Quaternion.FromToRotation(forward, ragdollDirection) * transform.rotation;
            hipsTransform.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * hipsTransform.rotation;
        }

        protected bool CheckIfLieOnBack()
        {
            Vector3 left = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
            Vector3 right = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
            Vector3 hipsPos = hipsTransform.position;

            left -= hipsPos;
            left.y = 0f;
            right -= hipsPos;
            right.y = 0f;

            Quaternion q = Quaternion.FromToRotation(left, Vector3.right);
            Vector3 t = q * right;

            return t.z < 0f;
        }

        protected Vector3 GetRagdollDirection()
        {
            Vector3 ragdolledFeetPosition = (
                animator.GetBoneTransform(HumanBodyBones.Hips).position);
            Vector3 ragdolledHeadPosition = animator.GetBoneTransform(HumanBodyBones.Head).position;
            Vector3 ragdollDirection = ragdolledFeetPosition - ragdolledHeadPosition;
            ragdollDirection.y = 0;
            ragdollDirection = ragdollDirection.normalized;

            if (CheckIfLieOnBack())
                return ragdollDirection;
            else
                return -ragdollDirection;
        }

        /// <summary>
        /// Apply velocity 'predieVelocity' to to each rigid of character
        /// </summary>
        protected void ApplyVelocity(Vector3 predieVelocity)
        {
            for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
            {
                rigidbodyComponents[i].GetRigidBody().velocity = predieVelocity;
            }
        }

        protected void ActivateRagdollParts(bool activate)
        {
            controller.Enabled(!activate);

            for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
            {
                RigidbodyComponent rigidbody = rigidbodyComponents[i];
                Collider partColider = rigidbody.GetRigidBody().GetComponent<Collider>();

                if (partColider == null)
                {
                    string childName = rigidbody.GetRigidBody().name + "_ColliderRotator";
                    Transform transform = rigidbody.GetRigidBody().transform.Find(childName);
                    partColider = transform.GetComponent<Collider>();
                }

                partColider.isTrigger = !activate;

                if (activate)
                {
                    rigidbody.GetRigidBody().isKinematic = false;
                    StartCoroutine(FixTransformAndEnableJoint(rigidbody));
                }
                else
                {
                    rigidbody.GetRigidBody().isKinematic = true;
                }
            }
        }

        public bool Raycast(Ray ray, out RaycastHit hit, float distance)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, distance);

            for (int i = 0; i < hits.Length; ++i)
            {
                RaycastHit h = hits[i];
                if (h.transform != transform && h.transform.root == transform.root)
                {
                    hit = h;
                    return true;
                }
            }
            hit = new RaycastHit();
            return false;
        }

        public bool IsRagdolled()
        {
            return ragdollState == RagdollState.Ragdolled ||
                ragdollState == RagdollState.WaitStablePosition;
        }

        public void IsRagdolled(bool value)
        {
            if (value)
                RagdollIn();
            else
                RagdollOut();
        }

        public void AddExtraMove(Vector3 move)
        {
            if (IsRagdolled())
            {
                Vector3 airMove = new Vector3(move.x * AIR_SPEED, 0f, move.z * AIR_SPEED);
                for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
                {
                    RigidbodyComponent rigidbodyComponent = rigidbodyComponents[i];
                    rigidbodyComponent.GetRigidBody().AddForce(airMove / 100f, ForceMode.VelocityChange);
                }
            }
        }

        public void CharacterColliderEnabled(bool enabled)
        {
            if (characterCollider != null)
            {
                characterCollider.enabled = enabled;
            }
        }

        public void AutoUpdateTarget()
        {
            if (!IsRagdolled() && cameraTarget != null)
                cameraSystem.UpdateTarget(cameraTarget);
            else if (!IsRagdolled() && cameraTarget == null)
                cameraSystem.UpdateTarget(hipsTransform);
            else
                cameraSystem.UpdateTarget(transform);
        }

        public virtual void ComponentsToDisableState(bool enabled)
        {
            for (int i = 0, length = componentsToDisable.Length; i < length; i++)
            {
                componentsToDisable[i].enabled = enabled;
            }
        }

        public float GetRelativeVelocitLimit()
        {
            return relativeVelocityLimit;
        }

        public void SetRelativeVelocityLimit(float value)
        {
            relativeVelocityLimit = value;
        }

        public float GetStandDelay()
        {
            return standDelay;
        }

        public void SetStandDelay(float value)
        {
            standDelay = value;
        }

        public string GetAnimationGetUpFromBelly()
        {
            return getUpFromBellyStateName;
        }

        public void SetAnimationGetUpFromBelly(string value)
        {
            getUpFromBellyStateName = value;
        }

        public string GetAnimationGetUpFromBack()
        {
            return getUpFromBackStateName;
        }

        public void SetAnimationGetUpFromBack(string value)
        {
            getUpFromBackStateName = value;
        }

        public Transform GetHipsTransform()
        {
            return hipsTransform;
        }

        public void SetHipsTransform(Transform value)
        {
            hipsTransform = value;
        }

        public Rigidbody GetHipsRigidbody()
        {
            return hipsRigidbody;
        }

        public void SetHipsRigidbody(Rigidbody value)
        {
            hipsRigidbody = value;
        }

        public float GetBellyStandTime()
        {
            return bellyStandTime;
        }

        public void SetBellyStandTime(float value)
        {
            bellyStandTime = value;
        }

        public float GetBackStandTime()
        {
            return backStandTime;
        }

        public void SetBackStandTime(float value)
        {
            backStandTime = value;
        }

        public float GetRagdollingEndTime()
        {
            return ragdollingEndTime;
        }

        public Vector3 GetStoredHipsPosition()
        {
            return storedHipsPosition;
        }

        public void SetStoredHipsPosition(Vector3 value)
        {
            storedHipsPosition = value;
        }

        public Vector3 GetStoredHipsPositionPrivAnim()
        {
            return storedHipsPositionPrivAnim;
        }

        public void SetStoredHipsPositionPrivAnim(Vector3 value)
        {
            storedHipsPositionPrivAnim = value;
        }

        public Vector3 GetStoredHipsPositionPrivBlend()
        {
            return storedHipsPositionPrivBlend;
        }

        public void SetStoredHipsPositionPrivBlend(Vector3 value)
        {
            storedHipsPositionPrivBlend = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public IController GetControllerInterface()
        {
            return controller;
        }

        public TPCamera GetCameraSystem()
        {
            return cameraSystem;
        }

        public void SetCameraSystem(TPCamera value)
        {
            cameraSystem = value;
        }

        public Transform GetCameraTarget()
        {
            return cameraTarget;
        }

        public void SetCameraTarget(Transform value)
        {
            cameraTarget = value;
        }

        public Collider GetCharacterCollider()
        {
            return characterCollider;
        }

        public void SetCharacterCollider(Collider value)
        {
            characterCollider = value;
        }

        public Vector3 GetVelocity()
        {
            return RagdollState.Ragdolled == ragdollState ? hipsRigidbody.velocity : controller.GetVelocity();
        }

        public Behaviour[] GetComponentsToDisable()
        {
            return componentsToDisable;
        }

        public void SetComponentsToDisable(Behaviour[] value)
        {
            componentsToDisable = value;
        }
    }
}