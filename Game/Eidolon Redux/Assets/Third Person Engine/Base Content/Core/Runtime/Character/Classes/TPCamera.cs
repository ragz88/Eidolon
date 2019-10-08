/* ====================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ==================================================================== */

using System.Collections;
using ThirdPersonEngine.Utility;
using UnityEngine;

namespace ThirdPersonEngine.Runtime
{
    public class TPCamera : MonoBehaviour, ICoroutineCallbacks
    {
        [SerializeField] private TPController targetController;
        [SerializeField] private float smoothCameraRotation = 12.0f;
        [SerializeField] private LayerMask cullingLayer = 1 << 0;
        [SerializeField] private float cullingSmooth = 10.0f;
        [SerializeField] private float rightOffset = 0.0f;
        [SerializeField] private float defaultDistance = 2.5f;
        [SerializeField] private float minDistance = 0.7f;
        [SerializeField] private float maxDistance = 3.5f;
        [SerializeField] private float height = 1.4f;
        [SerializeField] private float followSpeed = 20.0f;
        [SerializeField] private float xMouseSensitivity = 3.0f;
        [SerializeField] private float yMouseSensitivity = 3.0f;
        [SerializeField] private float yMinLimit = -40.0f;
        [SerializeField] private float yMaxLimit = 80.0f;
        [SerializeField] private bool useScroll = true;
        [SerializeField] private float scrollSensitivity = 70.0f;

        [SerializeField] private CameraFOVSystem cameraFOVSystem = new CameraFOVSystem();

        private Camera _camera;
        private bool lockCamera = false;
        private int indexList;
        private int indexLookPoint;
        private string currentStateName;
        private Transform controllerTransform;
        private Vector2 movementSpeed;
        private Transform targetLookAt;
        private Vector3 currentTargetPos;
        private Vector3 lookPoint;
        private Vector3 currentPos;
        private Vector3 desiredPos;
        private float distance;
        private float mouseY = 0f;
        private float mouseX = 0f;
        private float currentHeight;
        private float cullingDistance;
        private float checkHeightRadius = 0.4f;
        private float clipPlaneMargin = 0f;
        private float forward = -1f;
        private float xMinLimit = -360f;
        private float xMaxLimit = 360f;
        private float cullingHeight = 0.2f;
        private float cullingMinDist = 0.1f;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            _camera = GetComponent<Camera>();
            targetController = targetController.GetComponent<TPController>();
            cameraFOVSystem.Initialize(_camera, targetController, this);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            controllerTransform = targetController.transform;
            currentTargetPos = new Vector3(controllerTransform.position.x, controllerTransform.position.y, controllerTransform.position.z);

            targetLookAt = new GameObject("TargetLookAt").transform;
            targetLookAt.position = controllerTransform.position;
            targetLookAt.hideFlags = HideFlags.HideInHierarchy;
            targetLookAt.rotation = controllerTransform.rotation;

            mouseY = controllerTransform.eulerAngles.x;
            mouseX = controllerTransform.eulerAngles.y;

            distance = defaultDistance;
            currentHeight = height;
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (targetController == null || targetLookAt == null)
            {
                return;
            }

            CameraMovement();
            CameraHandle();
            ScrollingCameraDistance();

            cameraFOVSystem.Update();
        }

        protected virtual void CameraHandle()
        {
            float vertical = TPCInput.GetAxis(INC.CAM_HORIZONTAL);
            float horizontal = TPCInput.GetAxis(INC.CAM_VERTICAL);

            RotateCamera(vertical, horizontal);

            if (!targetController.KeepDirection())
            {
                targetController.UpdateTargetDirection(_camera.transform);
            }

            if (targetController.GetMoveAmount() > 0 &&
                targetController.LocomotionTypeIs(LocomotionType.Strafe) &&
                targetController.AirControllConditions() &&
                targetController.Enabled())
            {
                targetController.RotateWithAnotherTransform(_camera.transform);
            }
        }

        /// <summary>
        /// Set the target for the camera
        /// </summary>
        public void UpdateTarget(Transform newTarget)
        {
            controllerTransform = newTarget != null ? newTarget : targetController.transform;
        }

        public void SetMainTarget(Transform newTarget)
        {
            controllerTransform = newTarget;
            mouseY = controllerTransform.rotation.eulerAngles.x;
            mouseX = controllerTransform.rotation.eulerAngles.y;
            Start();
        }

