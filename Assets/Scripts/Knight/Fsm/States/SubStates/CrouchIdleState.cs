using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CrouchIdleState : GroundedState {
        public CrouchIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // if movement - change to CrouchWalk State
            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.CrouchWalkState);
            }

            // if Crouch btn pressed - change to Idle State
            if (!CrouchInput) {
                StateMachine.ChangeState(PlayerScript.IdleState);
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