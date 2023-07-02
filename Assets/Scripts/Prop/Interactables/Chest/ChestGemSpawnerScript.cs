using System.Collections;
using UnityEngine;


namespace Prop.Interactables.Chest {
    public class ChestGemSpawnerScript : MonoBehaviour {
        [SerializeField] private GameObject gemType;
        [SerializeField] private GameObject spriteObj;
        private ChestAnimationScript _chestAnimaScript;

        
        private void Awake() {
            var spriteObjSibling = transform.parent.transform;
            _chestAnimaScript = spriteObjSibling.Find("Sprite").GetComponent<ChestAnimationScript>();
        }

        
        private void Start() {
            StartCoroutine(nameof(GemInstantiation));
        }

        
        private IEnumerator GemInstantiation() {
            // hibernate until condition is met
            yield return new WaitUntil(() => _chestAnimaScript.isOpen);

            // instantiate gem based on prefab. using this game object's position, and keep rotation identity to default
            Instantiate(gemType, transform.position, Quaternion.identity);

            // stop coroutine, since only one item is needed
            StopCoroutine(nameof(GemInstantiation));
        }
    }
}