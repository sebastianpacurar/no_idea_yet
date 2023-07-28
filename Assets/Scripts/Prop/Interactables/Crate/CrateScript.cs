using System.Collections.Generic;
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
        public CrateScript bottomCrate;
        [SerializeField] private List<CrateScript> sidewaysCrates;
        public bool isCarried;
        public bool isBeingThrown;
        public bool isGrounded;

        private CrateRayCasts _ray;
        private Rigidbody2D _rb;
        private BoxCollider2D _box;
        private FixedJoint2D _fj;
        public Vector2 crateSize;


        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _box = GetComponent<BoxCollider2D>();
            _ray = GetComponent<CrateRayCasts>();
            _fj = GetComponent<FixedJoint2D>();

            sidewaysCrates = new List<CrateScript>();
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
            ValidateSidewaysCrates();
            ValidateBottomCrate();
        }


        public bool HasBottomCrate() {
            return bottomCrate;
        }


        private void SetCrateSize() {
            var parentMap = transform.parent.localScale;
            var size = _box.size;
            crateSize = new Vector2(size.x * parentMap.x, size.y * parentMap.y);
        }


        private void FixedUpdate() {
            SetStackedCrateMass();
            SetGravity();
        }


        public bool IsSmallCrate => data.IsSmallCrate;
        public float AttachPosOffset => data.PosOffset;
        public Vector2 AimRangeValue => data.MinMaxAimRange;


        // is crate got thrown, and reaches ground, cart or sidewaysCrates, set to false
        private void HandleThrow() {
            if (isBeingThrown && (isGrounded || cartScript || sidewaysCrates.Count > 0 || bottomCrate)) {
                isBeingThrown = false;
            }
        }

        // validate aboveCrate 
        private void ValidateAboveCrate() {
            if (!aboveCrate) return;

            // if current crate is not carried but the above crate is carried, then set above crate to false
            //  reason - crate is being picked up based on position - check PlaceCrateOnPlayer() in PlayerScript.cs
            if (!isCarried && aboveCrate.isCarried) {
                aboveCrate = null;
            }

            // else apply for regular physics interaction 
            else {
                var dist = GetDistFromCrate(aboveCrate.transform.position);
                var threshold = GetParsedSize(aboveCrate);

                // if any of the distances is higher than the size + offset, set aboveCrate to null
                if (dist.x > threshold.x + 0.1f || dist.y > threshold.y + 0.1f) {
                    aboveCrate = null;
                }
            }
        }


        // validate bottomCrate
        private void ValidateBottomCrate() {
            if (!bottomCrate) return;

            // if current crate is carried and not attached to player then set bottom crate to false
            if (isCarried && !_fj.enabled) {
                bottomCrate = null;
            }

            // else apply for regular physics interaction 
            else {
                var dist = GetDistFromCrate(bottomCrate.transform.position);
                var threshold = GetParsedSize(bottomCrate);

                // if any of the distances is higher than the size + offset, set aboveCrate to null
                if (dist.x > threshold.x + 0.1f || dist.y > threshold.y + 0.1f) {
                    bottomCrate = null;
                }
            }
        }


        // validate sidewaysCrates
        private void ValidateSidewaysCrates() {
            if (sidewaysCrates.Count == 0) return;

            var cratesToRemove = new List<CrateScript>();

            // iterate over sidewaysCrates and add the crates needed to remove to cratesToRemove list
            foreach (var crate in sidewaysCrates) {
                var dist = GetDistFromCrate(crate.transform.position);
                var threshold = GetParsedSize(crate);

                // if any of the distances is higher than the size + offset, add crate to the removal list
                if (dist.x > threshold.x + 0.05f || dist.y > threshold.y * 0.9f) {
                    cratesToRemove.Add(crate);
                }
            }

            // remove all cratesToRemove present in sidewaysCrates
            foreach (var crate in cratesToRemove) {
                sidewaysCrates.Remove(crate);
            }
        }


        // return the distX and distY based on subtraction
        private Vector2 GetDistFromCrate(Vector2 targetCratePos) {
            var selfPos = transform.position;
            var distX = Mathf.Abs(targetCratePos.x - selfPos.x);
            var distY = Mathf.Abs(targetCratePos.y - selfPos.y);

            return new Vector2(distX, distY);
        }


        // similar to adding the bounds.extent of the sprites, but based on crate size
        private Vector2 GetParsedSize(CrateScript targetCrate) {
            var thresholdX = targetCrate.crateSize.x / 2 + crateSize.x / 2;
            var thresholdY = targetCrate.crateSize.y / 2 + crateSize.y / 2;

            return new Vector2(thresholdX, thresholdY);
        }


        // Set the PhysicsMaterial2D to provide friction or no friction
        private void SetPhysicsMaterial() {
            // start with high friction
            var frictionMat = data.HighFriction;

            if (isCarried) {
                frictionMat = data.NoFriction;
            }

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
                _rb.mass = data.CarryThrowMass;
            } else if (bottomCrate) {
                _rb.mass = data.StackMass;
            } else {
                _rb.mass = data.DefaultMass;
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
                    var nearbyCrate = other.gameObject.GetComponent<CrateScript>();

                    if (!sidewaysCrates.Contains(nearbyCrate)) {
                        sidewaysCrates.Add(nearbyCrate);
                    }
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