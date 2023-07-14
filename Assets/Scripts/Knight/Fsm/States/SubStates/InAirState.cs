using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class InAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        public InAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void Enter() {
            base.Enter();

            if (PlayerScript.isCarryingCrate) {
                PlayerScript.SetCrateCarryVars(false);
            }
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.SetLineRendererActive(false);

            _xInput = PlayerScript.Input.MoveVal;
            PlayerScript.CheckIfShouldFlip(_xInput);

            if (_isGrounded) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            PlayerScript.Anim.SetFloat("yVelocity", PlayerScript.CurrentVelocity.y);
        }


        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * _xInput);
        }
    }
}