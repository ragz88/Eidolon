/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using System.Collections;
using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class RespawnManager : Singleton<RespawnManager>, IRespawnManager
    {
        public enum RespawnType
        {
            Auto,
            ByKey
        }

        [SerializeField] private Transform character;
        [SerializeField] private RespawnType respawnType = RespawnType.Auto;
        [SerializeField] private KeyCode key = KeyCode.Space;
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float delay = 3.0f;
        [SerializeField] private int reSpawnHealth = 100;
        [SerializeField] private AudioClip spawnSound;
        [SerializeField] private ScreenFadeProperties screenFadeProperties = ScreenFadeProperties.DefaultProperties;
        [SerializeField] private float timeToFade = 0.5f;
        [SerializeField] private bool useScreenFade = true;

        private Action onRespawn;

        private ScreenFade screenFade;
        private IHealth healthInterface;
        private AudioSource audioSource;
        private float storedTime;
        private IEnumerator respawnCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            screenFade = ScreenFade.Instance;
            healthInterface = character.GetComponent<IHealth>();
            audioSource = GetComponent<AudioSource>();

            onRespawn += AddHealth;
            onRespawn += PlayRespawnSound;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (healthInterface != null && !healthInterface.IsAlive())
            {
                if (storedTime == 0)
                {
                    storedTime = Time.time;
                }
                else if (Time.time - storedTime >= delay && (CompareRespawnType(RespawnType.Auto) || CompareRespawnType(RespawnType.ByKey) && Input.GetKeyDown(key)))
                {
                    Respawn();
                    storedTime = 0;
                }
            }
        }

        /// <summary>
        /// Respawn character.
        /// </summary>
        public virtual void Respawn()
        {
            if (respawnCoroutine != null)
            {
                return;
            }
            respawnCoroutine = RespawnCoroutine();
            StartCoroutine(respawnCoroutine);
        }

        public virtual IEnumerator RespawnCoroutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0);
            if (useScreenFade)
            {
                screenFade.PingPongFade(screenFadeProperties);
                waitForSeconds = new WaitForSeconds(timeToFade);
            }
            yield return waitForSeconds;
            Vector3 spawnPoint = TPEMathf.RandomPositionInCircle(transform.position, radius);
            spawnPoint.y = transform.position.y;
            character.SetPositionAndRotation(spawnPoint, Quaternion.identity);
            onRespawn();
            respawnCoroutine = null;
            yield break;
        }

        private void PlayRespawnSound()
        {
            if (spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }
        }

        private void AddHealth()
        {
            if (healthInterface != null)
            {
                healthInterface.SetHealth(reSpawnHealth);
            }
        }

        public Transform GetCharacter()
        {
            return character;
        }

        public void SetCharacter(Transform value)
        {
            character = value;
        }

        public RespawnType GetRespawnType()
        {
            return respawnType;
        }

        public void SetRespawnType(RespawnType value)
        {
            respawnType = value;
        }

        public bool CompareRespawnType(RespawnType spawnType)
        {
            return this.respawnType == spawnType;
        }

        public KeyCode GetKey()
        {
            return key;
        }

        public void SetKey(KeyCode value)
        {
            key = value;
        }

        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float value)
        {
            radius = value;
        }

        public float GetDelay()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }

        public int GetRespawnHealth()
        {
            return reSpawnHealth;
        }

        public void SetRespawnHealth(int value)
        {
            reSpawnHealth = value;
        }

        public IHealth GetHealthInterface()
        {
            return healthInterface;
        }

        public AudioClip GetRespawnSound()
        {
            return spawnSound;
        }

        public void SetRespawnSound(AudioClip value)
        {
            spawnSound = value;
        }

        public ScreenFadeProperties GetScreenFadeProperties()
        {
            return screenFadeProperties;
        }

        public void SetScreenFadeProperties(ScreenFadeProperties value)
        {
            screenFadeProperties = value;
        }

        public float GetTimeToFade()
        {
            return timeToFade;
        }

        public void SetTimeToFade(float value)
        {
            timeToFade = value;
        }

        public bool UseScreenFade()
        {
            return useScreenFade;
        }

        public void UseScreenFade(bool value)
        {
            useScreenFade = value;
        }
    }
}