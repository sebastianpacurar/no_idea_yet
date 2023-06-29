using ScriptableObjects;
using UnityEngine;

namespace Enemy.Fsm.States {
    public abstract class EnemyState {
        protected readonly EnemyScript enemyScript;
        protected readonly EnemyStateMachine stateMachine;
        protected readonly EnemyDataSo enemyData;
        protected bool isAnimationFinished;

        protected float startTime;
        private readonly string _animBoolName;

        protected EnemyState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) {
            enemyScript = enemy;
            this.stateMachine = stateMachine;
            this.enemyData = enemyData;
            _animBoolName = animBoolName;
        }

        protected internal virtual void Enter() {
            DoChecks();
            enemyScript.Anim.SetBool(_animBoolName, true);
            startTime = Time.time;
            isAnimationFinished = false;
        }

        protected internal void Exit() {
            enemyScript.Anim.SetBool(_animBoolName, false);
        }

        protected internal virtual void LogicUpdate() { }

        protected internal virtual void PhysicsUpdate() {
            DoChecks();
        }

        protected virtual void DoChecks() { }

        public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    }
}