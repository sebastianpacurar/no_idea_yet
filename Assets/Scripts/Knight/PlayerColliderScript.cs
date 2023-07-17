using Cinemachine;
using Input;
using Prop.Interactables.Chest;
using Prop.Interactables.Door;
using UnityEngine;


namespace Knight {
    public class PlayerColliderScript : MonoBehaviour {
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private GameObject _targetObject;
        private ChestAnimationScript _chestAnimScript;
        private DoorScript _doorScript;
        private ChangeLevel _changeLevel;

        private InputManager _input;


        private void Awake() {
            _sr = GetComponentInChildren<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            _input = InputManager.Instance;
        }


        public void Update() {
            // if targetObject is null, then skip
            if (!_targetObject) return;

            // if x key is not pressed, then return
            if (!_input.IsInteractPressed) return;

            // if overlap target is a chest
            if (_targetObject.CompareTag("Chest")) {
                _chestAnimScript.OpenChest();
            }

            // if overlap target is a house door
            if (_targetObject.CompareTag("HouseDoor")) {
                HidePlayer();
                transform.position = _doorScript.GetLinkedDoorCellPos();
                RevealPlayer();
            }

            // if overlapped target is an exit door, then go to next level
            if (_targetObject.CompareTag("ExitDoor")) {
                HidePlayer();
                transform.position = _changeLevel.GetLinkedDoorCellPos();
                _changeLevel.GoToNextLevel();
                RevealPlayer();
            }

            // set IsInteractPressed to false immediately
            _input.IsInteractPressed = false;
        }


        private void OnTriggerEnter2D(Collider2D other) {
            // grab the target obj, and the ChestAnimation script
            if (other.transform.parent.CompareTag("Chest")) {
                _targetObject = other.transform.parent.gameObject;
                _chestAnimScript = _targetObject.GetComponentInChildren<ChestAnimationScript>();
            }

            // grab the target obj, and the Door script
            if (other.transform.parent.CompareTag("HouseDoor")) {
                _targetObject = other.transform.parent.gameObject;
                _doorScript = _targetObject.GetComponentInChildren<DoorScript>();
            }

            // grab the target obj, and the Door script
            if (other.transform.parent.CompareTag("ExitDoor")) {
                _targetObject = other.transform.parent.gameObject;
                _changeLevel = _targetObject.GetComponentInChildren<ChangeLevel>();
            }
        }


        // set target to null, and the used scripts as well, where necessary
        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;

            if (other.transform.parent.CompareTag("Chest")) _chestAnimScript = null;
            if (other.transform.parent.CompareTag("HouseDoor")) _doorScript = null;
            if (other.transform.parent.CompareTag("ExitDoor")) _changeLevel = null;
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