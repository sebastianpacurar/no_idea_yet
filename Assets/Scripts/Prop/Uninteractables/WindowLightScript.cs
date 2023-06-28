using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prop.Uninteractables {
    public class WindowLightScript : MonoBehaviour {
        [SerializeField] private float lightMinIntensity;
        [SerializeField] private Sprite lightOnSprite;
        [SerializeField] private Sprite lightOffSprite;

        private SpriteRenderer _sr;
        private Light2D _globalLight;
        private Light2D _localLight;
        private Sprite _selectedSprite;

        private void Awake() {
            _sr = GetComponent<SpriteRenderer>();
            _localLight = GetComponent<Light2D>();
        }

        private void Start() {
            _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
            _selectedSprite = lightOnSprite;
        }

        private void Update() {
            ToggleLights();
        }

        private void ToggleLights() {
            var intensity = _globalLight.intensity;

            // if correct light is at the correct day-night cycle then skip
            if (intensity < lightMinIntensity && _selectedSprite.Equals(lightOnSprite)) return;
            if (intensity > lightMinIntensity && _selectedSprite.Equals(lightOffSprite)) return;

            // if globalIntensity < minVal then it's night: set light intensity to 1
            if (intensity < lightMinIntensity) {
                // if globalIntensity < minVal then it's night
                _localLight.intensity = 1f;
                _selectedSprite = lightOnSprite;
            } else if (intensity > lightMinIntensity) {
                // if globalIntensity > minVal then it's night
                _localLight.intensity = 0f;
                _selectedSprite = lightOffSprite;
            }

            _sr.sprite = _selectedSprite;
        }
    }
}