using UnityEngine;

namespace Props {
    public class CrateScript : MonoBehaviour {
        // [Space(10)]
        // [Header("Target Collider")]
        // // the object on which the crate sits (it can be ground, crate or wheelbarrow
        // [SerializeField] private GameObject targetObj;

        [Header("Player Detection")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float rayDistance;
        [SerializeField] private float rayDistanceFactor;
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;

        private Rigidbody2D _rb;
        private BoxCollider2D _boxCollider;
        private RaycastHit2D _hitLeft, _hitRight, _hitBottom;
        private float _initialMass;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _initialMass = 4;
        }

        private void Start() {
            rayDistance = _boxCollider.size.x * rayDistanceFactor;
        }

        private void Update() {
            // if player is in range, horizontally, then cause crate to have no friction, else place 50f friction
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
            var cratePos = transform.position;
            _hitLeft = Physics2D.Raycast(cratePos, Vector2.left, rayDistance, playerLayer);
            _hitRight = Physics2D.Raycast(cratePos, Vector2.right, rayDistance, playerLayer);

            return _hitLeft.collider || _hitRight.collider;
        }

        // initial mass unit (for the crate which touches the ground layer) is 3f
        // decrease every vertical stacked unit with 0.25f from initial position
        private void SetStackedCrateMass() {
            _hitBottom = Physics2D.Raycast(transform.position, Vector2.down, 10f, groundLayer);

            if (_hitBottom.collider) {
                var hitPos = _hitBottom.point;

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


        // NOTE: For Debugging
        private void OnDrawGizmos() {
            var pos = transform.position;

            // left player detection
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, new Vector3((pos.x - rayDistance), pos.y, pos.z));

            // right player detection
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, new Vector3((pos.x + rayDistance), pos.y, pos.z));

            // bottom ground detection
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(pos, new Vector3(pos.x, -10f, pos.z));
        }
    }
}