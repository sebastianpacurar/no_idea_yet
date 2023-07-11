using ScriptableObjects;
using UnityEngine;

namespace Knight.Fsm.States {
    public class PlayerState {
        protected readonly PlayerScript PlayerScript;
        protected readonly PlayerStateMachine StateMachine;
        protected readonly PlayerDataSo PlayerData;

        // used to specify when the Animation finishes, therefore can be used as condition to change state
        protected bool IsAnimationFinished;

        // used to specify at which time the current state started
        protected float StartTime;

        // used to handle animations through states
        private readonly string _animBoolName;

        protected PlayerState(PlayerScript player, PlayerStateMachine stateMachine, PlayerDataSo playerData, string animBoolName) {
            PlayerScript = player;
            StateMachine = stateMachine;
            PlayerData = playerData;
            _animBoolName = animBoolName;
        }

        // execute when entering a state
        protected internal virtual void Enter() {
            DoChecks();
            PlayerScript.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationFinished = false;
        }

        // execute when Exiting a state
        protected internal virtual void Exit() {
            PlayerScript.Anim.SetBool(_animBoolName, false);
        }

        // overrides Update()
        protected internal virtual void LogicUpdate() { }

        // overrides FixedUpdate()
        protected internal virtual void PhysicsUpdate() {
            DoChecks();
        }

        // used to do specific checks regularly. Such vars are declared in Super States
        protected virtual void DoChecks() { }

        // // used as a unity animation event, to trigger an animation
        // public virtual void AnimationTrigger() { }

        // used as a unity animation event, to set IsAnimationFinished when the animation ends
        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}