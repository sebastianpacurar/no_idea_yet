using Knight.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Knight.Fsm.States.SubStates {
    public class CrouchAttackState : AbilityState {
        protected CrouchAttackState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }
    }
}