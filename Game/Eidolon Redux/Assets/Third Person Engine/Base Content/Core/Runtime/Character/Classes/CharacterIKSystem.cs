/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IController))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterIKSystem : MonoBehaviour
    {
        // Foot IK properties
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private LayerMask groundLayer = 1 << 0;
        [SerializeField] private float footOffset = 0.125f;
        [SerializeField] private float deltaAmplifier = 1.75f;
        [SerializeField] private float colliderSmooth = 17.5f;
        [SerializeField] private bool processFootRotation = false;

        // Upper body IK properties
        [SerializeField] private Transform lookTarget;
        [SerializeField] private float weight = 1.0f;
        [SerializeField] private float bodyWeight = 1.0f;
        [SerializeField] private float headWeight = 1.0f;
        [SerializeField] private float eyesWeight = 1.0f;
        [SerializeField] private float clampWeight = 1.0f;

        // Hands IK properties
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private float handIKSmooth = 7.0f;

        // Base IK properties
        [SerializeField] private bool ikIsActive = true;

        private Animator animator;
        private IController controller;
        private Rigidbody characterRigidbody;
        private float leftFootY;
        private float rightFootY;
        private float cacheColliderHeight;
        private float legDistance;
        private float handWeight;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<IController>();
            characterRigidbody = GetComponent<Rigidbody>();
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            cacheColliderHeight = capsuleCollider.height;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (animator != null && ikIsActive)
            {
                HandleColliderOffset();
            }
        }

        /// <summary>
        /// Callback for setting up animation IK (inverse kinematics).
        /// </summary>
        /// <param name="layerIndex">Index of the layer on which the IK solver is called.</param>
        protected virtual void OnAnimatorIK(int layerIndex)
        {
            if (animator != null && ikIsActive)
            {
                FootIKProcessing();
                UpperBodyIKProcessing();
                HandsIKProcessing();
            }
        }

        /// <summary>
        /// Processing character fool IK system.
        /// </summary>
        protected virtual void FootIKProcessing()
        {
            RaycastHit floorHit;
            Vector3 targetPosition = Vector3.zero;
            Quaternion targetRotation = Quaternion.identity;
            legDistance = GetStateBasedLegDistance();

            if (leftFoot != null && Physics.Linecast(GetFootOrigin(leftFoot.position), GetFootTarget(leftFoot.position), out floorHit, groundLayer, QueryTriggerInteraction.Ignore))
            {
                targetPosition = GetFootPosition(floorHit);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, targetPosition);

                leftFootY = targetPosition.y;

                if (processFootRotation)
                {
                    targetRotation = GetFootRotation(leftFoot, floorHit);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, targetRotation);
                }
            }

            if (rightFoot != null && Physics.Linecast(GetFootOrigin(rightFoot.position), GetFootTarget(rightFoot.position), out floorHit, groundLayer, QueryTriggerInteraction.Ignore))
            {
                targetPosition = GetFootPosition(floorHit);

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, targetPosition);

                rightFootY = targetPosition.y;

                if (processFootRotation)
                {
                    targetRotation = GetFootRotation(rightFoot, floorHit);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, targetRotation);
                }
            }
        }

        protected virtual void UpperBodyIKProcessing()
        {
            if (lookTarget != null)
            {
                animator.SetLookAtPosition(lookTarget.position);
                animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
            }
        }

        protected virtual void HandsIKProcessing()
        {
            bool handsConditions = leftHandTarget != null && rightHandTarget != null;
            handWeight = Mathf.SmoothStep(handWeight, handsConditions ? 1.0f : 0.0f, handIKSmooth * Time.deltaTime);

            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);

                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handWeight);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }

            if (rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }
        }

        /// <summary>
        /// Handle capsule collider offset.
        /// </summary>
        protected virtual void HandleColliderOffset()
        {
            if (GetPlaneSpeed(characterRigidbody.velocity) < 0.1f)
            {
                float delta = GetFootDelta();
                float targetHeight = cacheColliderHeight - (delta * deltaAmplifier);
                controller.SetColliderHeight(targetHeight);
            }
            else
            {
                controller.SetColliderHeight(cacheColliderHeight);
            }
        }

        public float GetStateBasedLegDistance()
        {
            return 1 / (GetPlaneSpeed(characterRigidbody.velocity) + 0.8f);
        }

        public float GetPlaneSpeed(Vector3 velocity)
        {
            velocity.y = 0;
            return velocity.magnitude;
        }

        public Quaternion GetFootRotation(Transform foot, RaycastHit hit)
        {
            Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            footRotation.eulerAngles = new Vector3(footRotation.eulerAngles.x, foot.rotation.eulerAngles.y, footRotation.eulerAngles.z);
            return footRotation;
        }

        public Vector3 GetFootPosition(RaycastHit hit)
        {
            Vector3 displacement = hit.point;
            displacement.y += footOffset;
            return displacement;
        }

        public Vector3 GetFootOrigin(Vector3 footPosition)
        {
            Vector3 origin = footPosition + ((legDistance + 0.25f) * Vector3.up);
            return origin;
        }

        public Vector3 GetFootTarget(Vector3 footPosition)
        {
            Vector3 target = footPosition - ((legDistance / 2f) * Vector3.up);
            return target;
        }

        public float GetFootDelta()
        {
            return Mathf.Abs(leftFootY - rightFootY);
        }

        public Transform GetLeftFoot()
        {
            return leftFoot;
        }

        public void SetLeftFoot(Transform value)
        {
            leftFoot = value;
        }

        public Transform GetRightFoot()
        {
            return rightFoot;
        }

        public void SetRightFoot(Transform value)
        {
            rightFoot = value;
        }

        public LayerMask GetGroundLayer()
        {
            return groundLayer;
        }

        public void SetGroundLayer(LayerMask value)
        {
            groundLayer = value;
        }

        public float GetFootOffset()
        {
            return footOffset;
        }

        public void SetFootOffset(float value)
        {
            footOffset = value;
        }

        public float GetDeltaAmplifier()
        {
            return deltaAmplifier;
        }

        public void SetDeltaAmplifier(float value)
        {
            deltaAmplifier = value;
        }

        public float GetColliderSmooth()
        {
            return colliderSmooth;
        }

        public void SetColliderSmooth(float value)
        {
            colliderSmooth = value;
        }

        public bool ProcessFootRotation()
        {
            return processFootRotation;
        }

        public void ProcessFootRotation(bool value)
        {
            processFootRotation = value;
        }

        public Transform GetLookTarget()
        {
            return lookTarget;
        }

        public void SetLookTarget(Transform value)
        {
            lookTarget = value;
        }

        public float GetLookWeight()
        {
            return weight;
        }

        public void SetLookWeight(float value)
        {
            weight = value;
        }

        public float GetBodyWeight()
        {
            return bodyWeight;
        }

        public void SetBodyWeight(float value)
        {
            bodyWeight = value;
        }

        public float GetHeadWeight()
        {
            return headWeight;
        }

        public void SetHeadWeight(float value)
        {
            headWeight = value;
        }

        public float GetEyesWeight()
        {
            return eyesWeight;
        }

        public void SetEyesWeight(float value)
        {
            eyesWeight = value;
        }

        public float GetClampWeight()
        {
            return clampWeight;
        }

        public void SetClampWeight(float value)
        {
            clampWeight = value;
        }

        public Transform GetLeftHandTarget()
        {
            return leftHandTarget;
        }

        public void SetLeftHandTarget(Transform value)
        {
            leftHandTarget = value;
        }

        public Transform GetRightHandTarget()
        {
            return rightHandTarget;
        }

        public void SetRightHandTarget(Transform value)
        {
            rightHandTarget = value;
        }

        public float GetHandIKSmooth()
        {
            return handIKSmooth;
        }

        public void SetHandIKSmooth(float value)
        {
            handIKSmooth = value;
        }

        public bool IKIsActive()
        {
            return ikIsActive;
        }

        public void IKIsActive(bool value)
        {
            ikIsActive = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public IController GetControllerInterface()
        {
            return controller;
        }

        protected void SetControllerInterface(IController value)
        {
            controller = value;
        }

        public Rigidbody GetCharacterRigidbody()
        {
            return characterRigidbody;
        }

        protected void SetCharacterRigidbody(Rigidbody value)
        {
            characterRigidbody = value;
        }
    }
}