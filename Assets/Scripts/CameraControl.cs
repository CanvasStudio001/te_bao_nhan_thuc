using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target; // The target object to rotate around
    public float rotationSpeed = 5.0f; // Speed of rotation
    public float zoomSpeed = 2.0f; // Speed of zoom
    public float panSpeed = 0.5f; // Speed of panning
    public float smoothTime = 0.1f; // Time for smooth transition
    public float inertiaDamping = 0.5f; // Damping for inertia effect

    public float minFOV = 1f; // Minimum field of view
    public float maxFOV = 60f; // Maximum field of view

    private Vector3 previousMousePosition;
    public CinemachineFreeLook freeLookCamera;

    private float targetFOV;
    private float zoomVelocity; // Velocity for smooth damping
    public bool isSmoothZooming; // Flag to indicate if smooth zooming should be applied
    public CinemachineBrain CameraBrain;

    private Vector3 targetPanPosition;
    private Vector3 panVelocity;
    public UIMain uIMain;

    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        if (freeLookCamera == null)
        {
            Debug.LogError("CinemachineFreeLook component not found on CameraControl.");
            enabled = false; // Disable this script if component is not found
        }

        if (target == null)
        {
            Debug.LogError("Target not set on CameraControl.");
        }

        targetFOV = freeLookCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView; // Initialize target FOV

        // Initialize panning variables
        targetPanPosition = target.position;
    }

    void Update()
    {
        if (target == null || freeLookCamera == null) return;

        HandleZoom();
        HandlePanning();

        previousMousePosition = Input.mousePosition;
        if (CameraBrain.IsBlending)
        {
            isSmoothZooming = false;
        }
    }

    private void HandleZoom()
    {
        if (uIMain == null)
        {
            return;

        }
        if (!uIMain.IsClickSwipe)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                targetFOV -= scrollInput * zoomSpeed;
                targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
                zoomVelocity = scrollInput * zoomSpeed;
                isSmoothZooming = true;
                isZooming = true;
            }


            // Zoom with pinch gesture (smooth zoom with pinch)
            else if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Calculate the previous position of each touch
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Calculate the distance between the touches in the previous and current frames
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Calculate the difference in distances between each frame
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // Invert the direction to match zoom behavior
                targetFOV += deltaMagnitudeDiff * 0.05f; // Zoom in when spreading, zoom out when pinching
                targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
                if (freeLookCamera.m_Lens.FieldOfView < targetFOV && uIMain != null)
                {
                    uIMain.NotTargetPoint();
                }
                isZooming = true;
                // Apply smooth zoom effect for pinch zoom
                isSmoothZooming = true; // Enable smooth zooming for both mouse scroll and pinch zoom
            }
            else
            {
                isZooming = false;
            }

        }


        // Smooth zoom effect (for both mouse scroll and pinch zoom)
        if (isSmoothZooming)
        {
            float currentFOV = freeLookCamera.m_Lens.FieldOfView;
            freeLookCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(currentFOV, targetFOV, ref zoomVelocity, smoothTime);

        }
    }
    public bool isZooming;
    public bool isPanning;
    private void HandlePanning()
    {
        if (!uIMain.IsClickSwipe)
        {
            if ((Input.GetMouseButton(1) || Input.GetMouseButton(2)) && Input.touchCount <= 0)
            {
                Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
                isPanning = true;
                // Convert mouseDelta to world space
                Vector3 right = freeLookCamera.transform.right; // Right direction of the camera
                Vector3 up = freeLookCamera.transform.up; // Up direction of the camera

                // Calculate the pan movement
                Vector3 panMovement = (right * -mouseDelta.x * panSpeed * Time.deltaTime) +
                                      (up * -mouseDelta.y * panSpeed * Time.deltaTime);

                // Update the targetPanPosition with smooth movement
                targetPanPosition += panMovement;

                // Smoothly interpolate the target position
                isSmoothZooming = true;
            }

            //  Handle touch input for panning
            else if (Input.touchCount == 2)
            {
                isPanning = true;
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    Vector2 touch0Delta = touch0.deltaPosition;
                    Vector2 touch1Delta = touch1.deltaPosition;

                    Vector2 averageDelta = (touch0Delta + touch1Delta) / 2;

                    Vector3 right = freeLookCamera.transform.right;
                    Vector3 up = freeLookCamera.transform.up;

                    Vector3 panMovement = (right * -averageDelta.x * panSpeed / 4 * Time.deltaTime) +
                                          (up * -averageDelta.y * panSpeed / 4 * Time.deltaTime);

                    targetPanPosition += panMovement;

                    isSmoothZooming = true;
                }
            }
            else
            {
                isPanning = false;
            }

        }

        if (isSmoothZooming) { target.position = Vector3.SmoothDamp(target.position, targetPanPosition, ref panVelocity, smoothTime); }
    }
    public void SwitchTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        // Update target position and reset panning
        targetPanPosition = newTarget.position;
        //  panVelocity = Vector3.zero;
    }

    public void ResetTargetFOV(float FOV)
    {
        targetFOV = FOV;
    }
}
