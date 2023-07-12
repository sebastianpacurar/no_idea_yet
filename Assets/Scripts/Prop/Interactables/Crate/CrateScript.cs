using Prop.Interactables.Cart;
using UnityEngine;

namespace Prop.Interactables.Crate {
    public class CrateScript : MonoBehaviour {
        [Header("Current PhysicsMaterial2D")]
        [SerializeField] private PhysicsMaterial2D selectedPm2D;

        [Header("Cart Interaction")]
        [SerializeField] private CartScript cartScript;

        [Header("Player Interaction")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;

        [Header("Debug")]
        public bool isBeingCarried;
        public bool isBeingThrown;

        private CrateRayCasts _ray;
        private Rigidbody2D _rb;
        private float _initialMass;


        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _ray = GetComponent<CrateRayCasts>();
            _initialMass = 4;
        }


        private void Update() {
            HandleThrow();

            // if player is in range, horizontally, then cause crate to have low friction, else place 20f friction
            SetPhysicsMaterial();
            selectedPm2D = _rb.sharedMaterial;
        }


        private void FixedUpdate() {
            SetStackedCrateMass();
        }

        // if throw in progress and crate touches any other colliders (ground, pushable), set isBeingThrown to false
        private void HandleThrow() {
            if (isBeingThrown && _rb.IsTouchingLayers()) {
                isBeingThrown = false;
            }
        }


        // Set the PhysicsMaterial2D to provide friction or no friction
        private void SetPhysicsMaterial() {
            // start with high friction
            var frictionMat = highFriction;

            if (!isBeingCarried && !isBeingThrown) {
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

                _rb.sharedMaterial = frictionMat;
            }
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
                _rb.mass = _initialMass;
            } else {
                var bottomRayHit = _ray.HitGroundBottom;

                if (bottomRayHit.collider) {
                    var hitPos = bottomRayHit.point;

                    // calculate the distance between the own position and the position of the raycast hit point
                    _rb.mass = Vector2.Distance(transform.position, hitPos) switch {
                        > 1 and <= 2 => _initialMass - 0.25f, // if 2nd level from ground
                        > 2 and <= 3 => _initialMass - 0.5f, // if 3rd level from ground
                        > 3 => 1f, // if 4th level and above from ground
                        _ => _initialMass // if 1st level from ground (the lobby area)
                    };
                }
            }
        }


        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Cart")) {
                if (CollisionData.IsCollisionBottom(other)) {
                    cartScript = other.gameObject.GetComponent<CartScript>();
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("Cart")) {
                cartScript = null;
            }
        }
    }
}