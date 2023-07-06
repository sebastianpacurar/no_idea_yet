using System.Linq;
using UnityEngine;


namespace Prop.Interactables.Cart {
    public class CartScript : MonoBehaviour {
        private Rigidbody2D _rb;
        private BoxCollider2D _box;

        [Header("References")]
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private float speedThreshold;
        [SerializeField] private float rotationFactor;
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;

        [Header("For Debugging")]
        [SerializeField] private float speed;
        [SerializeField] private float direction;
        [SerializeField] private Vector2 localVelocity;
        public bool isPlayerCollision;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _box = GetComponent<BoxCollider2D>();
        }

        private void Update() {
            SetSpeedAndDirection();

            // set the friction to low if player collision
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
                var average = CalculateAverageContactNormal(other);

                // if player is on the left or right of the cart, then return true
                if (IsCollisionSideways(average)) {
                    isPlayerCollision = true;
                }
            }
        }


        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                isPlayerCollision = false;
            }
        }

        // set high mass for cart when not collided
        private void HaltCartWhenNoPlayerCollision() {
            _rb.mass = isPlayerCollision ? 2 : 20;
        }


        // Set the PhysicsMaterial2D with the provided version
        private void SetPhysicsMaterial(PhysicsMaterial2D material) {
            _box.sharedMaterial = material;
        }


        // get the average of Collision ContactPoint2D normal values  
        private Vector2 CalculateAverageContactNormal(Collision2D collision) {
            // get the sum of all contact.normal values from contacts
            var average = collision.contacts.Aggregate(Vector2.zero, (current, contact) => current + contact.normal);

            // divide sum to the length to get the average
            average /= collision.contacts.Length;
            return average.normalized;
        }

        private bool IsCollisionSideways(Vector2 v) {
            var collisionLeft = v.x > 0f;
            var collisionRight = v.x < 0f;

            return collisionLeft || collisionRight;
        }
    }
}