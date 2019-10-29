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
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public partial class TPController : MonoBehaviour, IController
	{
		private const string JUMP_STATE = "Jump";
		private const string JUMP_IN_MOVE_STATE = "JumpMove";

		private readonly static int JumpStateHash = Animator.StringToHash(JUMP_STATE);
		private readonly static int JumpInMoveStateHash = Animator.StringToHash(JUMP_IN_MOVE_STATE);

		public enum SprintDirection
		{
			Free,
			Forward,
			ForwardWithSide
		}

		// Locomotion
		[SerializeField] private LocomotionType locomotionType = LocomotionType.Strafe;
		[SerializeField] private SprintDirection sprintDirection = SprintDirection.ForwardWithSide;
		[SerializeField] private float freeRotationSpeed = 10.0f;
		[SerializeField] private float strafeRotationSpeed = 10.0f;
		[SerializeField] private bool rootMotion = false;
		[SerializeField] private bool keepDirection = false;
		[SerializeField] private Speed freeSpeed = Speed.Stay;
		[SerializeField] private Speed freeCrouchSpeed = Speed.Crouch;
		[SerializeField] private Speed strafeSpeed = Speed.Stay;
		[SerializeField] private Speed strafeCrouchSpeed = Speed.Crouch;
		[SerializeField] private float crouchAmplitude = 0.67f;
		[SerializeField] private float crouchRate = 5.0f;
		[SerializeField] private bool crouchSprint = false;
		[SerializeField] private OnWallStopProperties onWallStopProperties = OnWallStopProperties.Default;
        public float zoneMovementSpeed = 1f;

        // Air
        [SerializeField] private bool airControl = true;
		[SerializeField] private float jumpTimer = 0.3f;
		[SerializeField] private float airValue = 3.0f;
		[SerializeField] private float jumpHeight = 4.0f;

		// Other
		[SerializeField] private float stepOffsetEnd = 0.45f;
		[SerializeField] private float stepOffsetStart = 0.05f;
		[SerializeField] private float stepSmooth = 4.0f;
		[SerializeField] private float slopeLimit = 45.0f;
		[SerializeField] private float slidingSpeed = 1.0f;
		[SerializeField] private float extraGravity = -10.0f;
		[SerializeField] private LayerMask groundLayer = 1 << 0;
		[SerializeField] private float groundMinDistance = 0.2f;
		[SerializeField] private float groundMaxDistance = 0.5f;

		private TPCAnimatorHandler animatorHandler = new TPCAnimatorHandler();

		// Actions
		private bool isGrounded;
		private bool isJumping;
		private bool isSprinting;
		private bool isCrouching;
		private bool isSliding;

		// Caches
		private Animator animator;
		private Rigidbody characterRigidbody;
		private PhysicMaterial maxFrictionPhysics;
		private PhysicMaterial frictionPhysics;
		private PhysicMaterial slippyPhysics;
		private CapsuleCollider capsuleCollider;

		// Other
		private float speed;
		private float direction;
		private float velocity;
		private float verticalVelocity;
		private float vertical;
		private float horizontal;
		private RaycastHit groundHit;
		private Vector3 targetDirection;
		private Quaternion targetRotation;
		private Quaternion freeRotation;
		private float groundDistance;
		private float jumpCounter;
		private float colliderHeight;
		private float wasColliderCenter;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		protected virtual void Awake()
		{
			animator = GetComponent<Animator>();
			characterRigidbody = GetComponent<Rigidbody>();
			capsuleCollider = GetComponent<CapsuleCollider>();

			animatorHandler.Initialize(animator, transform, this);

			colliderHeight = capsuleCollider.height;
			wasColliderCenter = capsuleCollider.center.y;

			CreatePhysicMaterial();
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		protected virtual void Update()
		{
			ReadInput();
			CheckGround();
			ControlLocomotion();
			AirUpdate();
			CrouchProcessing();
			animatorHandler.Update();
		}

		/// <summary>
		/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		/// </summary>
		protected virtual void FixedUpdate()
		{
			AirControlProcessing();
		}

		/// <summary>
		/// Callback for processing animation movements for modifying root motion.
		/// </summary>
		protected virtual void OnAnimatorMove()
		{
			animatorHandler.AnimatorMove();
		}

		protected virtual void ReadInput()
		{
			vertical = TPCInput.GetAxis(INC.CHAR_VERTICAL);
			horizontal = TPCInput.GetAxis(INC.CHAR_HORIZONTAL);

			if (TPCInput.GetButtonDown(INC.JUMP))
			{
				Jump();
			}
		}

		private void ControlLocomotion()
		{
			switch (locomotionType)
			{
				case LocomotionType.Free:
					FreeMovement();
					break;
				case LocomotionType.Strafe:
					StrafeMovement();
					break;
			}
		}

		public virtual void StrafeMovement()
		{
			if (GetMoveAmount() == 1 &&
				TPCInput.GetButton(INC.SPRINT) &&
				IsSprintDirection() &&
				CrouchSprintConditions())
			{

				// TODO: Verify contected is keyboard or gamepad. / Change handling of [speed] and [direction] variables.
				// PLUG: Override the variables for STANDALONE builds, means that STANDALONE builds use keyboard.
#if UNITY_STANDALONE
				float vertical = TPCInput.GetAxisRaw(INC.CHAR_VERTICAL);
				float horizontal = TPCInput.GetAxisRaw(INC.CHAR_HORIZONTAL);
#endif

				if (vertical != 0)
					speed = Mathf.SmoothStep(speed, 2 * Mathf.Sign(vertical), 8.5f * Time.deltaTime) ;
				else
					speed = 0;

				if (horizontal != 0)
					direction = Mathf.SmoothStep(direction, 2 * Mathf.Sign(horizontal), 8.5f * Time.deltaTime);
				else
					direction = 0;

				isSprinting = true;
			}
			else if (GetMoveAmount() > 0)
			{
				speed = Mathf.Clamp(vertical, -1, 1) * zoneMovementSpeed;
				direction = Mathf.Clamp(horizontal, -1, 1) * zoneMovementSpeed;
				isSprinting = false;
			}
			else
			{
				speed = 0;
				direction = 0;
				isSprinting = false;
			}

			if (speed > 0 && OnWallStopConditions())
			{
				speed = 0;
			}
		}

		public virtual void FreeMovement()
		{
			if (GetMoveAmount() == 1 &&
				TPCInput.GetButton(INC.SPRINT) &&
				IsSprintDirection() &&
				CrouchSprintConditions())
			{
				speed = Mathf.SmoothStep(speed, 2, 8.5f * Time.deltaTime);
				isSprinting = true;
			}
			else
			{
				speed = GetMoveAmount();
				isSprinting = false;
			}

			if (speed > 0 && OnWallStopConditions())
			{
				speed = 0;
			}

			if (speed > 0 && targetDirection.magnitude > 0.1f)
			{
				Vector3 lookDirection = targetDirection.normalized;
				freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
				float diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
				float eulerY = transform.eulerAngles.y;

				// Apply free directional rotation while not turning180 animations
				if (isGrounded || (!isGrounded && airControl))
				{
					if (diferenceRotation < 0 || diferenceRotation > 0)
					{
						eulerY = freeRotation.eulerAngles.y;
					}
					Vector3 euler = new Vector3(transform.eulerAngles.x, eulerY, transform.eulerAngles.z);
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), freeRotationSpeed * Time.deltaTime);
				}
			}
		}

		public virtual void ControlSpeed(float velocity)
		{
			this.velocity = velocity;
			if (rootMotion)
			{
				Vector3 v = (animator.deltaPosition * (velocity > 0 ? velocity : 1f)) / Time.deltaTime;
				v.y = characterRigidbody.velocity.y;
				characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, v, 20f * Time.deltaTime);
			}
			else
			{
				switch (locomotionType)
				{
					case LocomotionType.Free:
						Vector3 forwardVelocity = transform.forward * (velocity * Mathf.Clamp01(speed));
						forwardVelocity.y = characterRigidbody.velocity.y;
						forwardVelocity = Vector3.ClampMagnitude(forwardVelocity, 1.0f) * (velocity > 0.0f ? velocity : 1.0f);
						characterRigidbody.velocity = forwardVelocity;
						characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, forwardVelocity, 20.0f * Time.deltaTime);
						break;
					case LocomotionType.Strafe:
						Vector3 moveVector = transform.TransformDirection(new Vector3(direction, 0.0f, speed));
						moveVector = Vector3.ClampMagnitude(moveVector, 1.0f) * (velocity > 0.0f ? velocity : 1.0f);
						moveVector.y = characterRigidbody.velocity.y;
						characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, moveVector, 20.0f * Time.deltaTime);
						break;
				}
			}
		}

		public bool OnWallStopConditions()
		{
			if (!onWallStopProperties.CheckConditions())
			{
				return false;
			}

			RaycastHit raycastHit;
			float verticalSize = capsuleCollider.bounds.size.y;
			Vector3 centerDir = new Vector3(transform.position.x, verticalSize / 2, transform.position.z);
			Vector3 headDir = new Vector3(transform.position.x, verticalSize, transform.position.z);
			if (Physics.Raycast(centerDir, transform.forward, out raycastHit, onWallStopProperties.GetCheckRange(), onWallStopProperties.GetWallLayer(), QueryTriggerInteraction.Ignore) ||
				Physics.Raycast(headDir, transform.forward, out raycastHit, onWallStopProperties.GetCheckRange(), onWallStopProperties.GetWallLayer(), QueryTriggerInteraction.Ignore))
			{
				Vector3 incomingVec = raycastHit.point - centerDir;
				Vector3 reflectVec = Vector3.Reflect(incomingVec, raycastHit.normal);

				float angle = Mathf.Abs(Vector3.Angle(incomingVec, new Vector3(reflectVec.x, 0, reflectVec.z)) - 180);
				return onWallStopProperties.CheckAngle(angle);
			}
			return false;
		}

		public bool IsSprintDirection()
		{
			if (LocomotionTypeIs(LocomotionType.Strafe))
			{
				if (sprintDirection == SprintDirection.Free)
				{
					return true;
				}
				else if (sprintDirection == SprintDirection.Forward && speed > 0 && direction == 0)
				{
					return true;
				}
				else if (sprintDirection == SprintDirection.ForwardWithSide && !(speed < 0))
				{
					return true;
				}
				return false;
			}
			return true;
		}

		public void AirUpdate()
		{
			if (!isJumping)
			{
				return;
			}

			jumpCounter -= Time.deltaTime;
			if (jumpCounter <= 0)
			{
				jumpCounter = 0;
				isJumping = false;
			}
			// apply extra force to the jump height   
			Vector3 vel = characterRigidbody.velocity;
			vel.y = jumpHeight;
			characterRigidbody.velocity = vel;
		}

		public virtual void AirControlProcessing()
		{
			if (isGrounded || !GetJumpFwdCondition())
			{
				return;
			}

			float speedClamped = Mathf.Clamp(speed, -1, 1);
			Vector3 velY = transform.forward * airValue * speedClamped;
			velY.y = characterRigidbody.velocity.y;
			Vector3 velX = transform.right * airValue * Mathf.Clamp(direction, -1, 1);
			velX.x = characterRigidbody.velocity.x;

			if (airControl)
			{
				if (LocomotionTypeIs(LocomotionType.Strafe))
				{
					characterRigidbody.velocity = new Vector3(velX.x, velY.y, characterRigidbody.velocity.z);
					Vector3 vel = transform.forward * (airValue * speedClamped) + transform.right * (airValue * Mathf.Clamp(direction, -1, 1));
					characterRigidbody.velocity = new Vector3(vel.x, characterRigidbody.velocity.y, vel.z);
				}
				else
				{
					Vector3 vel = transform.forward * (airValue * speedClamped);
					characterRigidbody.velocity = new Vector3(vel.x, characterRigidbody.velocity.y, vel.z);
				}
			}
			else
			{
				Vector3 direction = transform.forward;
				if (vertical != 0)
				{
					direction = vertical > 0 ? transform.forward : -transform.forward;
				}
				if (horizontal != 0)
				{
					direction += horizontal > 0 ? transform.right : -transform.right;
				}

				Vector3 vel = direction * (airValue);
				characterRigidbody.velocity = new Vector3(vel.x, characterRigidbody.velocity.y, vel.z);
			}
		}

		public virtual void Jump()
		{
			// return if jumpCondigions is false
			if (!(isGrounded && !isJumping))
			{
				return;
			}

			// trigger jump behaviour
			jumpCounter = jumpTimer;
			isJumping = true;

			// trigger jump animations            
			if (characterRigidbody.velocity.magnitude < 1)
				animator.CrossFadeInFixedTime(JumpStateHash, 0.1f);
			else
				animator.CrossFadeInFixedTime(JumpInMoveStateHash, 0.2f);
		}

		public bool GetJumpFwdCondition()
		{
			Vector3 p1 = transform.position + capsuleCollider.center + Vector3.up * -capsuleCollider.height * 0.5F;
			Vector3 p2 = p1 + Vector3.up * capsuleCollider.height;
			return Physics.CapsuleCastAll(p1, p2, capsuleCollider.radius * 0.5f, transform.forward, 0.6f, groundLayer).Length == 0;
		}

		protected virtual void CrouchProcessing()
		{
			float capsuleHeight = colliderHeight;
			float capsuleVelociy = wasColliderCenter;

			if (TPCInput.GetButtonDown(INC.CROUCH) && (!isCrouching || (isCrouching && !Physics.Raycast(GetPlayerCenter(), Vector3.up, colliderHeight))))
			{
				isCrouching = !isCrouching;
			}

			if (isCrouching)
			{
				capsuleHeight = colliderHeight * crouchAmplitude;
				capsuleVelociy = wasColliderCenter * crouchAmplitude;
			}

			float lastCapsuleHeight = capsuleCollider.height;
			capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, capsuleHeight, crouchRate * Time.deltaTime);
			float fixedVelocity = Mathf.Lerp(capsuleCollider.center.y, capsuleVelociy, crouchRate * Time.deltaTime);
			capsuleCollider.center = new Vector3(0, fixedVelocity, 0);
			float fixedVerticalPosition = Mathf.Lerp(transform.position.y, transform.position.y + (capsuleCollider.height - lastCapsuleHeight) / 2.0f, crouchRate * Time.deltaTime);
			transform.position = new Vector3(transform.position.x, fixedVerticalPosition, transform.position.z);
		}

		protected void CheckGround()
		{
			CheckGroundDistance();

			// change the physics material to very slip when not grounded or maxFriction when is
			if (isGrounded && GetMoveAmount() == 0)
				capsuleCollider.material = maxFrictionPhysics;
			else if (isGrounded && GetMoveAmount() != 0)
				capsuleCollider.material = frictionPhysics;
			else
				capsuleCollider.material = slippyPhysics;

			float magVel = (float)System.Math.Round(new Vector3(characterRigidbody.velocity.x, 0, characterRigidbody.velocity.z).magnitude, 2);
			magVel = Mathf.Clamp(magVel, 0, 1);

			float groundCheckDistance = groundMinDistance;
			if (magVel > 0.25f)groundCheckDistance = groundMaxDistance;

			// clear the checkground to free the character to attack on air                
			bool onStep = StepOffset();

			if (groundDistance <= 0.05f)
			{
				isGrounded = true;
				Sliding();
			}
			else
			{
				if (groundDistance >= groundCheckDistance)
				{
					isGrounded = false;
					// check vertical velocity
					verticalVelocity = characterRigidbody.velocity.y;
					// apply extra gravity when falling
					if (!onStep && !isJumping)
						characterRigidbody.AddForce(transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
				}
				else if (!onStep && !isJumping)
				{
					characterRigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
				}
			}
		}

		protected void CheckGroundDistance()
		{
			if (capsuleCollider != null)
			{
				// radius of the SphereCast
				float radius = capsuleCollider.radius * 0.9f;
				float dist = 10f;
				// position of the SphereCast origin starting at the base of the capsule
				Vector3 pos = transform.position + Vector3.up * (capsuleCollider.radius);
				// ray for RayCast
				Ray ray1 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
				// ray for SphereCast
				Ray ray2 = new Ray(pos, -Vector3.up);
				// raycast for check the ground distance
				if (Physics.Raycast(ray1, out groundHit, colliderHeight / 2 + 2f, groundLayer))
				{
					dist = transform.position.y - groundHit.point.y;
				}
				// sphere cast around the base of the capsule to check the ground distance
				if (Physics.SphereCast(ray2, radius, out groundHit, capsuleCollider.radius + 2f, groundLayer))
				{
					// check if sphereCast distance is small than the ray cast distance
					if (dist > (groundHit.distance - capsuleCollider.radius * 0.1f))
					{
						dist = (groundHit.distance - capsuleCollider.radius * 0.1f);
					}
				}
				groundDistance = (float)System.Math.Round(dist, 2);
			}
		}

		protected float GroundAngle()
		{
			return Vector3.Angle(groundHit.normal, Vector3.up);
		}

		protected virtual void Sliding()
		{
			float groundAngleTwo = 0f;
			RaycastHit hitinfo;
			Ray ray = new Ray(transform.position, -transform.up);

			if (Physics.Raycast(ray, out hitinfo, 1f, groundLayer))
			{
				groundAngleTwo = Vector3.Angle(Vector3.up, hitinfo.normal);
			}

			if (GroundAngle() > slopeLimit + 1f && GroundAngle() <= 85 &&
				groundAngleTwo > slopeLimit + 1f && groundAngleTwo <= 85 &&
				groundDistance <= 0.05f && !StepOffset())
			{
				isSliding = true;
				isGrounded = false;
				float slideVelocity = (GroundAngle() - slopeLimit) * slidingSpeed;
				slideVelocity = Mathf.Clamp(slideVelocity, 0, 10);
				characterRigidbody.velocity = new Vector3(characterRigidbody.velocity.x, -slideVelocity, characterRigidbody.velocity.z);
			}
			else
			{
				isSliding = false;
				isGrounded = true;
			}
		}

		public virtual bool StepOffset()
		{
			if (Mathf.Sqrt(GetMoveAmount()) < 0.1 || !isGrounded)
			{
				return false;
			}

			RaycastHit _hit = new RaycastHit();
			Vector3 _movementDirection = LocomotionTypeIs(LocomotionType.Strafe) && GetMoveAmount() > 0 ? (transform.right * direction + transform.forward * speed).normalized : transform.forward;
			Ray rayStep = new Ray((transform.position + new Vector3(0, stepOffsetEnd, 0) + _movementDirection * ((capsuleCollider).radius + 0.05f)), Vector3.down);

			if (Physics.Raycast(rayStep, out _hit, stepOffsetEnd - stepOffsetStart, groundLayer) && !_hit.collider.isTrigger)
			{
				if (_hit.point.y >= (transform.position.y) && _hit.point.y <= (transform.position.y + stepOffsetEnd))
				{
					float _speed = LocomotionTypeIs(LocomotionType.Strafe) ? Mathf.Clamp(GetMoveAmount(), 0, 1) : Mathf.Clamp(speed, -1, 1);
					Vector3 velocityDirection = LocomotionTypeIs(LocomotionType.Strafe) ? (_hit.point - transform.position) : (_hit.point - transform.position).normalized;
					characterRigidbody.velocity = velocityDirection * stepSmooth * (_speed * (velocity > 1 ? velocity : 1));
					return true;
				}
			}
			return false;
		}

		public virtual void RotateToTarget(Transform target)
		{
			Quaternion rot = Quaternion.LookRotation(target.position - transform.position);
			Vector3 newPos = new Vector3(transform.eulerAngles.x, rot.eulerAngles.y, transform.eulerAngles.z);
			targetRotation = Quaternion.Euler(newPos);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strafeRotationSpeed * Time.deltaTime);
		}

		/// <summary>
		/// Update the targetDirection variable using referenceTransform or just input.Rotate by word  the referenceDirection
		/// </summary>
		/// <param name="referenceTransform"></param>
		public virtual void UpdateTargetDirection(Transform referenceTransform = null)
		{
			if (referenceTransform)
			{
				Vector3 forward = keepDirection ? referenceTransform.forward : referenceTransform.TransformDirection(Vector3.forward);
				forward.y = 0;

				forward = keepDirection ? forward : referenceTransform.TransformDirection(Vector3.forward);
				forward.y = 0; //set to 0 because of referenceTransform rotation on the X axis

				//get the right-facing direction of the referenceTransform
				Vector3 right = keepDirection ? referenceTransform.right : referenceTransform.TransformDirection(Vector3.right);

				// determine the direction the player will face based on input and the referenceTransform's right and forward directions
				targetDirection = horizontal * right + vertical * forward;
			}
			else
			{
				targetDirection = keepDirection ? targetDirection : new Vector3(horizontal, 0, vertical);
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="referenceTransform"></param>
		public virtual void RotateWithAnotherTransform(Transform referenceTransform)
		{
			Vector3 newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
			targetRotation = transform.rotation;
		}

		private void CreatePhysicMaterial()
		{
			// Slides the character through walls and edges
			frictionPhysics = new PhysicMaterial();
			frictionPhysics.name = "FrictionPhysics";
			frictionPhysics.staticFriction = .25f;
			frictionPhysics.dynamicFriction = .25f;
			frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

			// Prevents the collider from slipping on ramps
			maxFrictionPhysics = new PhysicMaterial();
			maxFrictionPhysics.name = "MaxFrictionPhysics";
			maxFrictionPhysics.staticFriction = 1f;
			maxFrictionPhysics.dynamicFriction = 1f;
			maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

			// Air physics 
			slippyPhysics = new PhysicMaterial();
			slippyPhysics.name = "SlippyPhysics";
			slippyPhysics.staticFriction = 0f;
			slippyPhysics.dynamicFriction = 0f;
			slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;
		}

		public float GetMoveAmount()
		{
			return Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));
		}

		public int GetRawMoveAmount()
		{
			float amount = GetMoveAmount();
			if (amount > 0)
			{
				return 1;
			}
			return 0;
		}

		public Vector3 GetPlayerCenter()
		{
			return capsuleCollider.bounds.center;
		}

		public Transform GetCharacterTransform()
		{
			return transform;
		}

		public float GetSpeed()
		{
			return speed;
		}

		public float GetDirection()
		{
			return direction;
		}

		public float GetVerticalVelocity()
		{
			return verticalVelocity;
		}

		public float GetGroundDistance()
		{
			return groundDistance;
		}

		public bool IsGrounded()
		{
			return isGrounded;
		}

		public bool IsCrouching()
		{
			return isCrouching;
		}

		public bool IsJumping()
		{
			return isJumping;
		}

		public bool IsSliding()
		{
			return isSliding;
		}

		public bool IsSprinting()
		{
			return isSprinting;
		}

		public bool KeepDirection()
		{
			return keepDirection;
		}

		public void KeepDirection(bool value)
		{
			keepDirection = value;
		}

		public LocomotionType GetLocomotionType()
		{
			return locomotionType;
		}

		public void SetLocomotionType(LocomotionType value)
		{
			locomotionType = value;
		}

		public bool LocomotionTypeIs(LocomotionType locomotionType)
		{
			return this.locomotionType == locomotionType;
		}

		public SprintDirection GetSprintDirection()
		{
			return sprintDirection;
		}

		public void SetSprintDirection(SprintDirection value)
		{
			sprintDirection = value;
		}

		public Speed GetFreeSpeed()
		{
			return freeSpeed;
		}

		public void SetFreeSpeed(Speed value)
		{
			freeSpeed = value;
		}

		public Speed GetFreeCrouchSpeed()
		{
			return freeCrouchSpeed;
		}

		public void SetFreeCrouchSpeed(Speed value)
		{
			freeCrouchSpeed = value;
		}

		public Speed GetStrafeSpeed()
		{
			return strafeSpeed;
		}

		public void SetStrafeSpeed(Speed value)
		{
			strafeSpeed = value;
		}

		public Speed GetStrafeCrouchSpeed()
		{
			return strafeCrouchSpeed;
		}

		public void SetStrafeCrouchSpeed(Speed value)
		{
			strafeCrouchSpeed = value;
		}

		public float GetFreeRotationSpeed()
		{
			return freeRotationSpeed;
		}

		public void SetFreeRotationSpeed(float value)
		{
			freeRotationSpeed = value;
		}

		public float GetStrafeRotationSpeed()
		{
			return strafeRotationSpeed;
		}

		public void SetStrafeRotationSpeed(float value)
		{
			strafeRotationSpeed = value;
		}

		public bool RootMotion()
		{
			return rootMotion;
		}

		public void RootMotion(bool value)
		{
			rootMotion = value;
		}

		public float GetCrouchAmplitude()
		{
			return crouchAmplitude;
		}

		public void SetCrouchAmplitude(float value)
		{
			crouchAmplitude = value;
		}

		public float GetCrouchRate()
		{
			return crouchRate;
		}

		public void SetCrouchRate(float value)
		{
			crouchRate = value;
		}

		public bool CrouchSprint()
		{
			return crouchSprint;
		}

		public void CrouchSprint(bool value)
		{
			crouchSprint = value;
		}

		public bool CrouchSprintConditions()
		{
			return !isCrouching || isCrouching && crouchSprint;
		}

		public OnWallStopProperties GetOnWallStopProperties()
		{
			return onWallStopProperties;
		}

		public void SetOnWallStopProperties(OnWallStopProperties value)
		{
			onWallStopProperties = value;
		}

		public bool AirControl()
		{
			return airControl;
		}

		public void AirControl(bool value)
		{
			airControl = value;
		}

		public bool AirControllConditions()
		{
			return !(!airControl && !isGrounded);
		}

		public float GetJumpTimer()
		{
			return jumpTimer;
		}

		public void SetJumpTimer(float value)
		{
			jumpTimer = value;
		}

		public float GetJumpForwardImpulse()
		{
			return airValue;
		}

		public void SetJumpForwardImpulse(float value)
		{
			airValue = value;
		}

		public float GetJumpHeight()
		{
			return jumpHeight;
		}

		public void SetJumpHeight(float value)
		{
			jumpHeight = value;
		}

		public float GetStepOffsetEnd()
		{
			return stepOffsetEnd;
		}

		public void SetStepOffsetEnd(float value)
		{
			stepOffsetEnd = value;
		}

		public float GetStepOffsetStart()
		{
			return stepOffsetStart;
		}

		public void SetStepOffsetStart(float value)
		{
			stepOffsetStart = value;
		}

		public float GetStepSmooth()
		{
			return stepSmooth;
		}

		public void SetStepSmooth(float value)
		{
			stepSmooth = value;
		}

		public float GetSlopeLimit()
		{
			return slopeLimit;
		}

		public void SetSlopeLimit(float value)
		{
			slopeLimit = value;
		}

		public float GetSlidingSpeed()
		{
			return slidingSpeed;
		}

		public void SetSlidingSpeed(float value)
		{
			slidingSpeed = value;
		}

		public float GetExtraGravity()
		{
			return extraGravity;
		}

		public void SetExtraGravity(float value)
		{
			extraGravity = value;
		}

		public LayerMask GetGroundLayer()
		{
			return groundLayer;
		}

		public void SetGroundLayer(LayerMask value)
		{
			groundLayer = value;
		}

		public float GetGroundMinDistance()
		{
			return groundMinDistance;
		}

		public void SetGroundMinDistance(float value)
		{
			groundMinDistance = value;
		}

		public float GetGroundMaxDistance()
		{
			return groundMaxDistance;
		}

		public void SetGroundMaxDistance(float value)
		{
			groundMaxDistance = value;
		}

		public float GetColliderHeight()
		{
			return colliderHeight;
		}

		public void SetColliderHeight(float value)
		{
			colliderHeight = value;
		}

		public Transform GetTransform()
		{
			return transform;
		}

		public Animator GetAnimator()
		{
			return animator;
		}

		public Rigidbody GetRigidbody()
		{
			return characterRigidbody;
		}

		public TPCAnimatorHandler GetAnimatorHandler()
		{
			return animatorHandler;
		}

		public void SetAnimatorHandler(TPCAnimatorHandler value)
		{
			animatorHandler = value;
		}

		public PhysicMaterial GetMaxFrictionPhysics()
		{
			return maxFrictionPhysics;
		}

		public PhysicMaterial GetFrictionPhysics()
		{
			return frictionPhysics;
		}

		public PhysicMaterial GetSlippyPhysics()
		{
			return slippyPhysics;
		}

		public CapsuleCollider GetCapsuleCollider()
		{
			return capsuleCollider;
		}

		public Vector3 GetVelocity()
		{
			return characterRigidbody.velocity;
		}

		public void Enabled(bool enabled)
		{
			this.enabled = enabled;
			characterRigidbody.isKinematic = !enabled;
			if (!enabled)
			{
				characterRigidbody.velocity = Vector3.zero;
				animatorHandler.ResetParameters();
			}
		}

		public bool Enabled()
		{
			return enabled;
		}
	}
}