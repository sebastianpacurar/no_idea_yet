using UnityEngine;

namespace Props {
    
    //TODO: do not use anywhere!
    public class ExperimentalPhysics : MonoBehaviour {
        [SerializeField] private bool isCollision;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float rayDistance;

        private Rigidbody2D _rb;
        private BoxCollider2D _box;
        private RaycastHit2D _hitLeft;
        private RaycastHit2D _hitRight;
        private ExperimentalPhysics parentCrate;
        private Vector3 initialLocalPosition;
        private Vector3 offset;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _box = GetComponent<BoxCollider2D>();
        }

        // private void Start() {
        //     rayDistance = _box.size.x / 2 + 0.1f;
        //     initialLocalPosition = transform.localPosition;
        // }
        
        private void Start()
        {
            // Store the initial local position of the crate
            initialLocalPosition = transform.localPosition;
        }

        public void SetOffset(Vector3 newOffset)
        {
            // Set the offset based on the provided newOffset
            offset = newOffset;
        }

        public void UpdatePosition()
        {
            // Calculate the new local position based on the initial local position and the offset
            Vector3 newPosition = initialLocalPosition + offset;

            // Update the local position of the crate
            transform.localPosition = newPosition;
        }

        private void OnCollisionStay2D(Collision2D collision) {
            var upwardDirection = Vector2.up;

            if (collision.gameObject.CompareTag("Pushable")) {
                foreach (var contact in collision.contacts) {
                    var colOrientation = contact.normal;
                    var obj = contact.collider.gameObject;

                    if (Vector2.Dot(colOrientation, upwardDirection) >= 0.5f) {
                        var crateScript = obj.GetComponent<ExperimentalPhysics>();
                        if (crateScript != null) crateScript.SetParentCrate(this);
                        transform.SetParent(obj.transform);
                        return;
                    }
                }
            }

            if (collision.gameObject.CompareTag("Terrain")) {
                foreach (var contact in collision.contacts) {
                    var colOrientation = contact.normal;

                    if (Vector2.Dot(colOrientation, upwardDirection) >= 0.5f) {
                        if (parentCrate != null) parentCrate.ClearParentCrate();
                        transform.SetParent(null);
                        return;
                    }
                }
            }
        }

        private void Update() {
            CastRays();

            if (parentCrate != null) {
                // Move the entire crate stack relative to the parent crate
                transform.localPosition = parentCrate.transform.localPosition + initialLocalPosition;
                Debug.Log($"Crate {name} position = {transform.position}; Parent Crate = {parentCrate.name} position = {parentCrate.transform.position}");
            }
        }

        private void FixedUpdate() {
            if (_rb.velocity.y < -0.1f) {
                _rb.gravityScale = 10;
            } else {
                _rb.gravityScale = 1;
            }
        }

        private void CastRays() {
            var cratePos = transform.position;
            _hitLeft = Physics2D.Raycast(cratePos, Vector2.left, rayDistance, playerLayer);
            _hitRight = Physics2D.Raycast(cratePos, Vector2.right, rayDistance, playerLayer);

            if (_hitLeft) {
                Debug.Log("Left Hit");
            } else if (_hitRight) {
                Debug.Log("Right Hit");
            }
        }

        private void OnDrawGizmos() {
            var pos = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, new Vector3((pos.x - rayDistance), pos.y, pos.z));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, new Vector3((pos.x + rayDistance), pos.y, pos.z));
        }

        public void SetParentCrate(ExperimentalPhysics crate) {
            parentCrate = crate;
        }

        public void ClearParentCrate() {
            parentCrate = null;
        }
    }
}