using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemap {
    public class SortingLayerFixer : MonoBehaviour {
        [Header("The 2 tilemaps used for props")]
        [SerializeField] private GameObject propsBeforePlayer;
        [SerializeField] private GameObject propsAfterPlayer;

        // set all sprite renderer sorting layers and order in layer properly
        // since tilemap sorting layer cannot override sprite renderer sorting layer, set them manually
        // NOTE: put in a Coroutine for better resource managing
        private void Start() {
            // grab the tilemap renderers of the 2 tilemap objects
            var beforePlayerTilemap = propsBeforePlayer.GetComponent<TilemapRenderer>();
            var afterPlayerTilemap = propsAfterPlayer.GetComponent<TilemapRenderer>();

            // grab all the sprite renderer components from both tilemap parent objects
            var beforePlayerSprites = propsBeforePlayer.GetComponentsInChildren<SpriteRenderer>();
            var afterPlayerSprites = propsAfterPlayer.GetComponentsInChildren<SpriteRenderer>();

            // set all beforeSprite objects to their relevant sorting layer and order in layer
            foreach (var sprite in beforePlayerSprites) {
                sprite.sortingLayerName = beforePlayerTilemap.sortingLayerName;
                sprite.sortingOrder = beforePlayerTilemap.sortingOrder;
            }

            // set all afterSprite objects to their relevant sorting layer and order in layer
            foreach (var sprite in afterPlayerSprites) {
                sprite.sortingLayerName = afterPlayerTilemap.sortingLayerName;
                sprite.sortingOrder = afterPlayerTilemap.sortingOrder;
            }
        }
    }
}