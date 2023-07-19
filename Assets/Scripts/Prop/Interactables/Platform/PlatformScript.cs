using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prop.Interactables.Platform {
    public class PlatformScript : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI countTxt;
        public int requiredCrateNo;
        public bool targetReached;

        [Header("Debug")]
        [SerializeField] private List<GameObject> cratesInRange;

        private Light2D _light;


        private void Awake() {
            _light = GetComponent<Light2D>();
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Crate")) return;
            if (cratesInRange.Contains(other.gameObject)) return;
            cratesInRange.Add(other.gameObject);
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Crate")) return;
            if (!cratesInRange.Contains(other.gameObject)) return;
            cratesInRange.Remove(other.gameObject);
        }


        private void Update() {
            targetReached = cratesInRange.Count >= requiredCrateNo;
            var res = requiredCrateNo - cratesInRange.Count;
            var txt = res >= 0 ? res.ToString() : "0";
            countTxt.text = txt;

            if (targetReached) {
                _light.color = Color.green;
                countTxt.color = Color.green;
            } else {
                _light.color = Color.yellow;
                countTxt.color = Color.yellow;
            }
        }
    }
}