using System.Collections;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.HouseDoor {
    public class HouseDoorLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;


        private void Start() {
            LabelUtils.SetSprites(gameObject, false);
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                LabelUtils.SetSprites(gameObject, value: true);
                StartCoroutine(nameof(ToggleBtnImgCoroutine));
            }
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                LabelUtils.SetSprites(gameObject, value: false);
                StopCoroutine(nameof(ToggleBtnImgCoroutine));
            }
        }
    }
}