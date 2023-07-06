using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class RunState : GroundedState {
        public RunState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput == 0f) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            } else if (SprintInput) {
                StateMachine.ChangeState(PlayerScript.SprintState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerData.RunSpeed * XInput);
        }

        protected override void DoChecks() {
            base.DoChecks();

            PlayerScript.CheckIfShouldFlip(XInput);
        }
    }
}