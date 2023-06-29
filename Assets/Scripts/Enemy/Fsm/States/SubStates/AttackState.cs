using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class AttackState : AbilityState {
        public AttackState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }
        
        //TODO: to be implemented
        
    }
}