using System.Collections;
using Knight;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Crate {
    public class CrateLabel : MonoBehaviour {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;
        [SerializeField] private SpriteRenderer[] listedSrs;

        [Header("Debug")]
        [SerializeField] private CrateScript crateScript;
        [SerializeField] private bool isWrapperObjDisplayed;
        [SerializeField] private bool isSpriteEnableOn;

        private PlayerScript _playerScript;
        private BoxCollider2D _box;


        private void Awake() {
            _box = GetComponent<BoxCollider2D>();
            listedSrs = new[] { wrapper, lightImg, darkImg };
            isSpriteEnableOn = false;
        }


        private void Start() {
            crateScript = transform.parent.GetComponent<CrateScript>();
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

            LabelUtils.SetSprites(listedSrs, false);
        }


        private void Update() {
            ToggleCollider();
        }


        private IEnumerator ToggleBtnImgCoroutine() {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            isSpriteEnableOn = true;

            LabelUtils.SetSprites(listedSrs, value: true);
            StartCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;
            isSpriteEnableOn = false;

            LabelUtils.SetSprites(listedSrs, value: false);
            StopCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        //TODO: issue here with label
        private void ToggleCollider() {
            isWrapperObjDisplayed = false;
            
            // display label if the crate is at the top of the stack and the player is not carrying any other crate
            if (!_playerScript.CheckPlayerCarry()) {
                if (!crateScript.aboveCrate) {
                    isWrapperObjDisplayed = true;
                }
            }
            // display label if the target crate is being carried
            else if (crateScript.isCarried) {
                isWrapperObjDisplayed = true;
            }

            wrapper.gameObject.SetActive(isWrapperObjDisplayed);
            _box.enabled = isWrapperObjDisplayed;
        }
    }
}