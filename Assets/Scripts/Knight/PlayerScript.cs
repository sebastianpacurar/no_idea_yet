using System;
using Input;
using Knight.Fsm;
using Knight.Fsm.States.SubStates;
using Prop.Interactables.Crate;
using ScriptableObjects;
using UnityEngine;


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
        [SerializeField] private CrateScript crateScript;
        private Transform _crateTransform;
        private Rigidbody2D _crateRb;
        public bool isCarryingCrate;
        #endregion

        #region Misc Vars
        public float CurrentSpeed { get; set; }
        public Vector2 CurrentVelocity { get; set; }
        private Vector2 _vectorHolder;
        #endregion

        [SerializeField] private Vector2 throwForce;

        [Header("Aim Trajectory")]
        [SerializeField] private LineRenderer predictionLineRenderer;
        [SerializeField] private int lineSegmentCount;
        [SerializeField] private float lineSegmentSpacing;
        [SerializeField] private float lineGravity;

        [SerializeField] private float aimChangeSpeed;
        [SerializeField] private Vector2 thresholdY;


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
                _crateTransform = targetObject.transform;
                crateScript = targetObject.GetComponentInChildren<CrateScript>();
                _crateRb = targetObject.GetComponentInChildren<Rigidbody2D>();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            // grab the target obj, and the crate script
            if (other.transform.parent.CompareTag("Crate")) {
                var targetObject = other.transform.parent.gameObject;

                if (targetObject.GetComponentInChildren<CrateScript>().Equals(crateScript)) {
                    _crateTransform = null;
                    crateScript = null;
                    _crateRb = null;
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

        // increase/decrease throw force vector => x = y /2
        public void SetAimTrajectory() {
            throwForce.x += Input.AimVal * aimChangeSpeed / 2 * Time.deltaTime;
            throwForce.x = Mathf.Clamp(throwForce.x, thresholdY.x / 2, thresholdY.y / 2);

            throwForce.y += Input.AimVal * aimChangeSpeed * Time.deltaTime;
            throwForce.y = Mathf.Clamp(throwForce.y, thresholdY.x, thresholdY.y);
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

        public void SetCrateIsThrown(bool value) {
            crateScript.isBeingThrown = value;
        }

        public void SetCrateIsCarried(bool value) {
            crateScript.isBeingCarried = value;
        }


        public void SetIsCarryingCrate(bool value) {
            isCarryingCrate = value;
        }


        public void SetCratePosition() {
            var pos = transform.position;
            _crateTransform.position = new Vector3(pos.x, pos.y + 1.25f, pos.z);
        }

        public void SetCrateVelocityToZero() {
            _vectorHolder.Set(0f, 0f);
            _crateRb.velocity = _vectorHolder;
        }

        public void SetLineRendererActive(bool value) {
            predictionLineRenderer.enabled = value;
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
            SetCrateIsThrown(true);
            SetCrateIsCarried(false);
            SetIsCarryingCrate(false);

            _vectorHolder.Set(GetFacingDirection() * throwForce.x, throwForce.y);
            _crateRb.AddForce(_vectorHolder, ForceMode2D.Impulse);
        }


        //TODO: needs more tweak
        public void GeneratePredictionLine() {
            var startPos = _crateRb.position;
            _vectorHolder.Set(GetFacingDirection() * throwForce.x, throwForce.y);

            SetLineRendererActive(true);
            predictionLineRenderer.positionCount = lineSegmentCount;
            predictionLineRenderer.SetPositions(CalculatePredictionLinePoints(startPos, _vectorHolder));
        }

        // TODO: isolate in helpers
        private Vector3[] CalculatePredictionLinePoints(Vector2 startPos, Vector2 force) {
            var linePoints = new Vector3[lineSegmentCount];

            var currentPos = startPos;
            var currentVelocity = force;
            var gravity = _crateRb.gravityScale;
            var mass = _crateRb.mass;

            for (int i = 0; i < lineSegmentCount; i++) {
                linePoints[i] = currentPos;

                // NOTE: formula to blend in gravity with mass is:   gravity * (Mathf.Pow(mass, 2))
                currentVelocity += Vector2.down * (lineGravity * gravity * (Mathf.Pow(mass, 2)) * lineSegmentSpacing);
                currentPos += currentVelocity * lineSegmentSpacing;
            }

            return linePoints;
        }


        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}