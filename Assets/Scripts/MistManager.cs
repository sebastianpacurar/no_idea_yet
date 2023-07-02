using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MistManager : MonoBehaviour {
    [SerializeField] private float minIntensity; // set when min intensity for day-night transition

    [SerializeField] private ParticleSystem mistBeforePlayer;
    [SerializeField] private ParticleSystem mistAfterPlayer;

    private ParticleSystem.EmissionModule _mistBeforeEmMod;
    private ParticleSystem.EmissionModule _mistAfterEmMod;
    private Light2D _globalLight;


    private void Awake() {
        _mistBeforeEmMod = mistBeforePlayer.emission;
        _mistAfterEmMod = mistAfterPlayer.emission;
    }


    private void Start() {
        _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
    }


    // if Late Night then enable particles, else disable them through emission module
    private void Update() {
        if (_globalLight.intensity <= minIntensity) {
            SetMistParticles(true);
        } else if (_globalLight.intensity >= minIntensity) {
            SetMistParticles(false);
        }
    }


    private void SetMistParticles(bool value) {
        _mistBeforeEmMod.enabled = value;
        _mistAfterEmMod.enabled = value;
    }
}