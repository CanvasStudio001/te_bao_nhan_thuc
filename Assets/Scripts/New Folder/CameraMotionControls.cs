using Cinemachine;
using UnityEngine;

public enum ZoomMode
{
    CameraFieldOfView,
    ZAxisDistance
}

public class CameraMotionControls : MonoBehaviour
{
    [Header("Automatic Rotation")]
    public bool autoRotate = true;
    public float rotationSpeed = 0.1f;
    public float startRotation = 180;

    [Header("Manual Rotation")]
    public float rotationSmoothing = 2f;
    public Transform target;
    public float rotationSensitivity = 1f;
    public Vector2 rotationLimit = new Vector2(5, 80);
    public float zAxisDistance = 0.45f;

    public ZoomMode zoomMode = ZoomMode.CameraFieldOfView;

    private CinemachineFreeLook camera;
    private float cameraFieldOfView;
    private Transform transform;
    public float xVelocity;
    public float yVelocity;
    private float xRotationAxis = 0f;
    private float yRotationAxis = 0f;
    private float zoomVelocity;
    private float zoomVelocityZAxis;

    public GameObject tempTarget;
    public Transform TransformRoot;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public bool canRotate;
    private bool isSwitchingCamera;
    public float SpeedAutoRotate = 3;
    public bool isOnPanel;
    public UIMain uiMain;
    private void Awake()
    {
        camera = GetComponent<CinemachineFreeLook>();
        transform = GetComponent<Transform>();
        InitTarget();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }
    public void SetUIMain(UIMain uIMain)
    {
        uiMain = uIMain;
    }

    private void Start()
    {
        cameraFieldOfView = camera.m_Lens.FieldOfView;
        // camera.LookAt = null;


    }

    void InitTarget()
    {
        xRotationAxis = startRotation / rotationSpeed;
        zAxisDistance = Vector3.Distance(transform.position, target.position);
        if (target == null || TransformRoot == null)
        {
            Debug.LogError("Target or TransformRoot not set.");
            return;
        }

        tempTarget = Instantiate(target.gameObject, TransformRoot);
        tempTarget.transform.position = target.position;
        tempTarget.transform.rotation = target.rotation;
        tempTarget.transform.localScale = target.localScale;

        target = tempTarget.transform;
        this.GetComponent<CameraControl>().target = tempTarget.transform;
    }

    public void SwitchTarget(Transform cinemachine, Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogError("New target is null.");
            return;
        }

        target.position = newTarget.position;
        target.rotation = newTarget.rotation;
        target.localScale = newTarget.localScale;

        zAxisDistance = Vector3.Distance(cinemachine.position, newTarget.position);
        GetComponent<CameraControl>().SwitchTarget(newTarget);

        UpdateFreeLookCamera();
    }

    private void UpdateFreeLookCamera()
    {
        if (camera != null && target != null)
        {
            // Ensure the camera's position is correctly set
            Vector3 desiredPosition = target.position - camera.transform.forward * zAxisDistance;
            camera.transform.position = Vector3.Lerp(camera.transform.position, desiredPosition, Time.deltaTime * rotationSmoothing);

            // Keep the camera looking at the target
            camera.transform.LookAt(target);
        }
    }
    public bool isRotating;

    private void LateUpdate()
    {

        //if(isOnPanel && !GetComponent<CameraControl>().isZooming && !GetComponent<CameraControl>().isPanning && !isRotating)
        //{
        //return;
        //}



        if (isSwitchingCamera) return;

        // Tự động xoay khi autoRotate bật
        if (autoRotate)
        {
            xVelocity += SpeedAutoRotate * Time.deltaTime;
        }

        if (target && canRotate)
        {
            if (!uiMain.IsClickSwipe)
            {
                if (Input.touchCount == 1) // Xử lý xoay bằng một ngón tay
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        xVelocity += touch.deltaPosition.x * rotationSensitivity * 0.08f;
                        yVelocity -= touch.deltaPosition.y * rotationSensitivity * 0.01f;
                        isRotating = true;
                    }
                }

                else if (Input.GetMouseButton(0) && Input.touchCount <= 0) // Xử lý xoay bằng chuột
                {
                    xVelocity += Input.GetAxis("Mouse X") * rotationSensitivity;
                    yVelocity -= Input.GetAxis("Mouse Y") * rotationSensitivity * 0.2f;
                    isRotating = true;
                }
                else
                {
                    isRotating = false;
                }
            }


            Rotate();
        }
    }



    private void Rotate()
    {
        //if (!canRotate) return;

        xRotationAxis += xVelocity;
        yRotationAxis += yVelocity;

        yRotationAxis = ClampAngleBetweenMinAndMax(yRotationAxis, rotationLimit.x, rotationLimit.y);

        Quaternion rotation = Quaternion.Euler(yRotationAxis, xRotationAxis * rotationSpeed, 0);
        Vector3 position = rotation * new Vector3(0f, 0f, -zAxisDistance) + target.position;

        transform.rotation = rotation;
        transform.position = position;

        xVelocity = Mathf.Lerp(xVelocity, 0, Time.deltaTime * rotationSmoothing);
        yVelocity = Mathf.Lerp(yVelocity, 0, Time.deltaTime * rotationSmoothing);
    }

    public void SetSwitchingState(bool isSwitching)
    {
        isSwitchingCamera = isSwitching;

        if (isSwitching)
        {
            canRotate = false;
            xVelocity = 0;
            yVelocity = 0;

        }
        else
        {
            canRotate = true;
        }
    }

    public void ResetRotationToTarget(Quaternion targetRotation)
    {
        Vector3 eulerAngles = targetRotation.eulerAngles;
        xRotationAxis = eulerAngles.y / rotationSpeed;
        yRotationAxis = eulerAngles.x;
        transform.rotation = Quaternion.Euler(yRotationAxis, xRotationAxis * rotationSpeed, 0);
    }

    private float ClampAngleBetweenMinAndMax(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}