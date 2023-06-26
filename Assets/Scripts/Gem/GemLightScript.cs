using ScriptableObjects.GemLight;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gem {
    public class GemLightScript : MonoBehaviour {
        [SerializeField] private GemLightDataSo data;
        private Light2D _light2D;

        // used by sine to manipulate intensity
        private float _frequency, _amplitude;
        // apply sine wave to intensity property of light2D component
        private float _intensity;

        private void Awake() {
            _light2D = GetComponent<Light2D>();
        }

        private void Update() {
            _light2D.color = data.LightColor;
            _light2D.falloffIntensity = data.FallOffStrength;
            _light2D.pointLightInnerRadius = data.LightRadius.x;
            _light2D.pointLightOuterRadius = data.LightRadius.y;

            _frequency = data.SineFrequencyAmplitude.x;
            _amplitude = data.SineFrequencyAmplitude.y;

            // set intensity to sine value.
            // sine value (Mathf.Sin(Time.time * _frequency) * _amplitude) is applied to _intensity through addition
            _light2D.intensity = Mathf.Sin(Time.time * _frequency) * _amplitude + _intensity;
        }
    }
}