using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CrouchWalkState : GroundedState {
        public CrouchWalkState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            PlayerScript.CurrentSpeed = PlayerData.CarrySpeed;
            PlayerScript.SetPickUpFalse();
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);

            // if no movement - change to CrouchIdle State
            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.CrouchIdleState);
            }

            // if Crouch btn pressed - change to Run State
            if (!CrouchInput) {
                StateMachine.ChangeState(PlayerScript.RunState);
            }

            // if crate has been thrown, change to CarryIdle State
            if (PickCrateInput) {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * XInput);
        }
    }
}