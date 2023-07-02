using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;

namespace Enemy.Fsm.States.SubStates {
    public class FollowPlayerState : GroundedState {
        public FollowPlayerState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            EnemyScript.CheckIfShouldFlip();
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (!CanFollowPlayer) {
                StateMachine.ChangeState(EnemyScript.IdleState);
            }

            if (CanAttackPlayer) {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            EnemyScript.SetVelocityX(EnemyData.FollowSpeed * EnemyScript.GetFacingDirection());
        }

        protected override void DoChecks() {
            base.DoChecks();
            EnemyScript.CheckIfShouldFlip();
        }
    }
}