using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get { return instance; } }
    private static CameraController instance;

    public Transform target;
    public Vector3 offset;
    public float speed;
    public float zoomSpeed = 2f;

    private float zoomDist;
    private float closeZoom = 6.25f;
    private float farZoom = 20f;

    private Camera camera;

    public UnityEvent onCameraFinishZoom = new UnityEvent();
    private bool doingZoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        camera = GetComponent<Camera>();
        zoomDist = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.DOMove(target.position + offset, speed);
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
            if (doingZoom && Vector3.Distance(transform.position, target.position + offset) < 5)
            {
                doingZoom = false;
                onCameraFinishZoom.Invoke();
                onCameraFinishZoom.RemoveAllListeners();
            }    
        }
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomDist, zoomSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
        doingZoom = true;
    }

    public void ZoomIn()
    {
        zoomDist = closeZoom;
    }

    public void ZoomOut()
    {
        zoomDist = farZoom;
    }
}
