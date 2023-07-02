using Prop.Interactables.Chest;
using Prop.Interactables.Door;
using UnityEngine;


namespace Knight {
    public class PlayerColliderScript : MonoBehaviour {
        [SerializeField] private float doorTransitionTime;

        private PlayerControllerScript _script;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private GameObject _targetObject;
        private ChestAnimationScript _chestAnimScript;
        private DoorScript _doorScript;

        private void Awake() {
            _script = GetComponent<PlayerControllerScript>();
            _sr = GetComponentInChildren<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Update() {
            // if targetObject is null, then skip
            if (!_targetObject) return;


            // if x key is pressed
            if (_script.isInteractPressed) {
                // if overlap target is a chest
                if (_targetObject.CompareTag("Chest")) {
                    _chestAnimScript.OpenChest();
                }

                // if overlap target is a door
                if (_targetObject.CompareTag("Door")) {
                    HidePlayer();
                    transform.position = _doorScript.GetLinkedDoorCellPos();
                    RevealPlayer();
                }

                _script.isInteractPressed = false;
            }
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.transform.parent.CompareTag("Chest")) {
                _targetObject = other.transform.parent.gameObject;
                _chestAnimScript = _targetObject.GetComponentInChildren<ChestAnimationScript>();
            }

            if (other.transform.parent.CompareTag("Door")) {
                _targetObject = other.transform.parent.gameObject;
                _doorScript = _targetObject.GetComponentInChildren<DoorScript>();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;

            if (other.transform.parent.CompareTag("Chest")) _chestAnimScript = null;
            if (other.transform.parent.CompareTag("Door")) _doorScript = null;
        }


        private void HidePlayer() {
            _sr.enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void RevealPlayer() {
            _sr.enabled = true;
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}