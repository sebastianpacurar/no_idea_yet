using System.Collections;
using UnityEngine;

namespace Chest {
    public class ChestControllerScript : MonoBehaviour {
        [SerializeField] private GameObject notificationObj;
        [SerializeField] private GameObject overlappingImg;
        private Animator _animator;
        public bool isOpen;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            notificationObj.SetActive(false);
        }

        private IEnumerator ToggleImgVisibility() {
            while (true) {
                yield return new WaitForSeconds(0.5f);
                var currentActiveStatus = overlappingImg.activeInHierarchy;
                overlappingImg.SetActive(!currentActiveStatus);
            }
        }

        #region collider logic
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                HandleNotificationReveal();
            }
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                HandleNotificationHide();
            }
        }
        #endregion


        private void HandleNotificationReveal() {
            notificationObj.SetActive(true);
            StartCoroutine(nameof(ToggleImgVisibility));
        }

        private void HandleNotificationHide() {
            if (isOpen) {
                CloseChest();
            }

            notificationObj.SetActive(false);
            StopCoroutine(nameof(ToggleImgVisibility));
        }

        public void OpenChest() {
            _animator.SetBool("IsOpened", true);
            isOpen = true;
        }

        public void CloseChest() {
            _animator.SetBool("IsOpened", false);
            isOpen = false;
        }
    }
}