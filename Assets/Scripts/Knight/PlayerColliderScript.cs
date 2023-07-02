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

            // if x key is not pressed, then return
            if (!_script.isInteractPressed) return;

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

            // set isInteractPressed to false immediately
            _script.isInteractPressed = false;
        }


        private void OnTriggerEnter2D(Collider2D other) {
            // grab the target obj, and the ChestAnimation script
            if (other.transform.parent.CompareTag("Chest")) {
                _targetObject = other.transform.parent.gameObject;
                _chestAnimScript = _targetObject.GetComponentInChildren<ChestAnimationScript>();
            }

            // grab the target obj, and the Door script
            if (other.transform.parent.CompareTag("Door")) {
                _targetObject = other.transform.parent.gameObject;
                _doorScript = _targetObject.GetComponentInChildren<DoorScript>();
            }
        }


        // set target to null, and the used scripts as well, where necessary
        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;

            if (other.transform.parent.CompareTag("Chest")) _chestAnimScript = null;
            if (other.transform.parent.CompareTag("Door")) _doorScript = null;
        }


        // block all physics and hide sprite
        private void HidePlayer() {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _sr.enabled = false;
        }


        // release all physics, except rotation on Z-axis, and reveal sprite
        private void RevealPlayer() {
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            _sr.enabled = true;
        }
    }
}