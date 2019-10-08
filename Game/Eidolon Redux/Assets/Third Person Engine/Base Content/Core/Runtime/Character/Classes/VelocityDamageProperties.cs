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
    public partial class CharacterHealth
    {
        [System.Serializable]
        public struct VelocityDamageProperties : IEquatable<VelocityDamageProperties>
        {
            [SerializeField] private int damage;
            [SerializeField] private float minSpeed;
            [SerializeField] private float maxSpeed;

            /// <summary>
            /// FallDamage properties constructor.
            /// </summary>
            internal VelocityDamageProperties(int damage, float minSpeed, float maxSpeed)
            {
                this.damage = damage;
                this.minSpeed = minSpeed;
                this.maxSpeed = maxSpeed;
            }

            /// <summary>
            /// Return damage (in health point value).
            /// </summary>
            public int GetDamage()
            {
                return damage;
            }

            /// <summary>
            /// Set damage (in health point value).
            /// </summary>
            public void SetDamage(int value)
            {
                damage = value;
            }

            /// <summary>
            /// Return min speed for take damage.
            /// </summary>
            public float GetMinSpeed()
            {
                return minSpeed;
            }

            /// <summary>
            /// Set min speed for take damage.
            /// </summary>
            public void SetMinSpeed(float value)
            {
                minSpeed = value;
            }

            /// <summary>
            /// Return max height position for take damage.
            /// </summary>
            public float GetMaxSpeed()
            {
                return maxSpeed;
            }

            /// <summary>
            /// Set max height position for take damage.
            /// </summary>
            public void SetMaxSpeed(float value)
            {
                maxSpeed = value;
            }

            /// <summary>
            /// Empty FallDamage properties.
            /// </summary>
            public static VelocityDamageProperties Empty
            {
                get
                {
                    return new VelocityDamageProperties(0, 0, 0);
                }
            }

            public static bool operator ==(VelocityDamageProperties left, VelocityDamageProperties right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(VelocityDamageProperties left, VelocityDamageProperties right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is VelocityDamageProperties metrics) && Equals(metrics);
            }

            public bool Equals(VelocityDamageProperties other)
            {
                return (damage, minSpeed, maxSpeed) == (other.damage, other.minSpeed, other.maxSpeed);
            }

            public override int GetHashCode()
            {
                return (damage, minSpeed, maxSpeed).GetHashCode();
            }
        }
    }
}