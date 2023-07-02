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

            // set walkTime based on min and max enemyData values
            var minWalkSeconds = EnemyData.MinMaxWalkTime.x;
            var maxWalkSeconds = EnemyData.MinMaxWalkTime.y;
            _walkTime = Random.Range(minInclusive: minWalkSeconds, maxInclusive: maxWalkSeconds);

            // grab a random number between -1 and 1 which represent the direction on X axis
            var randDir = Random.Range(0, 2) switch {
                0 => _direction = -1,
                1 => _direction = 1,
                _ => 1,
            };

            // if the direction is opposite to the facing direction, then flip enemy to face the correct direction
            if (!randDir.Equals(EnemyScript.GetFacingDirection())) {
                EnemyScript.Flip();
            }
        }


        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            // if walkTime has passed, and player still unreachable, then change state to Idle State
            if (Time.time >= StartTime + _walkTime) {
                StateMachine.ChangeState(EnemyScript.IdleState);
            }

            // if player in Follow range, then change state to FollowPlayer State
            if (CanFollowPlayer) {
                StateMachine.ChangeState(EnemyScript.FollowPlayerState);
            }

            // if player is in Attack range, then change state to Attack State 
            if (CanAttackPlayer) {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }


        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            // update movement velocity, based on enemyData speed when walking
            EnemyScript.SetVelocityX(EnemyData.MoveSpeed * _direction);
        }
    }
}