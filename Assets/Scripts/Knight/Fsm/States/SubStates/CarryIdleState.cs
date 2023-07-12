using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CarryIdleState : GroundedState {
        public CarryIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            PlayerScript.SetVelocityX(0f);
            PlayerScript.SetPickUpFalse();
            PlayerScript.SetCrateIsCarried(true);
            PlayerScript.SetIsCarryingCrate(true);
            PlayerScript.SetCratePosition();
            PlayerScript.SetCrateVelocityToZero();
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.GeneratePredictionLine();
            PlayerScript.SetAimTrajectory();


            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.CarryWalkState);
            }

            if (PickCrateInput) {
                PlayerScript.SetPickUpFalse();
                PlayerScript.ThrowCrate();
                StateMachine.ChangeState(PlayerScript.IdleState);
            }
        }

        protected internal override void Exit() {
            base.Exit();

            PlayerScript.SetLineRendererActive(false);
        }
    }
}