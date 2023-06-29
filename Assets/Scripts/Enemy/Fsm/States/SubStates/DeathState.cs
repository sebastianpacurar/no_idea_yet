using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class DeathState : AbilityState {
        public DeathState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }
    }
}