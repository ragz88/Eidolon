/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

namespace ThirdPersonEngine.Runtime
{
    /// <summary>
    /// Character locomotion type	
    /// 	Description:	
    /// 		Strafe - The character moves forward, backward, left, right with a specific animation.
    /// 		Free - The character moves in all directions, with the direction of the body in the direction of movement.
    /// </summary>
    public enum LocomotionType
    {
        Strafe,
        Free
    }
}