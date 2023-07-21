using System.Collections;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.HouseDoor {
    public class HouseDoorLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;

        [SerializeField] private SpriteRenderer[] listedSrs;

        private void Awake() {
            listedSrs = new[] { wrapper, lightImg, darkImg };
        }


        private void Start() {
            LabelUtils.SetSprites(listedSrs, false);
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                LabelUtils.SetSprites(listedSrs, value: true);
                StartCoroutine(nameof(ToggleBtnImgCoroutine));
            }
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                LabelUtils.SetSprites(listedSrs, value: false);
                StopCoroutine(nameof(ToggleBtnImgCoroutine));
            }
        }
    }
}