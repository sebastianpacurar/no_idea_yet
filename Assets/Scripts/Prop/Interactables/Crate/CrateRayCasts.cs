using UnityEngine;


namespace Prop.Interactables.Crate
{
    public class CrateRayCasts : MonoBehaviour
    {
        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask groundLayer;

        [Header("Player Detection")]
        [SerializeField] private float distFromPlayerFactorX;

        [Header("Debug")]
        public float distFromPlayerX; // left and right rays. used only on X axis
        public float distFromGround = 10f;

        private Transform _playerTransform;
        private BoxCollider2D _boxCollider;

        public RaycastHit2D HitPlayerLeft { get; private set; }
        public RaycastHit2D HitPlayerRight { get; private set; }


        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }


        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }


        private void Update()
        {
            CastRays();
        }

        // get a dynamic origin Y based on the crate sprite's size
        public Vector2 GetRayOriginVector()
        {
            var cratePos = transform.position;
            var playerPos = _playerTransform.position;
            var maxPointY = _boxCollider.bounds.extents.y;
            var offset = 0.1f;


            // set origin Y dynamic to move after the player until it reaches the top or bottom of the box +/- 0.1f 
            var originY = Mathf.Clamp(playerPos.y, (cratePos.y - maxPointY) + offset, (cratePos.y + maxPointY) - offset);


            // keep origin X fixed
            var originX = cratePos.x;

            return new Vector2(originX, originY);
        }


        private void CastRays()
        {
            var origin = GetRayOriginVector();

            distFromPlayerX = _boxCollider.size.x * distFromPlayerFactorX;

            HitPlayerLeft = Physics2D.Raycast(origin, Vector2.left, distFromPlayerX, playerLayer);
            HitPlayerRight = Physics2D.Raycast(origin, Vector2.right, distFromPlayerX, playerLayer);
        }
    }
}