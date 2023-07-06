using NPC.Enemy.Fsm.States.SuperStates;
using ScriptableObjects;


namespace NPC.Enemy.Fsm.States.SubStates {
    public class FollowPlayerState : GroundedState {
        public FollowPlayerState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        
        protected internal override void Enter() {
            base.Enter();

            EnemyScript.CheckIfShouldFlip();
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // if player is out of sight, change to Idle State
            if (!CanFollowPlayer) {
                StateMachine.ChangeState(EnemyScript.IdleState);
            }

            // if player is in Attack range, then change to Attack State
            if (CanAttackPlayer) {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }

        
        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            // update movement velocity, based on enemyData speed when following
            EnemyScript.SetVelocityX(EnemyData.FollowSpeed * EnemyScript.GetFacingDirection());
        }

        
        protected override void DoChecks() {
            base.DoChecks();

            // flip enemy if not facing player, check if should flip, during every FixedUpdate() execution
            EnemyScript.CheckIfShouldFlip();
        }
    }
}