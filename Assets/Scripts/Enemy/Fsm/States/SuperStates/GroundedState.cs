using ScriptableObjects;


namespace Enemy.Fsm.States.SuperStates {
    public class GroundedState : EnemyState {
        protected GroundedState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        // define all the checks used in the sub states
        protected bool CanFollowPlayer;
        protected bool CanAttackPlayer;
        protected bool IsDead;
        private bool _isLateNight;
        private bool _isHit;

        
        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // set to hit state
            if (_isHit) {
                EnemyScript.SetHitFalse();
                StateMachine.ChangeState(EnemyScript.TakeHitState);
            }

            // set to death state if day light comes
            if (!_isLateNight) {
                StateMachine.ChangeState(EnemyScript.DeathState);
            }
        }

        
        protected override void DoChecks() {
            base.DoChecks();
            CanFollowPlayer = EnemyScript.CheckIfCanFollowPlayer();
            CanAttackPlayer = EnemyScript.CheckIfCanAttackPlayer();
            _isLateNight = EnemyScript.CheckIfLateNightInterval();
            _isHit = EnemyScript.CheckIfHit();
            IsDead = EnemyScript.CheckIfDead();
        }
    }
}