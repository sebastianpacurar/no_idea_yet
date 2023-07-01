using ScriptableObjects;


namespace Enemy.Fsm.States.SuperStates {
    public class GroundedState : EnemyState {
        protected GroundedState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected bool canFollowPlayer;
        protected bool canAttackPlayer;
        protected bool isDead;
        private bool _isHit;

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (_isHit) {
                enemyScript.SetHitFalse();
                stateMachine.ChangeState(enemyScript.TakeHitState);
            }
        }

        protected override void DoChecks() {
            base.DoChecks();
            canFollowPlayer = enemyScript.CheckIfCanFollowPlayer();
            canAttackPlayer = enemyScript.CheckIfCanAttackPlayer();
            _isHit = enemyScript.CheckIfHit();
            isDead = enemyScript.CheckIfDead();
        }
    }
}