using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CrouchIdleState : GroundedState {
        public CrouchIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.CrouchWalkState);
            }

            if (!CrouchInput) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            if (PickCrateInput) {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();
            PlayerScript.SetVelocityX(0);
        }
    }
}