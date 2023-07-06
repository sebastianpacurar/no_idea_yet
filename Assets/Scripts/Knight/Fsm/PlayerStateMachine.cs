using Knight.Fsm.States;

namespace Knight.Fsm {
    public class PlayerStateMachine {
        // the state which will be executed during runtime
        public PlayerState CurrentState { get; private set; }


        // used only first time, to initialize first state (this case to Idle State)
        public void Initialize(PlayerState startingState) {
            CurrentState = startingState;
            CurrentState.Enter();
        }


        // used to change state if a specific condition is met
        public void ChangeState(PlayerState newState) {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}