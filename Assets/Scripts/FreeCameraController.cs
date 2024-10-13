using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeCameraController : MonoBehaviour
{
    public Camera MainCamera;
    public Camera UICamera;
    public CinemachineBrain brain;
    public CinemachineFreeLook freeLookCamera;
    public CinemachineVirtualCamera defaultCamera;
    [SerializeField]
    private List<CinemachineVirtualCamera> cinemachines;
    [SerializeField]
    private List<Button> Buttons;
    [SerializeField]
    private List<Transform> Points;
    [SerializeField]
    private List<CinemachineVirtualCamera> cinemachinesDV;
    [SerializeField]
    private List<Button> ButtonsDV;
    [SerializeField]
    private List<Transform> PointsDV;
    [SerializeField]
    private GameObject objGroupBtnDV;
    [SerializeField]
    private GameObject objGroupBtnTV;
    public List<(Button, CinemachineVirtualCamera, Transform)> buttonCameraList;
    private bool isSwitchingCamera = false;
    public float transitionDuration = 1.0f;

    public Transform PointDefault;
    [HideInInspector]
    public List<Button> lsButtons = new List<Button>();
    [HideInInspector]
    public List<Transform> lsPoints = new List<Transform>();
    [HideInInspector]
    public List<CinemachineVirtualCamera> lsCinemachines = new List<CinemachineVirtualCamera>();
    private void Awake()
    {
        buttonCameraList = new List<(Button, CinemachineVirtualCamera, Transform)>();

        GetListDataTV();
    }
    public void GetListDataTV()
    {
        lsButtons.Clear();
        lsButtons.AddRange(Buttons);
        lsPoints.Clear();
        lsPoints.AddRange(Points);
        lsCinemachines.Clear();
        lsCinemachines.AddRange(cinemachines);
        objGroupBtnDV.SetActive(false);
        objGroupBtnTV.SetActive(true);
        lsButtons[0].gameObject.SetActive(false);
        for (int i = 0; i < lsButtons.Count; i++)
        {
            if (i > 0)
            {
                lsButtons[i].gameObject.SetActive(false);
            }
            int index = i;
            lsButtons[i].onClick.RemoveAllListeners();
            lsButtons[i].onClick.AddListener(() => { ChangeCamera(lsButtons[index]); });
            lsCinemachines[i].LookAt = lsPoints[i];
            lsButtons[i].GetComponent<Billboard>().cam = MainCamera.transform;
            if (i > 0)
            {
                lsButtons[i].gameObject.SetActive(true);
            }
        }
        if (lsButtons.Count == lsCinemachines.Count)
        {
            for (int i = 0; i < lsButtons.Count; i++)
            {
                buttonCameraList.Add((lsButtons[i], lsCinemachines[i], lsPoints[i]));
            }
        }
    }
    public void GetListDataDV()
    {
        lsButtons.Clear();
        lsButtons.AddRange(ButtonsDV);
        lsPoints.Clear();
        lsPoints.AddRange(PointsDV);
        lsCinemachines.Clear();
        lsCinemachines.AddRange(cinemachinesDV);
        objGroupBtnDV.SetActive(true);
        objGroupBtnTV.SetActive(false);
        lsButtons[0].gameObject.SetActive(false);
        for (int i = 0; i < lsButtons.Count; i++)
        {
            lsButtons[i].GetComponent<HorizontalLayoutGroup>().enabled = false;
            if (i > 0)
            {
                lsButtons[i].gameObject.SetActive(false);
            }
            int index = i;
            lsButtons[i].onClick.RemoveAllListeners();
            lsButtons[i].onClick.AddListener(() => { ChangeCamera(lsButtons[index]); });
            lsCinemachines[i].LookAt = lsPoints[i];
            lsButtons[i].GetComponent<Billboard>().cam = MainCamera.transform;
            if (i > 0)
            {
                lsButtons[i].gameObject.SetActive(true);
            }
        }
        if (lsButtons.Count == lsCinemachines.Count)
        {
            for (int i = 0; i < lsButtons.Count; i++)
            {
                buttonCameraList.Add((lsButtons[i], lsCinemachines[i], lsPoints[i]));
            }
        }
        StartCoroutine(DelayActiveHorizontalGroup());
    }
    IEnumerator DelayActiveHorizontalGroup()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < lsButtons.Count; i++)
        {
            lsButtons[i].GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
    }
    void Start()
    {

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        //lsButtons[0].gameObject.SetActive(false);
        //for (int i = 0; i < lsButtons.Count; i++)
        //{
        //    int index = i;
        //    lsButtons[i].onClick.AddListener(() => { ChangeCamera(lsButtons[index]); });
        //    lsCinemachines[i].LookAt = lsPoints[i];
        //    lsButtons[i].GetComponent<Billboard>().cam = MainCamera.transform;
        //}
        //for (int i = 0; i < ButtonsDV.Count; i++)
        //{
        //    int index = i;
        //    ButtonsDV[i].onClick.AddListener(() => { ChangeCamera(ButtonsDV[index]); });
        //    cinemachinesDV[i].LookAt = PointsDV[i];
        //    ButtonsDV[i].GetComponent<Billboard>().cam = MainCamera.transform;
        //}

        //if (lsButtons.Count == lsCinemachines.Count)
        //{
        //    for (int i = 0; i < lsButtons.Count; i++)
        //    {
        //        buttonCameraList.Add((lsButtons[i], lsCinemachines[i], lsPoints[i]));
        //    }
        //}
        Debug.Log(" brain.m_DefaultBlend.m_Time" + brain.m_DefaultBlend.m_Time);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetPosition();
        }
        UICamera.fieldOfView = MainCamera.fieldOfView;
        if (freeLookCamera.GetComponent<CameraMotionControls>().canRotate)
        {
            freeLookCamera.Priority = 20;
            foreach (var item in lsCinemachines)
            {
                item.Priority = 5;
            }
        }

        // freeLookCamera.m_Lens.FieldOfView = MainCamera.fieldOfView;
    }

    public void ResetPosition()
    {
        defaultCamera.Priority = 20;
        foreach (var item in lsCinemachines)
        {
            item.Priority = 5;
        }
        Debug.Log("reset point");
        StartCoroutine(SmoothTransition(defaultCamera.transform, PointDefault));

    }

    public void ChangeCamera(Button button)
    {
        if (lsCinemachines.Count > 0 && !isSwitchingCamera)
        {
            foreach (var item in buttonCameraList)
            {
                if (item.Item1 == button)
                {
                    Debug.Log($"item.Item2: {item.Item2.name} item.Item3: {item.Item3}");
                    item.Item2.Priority = 20;
                    freeLookCamera.Priority = 5;

                    StartCoroutine(SmoothTransition(item.Item2.transform, item.Item3));
                }
                else
                {
                    item.Item2.Priority = 5;
                }
            }
            // StartCoroutine(SwitchBackToFreeLook(transitionDuration));
        }
    }
    public bool isCoroutineRunning = false;
    private IEnumerator SmoothTransition(Transform targetTransform, Transform point)
    {
        Debug.Log("SmoothTransition: " + targetTransform.gameObject.name + " point: " + point.gameObject.name);
        var cameraMotionControls = freeLookCamera.GetComponent<CameraMotionControls>();
        cameraMotionControls.SetSwitchingState(true);
        Vector3 targetPosition = targetTransform.position;
        Quaternion targetRotation = targetTransform.rotation;
        float targetFOV = targetTransform.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;

        float elapsedTime = 0f;


        freeLookCamera.m_Orbits[0].m_Radius = cameraMotionControls.zAxisDistance;
        freeLookCamera.m_Orbits[1].m_Radius = cameraMotionControls.zAxisDistance;
        freeLookCamera.m_Orbits[2].m_Radius = cameraMotionControls.zAxisDistance;

        while (elapsedTime < brain.m_DefaultBlend.m_Time)
        {

            isCoroutineRunning = true;
            //freeLookCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / transitionDuration);
            //freeLookCamera.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / transitionDuration);
            //freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        freeLookCamera.GetComponent<CameraMotionControls>().SwitchTarget(targetTransform, point);
        freeLookCamera.transform.rotation = targetRotation;
        freeLookCamera.m_Lens.FieldOfView = targetFOV;
        freeLookCamera.GetComponent<CameraControl>().ResetTargetFOV(targetFOV);
        cameraMotionControls.ResetRotationToTarget(targetRotation);
        cameraMotionControls.SetSwitchingState(false);
        freeLookCamera.Priority = 20;
        defaultCamera.Priority = 5;
        freeLookCamera.LookAt = null;
        freeLookCamera.Follow = null;
        isCoroutineRunning = false;
    }

    private IEnumerator SwitchBackToFreeLook(float duration)
    {
        yield return new WaitForSeconds(duration);
        freeLookCamera.Priority = 20;
        foreach (var item in lsCinemachines)
        {
            item.Priority = 5;
        }
    }
}
