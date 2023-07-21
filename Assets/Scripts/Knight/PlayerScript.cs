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
        #endregion

        #region Misc Vars
        public float CurrentSpeed { get; set; }
        public Vector2 CurrentVelocity { get; set; }
        private Vector2 _vector2Holder;
        private Vector3 _vector3Holder;
        #endregion


        [Header("Aim Trajectory")]
        [SerializeField] private LineRenderer predictionLineRenderer;
        [SerializeField] private int lineSegmentCount;
        [SerializeField] private float lineSegmentSpacing;
        [SerializeField] private float lineGravity;

        [Header("Debug")]
        [SerializeField] private Vector2 throwForce;
        [SerializeField] private bool isCarrying;


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

            ValidateCarryDistance();

            if (Input.IsPickCratePressed && !CheckIfCanGrabCrate()) {
                Input.IsPickCratePressed = false;
            }

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        // set crate data for crate in range with Label collider
        private void OnTriggerStay2D(Collider2D other) {
            // if parent of Label is Crate
            if (other.transform.parent.CompareTag("Crate")) {
                // save crate's script, transform and rigidbody
                var targetObject = other.transform.parent.gameObject;
                _crateTransform = targetObject.transform;
                crateScript = targetObject.GetComponentInChildren<CrateScript>();
                _crateRb = targetObject.GetComponentInChildren<Rigidbody2D>();

                SetCrateData(_crateTransform, crateScript, _crateRb);
            }
        }

        // unset crate data for current crate out of range of Label collider
        private void OnTriggerExit2D(Collider2D other) {
            // if parent of Label is Crate
            if (other.transform.parent.CompareTag("Crate")) {
                var targetObject = other.transform.parent.gameObject;

                // if the collision object is the same with the assigned script then perform cleanup
                //   set null only if crate is not carried
                if (targetObject.GetComponentInChildren<CrateScript>().Equals(crateScript) && !isCarrying) {
                    UnsetCrateData();
                }
            }
        }
        #endregion


        #region Player Set Functions
        // stop movement
        public void SetVelocityX(float velocity) {
            _vector2Holder.Set(velocity, CurrentVelocity.y);
            _rb.velocity = _vector2Holder;
            CurrentVelocity = _vector2Holder;
        }


        // increase/decrease throw force vector => x = y /2, therefore Vector2(y/2, y)
        private void SetAimTrajectory() {
            var threshold = playerData.MinMaxAimRange;
            var aimSpeed = playerData.AimSpeed;

            throwForce.x += Input.AimVal * aimSpeed / 2 * Time.deltaTime;
            throwForce.x = Mathf.Clamp(throwForce.x, threshold.x / 2, threshold.y / 2);

            throwForce.y += Input.AimVal * aimSpeed * Time.deltaTime;
            throwForce.y = Mathf.Clamp(throwForce.y, threshold.x, threshold.y);
        }


        // perform jump
        public void AddJumpForce(float force) {
            _vector2Holder.Set(0f, force);
            _rb.AddForce(_vector2Holder, ForceMode2D.Impulse);
            CurrentVelocity = _rb.velocity;
        }


        // set jump input to false
        public void SetJumpFalse() => Input.IsJumpPressed = false;


        // if Pick Crate input is true, then set to false 
        public void SetPickUpFalse() {
            if (Input.IsPickCratePressed) {
                Input.IsPickCratePressed = false;
            }
        }

        
        public void SetLineRendererActive(bool value) {
            predictionLineRenderer.enabled = value;
        }


        // place crate on top of player using transform
        public void SetCrateOnPlayer() {
            // set crate velocity to 0
            _vector2Holder.Set(0f, 0f);
            _crateRb.velocity = _vector2Holder;

            // set crate position on top of the player
            var pos = transform.position;
            var offset = 1.25f;
            _vector3Holder.Set(pos.x, pos.y + offset, pos.z);
            _crateTransform.position = _vector3Holder;
        }


        public void SetCrateVelToPlayerVel() {
            var xVal = CurrentVelocity.x > 0 ? -0.1f : 0.1f;

            _vector2Holder.Set(CurrentVelocity.x + xVal, CurrentVelocity.y);
            _crateRb.velocity = _vector2Holder;
        }


        public void SetCrateVelToZero() {
            _crateRb.velocity = Vector2.zero;
        }


        //
        // Crate Related

        private void SetPlayerCarry(bool value) => isCarrying = value;
        private void SetCrateCarry(bool value) => crateScript.isCarried = value;
        private void SetThrowTrue() => crateScript.isBeingThrown = true;

        
        // set isCarrying and isCarried to false
        public void SetCarryProps(bool value) {
            SetPlayerCarry(value);
            SetCrateCarry(value);
        }

        
        // set crate in range
        private void SetCrateData(Transform t, CrateScript s, Rigidbody2D rb) {
            _crateTransform = t;
            crateScript = s;
            _crateRb = rb;
        }


        // unset crate in range
        private void UnsetCrateData() {
            _crateTransform = null;
            crateScript = null;
            _crateRb = null;
        }


        // set carry props to false, and unset crate data
        private void CleanUpCrateData() {
            SetCarryProps(false);
            UnsetCrateData();
        }

        
        // if distance between player and carried crate is bigger than 2.5f, then reset current carry state
        public void ValidateCarryDistance() {
            if (CheckPlayerCarry()) {
                if (Vector2.Distance(transform.position, _crateTransform.position) > 2.5f) {
                    CleanUpCrateData();
                }
            }
        }
        #endregion


        #region Check Functions
        public bool CheckIfGrounded() {
            var pos = groundChecker.position;
            var size = new Vector2(1.1f, 0.8f);
            var direction = CapsuleDirection2D.Horizontal;
            var angle = 0f;

            return Physics2D.OverlapCapsule(pos, size, direction, angle, groundLayer | pushableLayer);
        }


        public void CheckIfShouldFlip(int xInput) {
            if (xInput != 0 && !CheckIfFacingInputDirection(xInput)) {
                Flip();
            }
        }

        public bool CheckPlayerCarry() => isCarrying;


        private bool CheckIfFacingInputDirection(int xInput) {
            return xInput == GetFacingDirection();
        }


        private bool CheckIfCanGrabCrate() {
            return crateScript;
        }
        #endregion


        #region Misc Functions
        public void ThrowCrate() {
            SetThrowTrue();
            SetCarryProps(false);
            SetCrateVelToZero();

            _vector2Holder.Set(GetFacingDirection() * throwForce.x, throwForce.y);
            _crateRb.AddForce(_vector2Holder, ForceMode2D.Impulse);
        }


        public void GeneratePredictionLine() {
            var startPos = _crateRb.position;
            _vector2Holder.Set(GetFacingDirection() * throwForce.x, throwForce.y);

            SetLineRendererActive(true);
            predictionLineRenderer.positionCount = lineSegmentCount;
            predictionLineRenderer.SetPositions(CalculatePredictionLinePoints(startPos, _vector2Holder));

            SetAimTrajectory();
        }


        private Vector3[] CalculatePredictionLinePoints(Vector2 startPos, Vector2 force) {
            var linePoints = new Vector3[lineSegmentCount];

            var currentPos = startPos;
            var currentVelocity = force;
            var gravityScale = _crateRb.gravityScale;
            var mass = _crateRb.mass;

            for (var i = 0; i < lineSegmentCount; i++) {
                linePoints[i] = currentPos;

                // NOTE: formula to blend in gravity with mass is: (lineGravity * gravityScale) * (Mathf.Pow(mass, 2))
                currentVelocity += Vector2.down * (lineGravity * gravityScale * Mathf.Pow(mass, 2) * lineSegmentSpacing);
                currentPos += currentVelocity * lineSegmentSpacing;
            }

            return linePoints;
        }


        private int GetFacingDirection() {
            return transform.rotation.eulerAngles.y switch {
                0f => 1,
                180f => -1,
                _ => 0
            };
        }


        private void Flip() {
            var currRot = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currRot.Equals(0) ? 180f : 0f, 0f);
        }


        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}