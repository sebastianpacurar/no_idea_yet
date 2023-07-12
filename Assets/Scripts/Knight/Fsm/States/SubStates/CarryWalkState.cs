using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CarryWalkState : GroundedState {
        public CarryWalkState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            PlayerScript.CurrentSpeed = PlayerData.CrouchSpeed;
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);
            PlayerScript.SetCratePosition();


            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }

            if (PickCrateInput) {
                PlayerScript.SetPickUpFalse();
                PlayerScript.ThrowCrate();
                StateMachine.ChangeState(PlayerScript.RunState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();
            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * XInput);
        }
    }
}