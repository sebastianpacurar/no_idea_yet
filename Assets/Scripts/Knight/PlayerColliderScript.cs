using Input;
using Prop.Interactables.Chest;
using Prop.Interactables.Door.ExitDoor;
using Prop.Interactables.Door.HouseDoor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;


namespace Knight {
    public class PlayerColliderScript : MonoBehaviour {
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private GameObject _targetObject;
        private ChestAnimationScript _chestAnimScript;
        private HouseDoorScript houseDoorScript;
        private ExitDoorScript exitDoorScript;
        private Tilemap _houseDoors;

        private InputManager _input;

        // used for fade animation
        public bool isTransitioning;


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
                transform.position = TileMapUtils.GetWorldToCell(_houseDoors, houseDoorScript.linkedDoor.transform.position);
                RevealPlayer();
            }

            // if overlapped target is an exit door, then go to next level
            if (_targetObject.CompareTag("ExitDoor")) {
                HidePlayer();
                exitDoorScript.GoToNextLevel();
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
                houseDoorScript = _targetObject.GetComponentInChildren<HouseDoorScript>();
            }

            // grab the target obj, and the Door script
            if (other.transform.parent.CompareTag("ExitDoor")) {
                _targetObject = other.transform.parent.gameObject;
                exitDoorScript = _targetObject.GetComponentInChildren<ExitDoorScript>();
            }
        }


        // set target to null, and the used scripts as well, where necessary
        private void OnTriggerExit2D(Collider2D other) {
            _targetObject = null;

            if (other.transform.parent.CompareTag("Chest")) _chestAnimScript = null;
            if (other.transform.parent.CompareTag("HouseDoor")) houseDoorScript = null;
            if (other.transform.parent.CompareTag("ExitDoor")) exitDoorScript = null;
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