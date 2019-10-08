/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    /// <summary>
    /// Simple third person character footstep sound system.
    /// </summary>
    [RequireComponent(typeof(IController))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class SimpleFootstepSystem : MonoBehaviour
    {
        [SerializeField] private FootstepProperties footstepProperties;
        [SerializeField] private float stepInterval = 1.0f;

        private TerrainTextureDetector terrainTextureDetector;
        private IController controller;
        private AudioSource audioSource;
        private CapsuleCollider capsuleCollider;
        private LayerMask ignoreLayer;
        private Vector3 lastPosition;
        private float movedInterval;
        private bool jumped = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = GetComponent<IController>();
            audioSource = GetComponent<AudioSource>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            Terrain terrain = GameObject.FindObjectOfType<Terrain>();
            if (terrain != null)
            {
                terrainTextureDetector = new TerrainTextureDetector(terrain.terrainData);
            }
            ignoreLayer = LNC.IgnorePlayer;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (footstepProperties != null)
            {
                if (controller.IsGrounded())
                {
                    if (controller.IsJumping() && !jumped)
                    {
                        JumpSoundProcessing();
                        jumped = true;
                        return;
                    }
                    StepSoundProcessing();
                }
            }
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (enabled &&footstepProperties != null && (jumped || !controller.IsGrounded()) && other.relativeVelocity.y > 0 && Physics.Raycast(capsuleCollider.bounds.center, -Vector3.up, Mathf.Infinity, controller.GetGroundLayer(), QueryTriggerInteraction.Ignore))
            {
                LandSoundProcessing();
                jumped = false;
            }
        }

        protected virtual void StepSoundProcessing()
        {
            if (IntervalIsMoved())
            {
                FootstepProperty footstepProperty = GetFootstepPropertyOfSurface(capsuleCollider.bounds.center, -Vector3.up);
                if (footstepProperty.IsEmpty())
                {
                    return;
                }

                AudioClip clip = footstepProperty.GetRandomStepSound();
                PlayStepSound(clip);

                ResetMovedInterval();
            }
        }

        protected virtual void JumpSoundProcessing()
        {
            FootstepProperty footstepProperty = GetFootstepPropertyOfSurface(capsuleCollider.bounds.center, -Vector3.up);
            if (footstepProperty.IsEmpty())
            {
                return;
            }

            AudioClip clip = footstepProperty.GetJumpSound(0);
            PlayStepSound(clip);
        }

        protected virtual void LandSoundProcessing()
        {
            FootstepProperty footstepProperty = GetFootstepPropertyOfSurface(capsuleCollider.bounds.center, -Vector3.up);
            if (footstepProperty.IsEmpty())
            {
                return;
            }

            AudioClip clip = footstepProperty.GetLandSound(0);
            PlayStepSound(clip);
            ResetMovedInterval();
        }

        public FootstepProperty GetFootstepPropertyOfSurface(Vector3 position, Vector3 direction, float maxDistance = Mathf.Infinity)
        {
            RaycastHit rayCastHit;
            if (Physics.Raycast(position, direction, out rayCastHit, maxDistance))
            {
                string tag = rayCastHit.collider.tag;
                PhysicMaterial physicMaterial = rayCastHit.collider.sharedMaterial;
                for (int i = 0, length = footstepProperties.GetLength(); i < length; i++)
                {
                    FootstepProperty property = footstepProperties.GetProperty(i);
                    if (physicMaterial != null && property.ComparePhysicMaterial(physicMaterial))
                    {
                        return property;
                    }
                    if (terrainTextureDetector.GetTerrainData() != null && tag == "Terrain")
                    {
                        Texture2D texture = terrainTextureDetector.GetActiveTexture(position);
                        if (property.CompareTexture(texture))
                        {
                            return property;
                        }
                    }
                }
            }
            return FootstepProperty.Empty;
        }

        public bool IntervalIsMoved()
        {
            movedInterval += (lastPosition - transform.position).magnitude;
            lastPosition = transform.position;
            return movedInterval >= stepInterval;
        }

        /// <summary>
        /// Return contact point terrain texture.
        /// </summary>
        public Texture2D GetSurfaceTexture(Collider collider, Vector3 contactPos)
        {
            Texture2D texture = terrainTextureDetector.GetActiveTexture(contactPos);
            return texture;
        }

        public void ResetMovedInterval()
        {
            movedInterval = 0;
        }

        public void PlayStepSound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        public FootstepProperties GetFootstepProperties()
        {
            return footstepProperties;
        }

        public void SetFootstepProperties(FootstepProperties value)
        {
            footstepProperties = value;
        }

        public float GetStepInterval()
        {
            return stepInterval;
        }

        public void SetStepInterval(float value)
        {
            stepInterval = value;
        }

        public IController GetController()
        {
            return controller;
        }

        public void SetController(TPController value)
        {
            controller = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }

        public LayerMask GetIgnoreLayer()
        {
            return ignoreLayer;
        }

        protected void SetIgnoreLayer(LayerMask value)
        {
            ignoreLayer = value;
        }

        protected Vector3 GetLastPosition()
        {
            return lastPosition;
        }

        protected void SetLastPosition(Vector3 value)
        {
            lastPosition = value;
        }

        public float GetMovedInterval()
        {
            return movedInterval;
        }

        protected void SetMovedInterval(float value)
        {
            movedInterval = value;
        }
    }
}