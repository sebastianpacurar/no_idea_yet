using System.Linq;
using Prop.Interactables.Platform;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorScript : MonoBehaviour {
        [Header("Exit Door link and platform data")]
        public GameObject linkedDoor;
        public PlatformScript[] platforms;

        [Header("Door Objects")]
        [SerializeField] private GameObject closeDoor;
        [SerializeField] private GameObject openedDoor;


        [Header("Debug")]
        [SerializeField] private PolygonCollider2D levelArea;
        private LevelManager _levelManager;


        private void Start() {
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            SetLevelArea();
            SetPlatforms();
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
        public void GoToNextLevel(Vector3 targetDoor) {
            if (IsTargetReached()) {
                var levelNo = int.Parse(linkedDoor.name.Split("_")[1]);
                _levelManager.MoveToLevel(levelNo, targetDoor);
            }
        }


        // check if all platforms have the targetReached bool set to true
        public bool IsTargetReached() {
            return platforms.All(platform => platform.targetReached);
        }


        // set all the platform scripts in the level area
        private void SetPlatforms() {
            var objects = AreaUtils.FindObjectsWithTagInArea("ExitPlatform", levelArea);
            platforms = objects.Select(obj => obj.GetComponent<PlatformScript>()).ToArray();
        }


        // set the corresponding level area polygon
        private void SetLevelArea() {
            var col2D = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("LevelArea"));

            if (col2D is PolygonCollider2D polygonCollider2D) {
                levelArea = polygonCollider2D;
            }
        }
    }
}