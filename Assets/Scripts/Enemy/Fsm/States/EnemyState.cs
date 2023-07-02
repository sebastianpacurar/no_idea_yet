using ScriptableObjects;
using UnityEngine;


namespace Enemy.Fsm.States {
    public abstract class EnemyState {
        protected readonly EnemyScript EnemyScript;
        protected readonly EnemyStateMachine StateMachine;
        protected readonly EnemyDataSo EnemyData;

        // used to specify when the Animation finishes, therefore can be used as condition to change state
        protected bool IsAnimationFinished;

        // used to specify at which time the current state started
        protected float StartTime;

        // used to handle animations through states
        private readonly string _animBoolName;

        protected EnemyState(EnemyScript enemy, EnemyStateMachine stateMachine, EnemyDataSo enemyData, string animBoolName) {
            EnemyScript = enemy;
            StateMachine = stateMachine;
            EnemyData = enemyData;
            _animBoolName = animBoolName;
        }

        // execute when entering a state
        protected internal virtual void Enter() {
            DoChecks();
            EnemyScript.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationFinished = false;
        }

        // execute when Exiting a state
        protected internal virtual void Exit() {
            EnemyScript.Anim.SetBool(_animBoolName, false);
        }

        // overrides Update()
        protected internal virtual void LogicUpdate() { }

        // overrides FixedUpdate()
        protected internal virtual void PhysicsUpdate() {
            DoChecks();
        }

        // used to do specific checks regularly. Such vars are declared in Super States
        protected virtual void DoChecks() { }

        // used as a unity animation event, to set IsAnimationFinished when the animation ends
        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}