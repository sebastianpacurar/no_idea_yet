using UnityEngine;

namespace Props {
    public class CartScript : MonoBehaviour {
        private Rigidbody2D _rb;
        private BoxCollider2D _box;

        [Header("References")]
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private float speedThreshold;
        [SerializeField] private float rotationFactor;
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;

        [Header("For Debugging purposes")]
        [SerializeField] private float speed;
        [SerializeField] private float direction;
        [SerializeField] private Vector2 localVelocity;
        [SerializeField] private bool isPlayerCollision;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _box = GetComponent<BoxCollider2D>();
        }

        private void Update() {
            SetSpeedAndDirection();

            // in Update() because changing the friction of box not rigidbody
            SetPhysicsMaterial(isPlayerCollision ? lowFriction : highFriction);

            // rotate wheels
            foreach (var wheel in wheels) {
                wheel.transform.Rotate(0, 0, speed * direction * Time.deltaTime);
            }
        }

        private void FixedUpdate() {
            SetMaxVelocity();
            HaltCartWhenNoPlayerCollision();
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

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                isPlayerCollision = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                isPlayerCollision = false;
            }
        }

        private void HaltCartWhenNoPlayerCollision() {
            _rb.mass = isPlayerCollision ? 2 : 20;
        }

        // Set the PhysicsMaterial2D with the provided version
        private void SetPhysicsMaterial(PhysicsMaterial2D material) {
            _box.sharedMaterial = material;
        }
    }
}