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


            // if player in transition prevent movement, and force Idle State
            if (PlayerScript.CheckPlayerTransition()) {
                XInput = 0;
                PlayerScript.SetVelocityX(0);
            }

            // if player not in transition
            else {
                // if jump pressed and not carrying crate
                if (_jumpInput && !_isCarry) {
                    PlayerScript.SetJumpFalse();

                    // if grounded perform jump - change to Jump State
                    if (_isGrounded) {
                        StateMachine.ChangeState(PlayerScript.JumpState);
                    }
                }

                // if not on ground (fall) - change to InAir State
                if (!_isGrounded) {
                    StateMachine.ChangeState(PlayerScript.InAirState);
                }
            }
        }


        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
            _isCarry = PlayerScript.CheckPlayerCarry();
        }
    }
}