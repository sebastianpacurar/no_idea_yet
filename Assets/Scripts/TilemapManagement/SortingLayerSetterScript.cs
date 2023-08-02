using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace TilemapManagement
{
    public class SortingLayerSetterScript : MonoBehaviour
    {
        [Header("The Tilemaps used for props")]
        [SerializeField] private GameObject[] targetTilemaps;

        // used to grab the sprite layer and renderer, for dynamic allocation
        private TilemapRenderer _beforePlayerTilemap;
        private TilemapRenderer _afterPlayerTilemap;

        // grab all sprite renderer components from the above GameObjects which contain the tilemap
        private SpriteRenderer[] _beforePlayerSrs;
        private SpriteRenderer[] _afterPlayerSrs;


        private void Start()
        {
            // start the coroutine to fix the sorting layers in an asynchronous way
            StartCoroutine(nameof(FixSortingLayerAndOrder));
        }


        // set all sprite renderer sorting layers and order in layer properly
        // since tilemap sorting layer cannot override sprite renderer sorting layer, set them manually
        private IEnumerator FixSortingLayerAndOrder()
        {
            foreach (var obj in targetTilemaps)
            {
                var tilemap = obj.GetComponent<TilemapRenderer>();
                var sprites = obj.GetComponentsInChildren<SpriteRenderer>();

                // for every spriteRenderer in children, set the layer name and order to the values of TileMapRenderer
                foreach (var sprite in sprites)
                {
                    // prevent InteractionNotification to get overridden
                    if (sprite.material.name.Contains("Sprite-Unlit")) continue;

                    // set the layer name and order to the values of TileMapRenderer
                    sprite.sortingLayerName = tilemap.sortingLayerName;
                    sprite.sortingOrder = tilemap.sortingOrder;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}