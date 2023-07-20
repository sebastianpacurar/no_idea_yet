using System.Collections;
using Knight;
using UnityEngine;
using UnityEngine.Android;
using Utils;

namespace Prop.Interactables.Crate {
    public class CrateLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;

        [Header("Debug")]
        [SerializeField] private CrateScript crateScript;

        private PlayerScript _playerScript;
        private BoxCollider2D _box;


        private void Awake() {
            _box = GetComponent<BoxCollider2D>();
        }


        private void Start() {
            crateScript = transform.parent.GetComponent<CrateScript>();
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

            LabelUtils.SetSprites(gameObject, false);
        }


        private void Update() {
            HandleLabelDisplay();
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;

            LabelUtils.SetSprites(gameObject, value: true);
            StartCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;

            LabelUtils.SetSprites(gameObject, value: false);
            StopCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        //TODO: issue here with label
        private void HandleLabelDisplay() {
            var isDisplayed = false;

            //TODO: Weird issues happening here

            // display label if the crate is at the top of the stack and the player is not carrying any other crate
            if (!_playerScript.CheckIfCarryingCrate()) {
                if (!crateScript.crateAbove) {
                    isDisplayed = true;
                }
            }
            // display label if the target crate is being carried
            else if (crateScript.isBeingCarried) {
                isDisplayed = true;
            }

            wrapper.gameObject.SetActive(isDisplayed);
            _box.enabled = isDisplayed;
        }
    }
}