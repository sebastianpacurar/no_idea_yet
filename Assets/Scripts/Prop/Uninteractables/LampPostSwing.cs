using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prop.Uninteractables {
    public class LampPostLogic : MonoBehaviour {
        [Header("Light On / Off")]
        [SerializeField] private GameObject lightOnObj;
        [SerializeField] private GameObject lightOffObj;
        [SerializeField] private float minIntensity; // set when min intensity for day-night transition

        [Header("Lamp Swing")]
        [SerializeField] private GameObject swingObj;
        [SerializeField] private float sinFreq;
        [SerializeField] private float sinAmplitude;
        [SerializeField] private float stopSmoothTime;

        private Light2D _globalLight;

        // use as ref for SmoothDampAngle
        private float _rotVelocity = 0f;

        private void Start() {
            _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
        }

        private void Update() {
            HandleSwing();
            HandleLampTurnOnOff();
        }

        private void HandleSwing() {
            // daytime
            if (_globalLight.intensity < minIntensity) {
                // calculate the rotation based on Sin movement
                var rotZ = Mathf.Sin(Time.time * sinFreq) * sinAmplitude + swingObj.transform.localRotation.z;
                swingObj.transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
            }
            // nighttime
            else {
                // stop swinging using smoothness
                var rotZ = Mathf.SmoothDampAngle(swingObj.transform.localRotation.eulerAngles.z, Quaternion.Euler(0f, 0f, 0f).z, ref _rotVelocity, stopSmoothTime);
                swingObj.transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
            }
        }

        private void HandleLampTurnOnOff() {
            // cause lamp to turn off after the swinging is over, and z rotation of swingObj equals to 0
            lightOnObj.SetActive(_globalLight.intensity < minIntensity + 0.05f);
            lightOffObj.SetActive(_globalLight.intensity > minIntensity + 0.05f);
        }
    }
}