using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates
{
    public class CrouchIdleState : GroundedState
    {
        private bool _isTopGrounded;

        public CrouchIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        protected internal override void Enter()
        {
            base.Enter();

            PlayerScript.SetCrouchInputFalse();
        }


        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            // if movement - change to CrouchWalk State
            if (XInput != 0)
            {
                StateMachine.ChangeState(PlayerScript.CrouchWalkState);
            }

            // if Crouch btn pressed - change to Idle State
            if (CrouchInput && !_isTopGrounded)
            {
                StateMachine.ChangeState(PlayerScript.IdleState);
            }

            // if PickUp btn pressed - change to CarryIdle State
            if (PickCrateInput)
            {
                StateMachine.ChangeState(PlayerScript.CarryIdleState);
            }
        }


        protected internal override void Exit()
        {
            base.Exit();

            PlayerScript.SetCrouchInputFalse();
        }


        protected override void DoChecks()
        {
            base.DoChecks();

            _isTopGrounded = PlayerScript.CheckIfTopGrounded();
        }


        protected internal override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            PlayerScript.SetVelocityX(0);
        }
    }
}