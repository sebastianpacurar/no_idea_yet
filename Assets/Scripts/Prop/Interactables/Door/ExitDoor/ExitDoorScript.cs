using Prop.Interactables.Platform;
using UnityEngine;

namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorScript : MonoBehaviour {
        [Header("Exit Door link and platform data")]
        public GameObject linkedDoor;
        public PlatformScript platform;

        [Header("Door Objects")]
        [SerializeField] private GameObject closeDoor;
        [SerializeField] private GameObject openedDoor;

        private LevelManager _levelManager;


        private void Start() {
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }


        private void Update() {
            ToggleVisibleDoor();
        }


        private void ToggleVisibleDoor() {
            // if target reached and OpenedDoor is not active, then disable ClosedDoor and enable OpenedDoor
            if (platform.targetReached && !openedDoor.activeInHierarchy) {
                closeDoor.SetActive(false);
                openedDoor.SetActive(true);
            }
            // if target not reached and ClosedDoor is not active, then disable OpenedDoor and enable ClosedDoor
            else if (!platform.targetReached && !closeDoor.activeInHierarchy) {
                closeDoor.SetActive(true);
                openedDoor.SetActive(false);
            }
        }


        public void GoToNextLevel() {
            if (platform.targetReached) {
                var levelNo = int.Parse(linkedDoor.name.Split("_")[1]);
                _levelManager.MoveToLevel(levelNo);
            }
        }
    }
}