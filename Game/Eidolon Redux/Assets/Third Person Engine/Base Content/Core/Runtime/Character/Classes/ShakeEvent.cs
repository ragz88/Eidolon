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
    public partial class ShakeCamera
    {
        [System.Serializable]
        public class ShakeEvent
        {
            public enum Target
            {
                None,
                Position,
                Rotation,
                Both
            }

            [SerializeField] private Vector3 noise;

            private float duration;
            private float timeRemaining;
            private ShakeProperties data;
            private Vector3 noiseOffset;

            public ShakeEvent(ShakeProperties data)
            {
                this.data = data;

                duration = data.GetDuration();
                timeRemaining = duration;

                float rand = 32.0f;

                noiseOffset.x = Random.Range(0.0f, rand);
                noiseOffset.y = Random.Range(0.0f, rand);
                noiseOffset.z = Random.Range(0.0f, rand);
            }

            public void Update()
            {
                float deltaTime = Time.deltaTime;

                timeRemaining -= deltaTime;

                float noiseOffsetDelta = deltaTime * data.GetFrequency();

                noiseOffset.x += noiseOffsetDelta;
                noiseOffset.y += noiseOffsetDelta;
                noiseOffset.z += noiseOffsetDelta;

                noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
                noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

                noise -= Vector3.one * 0.5f;

                noise *= data.GetAmplitude();

                float agePercent = 1.0f - (timeRemaining / duration);
                noise *= data.GetBlendOverLifetime().Evaluate(agePercent);
            }

            public bool IsAlive()
            {
                return timeRemaining > 0.0f;
            }

            public Target GetTarget()
            {
                return data.GetTarget();
            }

            public Vector3 GetNoise()
            {
                return noise;
            }

            public void SetNoise(Vector3 value)
            {
                noise = value;
            }

            public float GetDuration()
            {
                return duration;
            }

            public void SetDuration(float value)
            {
                duration = value;
            }

            public float GetTimeRemaining()
            {
                return timeRemaining;
            }

            public void SetTimeRemaining(float value)
            {
                timeRemaining = value;
            }

            public ShakeProperties GetData()
            {
                return data;
            }

            public void SetData(ShakeProperties value)
            {
                data = value;
            }

            public Vector3 GetNoiseOffset()
            {
                return noiseOffset;
            }

            public void SetNoiseOffset(Vector3 value)
            {
                noiseOffset = value;
            }
        }
    }
}