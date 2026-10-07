using Unity.VisualScripting;
using UnityEngine;

public class GeneralCameraControl : MonoBehaviour
{
    private Camera cam;

    [SerializeField] float BaseFOV;
    [SerializeField] float zoomLevel1;
    [SerializeField] float zoomLevel2;

    [SerializeField] float zoomSpeed = 1;

    [Header("Note: put something in here if you want to be able to call events specifically on this camera, read code for doc")]
    [SerializeField] string customEventName;

    private float targFov;

    void Awake()
    {
        TryGetComponent<Camera>(out cam);
        if(cam == null) Destroy(this);
    }

    void Start()
    {
        EventBus.RequestEvent(customEventName + "ResetZoom", true).ping += UnZoom;
        EventBus.RequestEvent(customEventName + "CameraZoom1", true).ping += Zoom1;
        EventBus.RequestEvent(customEventName + "CameraZoom2", true).ping += Zoom2;
    }

    void UnZoom()
    {
        targFov = BaseFOV;
    }
    void Zoom1()
    {
        targFov = zoomLevel1;
    }
    void Zoom2()
    {
        targFov = zoomLevel2;
    }
    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targFov, Time.deltaTime * zoomSpeed);
    }
}
