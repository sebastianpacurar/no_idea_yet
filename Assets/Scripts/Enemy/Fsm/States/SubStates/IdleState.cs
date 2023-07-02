using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Fsm.States.SubStates {
    public class IdleState : GroundedState {
        private float _idleTime;

        public IdleState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            var minIdleSeconds = EnemyData.MinMaxIdleTime.x;
            var maxIdleSeconds = EnemyData.MinMaxIdleTime.y;
            _idleTime = Random.Range(minInclusive: minIdleSeconds, maxInclusive: maxIdleSeconds);

            EnemyScript.SetVelocityX(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (Time.time >= StartTime + _idleTime) {
                StateMachine.ChangeState(EnemyScript.WalkState);
            }

            if (CanFollowPlayer) {
                StateMachine.ChangeState(EnemyScript.FollowPlayerState);
            }

            if (CanAttackPlayer) {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }
    }
}