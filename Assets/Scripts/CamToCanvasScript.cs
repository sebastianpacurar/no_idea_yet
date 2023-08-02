using UnityEngine;


public class CamToCanvasScript : MonoBehaviour{
    [SerializeField] private RenderMode renderMode;
    [SerializeField] private Canvas[] canvasObjects;
    private Camera _mainCam;


    private void Awake()
    {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    // set to screen space camera render mode, and attach the camera component to the worldCamera for target canvas object(s)
    private void Start()
    {
        foreach (var canvas in canvasObjects)
        {
            canvas.renderMode = renderMode;
            canvas.worldCamera = _mainCam;
        }
    }
}