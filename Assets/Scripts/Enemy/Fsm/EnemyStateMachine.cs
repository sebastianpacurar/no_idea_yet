using Enemy.Fsm.States;


namespace Enemy.Fsm {
    public class EnemyStateMachine {
        // the state which will be executed during runtime
        public EnemyState CurrentState { get; private set; }

        
        // used only first time, to initialize first state (this case to Idle State)
        public void Initialize(EnemyState startingState) {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        
        // used to change state if a specific condition is met
        public void ChangeState(EnemyState newState) {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}