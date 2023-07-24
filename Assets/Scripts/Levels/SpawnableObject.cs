using UnityEngine;

namespace Levels {
    // Data about GameObjects on a specific level
    public class SpawnableObject {
        public GameObject GameObject { get; set; } // general data about the target obj
        public string Name { get; set; } // name (for debugging)
        public Vector3 Position { get; set; } // spawn position
        public Transform Parent { get; set; } // parent, used for attaching objects to tilemaps when instantiating them
        public GameObject Prefab { get; set; } // the prefab used to instantiate this object
    }
}