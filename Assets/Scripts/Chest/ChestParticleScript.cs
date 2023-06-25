using ScriptableObjects.ChestParticles;
using UnityEngine;

namespace Chest {
    public class ChestParticleScript : MonoBehaviour {
        [SerializeField] private ChestParticleDataSo data;
        [SerializeField] private ChestAnimationScript animScript;
        private ParticleSystem _ps;

        // declare all used Particle System modules separately
        private ParticleSystem.MainModule _main;
        private ParticleSystem.EmissionModule _em;
        private ParticleSystem.ColorOverLifetimeModule _colorOverLife;
        private ParticleSystem.SizeOverLifetimeModule _sizeOverLife;
        private ParticleSystem.SizeBySpeedModule _sizeBySpeed;

        // initialize the ParticleSystem component and its needed modules
        private void Awake() {
            _ps = GetComponent<ParticleSystem>();
            _main = _ps.main;
            _em = _ps.emission;
            _colorOverLife = _ps.colorOverLifetime;
            _sizeOverLife = _ps.sizeOverLifetime;
            _sizeBySpeed = _ps.sizeBySpeed;
        }

        // Populate the needed values with the info from data
        private void Start() {
            _main.startLifetime = new ParticleSystem.MinMaxCurve(data.StartLifetime[0], data.StartLifetime[1]);
            _main.startSpeed = new ParticleSystem.MinMaxCurve(data.StartSpeed[0], data.StartSpeed[1]);
            _main.startSize = new ParticleSystem.MinMaxCurve(data.StartSize[0], data.StartSize[1]);

            _em.rateOverTime = new ParticleSystem.MinMaxCurve(data.EmRateOverTime[0], data.EmRateOverTime[1]);

            _colorOverLife.color = new ParticleSystem.MinMaxGradient(data.ColOverLifetime);

            _sizeOverLife.size = new ParticleSystem.MinMaxCurve(data.SizeOverLifetime[0], data.SizeOverLifetime[1]);

            _sizeBySpeed.size = new ParticleSystem.MinMaxCurve(data.SizeBySpeed[0], data.SizeBySpeed[1]);
        }

        // stop the emission module (stop spawning particles)
        private void Update() {
            if (animScript.isOpen) {
                _em.enabled = false;
            }
        }
    }
}