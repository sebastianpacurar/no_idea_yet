using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


// TODO: not working in case the objects are nested and have different sprite renderers
// TODO: should increment the order in layer recursively
namespace TilemapManagement {
    public class SortingLayerSetterScript : MonoBehaviour {
        [Header("The 2 tilemaps used for props")]
        [SerializeField] private GameObject propsBeforePlayer;
        [SerializeField] private GameObject propsAfterPlayer;

        // used to grab the sprite layer and renderer, for dynamic allocation
        private TilemapRenderer _beforePlayerTilemap;
        private TilemapRenderer _afterPlayerTilemap;

        // grab all sprite renderer components from the above GameObjects which contain the tilemap
        private SpriteRenderer[] _beforePlayerSrs;
        private SpriteRenderer[] _afterPlayerSrs;


        private void Start() {
            // grab the tilemap renderers of the 2 tilemap objects
            _beforePlayerTilemap = propsBeforePlayer.GetComponent<TilemapRenderer>();
            _afterPlayerTilemap = propsAfterPlayer.GetComponent<TilemapRenderer>();

            // grab all the sprite renderer components from both tilemap parent objects
            _beforePlayerSrs = propsBeforePlayer.GetComponentsInChildren<SpriteRenderer>();
            _afterPlayerSrs = propsAfterPlayer.GetComponentsInChildren<SpriteRenderer>();

            // start the coroutine to fix the sorting layers in an asynchronous way
            StartCoroutine(nameof(FixSortingLayerAndOrder));
        }


        // set all sprite renderer sorting layers and order in layer properly
        // since tilemap sorting layer cannot override sprite renderer sorting layer, set them manually
        private IEnumerator FixSortingLayerAndOrder() {
            yield return new WaitForEndOfFrame();

            // set all beforeSprite objects to their relevant sorting layer and order in layer
            foreach (var sprite in _beforePlayerSrs) {
                sprite.sortingLayerName = _beforePlayerTilemap.sortingLayerName;
                sprite.sortingOrder = _beforePlayerTilemap.sortingOrder;
            }

            // set all afterSprite objects to their relevant sorting layer and order in layer
            foreach (var sprite in _afterPlayerSrs) {
                sprite.sortingLayerName = _afterPlayerTilemap.sortingLayerName;
                sprite.sortingOrder = _afterPlayerTilemap.sortingOrder;
            }
        }
    }
}