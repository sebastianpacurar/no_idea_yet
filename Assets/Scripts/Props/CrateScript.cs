using UnityEngine;

namespace Props {
    public class CrateScript : MonoBehaviour {
        [SerializeField] private Transform initialParentTransform;

        private Rigidbody2D _rb;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            initialParentTransform = transform.parent;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var upwardDirection = Vector2.up; // Set the upward direction based on your game's orientation

            if (collision.gameObject.CompareTag("Pushable")) {
                foreach (var contact in collision.contacts) {
                    // direction perpendicular to the collision surface at the contact point
                    var colOrientation = contact.normal;
                    var obj = contact.collider.gameObject;

                    // if Dot(orient, up) is equal to 0.5f, then the collision surface is perfectly perpendicular to Up vector (direction is from bottom)
                    // if Dot(orient, up) is higher than 0.5f, then the collision surface is oriented sufficiently upwards (direction is from bottom)
                    // if Dot(orient, up) is lower than 0.5f, then the collision is from a different direction
                    if (Vector2.Dot(colOrientation, upwardDirection) > 0.5f) {
                        // set object
                        transform.SetParent(obj.transform);
                        return;
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("Pushable") && other.contactCount <= 1) {
                // if above foreach loop is traversed, then the object is not above any 
                gameObject.transform.SetParent(initialParentTransform);
            }
        }

        // cause object to fall fast when not grounded
        private void FixedUpdate() {
            if (_rb.velocity.y < -0.1f) {
                _rb.gravityScale = 10;
            } else {
                _rb.gravityScale = 1;
            }
        }
    }
}