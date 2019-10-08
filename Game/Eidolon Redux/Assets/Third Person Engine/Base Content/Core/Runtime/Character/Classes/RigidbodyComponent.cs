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
    public partial class AdaptiveRagdollSystem
    {
        internal struct RigidbodyComponent
        {
            private Rigidbody rigidBody;
            private CharacterJoint joint;
            private Vector3 connectedAnchorDefault;

            public RigidbodyComponent(Rigidbody rigidBody)
            {
                this.rigidBody = rigidBody;
                joint = rigidBody.GetComponent<CharacterJoint>();
                if (joint != null)
                    connectedAnchorDefault = joint.connectedAnchor;
                else
                    connectedAnchorDefault = Vector3.zero;
            }

            public Rigidbody GetRigidBody()
            {
                return rigidBody;
            }

            public CharacterJoint GetJoint()
            {
                return joint;
            }

            public Vector3 GetConnectedAnchorDefault()
            {
                return connectedAnchorDefault;
            }
        }
    }
}