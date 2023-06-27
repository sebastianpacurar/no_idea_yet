using UnityEngine;

namespace Chest {
    public class ChestAnimationScript : MonoBehaviour {
        [SerializeField] private GameObject notificationObj;
        private Animator _animator;
        private BoxCollider2D _box;
        public bool isOpen;

        private void Awake() {
            _animator = GetComponent<Animator>();
            _box = transform.parent.GetComponent<BoxCollider2D>();
        }

        public void OpenChest() {
            _animator.SetBool("IsOpened", true);
            isOpen = true;
        }

        // destroy the gameObject along with the xLight and xDark images
        // called at the end of Open chest animation
        public void CleanUp() {
            // destroy notification object and set collider to false
            Destroy(notificationObj);
            _box.enabled = false;
        }
    }
}