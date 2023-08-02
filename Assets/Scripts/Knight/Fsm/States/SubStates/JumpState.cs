using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates
{
    public class JumpState : AbilityState
    {
        public JumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter()
        {
            base.Enter();
            PlayerScript.AddJumpForce(PlayerData.JumpForce);
            IsAbilityDone = true;
        }
    }
}