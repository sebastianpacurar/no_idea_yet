using ScriptableObjects;

namespace Enemy.Fsm.States.SuperStates {
    public class GroundedState : EnemyState {
        protected GroundedState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected bool canFollowPlayer;
        protected bool canAttackPlayer;
        protected float facingDirection;

        protected override void DoChecks() {
            base.DoChecks();
            canFollowPlayer = enemyScript.CheckIfCanFollowPlayer();
            canAttackPlayer = enemyScript.CheckIfCanAttackPlayer();
        }
    }
}