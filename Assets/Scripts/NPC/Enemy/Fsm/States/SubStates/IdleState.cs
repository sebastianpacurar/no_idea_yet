using NPC.Enemy.Fsm.States.SuperStates;
using ScriptableObjects;
using UnityEngine;


namespace NPC.Enemy.Fsm.States.SubStates
{
    public class IdleState : GroundedState
    {
        private float _idleTime;

        public IdleState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }


        protected internal override void Enter()
        {
            base.Enter();

            // set idleTime based on min and max enemyData values
            var minIdleSeconds = EnemyData.MinMaxIdleTime.x;
            var maxIdleSeconds = EnemyData.MinMaxIdleTime.y;
            _idleTime = Random.Range(minInclusive: minIdleSeconds, maxInclusive: maxIdleSeconds);

            // set velocity to 0
            EnemyScript.SetVelocityX(0f);
        }


        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            // if idleTime has passed, and player still unreachable, then change state to Walk State
            if (Time.time >= StartTime + _idleTime)
            {
                StateMachine.ChangeState(EnemyScript.WalkState);
            }

            // if player in Follow range, then change state to FollowPlayer State
            if (CanFollowPlayer)
            {
                StateMachine.ChangeState(EnemyScript.FollowPlayerState);
            }

            // if player is in Attack range, then change state to Attack State 
            if (CanAttackPlayer)
            {
                StateMachine.ChangeState(EnemyScript.AttackState);
            }
        }
    }
}