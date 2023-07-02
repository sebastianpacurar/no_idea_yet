using UnityEngine;


namespace Prop.Interactables.Chest {
    public class ChestAnimationScript : MonoBehaviour {
        [SerializeField] private GameObject notificationObj;
        private Animator _animator;
        public bool isOpen;

        
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        
        public void OpenChest() {
            _animator.SetBool("IsOpened", true);
            isOpen = true;
        }

        
        // destroy the gameObject along with the xLight and xDark images
        // called at the end of Open chest animation
        public void CleanUp() {
            // destroy notification object
            Destroy(notificationObj);
        }
    }
}