using System.Collections;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.ExitDoor {
    public class ExitDoorLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;

        private ExitDoorScript _exitDoor;
        private BoxCollider2D _box;


        private void Awake() {
            _box = GetComponent<BoxCollider2D>();
        }


        private void Start() {
            _exitDoor = transform.parent.gameObject.GetComponent<ExitDoorScript>();
            LabelUtils.SetSprites(gameObject, false);
        }

        private void Update() {
            ToggleBoxCollider();
        }


        private void ToggleBoxCollider() {
            // if target reached and box is disabled, then enable box
            if (_exitDoor.IsTargetReached() && !_box.enabled) {
                _box.enabled = true;
            }
            // if target not reached and box is enabled, then disable box
            else if (!_exitDoor.IsTargetReached() && _box.enabled) {
                _box.enabled = false;
            }
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!_exitDoor.IsTargetReached()) return;

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