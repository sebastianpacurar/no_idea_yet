using System.Collections;
using UnityEngine;

namespace Utils {
    public static class LabelUtils {
        // set all sprite renderers of a specific label to value
        public static void SetSprites(GameObject obj, bool value) {
            foreach (var sr in obj.GetComponentsInChildren<SpriteRenderer>()) {
                sr.enabled = value;
            }
        }

        // generic ToggleBtnImg for image swapping every half a second
        // swap sorting order of overlapping keyboard keys
        public static IEnumerator ToggleBtnImg(SpriteRenderer a, SpriteRenderer b) {
            while (true) {
                yield return new WaitForSeconds(0.5f);
                (a.sortingOrder, b.sortingOrder) = (b.sortingOrder, a.sortingOrder);
            }
        }
    }
}