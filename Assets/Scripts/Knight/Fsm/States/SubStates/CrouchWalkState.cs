using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CrouchWalkState : GroundedState {
        private bool _isTopGrounded;

        public CrouchWalkState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            PlayerScript.CurrentSpeed = PlayerData.CarrySpeed;
            PlayerScript.SetCrouchInputFalse();
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            PlayerScript.CheckIfShouldFlip(XInput);

            // if no movement - change to CrouchIdle State
            if (XInput == 0) {
                StateMachine.ChangeState(PlayerScript.CrouchIdleState);
            }

            // if Crouch btn pressed - change to Run State
            if (CrouchInput && !_isTopGrounded) {
                StateMachine.ChangeState(PlayerScript.RunState);
            }

            // if crate has been thrown, change to CarryIdle State
            if (PickCrateInput) {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }
        }


        protected internal override void Exit() {
            base.Exit();
            PlayerScript.SetCrouchInputFalse();
        }


        protected override void DoChecks() {
            base.DoChecks();

            _isTopGrounded = PlayerScript.CheckIfTopGrounded();
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            PlayerScript.SetVelocityX(PlayerScript.CurrentSpeed * XInput);
        }
    }
}