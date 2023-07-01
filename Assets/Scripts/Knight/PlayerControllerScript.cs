using UnityEngine;
using UnityEngine.InputSystem;


namespace Knight {
    public class PlayerControllerScript : MonoBehaviour {
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpForce;
        public float moveInputVal;
        public bool isGrounded;
        public bool isSprinting;
        public bool isSprintPressed;
        public bool isJumpPressed;
        public bool isAttacking;
        public bool isInteractPressed;
        public bool isHit;
        public bool isDead;

        [Space(10)]
        [Header("References")]
        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask pushableLayer;

        private InputControls _controls;
        private Rigidbody2D _rb;

        #region input actions
        private InputAction _runAction;
        private InputAction _sprintAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _interactAction;
        #endregion


        # region unity callback methods
        private void Awake() {
            InitializeInputControls();
            _rb = GetComponent<Rigidbody2D>();
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


        #region input actions related methods
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

        private void Sprint(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    isSprintPressed = true;

                    break;
                case InputActionPhase.Canceled:
                    isSprintPressed = false;

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

        private void Attack(InputAction.CallbackContext ctx) {
            if (isGrounded && !isAttacking) isAttacking = true;
        }

        private void Interact(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    isInteractPressed = true;
                    break;
            }
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
            if (isJumpPressed && isGrounded && !isAttacking) {
                var jumpVector = new Vector2(0f, jumpForce);
                _rb.AddForce(jumpVector, ForceMode2D.Impulse);
                isJumpPressed = false;
            }
        }

        // HACK: use sprint speed when jumping, even though the input is canceled (sprint button released)
        private void CheckIfCanSprint() {
            if (isGrounded && moveInputVal != 0f && isSprintPressed) {
                // activate sprinting only when grounded, move keys are pressed and sprint is pressed
                isSprinting = true;
            } else if ((isSprinting && isGrounded && !isSprintPressed) || moveInputVal == 0f) {
                // deactivate when reaches land or move keys are not pressed.
                // used to cause the player to have same speed when jumping from sprint
                isSprinting = false;
            }
        }

        private void HandleRunAndSprint() {
            var speed = isSprinting ? sprintSpeed : runSpeed;

            _rb.velocity = new Vector2(moveInputVal * speed, _rb.velocity.y);
        }

        private void InitializeInputControls() {
            _controls = new InputControls();
            _runAction = _controls.Player.Run;
            _sprintAction = _controls.Player.Sprint;
            _jumpAction = _controls.Player.Jump;
            _attackAction = _controls.Player.Attack;
            _interactAction = _controls.Player.Interact;
        }

        #region Subscribe/Unsubscribe methods as callbacks for events
        private void OnEnable() {
            _runAction.Enable();
            _sprintAction.Enable();
            _jumpAction.Enable();
            _attackAction.Enable();
            _interactAction.Enable();

            //Unsubscribe the Move, Jump and Attack methods (call relevant methods when actions are performed) 
            _runAction.performed += Move;
            _runAction.canceled += Move;
            _sprintAction.performed += Sprint;
            _sprintAction.canceled += Sprint;
            _jumpAction.performed += Jump;
            _jumpAction.canceled += Jump;
            _attackAction.performed += Attack;
            _interactAction.performed += Interact;
        }


        private void OnDisable() {
            //Unsubscribe the Move, Jump and Attack methods
            _runAction.performed -= Move;
            _runAction.canceled -= Move;
            _sprintAction.performed -= Sprint;
            _sprintAction.canceled -= Sprint;
            _jumpAction.performed -= Jump;
            _jumpAction.canceled -= Jump;
            _interactAction.canceled -= Interact;

            _runAction.Disable();
            _sprintAction.Disable();
            _jumpAction.Disable();
            _attackAction.Disable();
            _interactAction.Disable();
        }
        #endregion
    }
}