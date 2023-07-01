using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class TakeHitState : GroundedState {
        public TakeHitState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            enemyScript.DecrementLife();

            if (!isDead) {
                enemyScript.ApplyKnockBackForce();
            }
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (isAnimationFinished) {
                if (isDead) {
                    stateMachine.ChangeState(enemyScript.DeathState);
                } else {
                    stateMachine.ChangeState(enemyScript.IdleState);
                }
            }
        }

        protected internal override void Exit() {
            base.Exit();
        }
    }
}