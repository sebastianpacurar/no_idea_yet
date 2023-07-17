using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prop.Interactables.Platform {
    public class PlatformScript : MonoBehaviour {
        [SerializeField] private List<GameObject> cratesInRange;
        public bool targetReached;
        public int requiredCrateNo;
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

            if (targetReached) {
                _light.color = Color.green;
            } else {
                _light.color = Color.yellow;
            }
        }
    }
}