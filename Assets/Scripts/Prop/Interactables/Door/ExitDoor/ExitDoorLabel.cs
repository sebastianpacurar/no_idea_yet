using System.Collections;
using UnityEngine;
using Utils;

namespace Prop.Interactables.Door.ExitDoor
{
    public class ExitDoorLabel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer wrapper;
        [SerializeField] private SpriteRenderer lightImg;
        [SerializeField] private SpriteRenderer darkImg;

        [SerializeField] private SpriteRenderer[] listedSrs;


        private ExitDoorScript _exitDoor;
        private BoxCollider2D _box;


        private void Awake()
        {
            _box = GetComponent<BoxCollider2D>();
            listedSrs = new[] { wrapper, lightImg, darkImg };
        }


        private void Start()
        {
            _exitDoor = transform.parent.gameObject.GetComponent<ExitDoorScript>();
            LabelUtils.SetSprites(listedSrs, false);
        }

        private void Update()
        {
            ToggleBoxCollider();
        }


        private void ToggleBoxCollider()
        {
            // if target reached and box is disabled, then enable box
            if (_exitDoor.IsTargetReached() && !_box.enabled)
            {
                _box.enabled = true;
            }
            // if target not reached and box is enabled, then disable box
            else if (!_exitDoor.IsTargetReached() && _box.enabled)
            {
                _box.enabled = false;
            }
        }


        private IEnumerator ToggleBtnImgCoroutine()
        {
            yield return LabelUtils.ToggleBtnImg(lightImg, darkImg);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!_exitDoor.IsTargetReached()) return;

            // display label when 
            LabelUtils.SetSprites(listedSrs, value: true);
            StartCoroutine(nameof(ToggleBtnImgCoroutine));
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!wrapper.enabled) return;

            // hide label when wrapper is enabled
            LabelUtils.SetSprites(listedSrs, value: false);
            StopCoroutine(nameof(ToggleBtnImgCoroutine));
        }
    }
}