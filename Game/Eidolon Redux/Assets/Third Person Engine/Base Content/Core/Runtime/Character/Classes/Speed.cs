/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using System;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public partial class TPController
    {
        [Serializable]
        public struct Speed : IEquatable<Speed>
        {
            [SerializeField] private float walk;
            [SerializeField] private float run;
            [SerializeField] private float sprint;

            public Speed(float walk, float run, float sprint)
            {
                this.walk = walk;
                this.run = run;
                this.sprint = sprint;
            }

            public float GetWalk()
            {
                return walk;
            }

            public void SetWalk(float value)
            {
                walk = value;
            }

            public float GetRun()
            {
                return run;
            }

            public void SetRun(float value)
            {
                run = value;
            }

            public float GetSprint()
            {
                return sprint;
            }

            public void SetSprint(float value)
            {
                sprint = value;
            }

            public readonly static Speed Zero = new Speed(0, 0, 0);

            public readonly static Speed Stay = new Speed(2.5f, 3.0f, 4.0f);

            public readonly static Speed Crouch = new Speed(1.5f, 2.5f, 3.0f);

            public static bool operator ==(Speed left, Speed right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Speed left, Speed right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is Speed metrics) && Equals(metrics);
            }

            public bool Equals(Speed other)
            {
                return (walk, run, sprint) == (other.walk, other.run, other.sprint);
            }

            public override int GetHashCode()
            {
                return (walk, run, sprint).GetHashCode();
            }
        }
    }
}