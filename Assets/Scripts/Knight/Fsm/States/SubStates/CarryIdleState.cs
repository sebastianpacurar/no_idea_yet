using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CarryIdleState : GroundedState {
        public CarryIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            PlayerScript.SetPickUpFalse();
            PlayerScript.SetCrateIsCarried(true);
            PlayerScript.SetVelocityX(0f);
            PlayerScript.SetCratePosition();
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            
            if (XInput != 0) {
                StateMachine.ChangeState(PlayerScript.CarryWalkState);
            }

            if (PickCrateInput) {
                PlayerScript.SetPickUpFalse();
                PlayerScript.ThrowCrate();
                StateMachine.ChangeState(PlayerScript.IdleState);
            }
        }
    }
}