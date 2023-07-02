using UnityEngine;

namespace Prop.Interactables.Crate {
    public class CrateScript : MonoBehaviour {
        [Header("Player Detection")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;

        private CrateRayCasts _ray;
        private Rigidbody2D _rb;
        private float _initialMass;

        public bool isOnCart;

        
        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _ray = GetComponent<CrateRayCasts>();
            _initialMass = 4;
        }

        
        private void Update() {
            // if player is in range, horizontally, then cause crate to have low friction, else place 20f friction
            SetPhysicsMaterial(IsPlayerDetected() ? lowFriction : highFriction);
        }

        
        private void FixedUpdate() {
            SetGravityWhenFalling();
            SetStackedCrateMass();
        }

        
        // Set the PhysicsMaterial2D with the provided version
        private void SetPhysicsMaterial(PhysicsMaterial2D material) {
            _rb.sharedMaterial = material;
        }

        
        private bool IsPlayerDetected() {
            var isLeftInRange = _ray.HitPlayerLeft.collider;
            var isRightInRange = _ray.HitPlayerRight.collider;

            return isLeftInRange || isRightInRange;
        }

        
        // initial mass unit (for the crate which touches the ground layer) is 3f
        // decrease every vertical stacked unit with 0.25f from initial position
        private void SetStackedCrateMass() {
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

        
        // cause the crate to drop quickly, until it collides with the first bottom object
        private void SetGravityWhenFalling() {
            _rb.gravityScale = _rb.velocity.y < -0.1f ? 10 : 1;
        }

        
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name.Equals("Cart")) {
                isOnCart = true;
            }
        }

        
        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.name.Equals("Cart")) {
                isOnCart = false;
            }
        }
    }
}