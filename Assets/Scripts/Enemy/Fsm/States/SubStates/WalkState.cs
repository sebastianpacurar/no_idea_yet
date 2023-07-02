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

            var minWalkSeconds = EnemyData.MinMaxWalkTime.x;
            var maxWalkSeconds = EnemyData.MinMaxWalkTime.y;
            _walkTime = Random.Range(minInclusive: minWalkSeconds, maxInclusive: maxWalkSeconds);

            var randDir = Random.Range(0, 2) switch {
                0 => _direction = -1,
                1 => _direction = 1,
                _ => 1,
            };

            if (!randDir.Equals(EnemyScript.GetFacingDirection())) {
                EnemyScript.Flip();
            }
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (Time.time >= StartTime + _walkTime) {
                StateMachine.ChangeState(EnemyScript.IdleState);
            }

            if (CanFollowPlayer) {
                StateMachine.ChangeState(EnemyScript.FollowPlayerState);
            }

            if (CanAttackPlayer) {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            EnemyScript.SetVelocityX(EnemyData.MoveSpeed * _direction);
        }

        protected override void DoChecks() {
            base.DoChecks();
        }
    }
}