using System;
using Input;
using Knight.Fsm;
using Knight.Fsm.States.SubStates;
using Prop.Interactables.Crate;
using ScriptableObjects;
using UnityEngine;


namespace Knight
{
    public class PlayerScript : MonoBehaviour
    {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public RunState RunState { get; private set; }
        public InAirState InAirState { get; private set; }
        public JumpState JumpState { get; private set; }
        public CrouchIdleState CrouchIdleState { get; private set; }
        public CrouchWalkState CrouchWalkState { get; private set; }
        public CarryIdleState CarryIdleState { get; private set; }
        public CarryWalkState CarryWalkState { get; private set; }

        public PlayerDataSo playerData;
        #endregion

        #region References
        [SerializeField] private Transform topGroundChecker;
        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask pushableLayer;
        #endregion


        #region Components
        public InputManager Input { get; private set; }
        public Animator Anim { get; private set; }
        private Rigidbody2D _rb;
        private PlayerColliderScript _colliderScript;
        #endregion

        #region Crate Components
        [SerializeField] private CrateScript crateScript;
        private Transform _crateTransform;
        private Rigidbody2D _crateRb;
        private FixedJoint2D _crateJoint;
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
        private void Awake()
        {
            // instantiate state machine
            StateMachine = new PlayerStateMachine();

            // initiate states
            IdleState = new IdleState(this, StateMachine, playerData, "idle");
            RunState = new RunState(this, StateMachine, playerData, "run");
            InAirState = new InAirState(this, StateMachine, playerData, "inAir");
            JumpState = new JumpState(this, StateMachine, playerData, "inAir");
            CrouchIdleState = new CrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchWalkState = new CrouchWalkState(this, StateMachine, playerData, "crouchWalk");
            CarryIdleState = new CarryIdleState(this, StateMachine, playerData, "carryIdle");
            CarryWalkState = new CarryWalkState(this, StateMachine, playerData, "carryWalk");

            // get components
            _rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            _colliderScript = GetComponent<PlayerColliderScript>();
        }

        private void Start()
        {
            Input = InputManager.Instance;

            // start in Idle State
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            CurrentVelocity = _rb.velocity;

            if (Input.IsPickCratePressed && !CheckIfCanGrabCrate())
            {
                Input.IsPickCratePressed = false;
            }

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }


        // set crate data for crate in range with Label collider
        private void OnTriggerStay2D(Collider2D other)
        {
            // if parent of Label is Crate
            if (other.transform.parent.CompareTag("Crate"))
            {
                // save crate's script, transform, rigidbody and fixed joint
                var targetObject = other.transform.parent.gameObject;
                var t = targetObject.transform;
                var s = targetObject.GetComponentInChildren<CrateScript>();
                var rb = targetObject.GetComponentInChildren<Rigidbody2D>();
                var fj = targetObject.GetComponent<FixedJoint2D>();

                SetCrateInRange(t, s, rb, fj);
            }
        }


        // unset crate data for current crate out of range of Label collider
        private void OnTriggerExit2D(Collider2D other)
        {
            // if parent of Label is Crate
            if (other.transform.parent.CompareTag("Crate"))
            {
                var targetObject = other.transform.parent.gameObject;

                // if the collision object is the same with the assigned script then perform cleanup
                //   set null only if crate is not carried
                if (targetObject.GetComponentInChildren<CrateScript>().Equals(crateScript) && !isCarrying)
                {
                    UnsetCrateInRange();
                }
            }
        }
        #endregion


        #region Player Set Functions
        // stop movement
        public void SetVelocityX(float velocity)
        {
            _vector2Holder.Set(velocity, CurrentVelocity.y);
            _rb.velocity = _vector2Holder;
            CurrentVelocity = _vector2Holder;
        }


        // increase/decrease throw force vector => x = y /2, therefore Vector2(y/2, y)
        private void SetAimTrajectory()
        {
            var threshold = crateScript.AimRangeValue;
            var aimSpeed = playerData.AimSpeed;

            if (Input.IsShiftAimPressed)
            {
                aimSpeed *= playerData.AimMultiplier;
            }

            throwForce.x += Input.AimVal * aimSpeed / 2 * Time.deltaTime;
            throwForce.x = Mathf.Clamp(throwForce.x, threshold.x / 2, threshold.y / 2);

            throwForce.y += Input.AimVal * aimSpeed * Time.deltaTime;
            throwForce.y = Mathf.Clamp(throwForce.y, threshold.x, threshold.y);
        }


        // perform jump
        public void AddJumpForce(float force)
        {
            _vector2Holder.Set(0f, force);
            _rb.AddForce(_vector2Holder, ForceMode2D.Impulse);
            CurrentVelocity = _rb.velocity;
        }


        // set jump input to false
        public void SetJumpFalse() => Input.IsJumpPressed = false;


        // set pick crate to false if true
        public void SetPickUpFalse()
        {
            if (Input.IsPickCratePressed)
            {
                Input.IsPickCratePressed = false;
            }
        }

        // set crouch to false if true
        public void SetCrouchInputFalse()
        {
            if (Input.IsCrouchPressed)
            {
                Input.IsCrouchPressed = false;
            }
        }


        public void SetLineRendererActive(bool value)
        {
            predictionLineRenderer.enabled = value;
        }
        #endregion


        #region Crate Set Functionw
        private void SetPlayerCarry(bool value) => isCarrying = value;
        private void SetCrateCarry(bool value) => crateScript.isCarried = value;
        private void SetThrowTrue() => crateScript.isBeingThrown = true;


