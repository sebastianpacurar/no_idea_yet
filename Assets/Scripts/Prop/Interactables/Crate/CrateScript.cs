using Prop.Interactables.Cart;
using ScriptableObjects;
using UnityEngine;
using Utils;


namespace Prop.Interactables.Crate {
    public class CrateScript : MonoBehaviour {
        [SerializeField] private CratePhysicsDataSo data;

        [Header("Debug")]
        [SerializeField] private PhysicsMaterial2D selectedPm2D;
        [SerializeField] private CartScript cartScript;

        // south, west or east contact. used to check if isBeingThrown can be set to false
        public CrateScript aboveCrate;
        [SerializeField] private CrateScript nearbyCrate;
        [SerializeField] private CrateScript bottomCrate;
        public bool isCarried;
        public bool isBeingThrown;
        public bool isGrounded;

        private CrateRayCasts _ray;
        private Rigidbody2D _rb;
        private BoxCollider2D _box;

        private Vector2 _crateSize;


        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _box = GetComponent<BoxCollider2D>();
            _ray = GetComponent<CrateRayCasts>();
        }

        private void Start() {
            SetCrateSize();
        }


        private void Update() {
            HandleThrow();

            // if player is in range, horizontally, then cause crate to have low friction, else place 20f friction
            SetPhysicsMaterial();
            selectedPm2D = _rb.sharedMaterial;

            ValidateAboveCrate();
            ValidateNearbyCrate();
            ValidateBottomCrate();
        }

        private void SetCrateSize() {
            var parentMap = transform.parent.localScale;
            var size = _box.size;
            _crateSize = new Vector2(size.x * parentMap.x, size.y * parentMap.y);
        }


        private void FixedUpdate() {
            SetStackedCrateMass();
            SetGravity();
        }

        // is crate got thrown, and reaches ground, cart or nearbyCrate, set to false
        private void HandleThrow() {
            if (isBeingThrown && (isGrounded || cartScript || nearbyCrate || bottomCrate)) {
                isBeingThrown = false;
            }
        }

        // validate aboveCrate 
        private void ValidateAboveCrate() {
            if (!aboveCrate) return;

            // if current crate is not carried but the above crate is carried, then set above crate to false
            //  reason - crate is being picked up based on position - check SetCrateOnPlayer() in PlayerScript.cs
            if (!isCarried && aboveCrate.isCarried) {
                aboveCrate = null;
            }

            // else apply for regular physics interaction 
            else {
                var abovePos = aboveCrate.transform.position;
                var selfPos = transform.position;

                // axis based distance
                var distX = Mathf.Abs(abovePos.x - selfPos.x);
                var distY = Mathf.Abs(abovePos.y - selfPos.y);

                // if any of the distances is higher than the size + offset, set aboveCrate to null
                if (distX > _crateSize.x + 0.1f || distY > _crateSize.y + 0.1f) {
                    aboveCrate = null;
                }
            }
        }


        // validate bottomCrate
        private void ValidateBottomCrate() {
            if (!bottomCrate) return;

            // if current crate is carried then set bottom crate to false
            if (isCarried) {
                bottomCrate = null;
            }

            // else apply for regular physics interaction 
            else {
                var bottomPos = bottomCrate.transform.position;
                var selfPos = transform.position;

                // axis based distance
                var distX = Mathf.Abs(bottomPos.x - selfPos.x);
                var distY = Mathf.Abs(bottomPos.y - selfPos.y);

                // if any of the distances is higher than the size + offset, set aboveCrate to null
                if (distX > _crateSize.x + 0.1f || distY > _crateSize.y + 0.1f) {
                    bottomCrate = null;
                }
            }
        }


        // validate nearbyCrate
        private void ValidateNearbyCrate() {
            if (!nearbyCrate) return;
            var nearbyPos = nearbyCrate.transform.position;
            var selfPos = transform.position;

            // axis based distance
            var distX = Mathf.Abs(nearbyPos.x - selfPos.x);
            var distY = Mathf.Abs(nearbyPos.y - selfPos.y);

            // if any of the distances is higher than the size + offset, set aboveCrate to null
            if (distX > _crateSize.x + 0.05f || distY > _crateSize.y * 0.75f) {
                nearbyCrate = null;
            }
        }


        // Set the PhysicsMaterial2D to provide friction or no friction
        private void SetPhysicsMaterial() {
            // start with high friction
            var frictionMat = data.HighFriction;

            if (isGrounded && IsPlayerDetected()) {
                frictionMat = data.GroundFriction;
            }

            // if crate is not being carried and not in air from being thrown
            else if (!isCarried && !isBeingThrown) {
                if (IsPlayerDetected()) {
                    // set to lowFriction if player detected
                    frictionMat = data.LowFriction;

                    // if the crate is on a cart 
                    if (cartScript) {
                        // NOTE: this allows for crates to be pushed first if the crate is not entirely on the cart (stands in the way of player collider and cart collider)
                        //   if player is pushing the cart from left or right set high friction to crate, else set low friction to crate.
                        frictionMat = cartScript.isPlayerCollision ? data.HighFriction : data.LowFriction;
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
            if (isCarried || isBeingThrown) {
                _rb.mass = data.DefaultMass;
            }

            // if crate is in stack (either grounded or on cart, or on another crate)
            else {
                var bottomRayHit = _ray.HitGroundBottom;

                if (bottomRayHit.collider) {
                    var hitPos = bottomRayHit.point;

                    // calculate the distance between the own position and the position of the raycast hit point
                    _rb.mass = Vector2.Distance(transform.position, hitPos) switch {
                        > 1 and <= 2 => data.DefaultMass - 0.25f, // if 2nd level from ground
                        > 2 and <= 3 => data.DefaultMass - 0.5f, // if 3rd level from ground
                        > 3 => 1f, // if 4th level and above from ground
                        _ => data.DefaultMass // if 1st level from ground (the lobby area)
                    };
                }
            }
        }

        private void SetGravity() {
            // when crate is falling 
            if (_rb.velocity.y < -1f && !isBeingThrown && !isCarried) {
                _rb.gravityScale = data.FreeFallGravity;
            } else if (isCarried || isBeingThrown) {
                _rb.gravityScale = data.CarryThrowGravity;
            } else {
                _rb.gravityScale = data.DefaultGravity;
            }
        }


        private void OnCollisionStay2D(Collision2D other) {
            if (other.gameObject.CompareTag("Cart")) {
                if (CollisionUtils.IsCollisionBottom(other)) {
                    cartScript = other.gameObject.GetComponent<CartScript>();
                }
            }

            if (other.gameObject.CompareTag("Crate")) {
                if (CollisionUtils.IsCollisionTop(other)) {
                    aboveCrate = other.gameObject.GetComponent<CrateScript>();
                }

                if (CollisionUtils.IsCollisionSideways(other)) {
                    nearbyCrate = other.gameObject.GetComponent<CrateScript>();
                }

                if (CollisionUtils.IsCollisionBottom(other)) {
                    bottomCrate = other.gameObject.GetComponent<CrateScript>();
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
        }
    }
}