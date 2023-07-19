using System.Linq;
using UnityEngine;

namespace Utils {
    public static class AreaUtils {
        public static GameObject[] FindObjectsWithTagInArea(string tag, PolygonCollider2D areaCollider) {
            var allObjects = GameObject.FindGameObjectsWithTag(tag);
            // grab all objects whose positions overlap the provided areaCollider polygon
            var overlappingObjects = allObjects.Where(obj => areaCollider.OverlapPoint(obj.transform.position)).ToArray();

            return overlappingObjects;
        }

        public static GameObject FindObjectWithTagInArea(string tag, PolygonCollider2D areaCollider) {
            var allObjects = GameObject.FindGameObjectsWithTag(tag);
            // grab the first object whose position overlaps the provided areaCollider polygon
            var overlappingObject = allObjects.First(obj => areaCollider.OverlapPoint(obj.transform.position));

            return overlappingObject;
        }
    }
}