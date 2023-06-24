using UnityEngine;

namespace Props {
    public class CrateRayCasts : MonoBehaviour {
        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask groundLayer;

        [Header("Debug")]
        [SerializeField] private float distFromPlayerX; // left and right rays. used only on X axis
        [SerializeField] private float distFromPlayerFactorX;
        [SerializeField] private float distFromGround = 10f;
        
        private BoxCollider2D _boxCollider;

        public RaycastHit2D HitPlayerLeft { get; set; }
        public RaycastHit2D HitPlayerRight { get; set; }
        public RaycastHit2D HitGroundBottom { get; set; }

        private void Awake() {
            _boxCollider = GetComponent<BoxCollider2D>();
        }
        
        private void Update() {
            distFromPlayerX = _boxCollider.size.x * distFromPlayerFactorX;
            CastRays();
        }

        private void CastRays() {
            var cratePos = transform.position;

            HitPlayerLeft = Physics2D.Raycast(cratePos, Vector2.left, distFromPlayerX, playerLayer);
            HitPlayerRight = Physics2D.Raycast(cratePos, Vector2.right, distFromPlayerX, playerLayer);
            HitGroundBottom = Physics2D.Raycast(cratePos, Vector2.down, distFromGround, groundLayer);
        }
        
        // NOTE: For Debugging
        private void OnDrawGizmos() {
            var pos = transform.position;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, new Vector3((pos.x - distFromPlayerX), pos.y, pos.z));

            // right player detection
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, new Vector3((pos.x + distFromPlayerX), pos.y, pos.z));

            // bottom ground detection
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(pos, new Vector3(pos.x, -distFromGround, pos.z));
        }
    }
}