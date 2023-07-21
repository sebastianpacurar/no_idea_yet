using ScriptableObjects;

namespace Knight.Fsm.States.SuperStates {
    public class GroundedState : PlayerState {
        protected int XInput;
        protected bool SprintInput;
        protected bool CrouchInput;
        protected bool PickCrateInput;
        private bool _jumpInput;
        private bool _isGrounded;
        private bool _isCarry;

        protected GroundedState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            XInput = PlayerScript.Input.MoveVal;
            SprintInput = PlayerScript.Input.IsSprintPressed;
            _jumpInput = PlayerScript.Input.IsJumpPressed;
            CrouchInput = PlayerScript.Input.IsCrouching;
            PickCrateInput = PlayerScript.Input.IsPickCratePressed;

            if (_jumpInput && !_isCarry) {
                PlayerScript.SetJumpFalse();

                if (_isGrounded) {
                    StateMachine.ChangeState(PlayerScript.JumpState);
                }
            }

            if (!_isGrounded) {
                StateMachine.ChangeState(PlayerScript.InAirState);
            }
        }

        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
            _isCarry = PlayerScript.CheckPlayerCarry();
        }
    }
}