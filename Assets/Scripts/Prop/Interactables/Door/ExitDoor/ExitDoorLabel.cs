using System.Collections;
using Prop.Interactables.Platform;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;

        [Header("Debug")]
        [SerializeField] private PlatformScript platform;

        private BoxCollider2D _box;


        private void Awake() {
            _box = GetComponent<BoxCollider2D>();
        }


        private void Start() {
            platform = transform.parent.gameObject.GetComponent<ExitDoorScript>().platform;
            LabelUtils.SetSprites(gameObject, false);
        }

        private void Update() {
            ToggleBoxCollider();
        }


        private void ToggleBoxCollider() {
            // if target reached and box is disabled, then enable box
            if (platform.targetReached && !_box.enabled) {
                _box.enabled = true;
            }
            // if target not reached and box is enabled, then disable box
            else if (!platform.targetReached && _box.enabled) {
                _box.enabled = false;
            }
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!platform.targetReached) return;

            LabelUtils.SetSprites(gameObject, value: true);
            StartCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!wrapper.enabled) return;

            LabelUtils.SetSprites(gameObject, value: false);
            StopCoroutine(nameof(ToggleBtnImgCoroutine));
        }
    }
}