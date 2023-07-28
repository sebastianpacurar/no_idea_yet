using Knight.Fsm.States.SuperStates;
using ScriptableObjects;


namespace Knight.Fsm.States.SubStates {
    public class CarryWalkState : GroundedState {
        // private bool _checkIfFacingWall;

        public CarryWalkState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void Enter() {
            base.Enter();
            PlayerScript.CurrentSpeed = PlayerData.CrouchSpeed;
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);
            PlayerScript.GeneratePredictionLine();

            // if no movement - change to CarryIdle State
            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }
            
            // if PickUp btn pressed - perform Throw, and change to Idle State
            if (PickCrateInput) {
                PlayerScript.SetPickUpFalse();
                PlayerScript.ThrowCrate();
                PlayerScript.SetLineRendererActive(false);

                StateMachine.ChangeState(PlayerScript.IdleState);
            }
        }

        
        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();
            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * XInput);
        }
    }
}