using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Fsm.States.SubStates {
    public class IdleState : GroundedState {
        private float _idleTime;

        public IdleState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            var minIdleSeconds = enemyData.MinMaxIdleTime.x;
            var maxIdleSeconds = enemyData.MinMaxIdleTime.y;
            _idleTime = Random.Range(minInclusive: minIdleSeconds, maxInclusive: maxIdleSeconds);

            enemyScript.SetVelocityX(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (Time.time < startTime + _idleTime) {
                if (canFollowPlayer) {
                    stateMachine.ChangeState(enemyScript.FollowPlayerState);
                } else if (canAttackPlayer) {
                    stateMachine.ChangeState(enemyScript.AttackState);
                }
            } else {
                stateMachine.ChangeState(enemyScript.WalkState);
            }
        }
    }
}