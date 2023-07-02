using ScriptableObjects;
using UnityEngine;

namespace Enemy.Fsm.States {
    public abstract class EnemyState {
        protected readonly EnemyScript EnemyScript;
        protected readonly EnemyStateMachine StateMachine;
        protected readonly EnemyDataSo EnemyData;
        protected bool IsAnimationFinished;

        protected float StartTime;
        private readonly string _animBoolName;

        protected EnemyState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) {
            EnemyScript = enemy;
            StateMachine = stateMachine;
            EnemyData = enemyData;
            _animBoolName = animBoolName;
        }

        protected internal virtual void Enter() {
            DoChecks();
            EnemyScript.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationFinished = false;
        }

        protected internal virtual void Exit() {
            EnemyScript.Anim.SetBool(_animBoolName, false);
        }

        protected internal virtual void LogicUpdate() { }

        protected internal virtual void PhysicsUpdate() {
            DoChecks();
        }

        protected virtual void DoChecks() { }

        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}