using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class FollowPlayerState : GroundedState {
        public FollowPlayerState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            enemyScript.CheckIfShouldFlip();
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (!canFollowPlayer) {
                stateMachine.ChangeState(enemyScript.IdleState);
            }

            if (canAttackPlayer) {
                stateMachine.ChangeState(enemyScript.AttackState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            enemyScript.SetVelocityX(enemyData.FollowSpeed * enemyScript.GetFacingDirection);
        }

        protected override void DoChecks() {
            base.DoChecks();
        }
    }
}