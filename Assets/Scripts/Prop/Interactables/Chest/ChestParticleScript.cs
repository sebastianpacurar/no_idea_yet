using ScriptableObjects;
using UnityEngine;

namespace Prop.Interactables.Chest {
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

        // initialize the ParticleSystem component
        private void Awake() {
            _ps = GetComponent<ParticleSystem>();
        }

        // initialize the ParticleSystem modules
        private void Start() {
            _main = _ps.main;
            _em = _ps.emission;
            _colorOverLife = _ps.colorOverLifetime;
            _sizeOverLife = _ps.sizeOverLifetime;
            _sizeBySpeed = _ps.sizeBySpeed;
        }

        private void Update() {
            // stop the emission module (stop spawning particles)
            //  else perform data setup
            if (animScript.isOpen) {
                _em.enabled = false;
            } else {
                SetData();
            }
        }

        // Populate the needed values with the info from data
        // contents of data is of form Vector2(Min, Max) where X is Min and Y is Max
        private void SetData() {
            //main module - random between 2 constants
            _main.startLifetime = new ParticleSystem.MinMaxCurve(data.StartLifetime.x, data.StartLifetime.y);
            _main.startSpeed = new ParticleSystem.MinMaxCurve(data.StartSpeed.x, data.StartSpeed.y);
            _main.startSize = new ParticleSystem.MinMaxCurve(data.StartSize.x, data.StartSize.y);

            // Emission module - random between 2 constants
            _em.rateOverTime = new ParticleSystem.MinMaxCurve(data.EmRateOverTime.x, data.EmRateOverTime.y);
            // Color over Lifetime module - color gradient
            _colorOverLife.color = new ParticleSystem.MinMaxGradient(data.ColOverLifetime);
            // Size over Lifetime module - random between 2 constants
            _sizeOverLife.size = new ParticleSystem.MinMaxCurve(data.SizeOverLifetime.x, data.SizeOverLifetime.y);
            // Size by Speed module - random between 2 constants
            _sizeBySpeed.size = new ParticleSystem.MinMaxCurve(data.SizeBySpeed.x, data.SizeBySpeed.y);
        }
    }
}