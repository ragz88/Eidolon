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
    /// <summary>
    /// Advanced third person character footstep sound system.
    /// Extension of SimpleFootstepSystem.cs class.
    /// </summary>
    public class AdvancedFootstepSystem : SimpleFootstepSystem
    {
        [SerializeField] private Transform leftFootPivot;
        [SerializeField] private Transform rightFootPivot;
        [SerializeField] private float rayRange = 0.17f;

        protected override void StepSoundProcessing()
        {
            if (IntervalIsMoved())
            {
                FootstepProperty footstepProperty = FootstepProperty.Empty;

                if (leftFootPivot != null)
                {
                    footstepProperty = GetFootstepPropertyOfSurface(leftFootPivot.position, -Vector3.up, rayRange);
                    if (!footstepProperty.IsEmpty())
                    {
                        AudioClip clip = footstepProperty.GetRandomStepSound();
                        PlayStepSound(clip);
                        ResetMovedInterval();
                    }
                }

                if (rightFootPivot != null)
                {
                    footstepProperty = GetFootstepPropertyOfSurface(rightFootPivot.position, -Vector3.up, rayRange);
                    if (!footstepProperty.IsEmpty())
                    {
                        AudioClip clip = footstepProperty.GetRandomStepSound();
                        PlayStepSound(clip);
                        ResetMovedInterval();
                    }
                }
            }
        }

        public Transform GetLeftFootPivot()
        {
            return leftFootPivot;
        }

        public void SetLeftFootPivot(Transform value)
        {
            leftFootPivot = value;
        }

        public Transform GetRightFootPivot()
        {
            return rightFootPivot;
        }

        public void SetRightFootPivot(Transform value)
        {
            rightFootPivot = value;
        }

        public float GetRayRange()
        {
            return rayRange;
        }

        public void SetRayRange(float value)
        {
            rayRange = value;
        }
    }
}