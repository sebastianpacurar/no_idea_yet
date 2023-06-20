using System;
using Chest;
using UnityEngine;

namespace Knight {
    public class KnightColliderScript : MonoBehaviour {
        private KnightControllerScript _script;
        private GameObject _targetObject;
        private ChestControllerScript _chestScript;

        private void Awake() {
            _script = GetComponent<KnightControllerScript>();
        }

        public void Update() {
            // if targetObject is null, then skip
            if (!_targetObject) return;

            // if the Player Collider overlaps the Chest Collider AND x key is pressed
            if (_script.isInteractPressed) {
                if (_targetObject.CompareTag("Chest")) {
                    if (_chestScript.isOpen) {
                        _chestScript.CloseChest();
                    } else {
                        _chestScript.OpenChest();
                    }
                }

                _script.isInteractPressed = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Chest")) {
                _targetObject = other.gameObject;
                _chestScript = _targetObject.GetComponent<ChestControllerScript>();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;
        }
    }
}