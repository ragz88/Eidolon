/* =======================================================================
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
    public interface IController
    {
        float GetDirection();

        float GetSpeed();

        float GetGroundDistance();

        float GetMoveAmount();

        int GetRawMoveAmount();

        float GetVerticalVelocity();

        bool LocomotionTypeIs(LocomotionType locomotionType);

        bool IsJumping();

        bool IsSprinting();

        bool IsCrouching();

        bool IsGrounded();

        bool IsSliding();

        Vector3 GetVelocity();

        void SetColliderHeight(float height);
        
        float GetColliderHeight();

        LayerMask GetGroundLayer();

        void Enabled(bool enabled);

        bool Enabled();
    }
}