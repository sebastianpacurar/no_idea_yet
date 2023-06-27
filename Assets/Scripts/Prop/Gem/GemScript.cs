using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gem {
    public class GemScript : MonoBehaviour {
        [SerializeField] private float ttlInSeconds;
        [SerializeField] private float upSpeed;
        private float _targetTime;
        private float _counter;
        private Vector3 _initialPos;
        private Animator _animator;
        private Light2D _light2D;

        private void Awake() {
            _animator = GetComponent<Animator>();
            _light2D = GetComponentInChildren<Light2D>();
        }

        private void Start() {
            _counter = 0f;
            _initialPos = transform.position;

            // current time + how many seconds to wait
            _targetTime = Time.time + ttlInSeconds;
        }

        private void Update() {
            MoveUpAndDisappear();
        }

        private void MoveUpAndDisappear() {
            // increase counter based on (speed * time per frame
            _counter += upSpeed * Time.deltaTime;

            if (Time.time < _targetTime) {
                // if current time smaller than target time, perform movement
                transform.position = new Vector3(_initialPos.x, _initialPos.y + _counter, _initialPos.z);
            } else {
                // else disable light, and trigger Disappear animation
                _light2D.enabled = false;
                _animator.SetTrigger("Disappear");
            }
        }

        // called at the end of Collected animation clip
        public void AddToCollection() {
            //TODO: here goes the add to player inventory part

            Destroy(gameObject);
        }
    }
}