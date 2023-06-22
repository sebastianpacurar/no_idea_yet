using UnityEngine;

namespace Props {
    public class WheelbarrowScript : MonoBehaviour {
        private Rigidbody2D _rb;

        [Header("References")]
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private float speedThreshold;
        [SerializeField] private float rotationFactor;

        [Header("For Debugging purposes")]
        [SerializeField] private float speed;
        [SerializeField] private float direction;
        [SerializeField] private Vector2 localVelocity;

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
            // transform velocity from WorldSpace to LocalSpace
            localVelocity = transform.InverseTransformDirection(_rb.velocity);

            // get the length (speed) of the localVelocity (multiplied by rotFactor to get desired effect)
            speed = localVelocity.magnitude * rotationFactor;

            // rotation is done on the opposite side of the direction
            direction = -Mathf.Sign(localVelocity.x);
        }

        public bool IsMoving() {
            return speed > 1f;
        }
    }
}