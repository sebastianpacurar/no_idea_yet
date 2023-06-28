using UnityEngine;

namespace NPC {
    public class NpcControllerScript : MonoBehaviour {
        [SerializeField] private float walkSpeed;
        public bool isMoving;

        private Rigidbody2D _rb;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            CheckIfMoving();
        }

        private void CheckIfMoving() {
            isMoving = _rb.velocity.x is > 0f or < 0f;
        }

        private void HandleWalkIdleCycle() { }
    }
}