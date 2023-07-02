using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prop.Interactables.Door {
    public class DoorScript : MonoBehaviour {
        [SerializeField] private GameObject linkedDoor;
        private Tilemap _doorMap;

        private void Awake() {
            _doorMap = transform.parent.GetComponent<Tilemap>();
        }


        public Vector3 GetLinkedDoorCellPos() {
            // get the cell pos in tilemap
            var cellPos = _doorMap.WorldToCell(linkedDoor.transform.position);

            // parse the position since the tilemap is non-uniform (x:2 , y:2.5)
            // var parsedPos = _doorMap.CellToWorld(cellPos) + _doorMap.cellSize / 2f;

            var parsedPos = _doorMap.CellToWorld(cellPos);
            return parsedPos;
        }
    }
}