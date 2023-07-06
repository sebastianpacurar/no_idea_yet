using NPC.Enemy.Fsm.States.SuperStates;
using ScriptableObjects;


namespace NPC.Enemy.Fsm.States.SubStates {
    public class AttackState : GroundedState {
        public AttackState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        
        protected internal override void Enter() {
            base.Enter();

            // set move to 0 when entering Attack State
            EnemyScript.SetVelocityX(0f);
        }

        
        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // change to Idle State when the animation finishes
            if (IsAnimationFinished) {
                StateMachine.ChangeState(EnemyScript.IdleState);
            }
        }
    }
}