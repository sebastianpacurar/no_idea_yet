using NPC.Enemy.Fsm;
using NPC.Enemy.Fsm.States.SubStates;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace NPC.Enemy
{
    public class EnemyScript : MonoBehaviour
    {
        #region State Variables
        public EnemyStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkState WalkState { get; private set; }
        public AttackState AttackState { get; private set; }
        public DeathState DeathState { get; private set; }
        public FollowPlayerState FollowPlayerState { get; private set; }
        public TakeHitState TakeHitState { get; private set; }

        public EnemyDataSo enemyData;
        #endregion

        #region Components
        public Animator Anim { get; private set; }
        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;
        #endregion

        #region Misc Vars
        public Vector2 CurrentVelocity;
        [SerializeField] private float minIntensity;

        [Header("For Debugging")]
        [SerializeField] private int livesLeft;
        [SerializeField] private bool isHit;
        [SerializeField] private Vector3 dirToPlayer;
        [SerializeField] private float alignX;

        private RaycastHit2D _detectPlayerAttackHit;
        private RaycastHit2D _detectPlayerFollowHit;
        private Transform _playerPos;
        private LayerMask _playerLayer;

        private Light2D _globalLight;
        #endregion


        #region Unity Callback Functions
        private void Awake()
        {
            // instantiate state machine
            StateMachine = new EnemyStateMachine();

            // initiate states
            IdleState = new IdleState(this, StateMachine, enemyData, "idle");
            WalkState = new WalkState(this, StateMachine, enemyData, "walk");
            FollowPlayerState = new FollowPlayerState(this, StateMachine, enemyData, "follow");
            AttackState = new AttackState(this, StateMachine, enemyData, "attack");
            TakeHitState = new TakeHitState(this, StateMachine, enemyData, "takeHit");
            DeathState = new DeathState(this, StateMachine, enemyData, "death");

            // get components
            _rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            _playerLayer = LayerMask.GetMask("Character");
        }

        private void Start()
        {
            _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
            livesLeft = enemyData.Lives;

            // start in Idle State
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            // will be used globally
            CurrentVelocity = _rb.velocity;

            var pos = transform.position;
            dirToPlayer = (_playerPos.position - pos).normalized;

            // cast rays for player detection and attack
            _detectPlayerFollowHit = Physics2D.Raycast(pos, dirToPlayer, enemyData.FollowDistance, _playerLayer);
            _detectPlayerAttackHit = Physics2D.Raycast(pos, dirToPlayer, enemyData.AttackDistance, _playerLayer);

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // prevent re-triggering hit state if dead
            if (CheckIfDead()) return;

            if (other.gameObject.CompareTag("PlayerHitPoint"))
            {
                isHit = true;
            }
        }
        #endregion


        #region Set Functions
        // stop movement
        public void SetVelocityX(float velocity)
        {
            _rb.velocity = new Vector2(velocity, CurrentVelocity.y);
            CurrentVelocity = _rb.velocity;
        }

        public void SetHitFalse()
        {
            isHit = false;
        }

        // gets pushed after hit by Player HitPoint object
        public void ApplyKnockBackForce()
        {
            var xForce = enemyData.KnockBackForce.x * -GetFacingDirection();
            var yForce = enemyData.KnockBackForce.y;
            _rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
        }

        // decrease life
        public void DecrementLife()
        {
            livesLeft -= 1;
        }
        #endregion


        #region Check Functions
        public void CheckIfShouldFlip()
        {
            if (_detectPlayerFollowHit)
            {
                alignX = Vector2.Dot(dirToPlayer, transform.right);

                if (alignX < 0f)
                {
                    Flip();
                }
            }
        }

        public bool CheckIfCanAttackPlayer()
        {
            return _detectPlayerAttackHit;
        }

        public bool CheckIfCanFollowPlayer()
        {
            return _detectPlayerFollowHit;
        }

        public bool CheckIfLateNightInterval()
        {
            return _globalLight.intensity <= minIntensity;
        }

        public bool CheckIfDead()
        {
            return livesLeft <= 0;
        }

        public bool CheckIfHit()
        {
            return isHit;
        }
        #endregion


        #region Misc Functions
        public float GetFacingDirection()
        {
            return transform.rotation.eulerAngles.y switch
            {
                0f => 1f,
                180f => -1f,
                _ => 0f
            };
        }

        public void Flip()
        {
            var currRot = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currRot.Equals(0) ? 180f : 0f, 0f);
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion


        #region Debug section
        private void OnDrawGizmos()
        {
            var pos = transform.position;
            var followX = GetFacingDirection().Equals(1) ? pos.x + enemyData.FollowDistance : pos.x - enemyData.FollowDistance;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, new Vector3(followX, pos.y, pos.z));

            var attackX = GetFacingDirection().Equals(1) ? pos.x + enemyData.AttackDistance : pos.x - enemyData.AttackDistance;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, new Vector3(attackX, pos.y, pos.z));
        }
        #endregion
    }
}