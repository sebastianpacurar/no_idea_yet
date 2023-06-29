using ScriptableObjects;

namespace Enemy.Fsm.States.SuperStates {
    public class AbilityState : EnemyState {
        protected bool isAbilityDone;

        public AbilityState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }


        protected internal override void Enter() {
            base.Enter();
            isAbilityDone = false;
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (isAbilityDone) {
                
            }
        }

        protected override void DoChecks() {
            base.DoChecks();
        }
    }
}