        /// <summary>    
        /// Convert a point in the screen in a Ray for the world
        /// </summary>
        public Ray ScreenPointToRay(Vector3 Point)
        {
            return _camera.ScreenPointToRay(Point);
        }

        /// <summary>
        /// Camera Rotation behaviour
        /// </summary>
        public void RotateCamera(float x, float y)
        {
            // free rotation 
            mouseX += x * xMouseSensitivity;
            mouseY -= y * yMouseSensitivity;

            movementSpeed.x = x;
            movementSpeed.y = -y;
            if (!lockCamera)
            {
                mouseY = Extensions.ClampAngle(mouseY, yMinLimit, yMaxLimit);
                mouseX = Extensions.ClampAngle(mouseX, xMinLimit, xMaxLimit);
            }
            else
            {
                mouseY = controllerTransform.root.localEulerAngles.x;
                mouseX = controllerTransform.root.localEulerAngles.y;
            }
        }

        /// <summary>
        /// Camera behaviour
        /// </summary>    
        private void CameraMovement()
        {
            if (controllerTransform == null)
            {
                return;
            }

            distance = Mathf.Lerp(distance, defaultDistance, cullingSmooth * Time.deltaTime);
            //_camera.fieldOfView = fov;
            cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
            Vector3 camDir = (forward * targetLookAt.forward) + (rightOffset * targetLookAt.right);

            camDir = camDir.normalized;

            Vector3 targetPos = new Vector3(controllerTransform.position.x, controllerTransform.position.y, controllerTransform.position.z);
            currentTargetPos = Vector3.Lerp(currentTargetPos, targetPos, Time.deltaTime * followSpeed);
            desiredPos = targetPos + new Vector3(0, height, 0);
            currentPos = currentTargetPos + new Vector3(0, currentHeight, 0);
            RaycastHit hitInfo;

            ClipPlanePoints planePoints = _camera.NearClipPlanePoints(currentPos + (camDir * (distance)), clipPlaneMargin);
            ClipPlanePoints oldPoints = _camera.NearClipPlanePoints(desiredPos + (camDir * distance), clipPlaneMargin);

            //Check if Height is not blocked 
            if (Physics.SphereCast(targetPos, checkHeightRadius, Vector3.up, out hitInfo, cullingHeight + 0.2f, cullingLayer))
            {
                float t = hitInfo.distance - 0.2f;
                t -= height;
                t /= (cullingHeight - height);
                cullingHeight = Mathf.Lerp(height, cullingHeight, Mathf.Clamp(t, 0.0f, 1.0f));
            }

            //Check if desired target position is not blocked       
            if (CullingRayCast(desiredPos, oldPoints, out hitInfo, distance + 0.2f, cullingLayer, Color.blue))
            {
                distance = hitInfo.distance - 0.2f;
                if (distance < defaultDistance)
                {
                    float t = hitInfo.distance;
                    t -= cullingMinDist;
                    t /= cullingMinDist;
                    currentHeight = Mathf.Lerp(cullingHeight, height, Mathf.Clamp(t, 0.0f, 1.0f));
                    currentPos = currentTargetPos + new Vector3(0, currentHeight, 0);
                }
            }
            else
            {
                currentHeight = height;
            }
            //Check if target position with culling height applied is not blocked
            if (CullingRayCast(currentPos, planePoints, out hitInfo, distance, cullingLayer, Color.cyan))
            {
                distance = Mathf.Clamp(cullingDistance, 0.0f, defaultDistance);
            }
            Vector3 lookPoint = currentPos + targetLookAt.forward * 2f;
            lookPoint += (targetLookAt.right * Vector3.Dot(camDir * (distance), targetLookAt.right));
            targetLookAt.position = currentPos;

            Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
            targetLookAt.rotation = Quaternion.Slerp(targetLookAt.rotation, newRot, smoothCameraRotation * Time.deltaTime);
            _camera.transform.position = currentPos + (camDir * (distance));
            Quaternion rotation = Quaternion.LookRotation((lookPoint) - _camera.transform.position);

            //lookTargetOffSet = Vector3.Lerp(lookTargetOffSet, Vector3.zero, 1 * Time.fixedDeltaTime);

            //rotation.eulerAngles += rotationOffSet + lookTargetOffSet;
            _camera.transform.rotation = rotation;
            movementSpeed = Vector2.zero;
        }

