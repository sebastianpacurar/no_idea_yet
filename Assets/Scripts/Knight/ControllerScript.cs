using UnityEngine;
using UnityEngine.InputSystem;


namespace Knight {
    public class ControllerScript : MonoBehaviour {
        [SerializeField] private float runSpeed;
        [SerializeField] private float jumpForce;
        public float moveInputVal;
        public bool isGrounded;
        public bool isJumpPressed;

        [Space(10)]
        [Header("References")]
        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;

        private InputControls _controls;
        private Rigidbody2D _rb;

        #region input actions
        private InputAction _runAction;
        private InputAction _jumpAction;
        #endregion


        private void Awake() {
            InitializeInputControls();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            // update the isGrounded variable
            CheckIfGrounded();
        }

        private void FixedUpdate() {
            var x = moveInputVal;

            // change the object based on its velocity
            _rb.velocity = new Vector2(x * runSpeed, _rb.velocity.y);

            if (isJumpPressed && isGrounded) {
                var jumpVector = new Vector2(0f, jumpForce);
                _rb.AddForce(jumpVector, ForceMode2D.Impulse);
                isJumpPressed = false;
            }

            FlipPlayerScale();
        }


        private void Move(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    moveInputVal = _runAction.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    moveInputVal = 0f;
                    break;
            }
        }

        private void Jump(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    isJumpPressed = true;
                    break;
            }
        }

        private void CheckIfGrounded() {
            var pos = groundChecker.position;
            var size = new Vector2(0.75f, 0.2f);
            var direction = CapsuleDirection2D.Horizontal;
            var angle = 0f;

            isGrounded = Physics2D.OverlapCapsule(pos, size, direction, angle, groundLayer);
        }

        // flip towards the facing direction. if left then -1, if right then 1 
        private void FlipPlayerScale() {
            if (_rb.velocity.x < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (_rb.velocity.x > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void InitializeInputControls() {
            _controls = new InputControls();
            _runAction = _controls.Player.Run;
            _jumpAction = _controls.Player.Jump;
        }

        private void OnEnable() {
            _runAction.Enable();
            _jumpAction.Enable();

            _runAction.performed += Move;
            _runAction.canceled += Move;
            _jumpAction.performed += Jump;
            _jumpAction.canceled += Jump;
        }


        private void OnDisable() {
            _runAction.performed -= Move;
            _runAction.canceled -= Move;
            _jumpAction.performed -= Jump;
            _jumpAction.canceled -= Jump;

            _runAction.Disable();
            _jumpAction.Disable();
        }
    }
}