using ScriptableObjects;

namespace Knight.Fsm.States.SuperStates
{
    public class AbilityState : PlayerState
    {
        protected bool IsAbilityDone;
        private bool _isGrounded;
        protected AbilityState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter()
        {
            base.Enter();
            IsAbilityDone = false;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAbilityDone)
            {
                if (_isGrounded && PlayerScript.CurrentVelocity.y < 0.01f)
                {
                    StateMachine.ChangeState(PlayerScript.IdleState);
                } else
                {
                    StateMachine.ChangeState(PlayerScript.InAirState);
                }
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = PlayerScript.CheckIfGrounded();
        }
    }
}