/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterGrabSystem : MonoBehaviour
    {
        [SerializeField] private Transform characterCamera;
        [SerializeField] private float detectionRange = 2.0f;
        [SerializeField] private LayerMask mask;

        private GrabObject grabObject;
        private TPController controller;
        private CharacterIKSystem characterIKSystem;
        private Rigidbody characterRigidbody;
        private IHealth healthInterface;
        private RaycastHit raycastHit;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = GetComponent<TPController>();
            characterIKSystem = GetComponent<CharacterIKSystem>();
            characterRigidbody = GetComponent<Rigidbody>();
            healthInterface = GetComponent<IHealth>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (TPCInput.GetButtonDown(INC.GRAB) &&
                grabObject == null &&
                Physics.Raycast(characterCamera.position, characterCamera.forward, out raycastHit, detectionRange, mask))
            {
                grabObject = raycastHit.transform.GetComponent<GrabObject>();
                Grab(grabObject);
            }
            else if (TPCInput.GetButtonDown(INC.GRAB) || (healthInterface != null && !healthInterface.IsAlive()) || !controller.enabled)
            {
                Drop();
            }
        }

        public virtual void Grab(GrabObject grabObject)
        {
            if (grabObject != null)
            {
                grabObject.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                grabObject.ConnectJoint(characterRigidbody);
                if (controller != null)
                {
                    TPController.OnWallStopProperties property = controller.GetOnWallStopProperties();
                    Collider collider = grabObject.GetComponent<Collider>();
                    float checkRange = (property.GetCheckRange() + property.GetCheckRange()) + collider.bounds.size.z;
                    property.SetCheckRange(checkRange);
                    controller.SetOnWallStopProperties(property);
                }

                if (characterIKSystem != null)
                {
                    characterIKSystem.SetLeftHandTarget(grabObject.GetLeftHandIKTarget());
                    characterIKSystem.SetRightHandTarget(grabObject.GetRightHandIKTarget());
                }
            }
        }

        public virtual void Drop()
        {
            if (grabObject == null)
            {
                return;
            }

            grabObject.RemoveJoint();
            if (controller != null)
            {
                TPController.OnWallStopProperties property = controller.GetOnWallStopProperties();
                Collider collider = grabObject.GetComponent<Collider>();
                float checkRange = (property.GetCheckRange() - collider.bounds.size.z) / 2;
                property.SetCheckRange(checkRange);
                controller.SetOnWallStopProperties(property);
            }
            if (characterIKSystem != null)
            {
                characterIKSystem.SetLeftHandTarget(null);
                characterIKSystem.SetRightHandTarget(null);
            }
            grabObject = null;
        }

        public Transform GetCharacterCamera()
        {
            return characterCamera;
        }

        public void SetCharacterCamera(Transform value)
        {
            characterCamera = value;
        }

        public float GetDetectionRange()
        {
            return detectionRange;
        }

        public void SetDetectionRange(float value)
        {
            detectionRange = value;
        }

        public LayerMask GetMask()
        {
            return mask;
        }

        public void SetMask(LayerMask value)
        {
            mask = value;
        }

        public GrabObject GetCurrentGrabObject()
        {
            return grabObject;
        }

        public TPController GetController()
        {
            return controller;
        }

        public CharacterIKSystem GetCharacterIKSystem()
        {
            return characterIKSystem;
        }

        public Rigidbody GetCharacterRigidbody()
        {
            return characterRigidbody;
        }

        public IHealth GetHealthInterface()
        {
            return healthInterface;
        }
    }
}