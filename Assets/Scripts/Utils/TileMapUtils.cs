using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils {
    public static class TileMapUtils {
        // return world to cell
        public static Vector3 GetWorldToCell(Tilemap tileMap, Vector3 tilePos) {
            return tileMap.WorldToCell(tilePos);
        }
    }
}