using System.Linq;
using Levels;
using UnityEngine;


namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorScript : MonoBehaviour {
        [Header("Door Objects")]
        [SerializeField] private GameObject closeDoor;
        [SerializeField] private GameObject openedDoor;

        private LevelManager _lm;
        private LevelData _ld;


        private void Start() {
            _lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            _ld = _lm.levelsData[_lm.currentLevel];
        }
        

        private void Update() {
            ToggleVisibleDoor();
        }


        private void ToggleVisibleDoor() {
            // if target reached and OpenedDoor is not active, then disable ClosedDoor and enable OpenedDoor
            if (IsTargetReached() && !openedDoor.activeInHierarchy) {
                closeDoor.SetActive(false);
                openedDoor.SetActive(true);
            }
            // if target not reached and ClosedDoor is not active, then disable OpenedDoor and enable ClosedDoor
            else if (!IsTargetReached() && !closeDoor.activeInHierarchy) {
                closeDoor.SetActive(true);
                openedDoor.SetActive(false);
            }
        }


        // if targetReached, then set currentLevel and perform Level Transition
        public void GoToNextLevel() {
            if (IsTargetReached()) {
                var nextLevelNo = _lm.currentLevel + 1;
                var entryDoor = _lm.levelsData[nextLevelNo].entryDoor;

                _lm.TransitionToNextLevel(entryDoor.position);
            }
        }


        // check if all platforms have the targetReached bool set to true
        public bool IsTargetReached() {
            return _ld.platforms.All(platform => platform.targetReached);
        }
    }
}