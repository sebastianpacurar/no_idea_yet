using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Levels {
    public class LevelData {
        public Transform entryDoor;

        private List<LevelObject> levelObjects;
        private readonly PolygonCollider2D _levelArea;

        public LevelData(PolygonCollider2D levelArea) {
            _levelArea = levelArea;
        }


        // set levelObjects for the current LevelData
        public void SetLevelObjects() {
            levelObjects = new List<LevelObject>();

            var crateObjects = AreaUtils.FindObjectsWithTagInArea("Crate", _levelArea);
            var cartObjects = AreaUtils.FindObjectsWithTagInArea("Cart", _levelArea);
            var tag = _levelArea.name.Equals("Lvl 1 Poly") ? "ExitDoor" : "EntryDoor";
            entryDoor = AreaUtils.FindObjectWithTagInArea(tag, _levelArea).transform;

            var gameObjects = crateObjects.Concat(cartObjects).ToArray();

            // attach all needed data to a new LevelObject 
            foreach (var obj in gameObjects) {
                var levelObject = new LevelObject {
                    GameObject = obj,
                    Name = obj.name,
                    Position = obj.transform.position,
                    Parent = obj.transform.parent,
                    Prefab = Resources.Load<GameObject>(GetPrefabLocation(obj.name)),
                };

                // add to level objects dict
                levelObjects.Add(levelObject);
            }
        }


        // restart level by setting all existing LevelObjects to their regular positions
        // in case the GameObject is destroyed then recreate it and attach it to the current LevelObject
        public void ResetLevel() {
            foreach (var obj in levelObjects) {
                if (obj.GameObject) {
                    // if game object exists, then reset position
                    obj.GameObject.transform.position = obj.Position;
                } else {
                    // else, recreate it and place it at its initial position since level accessed
                    var newObj = Object.Instantiate(obj.Prefab, obj.Position, Quaternion.identity, obj.Parent);
                    obj.GameObject = newObj;
                }
            }
        }

        // get prefab file locations
        private string GetPrefabLocation(string objName) {
            return objName switch {
                "SmallCrate" or "LargeCrate" => $"Prefabs/Pushables/Crates/{objName}",
                "CartLeft" or "CartRight" => $"Prefabs/Pushables/Carts/{objName}",
                _ => "",
            };
        }
    }
}