using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour {
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private float cycleDuration; // how long should a day last, in seconds
    [SerializeField] private float currentTime; // do not change

    private Light2D _globalLight;

    private void Start() {
        _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
        _globalLight.intensity = maxIntensity;
        currentTime = cycleDuration / 2; // start from mid of day

        StartCoroutine(StartCycle());
    }

    private IEnumerator StartCycle() {
        while (true) {
            // daytime - ranges from 0 to cycleDuration 
            while (currentTime < cycleDuration / 2f) {
                currentTime += Time.deltaTime;
                /*
                 * NOTE:
                 * with currentTime ranging from 0 to "cycleDuration / 2f" during the daytime phase,
                 *   and "cycleDuration / 2f" represents the duration of the daytime phase.
                 * currentTime / (cycleDuration / 2f) = range from 0 to 1 for daytime
                 * 
                */
                var t = currentTime / (cycleDuration / 2f);
                _globalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

                yield return null;
            }

            // night - ranges from "cycleDuration / 2f" to "cycleDuration".
            while (currentTime < cycleDuration) {
                currentTime += Time.deltaTime;
                /* NOTE:
                 * subtract cycleDuration / 2f from currentTime to shift the range to start from 0.
                 * currentTime - cycleDuration / 2f) / (cycleDuration / 2f) = range from 0 to 1 for nighttime
                */
                var t = (currentTime - cycleDuration / 2f) / (cycleDuration / 2f);
                _globalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);

                yield return null;
            }

            // Reset time for the next cycle
            currentTime = 0f;
        }
    }
}