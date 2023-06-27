using UnityEngine;


namespace Knight {
    public class PlayerAnimationScript : MonoBehaviour {
        private Rigidbody2D _rb;
        private PlayerControllerScript _controllerScript;

        [SerializeField] private bool isAscending;
        [SerializeField] private bool isDescending;
        [SerializeField] private bool isFalling;

        private Animator _animator;

        private void Awake() {
            var parent = transform.parent;
            _rb = parent.GetComponent<Rigidbody2D>();
            _controllerScript = parent.GetComponent<PlayerControllerScript>();
            _animator = GetComponent<Animator>();
        }

        private void Update() {
            SetVariables();
            HandleAnimations();
        }

        private void HandleAnimations() {
            var moveVal = _controllerScript.moveInputVal;
            var grounded = _controllerScript.isGrounded;
            var isSprinting = _controllerScript.isSprinting;
            var isAttacking = _controllerScript.isAttacking;

            // set initial state to default to idle. in case any of the below conditions fail, then Player is in idle
            var state = AnimationState.Idle;

            if (moveVal != 0f && grounded) state = AnimationState.Run;
            if (isSprinting) state = AnimationState.Sprint;
            if (isAscending) state = AnimationState.Ascend;
            if (isDescending) state = AnimationState.Descend;
            if (isFalling) state = AnimationState.Fall;
            if (isAttacking) state = AnimationState.Attack;

            _animator.SetInteger("State", (int)state);
        }

        private void SetVariables() {
            var velY = _rb.velocity.y;
            var grounded = _controllerScript.isGrounded;

            isAscending = velY > 0.1f && !grounded;
            isDescending = velY < 0f && !grounded;

            if (isFalling && grounded) {
                isFalling = false;
            }
        }

        // called in Descend animation
        public void TriggerFallAnimation() {
            isFalling = true;
        }

        // called in Attack animation
        public void StopAttackAnimation() {
            _controllerScript.isAttacking = false;
        }

        private enum AnimationState {
            Idle,
            Run,
            Ascend,
            Descend,
            Fall,
            Attack,
            Sprint,
            Death
        }
    }
}