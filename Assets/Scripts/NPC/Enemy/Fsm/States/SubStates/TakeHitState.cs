using NPC.Enemy.Fsm.States.SuperStates;
using ScriptableObjects;


namespace NPC.Enemy.Fsm.States.SubStates {
    public class TakeHitState : GroundedState {
        public TakeHitState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        
        protected internal override void Enter() {
            base.Enter();

            // decrement life of enemy by 1 unit when entering state
            EnemyScript.DecrementLife();

            // apply knock-back if enemy is still alive after life reduction
            if (!IsDead) {
                EnemyScript.ApplyKnockBackForce();
            }
        }

        
        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            
            // if animation finished and enemy is dead, change state to Death State, else change to Idle State
            if (IsAnimationFinished) {
                if (IsDead) {
                    StateMachine.ChangeState(EnemyScript.DeathState);
                } else {
                    StateMachine.ChangeState(EnemyScript.IdleState);
                }
            }
        }
    }
}