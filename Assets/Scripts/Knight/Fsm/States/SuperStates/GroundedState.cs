using ScriptableObjects;

namespace Knight.Fsm.States.SuperStates {
    public class GroundedState : PlayerState {
        protected int XInput;
        protected bool SprintInput;
        protected bool JumpInput;
        protected bool CrouchInput;
        protected bool PickCrateInput;
        private bool _isGrounded;

        protected GroundedState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();


            XInput = PlayerScript.Input.MoveVal;
            SprintInput = PlayerScript.Input.IsSprintPressed;
            JumpInput = PlayerScript.Input.IsJumpPressed;
            CrouchInput = PlayerScript.Input.IsCrouching;
            PickCrateInput = PlayerScript.Input.IsPickCratePressed;
            
            if (JumpInput && !PlayerScript.isCarryingCrate) {
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
        }
    }
}