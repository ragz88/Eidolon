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
using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public partial class ShakeCamera : Singleton<ShakeCamera>
    {
        private List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
        private IEnumerator onceShakeCoroutine;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (shakeEvents.Count > 0)
            {
                for (int i = shakeEvents.Count - 1; i != -1; i--)
                {
                    ShakeEvent shakeEvent = shakeEvents[i];
                    if (!shakeEvent.IsAlive())
                    {
                        shakeEvents.RemoveAt(i);
                        continue;
                    }
                    PlayShake(shakeEvent);
                }
            }
        }

        protected virtual void PlayShake(ShakeEvent shakeEvent)
        {
            Vector3 positionOffset = transform.localPosition;
            Vector3 rotationOffset = transform.localRotation.eulerAngles;
            shakeEvent.Update();
            switch (shakeEvent.GetTarget())
            {
                case ShakeEvent.Target.Position:
                    positionOffset += shakeEvent.GetNoise();
                    break;
                case ShakeEvent.Target.Rotation:
                    rotationOffset += shakeEvent.GetNoise();
                    break;
                case ShakeEvent.Target.Both:
                    positionOffset += shakeEvent.GetNoise();
                    rotationOffset += shakeEvent.GetNoise();
                    break;
            }
            transform.localPosition = positionOffset;
            transform.localRotation = Quaternion.Euler(rotationOffset);
        }

        public void AddShakeEvent(ShakeProperties properties)
        {
            shakeEvents.Add(new ShakeEvent(properties));
        }

        public void AddShakeEvent(ShakeEvent shakeEvent)
        {
            shakeEvents.Add(shakeEvent);
        }

        public void AddDamageShake()
        {
            AddShakeEvent(new ShakeCamera.ShakeProperties(3.0f, 5.0f, 0.5f, ShakeCamera.DefaultCurve(), ShakeCamera.ShakeEvent.Target.Rotation));
        }

        public void AddEarthquakeShake()
        {
            AddShakeEvent(new ShakeCamera.ShakeProperties(7.0f, 7.0f, 10.0f, ShakeCamera.DefaultCurve(), ShakeCamera.ShakeEvent.Target.Rotation));
        }

        public void AddHitShake()
        {
            AddShakeEvent(new ShakeProperties(0.5f, 5.0f, 0.5f, DefaultCurve(), ShakeEvent.Target.Rotation));
        }

        public void AddExplosionShake()
        {
            AddShakeEvent(new ShakeProperties(5.0f, 7.0f, 0.5f, DefaultCurve(), ShakeEvent.Target.Rotation));
        }

        public void AddExplosionShake(float radius, float distance, float minAmplitude = 0, float maxAmplitude = 0.55f)
        {
            float amplitude = Mathf.InverseLerp(0, radius, radius - distance);
            amplitude = Mathf.Clamp(amplitude, minAmplitude, maxAmplitude);
            AddShakeEvent(new ShakeProperties(amplitude, 5.0f, 0.5f, DefaultCurve(), ShakeEvent.Target.Rotation));
        }

        public List<ShakeEvent> GetShakeEvents()
        {
            return shakeEvents;
        }

        public void SetShakeEvents(List<ShakeEvent> value)
        {
            shakeEvents = value;
        }

        public static AnimationCurve DefaultCurve()
        {
            return new AnimationCurve(
                new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
                new Keyframe(0.2f, 1.0f),
                new Keyframe(1.0f, 0.0f));
        }
    }
}