/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public partial class TPController
    {
        [System.Serializable]
        public struct OnWallStopProperties
        {
            [SerializeField] private float checkRange;
            [SerializeField] private float angle;
            [SerializeField] private bool checkConditions;
            [SerializeField] private LayerMask wallLayer;

            public OnWallStopProperties(float checkRange, float angle, bool checkConditions, LayerMask mask)
            {
                this.checkRange = checkRange;
                this.angle = angle;
                this.checkConditions = checkConditions;
                this.wallLayer = mask;
            }

            public readonly static OnWallStopProperties Default = new OnWallStopProperties(0.5f, 70.0f, true, 1 << 0);

            public float GetCheckRange()
            {
                return checkRange;
            }

            public void SetCheckRange(float value)
            {
                checkRange = value;
            }

            public float GetAngle()
            {
                return angle;
            }

            public void SetAngle(float value)
            {
                angle = value;
            }

            public bool CheckAngle(float angle)
            {
                return angle <= this.angle;
            }

            public bool CheckConditions()
            {
                return checkConditions;
            }

            public void CheckConditions(bool value)
            {
                checkConditions = value;
            }

            public LayerMask GetWallLayer()
            {
                return wallLayer;
            }

            public void SetWallLayer(LayerMask value)
            {
                wallLayer = value;
            }
        }
    }
}