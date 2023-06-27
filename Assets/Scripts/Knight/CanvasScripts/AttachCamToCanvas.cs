using UnityEngine;

namespace Knight.CanvasScripts {
    public class AttachCamToCanvas : MonoBehaviour {
        [SerializeField] private Canvas imgCanvas;
        [SerializeField] private Canvas statsCanvas;

        private Camera _mainCam;

        private void Awake() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        // set to screen space camera render mode, and attach the camera component to the worldCamera, for both canvases
        private void Start() {
            imgCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            imgCanvas.worldCamera = _mainCam;

            statsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            statsCanvas.worldCamera = _mainCam;
        }
    }
}