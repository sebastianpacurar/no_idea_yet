using UnityEngine;

namespace ScriptableObjects.ChestParticles {
    [CreateAssetMenu(fileName = "ChestParticleDataSO", menuName = "Data/ChestParticleData")]
    public class ChestParticleDataSo : ScriptableObject {
        [Header("Main Module")]
        [Space(2)]
        [SerializeField] private float[] startLifetime = new float[2];
        [SerializeField] private float[] startSpeed = new float[2];
        [SerializeField] private float[] startSize = new float[2];

        [Space(10)]
        [Header("Emission Module")]
        [Space(2)]
        [SerializeField] private float[] emRateOverTime = new float[2];

        [Space(10)]
        [Header("Size over Lifetime Module")]
        [Space(2)]
        [SerializeField] private float[] sizeOverLifetime = new float[2];

        [Space(10)]
        [Header("Size by Speed Module")]
        [Space(2)]
        [SerializeField] private float[] sizeBySpeed = new float[2];

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
        public float[] StartLifetime => startLifetime;
        public float[] StartSpeed => startSpeed;
        public float[] StartSize => startSize;
        public float[] EmRateOverTime => emRateOverTime;
        public float[] SizeOverLifetime => sizeOverLifetime;
        public float[] SizeBySpeed => sizeBySpeed;
        public Gradient ColOverLifetime => colOverLifetime;
        #endregion
    }
}