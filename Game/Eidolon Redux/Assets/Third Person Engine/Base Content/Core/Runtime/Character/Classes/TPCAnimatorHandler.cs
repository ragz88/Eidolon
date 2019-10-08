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
	[System.Serializable]
	public class TPCAnimatorHandler
	{
		public const string SPEED = "Speed";
		public const string DIRECTION = "Direction";
		public const string GROUND_DISTANCE = "GroundDistance";
		public const string MOVE_AMOUNT = "MoveAmount";
		public const string VERTICAL_VELOCITY = "VerticalVelocity";
		public const string IS_GROUNDED = "IsGrounded";
		public const string IS_CROUCHING = "IsCrouching";

		public readonly static int SpeedHash = Animator.StringToHash(SPEED);
		public readonly static int DirectionHash = Animator.StringToHash(DIRECTION);
		public readonly static int GroundDistanceHash = Animator.StringToHash(GROUND_DISTANCE);
		public readonly static int MoveAmountHash = Animator.StringToHash(MOVE_AMOUNT);
		public readonly static int VerticalVelocityHash = Animator.StringToHash(VERTICAL_VELOCITY);
		public readonly static int IsGroundedHash = Animator.StringToHash(IS_GROUNDED);
		public readonly static int IsCrouchingHash = Animator.StringToHash(IS_CROUCHING);

		private Animator animator;
		private Transform transform;
		private TPController controller;

		/// <summary>
		/// Initialize is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		public virtual void Initialize(Animator animator, Transform transform, TPController controller)
		{
			this.animator = animator;
			this.transform = transform;
			this.controller = controller;
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		public virtual void Update()
		{
			animator.SetFloat(SpeedHash, controller.GetSpeed(), 0.1f, Time.deltaTime);
			if (controller.LocomotionTypeIs(LocomotionType.Strafe))
			{
				animator.SetFloat(DirectionHash, controller.GetDirection(), 0.1f, Time.deltaTime);
			}
			if (!controller.IsGrounded())
			{
				animator.SetFloat(VerticalVelocityHash, controller.GetVerticalVelocity());
			}
			animator.SetFloat(GroundDistanceHash, controller.GetGroundDistance());
			animator.SetInteger(MoveAmountHash, controller.GetRawMoveAmount());
			animator.SetBool(IsGroundedHash, controller.IsGrounded());
			animator.SetBool(IsCrouchingHash, controller.IsCrouching());

		}

		/// <summary>
		/// Callback for processing animation movements for modifying root motion.
		/// </summary>
		public virtual void AnimatorMove()
		{
			if (!controller.IsGrounded())
			{
				return;
			}

			transform.rotation = animator.rootRotation;

			float fixedSpeed = controller.GetMoveAmount();
			if (controller.LocomotionTypeIs(LocomotionType.Strafe))
			{
				if(controller.IsSprinting())
					fixedSpeed = 1.5f;
				if (fixedSpeed <= 0.5f)
					fixedSpeed = !controller.IsCrouching() ? controller.GetStrafeSpeed().GetWalk() : controller.GetStrafeCrouchSpeed().GetWalk();
				else if (fixedSpeed > 0.5f && fixedSpeed <= 1f)
					fixedSpeed = !controller.IsCrouching() ? controller.GetStrafeSpeed().GetRun() : controller.GetStrafeCrouchSpeed().GetRun();
				else
					fixedSpeed = !controller.IsCrouching() ? controller.GetStrafeSpeed().GetSprint() : controller.GetStrafeCrouchSpeed().GetSprint();
			}
			else if (controller.LocomotionTypeIs(LocomotionType.Free))
			{
				if(controller.IsSprinting())
					fixedSpeed = 2.0f;
				if (fixedSpeed <= 0.5f)
					fixedSpeed = !controller.IsCrouching() ? controller.GetFreeSpeed().GetWalk() : controller.GetFreeCrouchSpeed().GetWalk();
				else if (fixedSpeed > 0.5 && fixedSpeed <= 1f)
					fixedSpeed = !controller.IsCrouching() ? controller.GetFreeSpeed().GetRun() : controller.GetFreeCrouchSpeed().GetRun();
				else
					fixedSpeed = !controller.IsCrouching() ? controller.GetFreeSpeed().GetSprint() : controller.GetFreeCrouchSpeed().GetSprint();
			}
			controller.ControlSpeed(fixedSpeed);
		}

		public void ResetParameters()
		{
			animator.SetFloat(SpeedHash, 0);
			animator.SetFloat(DirectionHash, 0);
			animator.SetFloat(VerticalVelocityHash, 0);
			animator.SetInteger(MoveAmountHash, 0);
		}

		public Animator GetAnimator()
		{
			return animator;
		}

		public Transform GetTransform()
		{
			return transform;
		}

		public IController GetController()
		{
			return controller;
		}
	}
}