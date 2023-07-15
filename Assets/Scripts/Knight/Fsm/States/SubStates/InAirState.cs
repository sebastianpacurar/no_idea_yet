using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class InAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        private bool _isCrateBeingCarried;
        public InAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            _xInput = PlayerScript.Input.MoveVal;

            PlayerScript.SetLineRendererActive(false);
            PlayerScript.CheckIfShouldFlip(_xInput);

            if (_isGrounded) {
                if (_isCrateBeingCarried) {
                    if (_xInput == 0) {
                        StateMachine.ChangeState(PlayerScript.CarryIdleState);
                    } else {
                        StateMachine.ChangeState(PlayerScript.CarryWalkState);
                    }
                } else {
                    StateMachine.ChangeState(PlayerScript.IdleState);
                }
            }


            PlayerScript.Anim.SetFloat("yVelocity", PlayerScript.CurrentVelocity.y);
        }


        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
            _isCrateBeingCarried = PlayerScript.CheckIfCrateIsBeingCarried();
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * _xInput);

            if (_isCrateBeingCarried) {
                PlayerScript.SetCrateVelToPlayerVel();
            }
        }
    }
}