using UnityEngine;
using UnityEngine.InputSystem;


namespace Knight {
    public class ControllerScript : MonoBehaviour {
        [SerializeField] private float runSpeed;
        [SerializeField] private float moveInputVal;
        [SerializeField] private bool isGrounded;

        [Space(10)]
        [Header("References")]
        [SerializeField] private Transform groundChecker;

        private InputControls _controls;
        private Animator _animator;
        private Rigidbody2D _rb;

        #region input actions
        private InputAction _runAction;
        private InputAction _jumpAction;
        #endregion


        private void Awake() {
            InitializeInputControls();
            InitializeComponents();
            FlipPlayerScale();
        }

        private void Update() {
            // update the isGrounded variable
            CheckIfGrounded();
        }

        private void FixedUpdate() {
            var x = moveInputVal;

            // change the object based on its velocity
            _rb.velocity = new Vector2(x * moveInputVal, _rb.velocity.y);
        }


        private void Move(InputAction.CallbackContext ctx) {
            moveInputVal = _runAction.ReadValue<float>();
        }

        private void CheckIfGrounded() {
            var pos = groundChecker.position;
            var size = new Vector2(0.75f, 0.2f);
            var direction = CapsuleDirection2D.Horizontal;
            var angle = 0f;

            isGrounded = Physics2D.OverlapCapsule(pos, size, direction, angle);
        }

        private void HandleAnimations() { }

        // flip towards the facing direction. if left then -1, if right then 1 
        private void FlipPlayerScale() {
            if (_rb.velocity.x < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (_rb.velocity.x > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }


        private void OnEnable() {
            _runAction.Enable();
            _jumpAction.Enable();

            _runAction.performed += Move;
        }


        private void OnDisable() {
            _runAction.performed -= Move;

            _runAction.Disable();
            _jumpAction.Disable();
        }

        private void InitializeInputControls() {
            _controls = new InputControls();
            _runAction = _controls.Player.Run;
            _jumpAction = _controls.Player.Jump;
        }

        private void InitializeComponents() {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }
    }
}