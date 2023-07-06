using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class InAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        private float _speed;
        public InAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            _xInput = PlayerScript.Input.MoveVal;

            if (_isGrounded) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            PlayerScript.Anim.SetFloat("yVelocity", PlayerScript.currentVelocity.y);
        }

        protected override void DoChecks() {
            base.DoChecks();

            _isGrounded = PlayerScript.CheckIfGrounded();
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerData.RunSpeed * _xInput);
            PlayerScript.CheckIfShouldFlip(_xInput);
        }
    }
}