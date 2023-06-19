using UnityEngine;


namespace Knight {
    public class AnimationHandlerScript : MonoBehaviour {
        private Animator _animator;
        private Rigidbody2D _rb;
        private ControllerScript _controllerScript;

        [SerializeField] private bool isAscending;
        [SerializeField] private bool isDescending;
        [SerializeField] private bool isFalling;

        private void Awake() {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _controllerScript = GetComponent<ControllerScript>();
        }

        private void Update() {
            SetVariables();
            HandleAnimations();
        }

        private void HandleAnimations() {
            var moveVal = _controllerScript.moveInputVal;
            var grounded = _controllerScript.isGrounded;

            // set initial state to default to idle. in case any of the below conditions fail, then Player is in idle
            var state = AnimationState.Idle;

            if (moveVal != 0f && grounded) state = AnimationState.Run;
            else if (isAscending) state = AnimationState.Ascend;
            else if (isDescending) state = AnimationState.Descend;
            else if (isFalling) state = AnimationState.Fall;

            _animator.SetInteger("State", (int)state);
        }

        private void SetVariables() {
            var velY = _rb.velocity.y;
            var grounded = _controllerScript.isGrounded;
            
            isAscending = velY > 0.5f && !grounded;
            isDescending = velY < -0.5f && !grounded;

            if (isFalling && grounded) {
                isFalling = false;
            }
        }

        // called in Descend animation
        public void TriggerFallAnimation() {
            isFalling = true;
        }

        private enum AnimationState {
            Idle,
            Run,
            Ascend,
            Descend,
            Fall,
            FirstAttack,
            Death
        }
    }
}