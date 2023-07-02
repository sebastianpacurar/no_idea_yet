using System.Collections;
using UnityEngine;


namespace Prop.Interactables {
    public class InteractionNotificationScript : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer xLightImg;
        [SerializeField] private SpriteRenderer xDarkImg;

        
        private void Start() {
            // false since no contact with the player when script gets started
            SetSpriteVisibilityTo(false);
        }

        
        private IEnumerator ToggleBtnImg() {
            while (true) {
                // execute what's below yield, every half a second
                yield return new WaitForSeconds(0.5f);

                // swap the order in the Sorting Layer between the images
                (xLightImg.sortingOrder, xDarkImg.sortingOrder) = (xDarkImg.sortingOrder, xLightImg.sortingOrder);
            }
        }

        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                // reveal notification and start the ToggleBtnImg Coroutine

                SetSpriteVisibilityTo(true);
                StartCoroutine(nameof(ToggleBtnImg));
            }
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                // hide notification and stop the ToggleBtnImg Coroutine

                SetSpriteVisibilityTo(false);
                StopCoroutine(nameof(ToggleBtnImg));
            }
        }

        private void SetSpriteVisibilityTo(bool value) {
            wrapper.enabled = value;
            xDarkImg.enabled = value;
            xLightImg.enabled = value;
        }
    }
}