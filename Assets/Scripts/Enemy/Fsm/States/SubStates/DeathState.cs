using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class DeathState : GroundedState {
        public DeathState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            EnemyScript.SetVelocityX(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (IsAnimationFinished) {
                EnemyScript.DestroyEnemy();
            }
        }
    }
}