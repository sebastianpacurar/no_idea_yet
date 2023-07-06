using Input;
using UnityEngine;


namespace Knight {
    public class PlayerControllerScript : MonoBehaviour {
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpForce;

        public bool isSprinting;
        public bool isGrounded;
        public bool IsPickCratePressed;
        public bool isHit;
        public bool isDead;

        [Space(10)]
        [Header("References")]
        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask pushableLayer;

        private InputManager _input;
        private Rigidbody2D _rb;


        # region unity callback methods
        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }


        private void Start() {
            _input = InputManager.Instance;
        }


        private void Update() {
            CheckIfGrounded();
            CheckIfCanSprint();
        }


        private void FixedUpdate() {
            HandleRunAndSprint();
            HandleJumpFunctionality();
            FlipPlayerScale();
        }
        #endregion


        private void CheckIfGrounded() {
            var pos = groundChecker.position;
            var size = new Vector2(1.1f, 0.2f);
            var direction = CapsuleDirection2D.Horizontal;
            var angle = 0f;

            isGrounded = Physics2D.OverlapCapsule(pos, size, direction, angle, groundLayer | pushableLayer);
        }

        
        // flip towards the facing direction. if left then -1, if right then 1 
        private void FlipPlayerScale() {
            if (_rb.velocity.x < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (_rb.velocity.x > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        
        private void HandleJumpFunctionality() {
            if (_input.IsJumpPressed && isGrounded && !_input.IsAttacking) {
                var jumpVector = new Vector2(0f, jumpForce);
                _rb.AddForce(jumpVector, ForceMode2D.Impulse);
                _input.IsJumpPressed = false;
            }
        }

        
        // HACK: use sprint speed when jumping, even though the input is canceled (sprint button released)
        private void CheckIfCanSprint() {
            var moveVal = _input.MoveVal;
            var sprintPressed = _input.IsSprintPressed;

            if (isGrounded && moveVal != 0f && sprintPressed) {
                // activate sprinting only when grounded, move keys are pressed and sprint is pressed
                _input.IsSprintPressed = true;
            } else if ((isSprinting && isGrounded && !sprintPressed) || moveVal == 0f) {
                // deactivate when reaches land or move keys are not pressed.
                // used to cause the player to have same speed when jumping from sprint
                isSprinting = false;
            }
        }

        
        private void HandleRunAndSprint() {
            var speed = isSprinting ? sprintSpeed : runSpeed;

            _rb.velocity = new Vector2(_input.MoveVal * speed, _rb.velocity.y);
        }
    }
}