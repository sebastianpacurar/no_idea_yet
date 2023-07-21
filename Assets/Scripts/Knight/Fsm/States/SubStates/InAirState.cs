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

            // if player reaches ground
            if (_isGrounded) {
                // if player is carrying crate when reaching ground
                if (_isCarry) {
                    if (_xInput == 0) {
                        StateMachine.ChangeState(PlayerScript.CarryIdleState);
                    } else {
                        StateMachine.ChangeState(PlayerScript.CarryWalkState);
                    }
                }
                // if ground reached without carrying crate
                else {
                    StateMachine.ChangeState(PlayerScript.IdleState);
                }
            }

            // if falling while carrying crate
            if (_isCarry) {
                // reset carry state, causing _isCarry to return false
                PlayerScript.ValidateCarryDistance();
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

            // cause the crate to move along with the player based on its velocity
            if (_isCarry) {
                PlayerScript.SetCrateVelToPlayerVel();
            }
        }
    }
}