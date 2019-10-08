/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace ThirdPersonEngine.Runtime
{
    public partial class CharacterHealth
    {
        [System.Serializable]
        public class HealthCameraEffects
        {
            [SerializeField] private PostProcessingProfile profile;
            [SerializeField] private int startFrom = 50;
            [SerializeField] private float resetSpeed = 10;
            [SerializeField] private float chromaticAberrationSpeed = 2;
            [SerializeField] private float vignetteSmooth = 10;
            [SerializeField] private float vignetteMinValue = 0.3f;
            [SerializeField] private float vignetteMaxValue = 0.5f;

            private IHealth health;
            private VignetteModel.Settings vignetteSettings;
            private ChromaticAberrationModel.Settings chromaticAberrationSettings;

            /// <summary>
            /// Awake is called when the script instance is being loaded.
            /// </summary>
            public virtual void Initialize(IHealth health)
            {
                this.health = health;
                if (profile != null)
                {
                    vignetteSettings = profile.vignette.settings;
                    chromaticAberrationSettings = profile.chromaticAberration.settings;
                }
            }

            /// <summary>
            /// Processing health post processing effects
            /// </summary>
            public virtual void OnEffectUpdate()
            {
                if (profile == null || health == null)
                {
                    return;
                }

                if (health.GetHealth() <= startFrom)
                {
                    float healthInverseLerp = Mathf.InverseLerp(startFrom, 0, health.GetHealth());
                    float intensityPingPong = Mathf.PingPong(Time.time * (healthInverseLerp * chromaticAberrationSpeed), healthInverseLerp);
                    float intensityLerp = Mathf.Lerp(vignetteSettings.intensity, healthInverseLerp, Time.deltaTime * vignetteSmooth);
                    chromaticAberrationSettings.intensity = intensityPingPong;
                    vignetteSettings.intensity = Mathf.Clamp(intensityLerp, vignetteMinValue, vignetteMaxValue);
                    profile.chromaticAberration.settings = chromaticAberrationSettings;
                    profile.vignette.settings = vignetteSettings;
                }
                else if (profile.vignette.settings.intensity > 0 || profile.chromaticAberration.settings.intensity > 0)
                {
                    chromaticAberrationSettings.intensity = Mathf.Lerp(chromaticAberrationSettings.intensity, 0, Time.deltaTime * resetSpeed);
                    vignetteSettings.intensity = Mathf.Lerp(vignetteSettings.intensity, 0, Time.deltaTime * resetSpeed);
                    profile.chromaticAberration.settings = chromaticAberrationSettings;
                    profile.vignette.settings = vignetteSettings;
                }
            }

            public PostProcessingProfile GetProfile()
            {
                return profile;
            }

            public void SetProfile(PostProcessingProfile value)
            {
                profile = value;
                Initialize(health);
            }

            public IHealth GetHealth()
            {
                return health;
            }

            public int GetStartPoint()
            {
                return startFrom;
            }

            public void SetStartPoint(int value)
            {
                startFrom = value;
            }

            public float GetResetSmooth()
            {
                return resetSpeed;
            }

            public void SetResetSmooth(float value)
            {
                resetSpeed = value;
            }

            public float GetChromaticAberrationSpeed()
            {
                return chromaticAberrationSpeed;
            }

            public void SetChromaticAberrationSpeed(float value)
            {
                chromaticAberrationSpeed = value;
            }

            public float GetVignetteSmooth()
            {
                return vignetteSmooth;
            }

            public void SetVignetteSmooth(float value)
            {
                vignetteSmooth = value;
            }

            public float GetVignetteMinValue()
            {
                return vignetteMinValue;
            }

            public void SetVignetteMinValue(float value)
            {
                vignetteMinValue = value;
            }

            public float GetVignetteMaxValue()
            {
                return vignetteMaxValue;
            }

            public void SetVignetteMaxValue(float value)
            {
                vignetteMaxValue = value;
            }
        }
    }
}