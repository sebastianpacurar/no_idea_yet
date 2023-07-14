using Knight;
using UnityEngine;

namespace Prop.Interactables.Crate {
    public class CrateLabelScript : MonoBehaviour {
        [SerializeField] private GameObject eLabel;

        [Header("Debug")]
        [SerializeField] private CrateScript crateAbove;

        private CrateScript _crateScript;
        private PlayerScript _playerScript;


        private void Awake() {
            _crateScript = GetComponent<CrateScript>();
        }


        private void Start() {
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }


        private void Update() {
            HandleLabelDisplay();
        }

        
        private void OnCollisionStay2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Crate")) return;

            if (CollisionData.IsCollisionTop(other)) {
                crateAbove = other.gameObject.GetComponent<CrateScript>();
            }
        }

        
        private void OnCollisionExit2D(Collision2D other) {
            if (!other.gameObject.CompareTag("Crate")) return;

            if (crateAbove) {
                var otherCrate = other.gameObject;

                if (otherCrate.Equals(crateAbove.gameObject)) {
                    crateAbove = null;
                }
            }
        }


        private void HandleLabelDisplay() {
            var isDisplayed = false;

            // display label if the crate is at the top of the stack and the player is not carrying any other crate
            if (!_playerScript.isCarryingCrate) {
                if (!crateAbove) {
                    isDisplayed = true;
                }
            }
            // display label if the target crate is being carried
            else if (_crateScript.isBeingCarried) {
                isDisplayed = true;
            }

            eLabel.SetActive(isDisplayed);
        }
    }
}