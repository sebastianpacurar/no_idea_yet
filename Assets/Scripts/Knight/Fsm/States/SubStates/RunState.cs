using Knight.Fsm.States.SuperStates;
using ScriptableObjects;


namespace Knight.Fsm.States.SubStates {
    public class RunState : GroundedState {
        public RunState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            PlayerScript.CurrentSpeed = PlayerData.RunSpeed;
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);

            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            if (SprintInput) {
                StateMachine.ChangeState(PlayerScript.SprintState);
            }

            if (CrouchInput) {
                StateMachine.ChangeState(PlayerScript.CrouchWalkState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * XInput);
        }
    }
}