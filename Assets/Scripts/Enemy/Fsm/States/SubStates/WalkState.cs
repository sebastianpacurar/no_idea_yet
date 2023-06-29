using Enemy.Fsm.States.SuperStates;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Fsm.States.SubStates {
    public class WalkState : GroundedState {
        private float _walkTime;
        private float _direction;

        public WalkState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            var minWalkSeconds = enemyData.MinMaxWalkTime.x;
            var maxWalkSeconds = enemyData.MinMaxWalkTime.y;
            _walkTime = Random.Range(minInclusive: minWalkSeconds, maxInclusive: maxWalkSeconds);

            _direction = Random.Range(0, 2) switch {
                0 => _direction = -1,
                1 => _direction = 1,
                _ => 1,
            };
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (canFollowPlayer) {
                stateMachine.ChangeState(enemyScript.FollowPlayerState);
            }

            if (Time.time < startTime + _walkTime) {
                stateMachine.ChangeState(enemyScript.IdleState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            enemyScript.SetVelocityX(enemyData.MoveSpeed * _direction);
        }

        protected override void DoChecks() {
            base.DoChecks();
        }
    }
}