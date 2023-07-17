using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prop.Interactables.Door {
    public class ChangeLevel : MonoBehaviour {
        [SerializeField] private GameObject linkedDoor;
        private Tilemap _doorMap;
        private LevelManager _levelManager;


        private void Awake() {
            _doorMap = transform.parent.GetComponent<Tilemap>();
        }

        
        private void Start() {
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }


        public Vector3 GetLinkedDoorCellPos() {
            // get the cell pos in tilemap
            var cellPos = _doorMap.WorldToCell(linkedDoor.transform.position);

            // parse the position since the tilemap is non-uniform (x:2 , y:2.5)
            // var parsedPos = _doorMap.CellToWorld(cellPos) + _doorMap.cellSize / 2f;

            var parsedPos = _doorMap.CellToWorld(cellPos);
            return parsedPos;
        }

        // TODO: change this!!
        public void GoToNextLevel() {
            var levelNo = int.Parse(linkedDoor.name.Split("_")[1]);
            _levelManager.MoveToLevel(levelNo);
        }
    }
}