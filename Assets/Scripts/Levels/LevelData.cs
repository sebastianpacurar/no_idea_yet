using System.Collections.Generic;
using System.Linq;
using Prop.Interactables.Platform;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;


namespace Levels {
    public class LevelData {
        public Transform entryDoor;
        public PlatformScript[] platforms;

        private List<SpawnableObject> spawnableObjects;
        public readonly PolygonCollider2D levelArea;


        public LevelData(PolygonCollider2D levelArea) {
            this.levelArea = levelArea;
            SetGameObjects();
            SetSpawnableObjects(); // set spawnableObjects for the current LevelData
        }


        private void SetSpawnableObjects() {
            spawnableObjects = new List<SpawnableObject>();

            var crateObjects = AreaUtils.FindObjectsWithTagInArea("Crate", levelArea);
            var cartObjects = AreaUtils.FindObjectsWithTagInArea("Cart", levelArea);
            var gameObjects = crateObjects.Concat(cartObjects).ToArray();

            // attach all needed data to a new SpawnableObject 
            foreach (var obj in gameObjects) {
                var levelObject = new SpawnableObject {
                    GameObject = obj,
                    Name = obj.name,
                    Position = obj.transform.position,
                    Parent = obj.transform.parent,
                    Prefab = Resources.Load<GameObject>(GetPrefabLocation(obj.name)),
                };

                // add to spawnable objects dict
                spawnableObjects.Add(levelObject);
            }
        }

        private void SetGameObjects() {
            entryDoor = AreaUtils.FindObjectWithTagInArea("EntryDoor", levelArea).transform;
            platforms = AreaUtils.FindObjectsWithTagInArea("ExitPlatform", levelArea).Select(obj => obj.GetComponent<PlatformScript>()).ToArray();
        }


        // restart level by setting all existing spawnableObjects to their regular positions
        // in case the GameObject is destroyed then recreate it and attach it to the current SpawnableObject
        public void ResetLevel() {
            foreach (var obj in spawnableObjects) {
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