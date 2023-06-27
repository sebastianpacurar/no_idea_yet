using System.Collections;
using UnityEngine;

namespace Chest {
    public class ChestControllerScript : MonoBehaviour {
        [SerializeField] private GameObject notificationObj;
        [SerializeField] private SpriteRenderer xLightImg;
        [SerializeField] private SpriteRenderer xDarkImg;

        private void Start() {
            // false since no contact with the player when script gets started
            notificationObj.SetActive(false);
        }

        private IEnumerator ToggleSprite() {
            while (true) {
                // execute what's below yield, every half a second
                yield return new WaitForSeconds(0.5f);

                // swap the order in the Sorting Layer between the images
                (xLightImg.sortingOrder, xDarkImg.sortingOrder) = (xDarkImg.sortingOrder, xLightImg.sortingOrder);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                // reveal notification and start the ToggleSprite Coroutine

                notificationObj.SetActive(true);
                StartCoroutine(nameof(ToggleSprite));
            }
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                // hide notification and stop the ToggleSprite Coroutine

                notificationObj.SetActive(false);
                StopCoroutine(nameof(ToggleSprite));
            }
        }
    }
}