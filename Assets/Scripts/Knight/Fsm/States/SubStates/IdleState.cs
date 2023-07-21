using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class IdleState : GroundedState {
        public IdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // if no movement - change to Run State
            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.RunState);
            }

            // if Crouch btn pressed - change to CrouchIdle State
            if (CrouchInput) {
                StateMachine.ChangeState(PlayerScript.CrouchIdleState);
            }

            // if PickUp btn pressed - change to CarryIdle State
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