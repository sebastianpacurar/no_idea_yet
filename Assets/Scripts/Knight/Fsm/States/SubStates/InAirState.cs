using ScriptableObjects;


namespace Knight.Fsm.States.SubStates {
    public class InAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        private bool _isCarry;
        public InAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            _xInput = PlayerScript.Input.MoveVal;

            PlayerScript.SetLineRendererActive(false);
            PlayerScript.CheckIfShouldFlip(_xInput);

            // if falling while carrying crate
            if (_isCarry) {
                // reset carry state, causing _isCarry to return false
                PlayerScript.ValidateCarryDistance();
            }

            // if player reaches ground
            if (_isGrounded) {
                // if player is carrying crate when reaching ground
                if (_isCarry) {
                    StateMachine.ChangeState(PlayerScript.CarryIdleState);
                }
                // if ground reached without carrying crate
                else {
                    StateMachine.ChangeState(PlayerScript.IdleState);
                }
            }

            PlayerScript.Anim.SetFloat("yVelocity", PlayerScript.CurrentVelocity.y);
        }


        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
            _isCarry = PlayerScript.CheckPlayerCarry();
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();
            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * _xInput);
        }
    }
}