using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "ChestParticleDataSO", menuName = "Data/ChestParticleData")]
    public class ChestParticleDataSo : ScriptableObject {
        [Header("Main Module")]
        [Space(2)]
        [SerializeField] private Vector2 startLifetime;
        [SerializeField] private Vector2 startSpeed;
        [SerializeField] private Vector2 startSize;

        [Space(10)]
        [Header("Emission Module")]
        [Space(2)]
        [SerializeField] private Vector2 emRateOverTime;
        [Space(10)]
        [Header("Size over Lifetime Module")]
        [Space(2)]
        [SerializeField] private Vector2 sizeOverLifetime;

        [Space(10)]
        [Header("Size by Speed Module")]
        [Space(2)]
        [SerializeField] private Vector2 sizeBySpeed;

        [Space(10)]
        [Header("Color over Lifetime Module")]
        [Space(2)]
        [SerializeField] private Gradient colOverLifetime = new() {
            alphaKeys = new[] {
                new GradientAlphaKey(0, 0f),
                new GradientAlphaKey(1, 1f)
            },
            colorKeys = new[] {
                new GradientColorKey(Color.red, 1f),
                new GradientColorKey(Color.green, 1f),
                new GradientColorKey(Color.blue, 1f)
            },
            mode = GradientMode.Blend
        };


        #region getters
        public Vector2 StartLifetime => startLifetime;
        public Vector2 StartSpeed => startSpeed;
        public Vector2 StartSize => startSize;
        public Vector2 EmRateOverTime => emRateOverTime;
        public Vector2 SizeOverLifetime => sizeOverLifetime;
        public Vector2 SizeBySpeed => sizeBySpeed;
        public Gradient ColOverLifetime => colOverLifetime;
        #endregion
    }
}