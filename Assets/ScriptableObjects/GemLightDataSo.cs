using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "GemLightDataSO", menuName = "Data/GemLightData")]
    public class GemLightDataSo : ScriptableObject {
        [Header("Sine Data")]
        [Space(2)]
        [SerializeField] private Vector2 sineFrequencyAmplitude;

        [Space(10)]
        [Space(2)]
        [Header("Light Data")]
        [SerializeField] private float fallOffStrength;
        [SerializeField] private float initialIntensity;
        [SerializeField] private Vector2 lightRadius;
        [SerializeField] private Color lightColor;

        #region getters
        public Vector2 SineFrequencyAmplitude => sineFrequencyAmplitude;
        public float FallOffStrength => fallOffStrength;
        public float InitialIntensity => initialIntensity;
        public Vector2 LightRadius => lightRadius;
        public Color LightColor => lightColor;
        #endregion
    }
}