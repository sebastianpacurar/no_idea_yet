using ScriptableObjects;


namespace Enemy.Fsm.States.SuperStates {
    public class GroundedState : EnemyState {
        protected GroundedState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected bool CanFollowPlayer;
        protected bool CanAttackPlayer;
        protected bool IsDead;
        private bool _isHit;

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (_isHit) {
                EnemyScript.SetHitFalse();
                StateMachine.ChangeState(EnemyScript.TakeHitState);
            }
        }

        protected override void DoChecks() {
            base.DoChecks();
            CanFollowPlayer = EnemyScript.CheckIfCanFollowPlayer();
            CanAttackPlayer = EnemyScript.CheckIfCanAttackPlayer();
            _isHit = EnemyScript.CheckIfHit();
            IsDead = EnemyScript.CheckIfDead();
        }
    }
}