using Input;
using Knight.Fsm;
using Knight.Fsm.States.SubStates;
using Prop.Interactables.Crate;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Knight {
    public class PlayerScript : MonoBehaviour {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public RunState RunState { get; private set; }
        public SprintState SprintState { get; private set; }
        public InAirState InAirState { get; private set; }
        public JumpState JumpState { get; private set; }
        public CrouchIdleState CrouchIdleState { get; private set; }
        public CrouchWalkState CrouchWalkState { get; private set; }
        public CarryIdleState CarryIdleState { get; private set; }
        public CarryWalkState CarryWalkState { get; private set; }

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

        #region Crate Components
        public Transform crateTransform;
        public CrateScript crateScript;
        public Rigidbody2D crateRb;
        #endregion

        #region Misc Vars
        public float CurrentSpeed { get; set; }
        public Vector2 CurrentVelocity { get; set; }
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
            CrouchIdleState = new CrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchWalkState = new CrouchWalkState(this, StateMachine, playerData, "crouchWalk");
            CarryIdleState = new CarryIdleState(this, StateMachine, playerData, "carryIdle");
            CarryWalkState = new CarryWalkState(this, StateMachine, playerData, "carryWalk");

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
            CurrentVelocity = _rb.velocity;

            if (Input.IsPickCratePressed && !CheckIfCanGrabCrate()) {
                Input.IsPickCratePressed = false;
            }

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // grab the target obj, and the crate script
            if (other.transform.parent.CompareTag("Crate")) {
                var targetObject = other.transform.parent.gameObject;
                crateTransform = targetObject.transform;
                crateScript = targetObject.GetComponentInChildren<CrateScript>();
                crateRb = targetObject.GetComponentInChildren<Rigidbody2D>();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            // grab the target obj, and the crate script
            if (other.transform.parent.CompareTag("Crate")) {
                var targetObject = other.transform.parent.gameObject;

                if (targetObject.GetComponentInChildren<CrateScript>().Equals(crateScript)) {
                    crateTransform = null;
                    crateScript = null;
                    crateRb = null;
                }
            }
        }
        #endregion


        #region Set Functions
        // stop movement
        public void SetVelocityX(float velocity) {
            _vectorHolder.Set(velocity, CurrentVelocity.y);
            _rb.velocity = _vectorHolder;
            CurrentVelocity = _vectorHolder;
        }

        public void AddJumpForce(float force) {
            _vectorHolder.Set(0f, force);
            _rb.AddForce(_vectorHolder, ForceMode2D.Impulse);
            CurrentVelocity = _rb.velocity;
        }

        public void SetJumpFalse() {
            Input.IsJumpPressed = false;
        }

        public void SetPickUpFalse() {
            if (Input.IsPickCratePressed) {
                Input.IsPickCratePressed = false;
            }
        }

        public void SetCrateIsCarried(bool value) {
            crateScript.isBeingCarried = value;
        }

        public void SetCratePosition() {
            var pos = transform.position;
            crateTransform.position = new Vector3(pos.x, pos.y + 1f, pos.z);
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

        public bool CheckIfCanGrabCrate() {
            return crateScript;
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

        public void ThrowCrate() {
            SetCrateIsCarried(false);

            _vectorHolder.Set(GetFacingDirection() * 50f, 75f);
            crateRb.AddForce(_vectorHolder, ForceMode2D.Impulse);
        }


        // public virtual void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();


        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}