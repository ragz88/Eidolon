/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

namespace ThirdPersonEngine.Runtime
{
    public partial class AdaptiveRagdollSystem
    {
        //Possible states of the ragdoll
        internal enum RagdollState
        {
            /// <summary>
            /// Mecanim is fully in control
            /// </summary>
            Animated,
            /// <summary>
            /// Mecanim turned off, but when stable position will be found, the transition to Animated will heppend
            /// </summary>
            WaitStablePosition,
            /// <summary>
            /// Mecanim turned off, physics controls the ragdoll
            /// </summary>
            Ragdolled,
            /// <summary>
            /// Mecanim in control, but LateUpdate() is used to partially blend in the last ragdolled pose
            /// </summary>
            BlendToAnim,
        }
    }
}