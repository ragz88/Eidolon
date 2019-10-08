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
    [System.Serializable]
    public struct HealthRegenirationProperties : IEquatable<HealthRegenirationProperties>
    {
        [SerializeField] private float rate;
        [SerializeField] private int value;
        [SerializeField] private float delay;

        /// <summary>
        /// Regeniration properties constructor.
        /// </summary>
        /// <param name="rate">Rate (in seconds) of adding health points, (V/R - Value per rate).</param>
        /// <param name="value">Health point value.</param>
        /// <param name="delay">Delay before start adding health.</param>
        internal HealthRegenirationProperties(float rate, int value, float delay)
        {
            this.rate = rate;
            this.value = value;
            this.delay = delay;
        }

        /// <summary>
        /// Return rate of adding health points, (V/R - Value per rate).
        /// </summary>
        /// <returns></returns>
        public float GetRate()
        {
            return rate;
        }

        /// <summary>
        /// Set rate of adding health points, (V/R - Value per rate).
        /// </summary>
        /// <param name="value"></param>
        public void SetRate(float value)
        {
            rate = value;
        }

        /// <summary>
        /// Return health point value.
        /// </summary>
        /// <returns></returns>
        public int GetValue()
        {
            return value;
        }

        /// <summary>
        /// Set health point value.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Return delay before start adding health.
        /// </summary>
        /// <returns></returns>
        public float GetDelay()
        {
            return delay;
        }

        /// <summary>
        /// Set delay before start adding health.
        /// </summary>
        /// <param name="value"></param>
        public void SetDelay(float value)
        {
            delay = value;
        }

        /// <summary>
        /// Empty Regeniration properties.
        /// </summary>
        /// <returns></returns>
        public readonly static HealthRegenirationProperties Empty = new HealthRegenirationProperties(0, 0, 0);

        public static bool operator ==(HealthRegenirationProperties left, HealthRegenirationProperties right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HealthRegenirationProperties left, HealthRegenirationProperties right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is HealthRegenirationProperties metrics) && Equals(metrics);
        }

        public bool Equals(HealthRegenirationProperties other)
        {
            return (rate, value, delay) == (other.rate, other.value, other.delay);
        }

        public override int GetHashCode()
        {
            return (rate, value, delay).GetHashCode();
        }
    }
}