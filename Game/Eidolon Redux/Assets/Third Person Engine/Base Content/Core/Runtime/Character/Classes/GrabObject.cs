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
	public class GrabObject : MonoBehaviour
	{
		[SerializeField] private Vector3 anchor = Vector3.zero;
		[SerializeField] private Vector3 axis = new Vector3(1, 1.0f, 0.0f);
		[SerializeField] private Vector3 connectedAnchor = new Vector3(0.0f, 1.25f, 1.0f);
		[SerializeField] private JointSpring jointSpring = DefaultJointSpring;
		[SerializeField] private Transform leftHandIKTarget;
		[SerializeField] private Transform rightHandIKTarget;

		private HingeJoint objectHingeJoint;

		public HingeJoint ConnectJoint(Rigidbody rigidbody)
		{
			objectHingeJoint = gameObject.AddComponent<HingeJoint>();
			objectHingeJoint.connectedBody = rigidbody;
			objectHingeJoint.anchor = anchor;
			objectHingeJoint.axis = axis;
			objectHingeJoint.autoConfigureConnectedAnchor = false;
			objectHingeJoint.connectedAnchor = connectedAnchor;
			objectHingeJoint.useSpring = true;
			objectHingeJoint.spring = jointSpring;

			return objectHingeJoint;
		}

		public virtual void RemoveJoint()
		{
			Destroy(objectHingeJoint);
		}

		public Transform GetLeftHandIKTarget()
		{
			return leftHandIKTarget;
		}

		public void SetLeftHandIKTarget(Transform value)
		{
			leftHandIKTarget = value;
		}

		public Transform GetRightHandIKTarget()
		{
			return rightHandIKTarget;
		}

		public void SetRightHandIKTarget(Transform value)
		{
			rightHandIKTarget = value;
		}

		public HingeJoint GetCurrentHingeJoint()
		{
			return objectHingeJoint;
		}

		public readonly static JointSpring DefaultJointSpring = new JointSpring()
		{
			spring = 100.0f,
				damper = 0.0f,
				targetPosition = 0.0f
		};

		public Vector3 GetAnchor()
		{
			return anchor;
		}

		public void SetAnchor(Vector3 value)
		{
			anchor = value;
		}

		public Vector3 GetAxis()
		{
			return axis;
		}

		public void SetAxis(Vector3 value)
		{
			axis = value;
		}

		public Vector3 GetConnectedAnchor()
		{
			return connectedAnchor;
		}

		public void SetConnectedAnchor(Vector3 value)
		{
			connectedAnchor = value;
		}

		public JointSpring GetJointSpring()
		{
			return jointSpring;
		}

		public void SetJointSpring(JointSpring value)
		{
			jointSpring = value;
		}
	}
}