using NPC.Enemy.Fsm.States.SuperStates;
using ScriptableObjects;


namespace NPC.Enemy.Fsm.States.SubStates
{
    public class DeathState : GroundedState
    {
        public DeathState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName) { }


        protected internal override void Enter()
        {
            base.Enter();

            // set move to 0 when entering Death State
            EnemyScript.SetVelocityX(0f);
        }


        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            // destroy game object after Death Animation ends
            if (IsAnimationFinished)
            {
                EnemyScript.DestroyEnemy();
            }
        }
    }
}