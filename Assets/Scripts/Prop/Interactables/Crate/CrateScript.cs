using Prop.Interactables.Cart;
using UnityEngine;


namespace Prop.Interactables.Crate {
    public class CrateScript : MonoBehaviour {
        [Header("Player Interaction")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;
        [SerializeField] private PhysicsMaterial2D groundFriction;

        [Header("Physics")]
        [SerializeField] private float defaultMass;
        [SerializeField] private float defaultGravity;
        [SerializeField] private float freeFallGravity;
        [SerializeField] private float carryThrowGravity;

        [Header("Debug")]
        [SerializeField] private PhysicsMaterial2D selectedPm2D;
        [SerializeField] private CartScript cartScript;

        // south, west or east contact. used to check if isBeingThrown can be set to false
        [SerializeField] private CrateScript nearbyCrate;
        public bool isBeingCarried;
        public bool isBeingThrown;
        public bool isGrounded;

        private CrateRayCasts _ray;
        private Rigidbody2D _rb;


        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _ray = GetComponent<CrateRayCasts>();
        }


        private void Update() {
            HandleThrow();

            // if player is in range, horizontally, then cause crate to have low friction, else place 20f friction
            SetPhysicsMaterial();
            selectedPm2D = _rb.sharedMaterial;
        }


        private void FixedUpdate() {
            SetStackedCrateMass();
            SetGravity();
        }

        // is crate got thrown, and reaches ground, cart or nearbyCrate, set to false
        private void HandleThrow() {
            if (isBeingThrown && (isGrounded || cartScript || nearbyCrate)) {
                isBeingThrown = false;
            }
        }


        // Set the PhysicsMaterial2D to provide friction or no friction
        private void SetPhysicsMaterial() {
            // start with high friction
            var frictionMat = highFriction;

            // if grounded, prevent player from pushing at same speed as cart
            if (isGrounded) {
                frictionMat = groundFriction;
            }

            // if crate is not being carried and not in air from being thrown
            else if (!isBeingCarried && !isBeingThrown) {
                if (IsPlayerDetected()) {
                    // set to lowFriction if player detected
                    frictionMat = lowFriction;

                    // if the crate is on a cart 
                    if (cartScript) {
                        // NOTE: this allows for crates to be pushed first if the crate is not entirely on the cart (stands in the way of player collider and cart collider)
                        //   if player is pushing the cart from left or right set high friction to crate, else set low friction to crate.
                        frictionMat = cartScript.isPlayerCollision ? highFriction : lowFriction;
                    }
                }
            }

            _rb.sharedMaterial = frictionMat;
        }


        private bool IsPlayerDetected() {
            var isLeftInRange = _ray.HitPlayerLeft.collider;
            var isRightInRange = _ray.HitPlayerRight.collider;

            return isLeftInRange || isRightInRange;
        }


        // decrease every vertical stacked unit with 0.25f from initial position
        private void SetStackedCrateMass() {
            // if carried or thrown set initial mass to initial mass
            if (isBeingCarried || isBeingThrown) {
                _rb.mass = defaultMass;
            }

            // if crate is in stack (either grounded or on cart, or on another crate)
            else {
                var bottomRayHit = _ray.HitGroundBottom;

                if (bottomRayHit.collider) {
                    var hitPos = bottomRayHit.point;

                    // calculate the distance between the own position and the position of the raycast hit point
                    _rb.mass = Vector2.Distance(transform.position, hitPos) switch {
                        > 1 and <= 2 => defaultMass - 0.25f, // if 2nd level from ground
                        > 2 and <= 3 => defaultMass - 0.5f, // if 3rd level from ground
                        > 3 => 1f, // if 4th level and above from ground
                        _ => defaultMass // if 1st level from ground (the lobby area)
                    };
                }
            }
        }

        private void SetGravity() {
            // when crate is falling 
            if (_rb.velocity.y < -1f && !isBeingThrown && !isBeingCarried) {
                _rb.gravityScale = freeFallGravity;
            } else if (isBeingCarried || isBeingThrown) {
                _rb.gravityScale = carryThrowGravity;
            } else {
                _rb.gravityScale = defaultGravity;
            }
        }


        private void OnCollisionStay2D(Collision2D other) {
            if (other.gameObject.CompareTag("Cart")) {
                if (CollisionData.IsCollisionBottom(other)) {
                    cartScript = other.gameObject.GetComponent<CartScript>();
                }
            }

            if (other.gameObject.CompareTag("Crate")) {
                if (CollisionData.IsCollisionBottom(other) || CollisionData.IsCollisionSideways(other)) {
                    nearbyCrate = other.gameObject.GetComponent<CrateScript>();
                }
            }

            if (other.gameObject.CompareTag("Terrain")) {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("Cart")) {
                cartScript = null;
            }

            if (other.gameObject.CompareTag("Terrain")) {
                isGrounded = false;
            }

            if (other.gameObject.CompareTag("Crate") && nearbyCrate) {
                var otherCrate = other.gameObject;

                if (otherCrate.Equals(nearbyCrate.gameObject)) {
                    nearbyCrate = null;
                }
            }
        }
    }
}