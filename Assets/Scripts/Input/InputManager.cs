using UnityEngine;
using UnityEngine.InputSystem;


namespace Input {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

        public int MoveVal { get; set; }
        public bool IsSprintPressed { get; set; }
        public bool IsJumpPressed { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsCrouching { get; set; }
        public bool IsInteractPressed { get; set; }
        public bool IsPickCratePressed { get; set; }

        private InputControls _controls;
        private Rigidbody2D _rb;

        #region input actions
        private InputAction _runAction;
        private InputAction _sprintAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _crouchAction;
        private InputAction _interactAction;
        private InputAction _pickCrateAction;
        #endregion

        #region input action phases
        private const InputActionPhase PhaseStarted = InputActionPhase.Started;
        private const InputActionPhase PhasePerformed = InputActionPhase.Performed;
        private const InputActionPhase PhaseCanceled = InputActionPhase.Canceled;
        #endregion


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            InitializeInputControls();
        }


        #region input actions related methods
        private void OnMove(InputAction.CallbackContext ctx) {
            MoveVal = ctx.phase switch {
                PhaseStarted or PhasePerformed => (int)_runAction.ReadValue<float>(),
                PhaseCanceled => 0,
                _ => MoveVal
            };
        }


        private void OnSprint(InputAction.CallbackContext ctx) {
            IsSprintPressed = ctx.phase switch {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsSprintPressed
            };
        }


        private void OnJump(InputAction.CallbackContext ctx) {
            IsJumpPressed = ctx.phase switch {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsJumpPressed
            };
        }


        private void OnAttack(InputAction.CallbackContext ctx) {
            IsAttacking = ctx.phase switch {
                PhaseStarted or PhasePerformed => true,
                _ => IsAttacking
            };
        }

        private void OnCrouch(InputAction.CallbackContext ctx) {
            IsCrouching = ctx.phase switch {
                PhaseStarted or PhasePerformed => !IsCrouching,
                _ => IsCrouching
            };
        }


        private void OnInteract(InputAction.CallbackContext ctx) {
            IsInteractPressed = ctx.phase switch {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsInteractPressed
            };
        }


        private void OnPickCrate(InputAction.CallbackContext ctx) {
            IsPickCratePressed = ctx.phase switch {
                PhaseStarted or PhasePerformed => true,
                _ => IsPickCratePressed
            };
        }
        #endregion


        private void InitializeInputControls() {
            _controls = new InputControls();
            _runAction = _controls.Player.Run;
            _sprintAction = _controls.Player.Sprint;
            _jumpAction = _controls.Player.Jump;
            _crouchAction = _controls.Player.Crouch;
            _attackAction = _controls.Player.Attack;
            _interactAction = _controls.Player.Interact;
            _pickCrateAction = _controls.Player.PickCrate;
        }


        #region Subscribe/Unsubscribe methods as callbacks for events
        private void OnEnable() {
            _runAction.Enable();
            _sprintAction.Enable();
            _jumpAction.Enable();
            _attackAction.Enable();
            _crouchAction.Enable();
            _interactAction.Enable();
            _pickCrateAction.Enable();

            //Unsubscribe the Move, Jump and Attack methods (call relevant methods when actions are performed) 
            _runAction.performed += OnMove;
            _runAction.canceled += OnMove;
            _sprintAction.performed += OnSprint;
            _sprintAction.canceled += OnSprint;
            _jumpAction.performed += OnJump;
            _jumpAction.canceled += OnJump;
            _attackAction.performed += OnAttack;
            _crouchAction.performed += OnCrouch;
            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;
            _pickCrateAction.performed += OnPickCrate;
        }


        private void OnDisable() {
            //Unsubscribe the Move, Jump and Attack methods
            _runAction.performed -= OnMove;
            _runAction.canceled -= OnMove;
            _sprintAction.performed -= OnSprint;
            _sprintAction.canceled -= OnSprint;
            _jumpAction.performed -= OnJump;
            _jumpAction.canceled -= OnJump;
            _attackAction.performed -= OnAttack;
            _crouchAction.performed -= OnCrouch;
            _interactAction.performed -= OnInteract;
            _interactAction.canceled -= OnInteract;
            _pickCrateAction.performed -= OnPickCrate;

            _runAction.Disable();
            _sprintAction.Disable();
            _jumpAction.Disable();
            _attackAction.Disable();
            _crouchAction.Disable();
            _interactAction.Disable();
            _pickCrateAction.Disable();
        }
        #endregion
    }
}