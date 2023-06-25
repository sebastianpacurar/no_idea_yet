using UnityEngine;

namespace Gem {
    public class GemScript : MonoBehaviour {
        [SerializeField] private float timeToLive;
        [SerializeField] private float upSpeed;
        private float _startTime;
        private float _counter;
        private Vector3 _initialPos;
        private Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _counter = 0f;
            _initialPos = transform.position;
            _startTime = Time.time;
        }

        private void Update() {
            MoveUpAndDisappear();
        }

        private void MoveUpAndDisappear() {
            // increase counter based on (speed * time per frame
            _counter += upSpeed * Time.deltaTime;

            // while _counter is smaller than 
            if (Time.time < _startTime + timeToLive) {
                transform.position = new Vector3(_initialPos.x, _initialPos.y + _counter, _initialPos.z);
            } else {
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