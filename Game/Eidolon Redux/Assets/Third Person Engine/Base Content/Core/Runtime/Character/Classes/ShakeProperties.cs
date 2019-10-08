/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public partial class ShakeCamera
    {
        [System.Serializable]
        public struct ShakeProperties : IEquatable<ShakeProperties>
        {
            [SerializeField] private ShakeEvent.Target target;
            [SerializeField] private float amplitude;
            [SerializeField] private float frequency;
            [SerializeField] private float duration;
            [SerializeField] private AnimationCurve blendOverLifetime;

            public ShakeProperties(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, ShakeEvent.Target target)
            {
                this.target = target;
                this.amplitude = amplitude;
                this.frequency = frequency;
                this.duration = duration;
                this.blendOverLifetime = blendOverLifetime;
            }

            public static ShakeProperties Empty { get { return new ShakeProperties(0, 0, 0, null, ShakeEvent.Target.Position); } }

            public ShakeEvent.Target GetTarget()
            {
                return target;
            }

            public void SetTarget(ShakeEvent.Target value)
            {
                target = value;
            }

            public float GetAmplitude()
            {
                return amplitude;
            }

            public void SetAmplitude(float value)
            {
                amplitude = value;
            }

            public float GetFrequency()
            {
                return frequency;
            }

            public void SetFrequency(float value)
            {
                frequency = value;
            }

            public float GetDuration()
            {
                return duration;
            }

            public void SetDuration(float value)
            {
                duration = value;
            }

            public AnimationCurve GetBlendOverLifetime()
            {
                return blendOverLifetime;
            }

            public void SetBlendOverLifetime(AnimationCurve value)
            {
                blendOverLifetime = value;
            }

            public static bool operator ==(ShakeProperties left, ShakeProperties right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ShakeProperties left, ShakeProperties right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is ShakeProperties metrics) && Equals(metrics);
            }

            public bool Equals(ShakeProperties other)
            {
                return (amplitude, blendOverLifetime, duration, frequency, target) == (other.amplitude, other.blendOverLifetime, other.duration, other.frequency, other.target);
            }

            public override int GetHashCode()
            {
                return (amplitude, blendOverLifetime, duration, frequency, target).GetHashCode();
            }
        }
    }
}