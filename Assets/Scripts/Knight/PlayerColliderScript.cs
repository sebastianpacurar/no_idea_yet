using Chest;
using UnityEngine;

namespace Knight {
    public class PlayerColliderScript : MonoBehaviour {
        private PlayerControllerScript _script;
        private GameObject _targetObject;
        private ChestAnimationScript _chestAnimScript;

        private void Awake() {
            _script = GetComponent<PlayerControllerScript>();
        }

        public void Update() {
            // if targetObject is null, then skip
            if (!_targetObject) return;

            // if the Player Collider overlaps the Chest Collider AND x key is pressed
            if (_targetObject.CompareTag("Chest") && _script.isInteractPressed) {
                _chestAnimScript.OpenChest();
                _script.isInteractPressed = false;
            }
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Chest")) {
                _targetObject = other.gameObject;
                _chestAnimScript = _targetObject.GetComponentInChildren<ChestAnimationScript>();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;
            _chestAnimScript = null;
        }
    }
}