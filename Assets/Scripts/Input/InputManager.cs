using UnityEngine;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public int MoveVal { get; set; }
        public int AimVal { get; set; }
        public bool IsShiftAimPressed { get; set; }
        public bool IsJumpPressed { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsCrouchPressed { get; set; }
        public bool IsInteractPressed { get; set; }
        public bool IsPickCratePressed { get; set; }
        public bool IsRestartPressed { get; set; }

        private InputControls _controls;
        private Rigidbody2D _rb;

        #region input actions
        private InputAction _runAction;
        private InputAction _aimAction;
        private InputAction _shiftAimAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _crouchAction;
        private InputAction _interactAction;
        private InputAction _pickCrateAction;
        private InputAction _restartLevelAction;
        #endregion

        #region input action phases
        private const InputActionPhase PhaseStarted = InputActionPhase.Started;
        private const InputActionPhase PhasePerformed = InputActionPhase.Performed;
        private const InputActionPhase PhaseCanceled = InputActionPhase.Canceled;
        #endregion


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else
            {
                Instance = this;
            }

            InitializeInputControls();
        }


        #region input actions related methods
        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveVal = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => (int)_runAction.ReadValue<float>(),
                PhaseCanceled => 0,
                _ => MoveVal
            };
        }


        private void OnAim(InputAction.CallbackContext ctx)
        {
            AimVal = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => (int)_aimAction.ReadValue<float>(),
                PhaseCanceled => 0,
                _ => AimVal
            };
        }


        private void OnShiftAim(InputAction.CallbackContext ctx)
        {
            IsShiftAimPressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsShiftAimPressed
            };
        }


        private void OnJump(InputAction.CallbackContext ctx)
        {
            IsJumpPressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsJumpPressed
            };
        }


        private void OnAttack(InputAction.CallbackContext ctx)
        {
            IsAttacking = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                _ => IsAttacking
            };
        }


        private void OnCrouch(InputAction.CallbackContext ctx)
        {
            IsCrouchPressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                _ => IsCrouchPressed
            };
        }


        private void OnInteract(InputAction.CallbackContext ctx)
        {
            IsInteractPressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                PhaseCanceled => false,
                _ => IsInteractPressed
            };
        }


        private void OnPickCrate(InputAction.CallbackContext ctx)
        {
            IsPickCratePressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                _ => IsPickCratePressed
            };
        }


        private void OnRestartLevel(InputAction.CallbackContext ctx)
        {
            IsRestartPressed = ctx.phase switch
            {
                PhaseStarted or PhasePerformed => true,
                _ => IsRestartPressed
            };
        }
        #endregion


        private void InitializeInputControls()
        {
            _controls = new InputControls();
            _runAction = _controls.Player.Run;
            _aimAction = _controls.Player.Aim;
            _shiftAimAction = _controls.Player.ShiftAim;
            _jumpAction = _controls.Player.Jump;
            _crouchAction = _controls.Player.Crouch;
            _attackAction = _controls.Player.Attack;
            _interactAction = _controls.Player.Interact;
            _pickCrateAction = _controls.Player.PickCrate;
            _restartLevelAction = _controls.UI.RestartLevel;
        }


        #region Subscribe/Unsubscribe methods as callbacks for events
        private void OnEnable()
        {
            _runAction.Enable();
            _aimAction.Enable();
            _shiftAimAction.Enable();
            _jumpAction.Enable();
            _attackAction.Enable();
            _crouchAction.Enable();
            _interactAction.Enable();
            _pickCrateAction.Enable();
            _restartLevelAction.Enable();

            //Unsubscribe the Move, Jump and Attack methods (call relevant methods when actions are performed) 
            _runAction.performed += OnMove;
            _runAction.canceled += OnMove;
            _aimAction.performed += OnAim;
            _aimAction.canceled += OnAim;
            _shiftAimAction.performed += OnShiftAim;
            _shiftAimAction.canceled += OnShiftAim;
            _jumpAction.performed += OnJump;
            _jumpAction.canceled += OnJump;
            _attackAction.performed += OnAttack;
            _crouchAction.performed += OnCrouch;
            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;
            _pickCrateAction.performed += OnPickCrate;
            _restartLevelAction.performed += OnRestartLevel;
        }


        private void OnDisable()
        {
            //Unsubscribe the Move, Jump and Attack methods
            _runAction.performed -= OnMove;
            _runAction.canceled -= OnMove;
            _aimAction.performed -= OnAim;
            _aimAction.canceled -= OnAim;
            _shiftAimAction.performed -= OnShiftAim;
            _shiftAimAction.canceled -= OnShiftAim;
            _jumpAction.performed -= OnJump;
            _jumpAction.canceled -= OnJump;
            _attackAction.performed -= OnAttack;
            _crouchAction.performed -= OnCrouch;
            _interactAction.performed -= OnInteract;
            _interactAction.canceled -= OnInteract;
            _pickCrateAction.performed -= OnPickCrate;
            _restartLevelAction.performed -= OnRestartLevel;

            _runAction.Disable();
            _aimAction.Disable();
            _shiftAimAction.Disable();
            _jumpAction.Disable();
            _attackAction.Disable();
            _crouchAction.Disable();
            _interactAction.Disable();
            _pickCrateAction.Disable();
            _restartLevelAction.Disable();
        }
        #endregion
    }
}