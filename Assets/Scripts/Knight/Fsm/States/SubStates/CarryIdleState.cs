using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CarryIdleState : GroundedState {
        public CarryIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void Enter() {
            base.Enter();

            PlayerScript.SetPickUpFalse();
            PlayerScript.SetCrateIsBeingCarried(true);
            PlayerScript.SetIsCarrying(true);
            PlayerScript.SetCrateOnPlayer();
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.GeneratePredictionLine();

            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.CarryWalkState);
            }

            if (PickCrateInput) {
                PlayerScript.SetPickUpFalse();
                PlayerScript.ThrowCrate();
                PlayerScript.SetLineRendererActive(false);

                StateMachine.ChangeState(PlayerScript.IdleState);
            }
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(0f);
            PlayerScript.SetCrateVelToZero();
        }
    }
}