        /// <summary>
        /// Custom Raycast using NearClipPlanesPoints
        /// </summary>
        protected bool CullingRayCast(Vector3 from, ClipPlanePoints _to, out RaycastHit hitInfo, float distance, LayerMask cullingLayer, Color color)
        {
            bool value = false;

            if (Physics.Raycast(from, _to.LowerLeft - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(from, _to.LowerRight - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(from, _to.UpperLeft - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(from, _to.UpperRight - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore))
            {
                value = true;
                cullingDistance = hitInfo.distance;
            }
            return value;
        }

        /// <summary>
        /// Change distance between camera and character by scrolling mouse wheel
        /// </summary>
        protected virtual void ScrollingCameraDistance()
        {
            if (!useScroll)
                return;

            float mouseWheelAxis = TPCInput.GetAxis(INC.MOUSE_WHEEL) * scrollSensitivity;
            float scrolledDistance = defaultDistance - (mouseWheelAxis * Time.deltaTime);
            defaultDistance = Mathf.Clamp(scrolledDistance, minDistance, maxDistance);
        }

        public void Start(IEnumerator method)
        {
            StartCoroutine(method);
        }

        public void Stop(IEnumerator method)
        {
            StopCoroutine(method);
        }

        public void StopAll()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Camera direction relative to the target
        /// </summary>
        public float DirectionRelativeController()
        {
            return Vector3.Angle(_camera.transform.forward, targetController.transform.forward);
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public TPController GetTargetController()
        {
            return targetController;
        }

        public void SetTargetController(TPController value)
        {
            targetController = value;
        }

        public TPController GetController()
        {
            return targetController;
        }

        public float GetSmoothCameraRotation()
        {
            return smoothCameraRotation;
        }

        public void SetSmoothCameraRotation(float value)
        {
            smoothCameraRotation = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public float GetCullingSmooth()
        {
            return cullingSmooth;
        }

        public void SetCullingSmooth(float value)
        {
            cullingSmooth = value;
        }

        public float GetRightOffset()
        {
            return rightOffset;
        }

        public void SetRightOffset(float value)
        {
            rightOffset = value;
        }

        public float GetDefaultDistance()
        {
            return defaultDistance;
        }

        public void SetDefaultDistance(float value)
        {
            defaultDistance = value;
        }

        public float GetMinDistance()
        {
            return minDistance;
        }

        public void SetMinDistance(float value)
        {
            minDistance = value;
        }

        public float GetMaxDistance()
        {
            return maxDistance;
        }

        public void SetMaxDistance(float value)
        {
            maxDistance = value;
        }

        public float GetHeight()
        {
            return height;
        }

        public void SetHeight(float value)
        {
            height = value;
        }

        public float GetFollowSpeed()
        {
            return followSpeed;
        }

        public void SetFollowSpeed(float value)
        {
            followSpeed = value;
        }

        public float GetSmoothFollow()
        {
            return cullingSmooth;
        }

        public void SetSmoothFollow(float value)
        {
            cullingSmooth = value;
        }

        public float GetMouseSensitivityX()
        {
            return xMouseSensitivity;
        }

        public void SetMouseSensitivityX(float value)
        {
            xMouseSensitivity = value;
        }

        public float GetMouseSensitivityY()
        {
            return yMouseSensitivity;
        }

        public void SetMouseSensitivityY(float value)
        {
            yMouseSensitivity = value;
        }

        public float GetMinLimitY()
        {
            return yMinLimit;
        }

        public void SetMinLimitY(float value)
        {
            yMinLimit = value;
        }

        public float GetMaxLimitY()
        {
            return yMaxLimit;
        }

        public void SetMaxLimitY(float value)
        {
            yMaxLimit = value;
        }

        public bool UseScroll()
        {
            return useScroll;
        }

        public void UseScroll(bool value)
        {
            useScroll = value;
        }

        public float GetScrollSensitivity()
        {
            return scrollSensitivity;
        }

        public void SetScrollSensitivity(float value)
        {
            scrollSensitivity = value;
        }

        public CameraFOVSystem GetCameraFOVSystem()
        {
            return cameraFOVSystem;
        }

        public void SetCameraFOVSystem(CameraFOVSystem value)
        {
            cameraFOVSystem = value;
        }

        public bool LockCamera()
        {
            return lockCamera;
        }

        public void LockCamera(bool value)
        {
            lockCamera = value;
        }
    }
}