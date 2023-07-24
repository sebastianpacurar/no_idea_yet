using System.Linq;
using Levels;
using Prop.Interactables.Platform;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorScript : MonoBehaviour {
        [Header("Door Objects")]
        [SerializeField] private GameObject closeDoor;
        [SerializeField] private GameObject openedDoor;

        [Space(5)]
        [Header("Current Level Platforms")]
        public PlatformScript[] platforms;

        [Space(5)]
        [Header("Next Level Target Door")]
        public Transform entryDoor;

        [Space(5)]
        [Header("Debug")]
        [SerializeField] private int currLevel;
        [SerializeField] private PolygonCollider2D levelArea;

        private LevelManager _lm;


        private void Start() {
            _lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            PerformSetup();
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
                _lm.MoveToNextLevel(entryDoor.position);
            }
        }


        // check if all platforms have the targetReached bool set to true
        public bool IsTargetReached() {
            return platforms.All(platform => platform.targetReached);
        }


        // set the corresponding level area polygon2D
        private void SetLevelArea() {
            var col2D = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("LevelArea"));

            if (col2D is PolygonCollider2D polygonCollider2D) {
                levelArea = polygonCollider2D;
            }
        }


        // find key based on matching value in levels dictionary
        private void SetCurrentLevel() {
            foreach (var pair in _lm.levels.Where(pair => pair.Value == levelArea)) {
                currLevel = pair.Key;
            }
        }


        // set all the platform scripts in the current level area polygon2D
        private void SetPlatforms() {
            var objects = AreaUtils.FindObjectsWithTagInArea("ExitPlatform", levelArea);
            platforms = objects.Select(obj => obj.GetComponent<PlatformScript>()).ToArray();
        }


        // find the position of the next level's EntryDoor game object
        private void SetNextLevelEntryDoor() {
            var nextLevelPoly2D = _lm.levels[currLevel + 1];
            entryDoor = AreaUtils.FindObjectWithTagInArea("EntryDoor", nextLevelPoly2D).transform;
        }


        private void PerformSetup() {
            SetLevelArea();
            SetPlatforms();
            SetCurrentLevel();
            SetNextLevelEntryDoor();
        }
    }
}