using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class TakeHitState : GroundedState {
        public TakeHitState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            EnemyScript.DecrementLife();

            if (!IsDead) {
                EnemyScript.ApplyKnockBackForce();
            }
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

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