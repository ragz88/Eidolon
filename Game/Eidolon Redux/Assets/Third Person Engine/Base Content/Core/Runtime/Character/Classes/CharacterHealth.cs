/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(IController))]
    [RequireComponent(typeof(IRagdoll))]
    [RequireComponent(typeof(AudioSource))]
    public partial class CharacterHealth : MonoBehaviour, IHealth
    {

        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int minHealth = 0;
        [SerializeField] private bool useRegeniration = false;
        [SerializeField] private HealthSoundEffects healthSoundEffects;
        [SerializeField] private VelocityDamageProperties[] velocityDamageProperties;
        [SerializeField] private HealthRegenerationSystem regenerationSystem = new HealthRegenerationSystem();
        [SerializeField] private HealthCameraEffects healthCameraEffects = new HealthCameraEffects();

        private Action onDeathAction;

        private IController controller;
        private IRagdoll ragdollCallbacks;
        private AudioSource audioSource;

        private float storedTime;
        private bool deathActionIsCalled;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            ragdollCallbacks = GetComponent<IRagdoll>();
            controller = GetComponent<TPController>();
            audioSource = GetComponent<AudioSource>();

            regenerationSystem.Initialize(this);
            healthCameraEffects.Initialize(this);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            HipsDamageHandler hipsDamageHandler = ragdollCallbacks.GetHipsTransform().gameObject.AddComponent<HipsDamageHandler>();
            hipsDamageHandler.Initialize(this);

            onDeathAction += PlayDeathSound;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            RagdollHandle();
            OnDeathActionHandle();
            healthCameraEffects.OnEffectUpdate();
            HeartbeatProcessing();
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        private void OnCollisionEnter(Collision other)
        {
            if (!enabled)
            {
                return;
            }

            VelocityDamageProcessing(other.relativeVelocity);
        }

        public virtual void TakeDamage(int amount)
        {
            amount = Mathf.Abs(amount);
            health = (health - amount) >= minHealth ? health - amount : minHealth;
            ShakeCamera.Instance.AddDamageShake();
            if (useRegeniration)
                regenerationSystem.StartRegeneraionProcessing(this);
            if (IsAlive())
                PlayTakeDamageSound();
        }

        public virtual void TakeDamage(int amount, ShakeCamera.ShakeProperties shakeProperties)
        {
            amount = Mathf.Abs(amount);
            health = (health - amount) >= minHealth ? health - amount : minHealth;
            ShakeCamera.Instance.AddShakeEvent(shakeProperties);
            if (useRegeniration)
                regenerationSystem.StartRegeneraionProcessing(this);
            if (IsAlive())
                PlayTakeDamageSound();
        }

        public virtual void VelocityDamageProcessing(Vector3 velocity)
        {
            for (int i = 0; i < velocityDamageProperties.Length; i++)
            {
                VelocityDamageProperties property = velocityDamageProperties[i];
                if (TPEMathf.InRange(velocity.magnitude, property.GetMinSpeed(), property.GetMaxSpeed()))
                {
                    TakeDamage(property.GetDamage());
                    PlayVelocityDamageSound();
                }
            }
        }

        private void RagdollHandle()
        {
            if (IsAlive())
            {
                if (ragdollCallbacks.IsRagdolled())
                {
                    ragdollCallbacks.IsRagdolled(false);
                }
            }
            if (!IsAlive() && !ragdollCallbacks.IsRagdolled())
            {
                ragdollCallbacks.IsRagdolled(true);
            }
        }

        private void OnDeathActionHandle()
        {
            if (!IsAlive() && !deathActionIsCalled)
            {
                onDeathAction();
                deathActionIsCalled = true;
            }
            else if (IsAlive() && deathActionIsCalled)
            {
                deathActionIsCalled = false;
            }
        }

        public virtual void HeartbeatProcessing()
        {
            if (healthSoundEffects.GetHeartbeatSound() != null && health <= healthSoundEffects.GetHeartbeatStartFrom() && Time.time - storedTime >= healthSoundEffects.GetHeartbeatRate())
            {
                audioSource.PlayOneShot(healthSoundEffects.GetHeartbeatSound());
                storedTime = Time.time;
            }
        }

        public virtual void PlayTakeDamageSound()
        {
            if (healthSoundEffects.GetTakeDamageSound() != null)
            {
                audioSource.PlayOneShot(healthSoundEffects.GetTakeDamageSound());
            }
        }

        public virtual void PlayVelocityDamageSound()
        {
            if (healthSoundEffects.GetVelocityDamageSound())
            {
                audioSource.PlayOneShot(healthSoundEffects.GetVelocityDamageSound());
            }
        }

        public virtual void PlayDeathSound()
        {
            if (healthSoundEffects.GetDeathSound() != null)
            {
                audioSource.PlayOneShot(healthSoundEffects.GetDeathSound());
            }
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int value)
        {
            if (value > maxHealth)
                health = maxHealth;
            else if (value < minHealth)
                health = minHealth;
            else
                health = value;
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }

        public int GetMinHealth()
        {
            return minHealth;
        }

        public void SetMinHealth(int value)
        {
            minHealth = value > 0 ? value : 0;
        }

        public float GetHealthPercent()
        {
            return TPEMathf.GetPersent(health, maxHealth);
        }

        public HealthRegenerationSystem GetRegenerationSystem()
        {
            return regenerationSystem;
        }

        public IController GetControllerCallbacks()
        {
            return controller;
        }

        public bool RegenirationIsActive()
        {
            return useRegeniration;
        }

        public void RegenirationActive(bool value)
        {
            useRegeniration = value;
        }

        public HealthSoundEffects GetHealthSoundEffects()
        {
            return healthSoundEffects;
        }

        public void SetHealthSoundEffects(HealthSoundEffects value)
        {
            healthSoundEffects = value;
        }

        public VelocityDamageProperties[] GetFallDamageProperties()
        {
            return velocityDamageProperties;
        }

        public void SetFallDamagePropertiesRange(VelocityDamageProperties[] value)
        {
            velocityDamageProperties = value;
        }

        public HealthCameraEffects GetHealthCameraEffects()
        {
            return healthCameraEffects;
        }

        public void SetHealthCameraEffects(HealthCameraEffects value)
        {
            healthCameraEffects = value;
        }

        private Action OnDeathAction { get { return onDeathAction; } set { onDeathAction = value; } }

    }
}