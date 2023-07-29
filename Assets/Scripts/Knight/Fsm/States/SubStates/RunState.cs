using Knight.Fsm.States.SuperStates;
using ScriptableObjects;


namespace Knight.Fsm.States.SubStates {
    public class RunState : GroundedState {
        public RunState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            PlayerScript.CurrentSpeed = PlayerData.RunSpeed;
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);

            // if no movement - change to Idle State
            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            // if Crouch btn pressed - change to CrouchWalk State
            if (CrouchInput) {
                StateMachine.ChangeState(PlayerScript.CrouchWalkState);
            }

            // if PickUp btn pressed - change to CarryIdle State
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