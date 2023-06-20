using UnityEngine;

namespace Props {
    public class WheelbarrowScript : MonoBehaviour {
        private Rigidbody2D _rb;

        [Header("References")]
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private float speedThreshold;

        [Header("For Debugging purposes")]
        [SerializeField] private float speed;
        [SerializeField] private float direction;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            SetSpeedAndDirection();

            foreach (var wheel in wheels) {
                wheel.transform.Rotate(0, 0, speed * direction * Time.deltaTime);
            }
        }

        private void FixedUpdate() {
            SetMaxVelocity();
        }

        private void SetMaxVelocity() {
            var parsedX = Mathf.Clamp(_rb.velocity.x, -speedThreshold, speedThreshold);
            _rb.velocity = new Vector2(parsedX, _rb.velocity.y);
        }

        private void SetSpeedAndDirection() {
            var localVelocity = transform.InverseTransformDirection(_rb.velocity);
            speed = localVelocity.magnitude * 100;

            // rotation is done on the opposite side of the direction
            direction = -Mathf.Sign(localVelocity.x);
        }
    }
}