using Input;
using Knight.Fsm;
using Knight.Fsm.States.SubStates;
using ScriptableObjects;
using UnityEngine;
using IdleState = Knight.Fsm.States.SubStates.IdleState;

namespace Knight {
    public class PlayerScript : MonoBehaviour {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public RunState RunState { get; private set; }
        public SprintState SprintState { get; private set; }
        public InAirState InAirState { get; private set; }
        public JumpState JumpState { get; private set; }
        // public WalkState WalkState { get; private set; }
        // public AttackState AttackState { get; private set; }
        // public DeathState DeathState { get; private set; }
        // public FollowPlayerState FollowPlayerState { get; private set; }
        // public TakeHitState TakeHitState { get; private set; }

        public PlayerDataSo playerData;
        #endregion

        #region References
        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask pushableLayer;
        #endregion


        #region Components
        public InputManager Input { get; private set; }
        public Animator Anim { get; private set; }
        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;
        #endregion

        #region Misc Vars
        public Vector2 currentVelocity;
        private Vector2 _vectorHolder;
        #endregion


        #region Unity Callback Functions
        private void Awake() {
            // instantiate state machine
            StateMachine = new PlayerStateMachine();

            // initiate states
            IdleState = new IdleState(this, StateMachine, playerData, "idle");
            RunState = new RunState(this, StateMachine, playerData, "run");
            SprintState = new SprintState(this, StateMachine, playerData, "sprint");
            InAirState = new InAirState(this, StateMachine, playerData, "inAir");
            JumpState = new JumpState(this, StateMachine, playerData, "inAir");

            // WalkState = new WalkState(this, StateMachine, enemyData, "walk");
            // FollowPlayerState = new FollowPlayerState(this, StateMachine, enemyData, "follow");
            // AttackState = new AttackState(this, StateMachine, enemyData, "attack");
            // TakeHitState = new TakeHitState(this, StateMachine, enemyData, "takeHit");
            // DeathState = new DeathState(this, StateMachine, enemyData, "death");

            // get components
            _rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
        }

        private void Start() {
            Input = InputManager.Instance;

            // start in Idle State
            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            currentVelocity = _rb.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // prevent re-triggering hit state if dead
        }
        #endregion


        #region Set Functions
        // stop movement
        public void SetVelocityX(float velocity) {
            _vectorHolder.Set(velocity, currentVelocity.y);
            _rb.velocity = _vectorHolder;
            currentVelocity = _vectorHolder;
        }

        public void SetVelocityY(float velocity) {
            _vectorHolder.Set(currentVelocity.x, velocity);
            _rb.velocity = _vectorHolder;
            currentVelocity = _vectorHolder;
        }

        public void AddJumpForce(float force) {
            _vectorHolder.Set(0f, force);
            _rb.AddForce(_vectorHolder, ForceMode2D.Impulse);
            currentVelocity = _rb.velocity;
        }

        public void SetJumpFalse() {
            Input.IsJumpPressed = false;
        }
        #endregion


        #region Check Functions
        public bool CheckIfGrounded() {
            var pos = groundChecker.position;
            var size = new Vector2(1.1f, 0.2f);
            var direction = CapsuleDirection2D.Horizontal;
            var angle = 0f;

            return Physics2D.OverlapCapsule(pos, size, direction, angle, groundLayer | pushableLayer);
        }


        public void CheckIfShouldFlip(int xInput) {
            if (xInput != 0 && !CheckIfFacingInputDirection(xInput)) {
                Flip();
            }
        }


        public bool CheckIfFacingInputDirection(int xInput) {
            return xInput == GetFacingDirection();
        }
        #endregion


        #region Misc Functions
        public int GetFacingDirection() {
            return transform.rotation.eulerAngles.y switch {
                0f => 1,
                180f => -1,
                _ => 0
            };
        }


        public void Flip() {
            var currRot = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currRot.Equals(0) ? 180f : 0f, 0f);
        }


        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}