        // set isCarrying and isCarried to false
        public void SetCarryProps(bool value)
        {
            SetPlayerCarry(value);
            SetCrateCarry(value);
        }


        // active during Carry states
        private void EnableFixedJoint()
        {
            _crateJoint.enabled = true;
            _crateJoint.anchor = crateScript.FixedJointAnchor;
            _crateJoint.connectedAnchor = new Vector2(0, 0f);
            _crateJoint.connectedBody = _rb;
        }


        // attach crate based on transform on top of player and attach fixed joint
        public void AttachCrateToPlayer()
        {
            PlaceCrateOnPlayer();
            EnableFixedJoint();
        }


        // set crate speed to 0
        private void SetCrateVelToZero()
        {
            _crateRb.velocity = Vector2.zero;
        }


        // place crate above player based on transform
        private void PlaceCrateOnPlayer()
        {
            // set crate velocity to 0
            _vector2Holder.Set(0f, 0f);
            _crateRb.velocity = _vector2Holder;

            // set crate position on top of the player, based on the crate posOffset
            var pos = transform.position;
            var offset = crateScript.AttachPosOffset;
            _vector3Holder.Set(pos.x, pos.y + offset, pos.z);
            _crateTransform.position = _vector3Holder;
        }


        // set crate in range
        private void SetCrateInRange(Transform t, CrateScript s, Rigidbody2D rb, FixedJoint2D fj)
        {
            _crateTransform = t;
            crateScript = s;
            _crateRb = rb;
            _crateJoint = fj;
        }


        // unset crate in range
        private void UnsetCrateInRange()
        {
            _crateTransform = null;
            crateScript = null;
            _crateRb = null;

            // fixed joint related
            _crateJoint.connectedBody = null; //clear on crate side
            _crateJoint.enabled = false; // clear on crate side
            _crateJoint = null; // clear on player side
        }


        // set carry props to false, and unset crate data
        private void CleanUpCrateData()
        {
            SetCarryProps(false);
            UnsetCrateInRange();
        }


        // if distance between player and carried crate is bigger than crate GripDistance, then release crate and cleanup data
        public void ValidateCarryDistance()
        {
            if (CheckIfCarriedCrateHangsOnEdge())
            {
                CleanUpCrateData();
            }
        }
        #endregion


        #region Check Functions
        // check if player grounded on Ground and Pushable layers
        public bool CheckIfGrounded()
        {
            var pos = groundChecker.position;
            _vector2Holder.Set(0.5f, 0.3f);
            var size = _vector2Holder;

            return Physics2D.OverlapCapsule(pos, size, CapsuleDirection2D.Horizontal, 0f, groundLayer | pushableLayer);
        }


        // check if there is space to uncrouch
        public bool CheckIfTopGrounded()
        {
            var pos = topGroundChecker.position;
            _vector2Holder.Set(1.115f, 1f);
            var size = _vector2Holder;

            return Physics2D.OverlapCapsule(pos, size, CapsuleDirection2D.Horizontal, 0f, groundLayer);
        }


        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && !CheckIfFacingInputDirection(xInput))
            {
                Flip();
            }
        }


        public bool CheckPlayerTransition()
        {
            return _colliderScript.isTransitioning;
        }


        public bool CheckPlayerCarry() => isCarrying;


        // check if carried crate is blocked by anything while in air
        private bool CheckIfCarriedCrateHangsOnEdge()
        {
            try
            {
                return crateScript.isGrounded || crateScript.HasBottomCrate();
            } catch (NullReferenceException)
            {
                return false;
            }
        }


        private bool CheckIfFacingInputDirection(int xInput)
        {
            return xInput == GetFacingDirection();
        }


        private bool CheckIfCanGrabCrate()
        {
            return crateScript;
        }
        #endregion


        #region Misc Functions
        public void ThrowCrate()
        {
            SetThrowTrue();
            SetCrateVelToZero();

            _vector2Holder.Set(GetFacingDirection() * throwForce.x, throwForce.y);
            _crateRb.AddForce(_vector2Holder, ForceMode2D.Impulse);

            CleanUpCrateData();
        }


        public void GeneratePredictionLine()
        {
            var startPos = _crateTransform.position;
            _vector2Holder.Set(GetFacingDirection() * throwForce.x, throwForce.y);

            SetLineRendererActive(true);
            predictionLineRenderer.positionCount = lineSegmentCount;
            predictionLineRenderer.SetPositions(CalculatePredictionLinePoints(startPos, _vector2Holder));

            SetAimTrajectory();
        }


        private Vector3[] CalculatePredictionLinePoints(Vector2 startPos, Vector2 force)
        {
            var linePoints = new Vector3[lineSegmentCount];

            var currentPos = startPos;
            var currentVelocity = force;
            var gravityScale = _crateRb.gravityScale;
            var mass = _crateRb.mass;

            for (var i = 0; i < lineSegmentCount; i++)
            {
                linePoints[i] = currentPos;

                // NOTE: formula to blend in gravity with mass is: (lineGravity * gravityScale) * (Mathf.Pow(mass, 2))
                currentVelocity += Vector2.down * (lineGravity * gravityScale * Mathf.Pow(mass, 2) * lineSegmentSpacing);
                currentPos += currentVelocity * lineSegmentSpacing;
            }

            return linePoints;
        }


        private int GetFacingDirection()
        {
            return transform.rotation.eulerAngles.y switch
            {
                0f => 1,
                180f => -1,
                _ => 0
            };
        }


        private void Flip()
        {
            var currRot = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currRot.Equals(0) ? 180f : 0f, 0f);
        }


        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}