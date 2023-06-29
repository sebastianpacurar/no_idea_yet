using System;
using Enemy.Fsm;
using Enemy.Fsm.States.SubStates;
using ScriptableObjects;
using UnityEngine;

namespace Enemy {
    public class EnemyScript : MonoBehaviour {
        #region State Variables
        public EnemyStateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkState WalkState { get; private set; }
        public AttackState AttackState { get; private set; }
        public DeathState DeathState { get; private set; }
        public FollowPlayerState FollowPlayerState { get; private set; }

        public EnemyDataSo enemyData;
        #endregion

        #region Components
        public Animator Anim { get; private set; }
        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;
        #endregion

        #region Misc Vars
        public Vector2 currentVelocity;
        public bool isFacingPlayer;

        private RaycastHit2D _detectPlayerAttackHit;
        private RaycastHit2D _detectPlayerFollowHit;
        private Transform _playerPos;
        private LayerMask _playerLayer;
        #endregion


        #region Unity Callback Functions
        private void Awake() {
            StateMachine = new EnemyStateMachine();

            IdleState = new IdleState(this, StateMachine, enemyData, "idle");
            WalkState = new WalkState(this, StateMachine, enemyData, "walk");
            FollowPlayerState = new FollowPlayerState(this, StateMachine, enemyData, "follow");
            AttackState = new AttackState(this, StateMachine, enemyData, "attack");
            DeathState = new DeathState(this, StateMachine, enemyData, "death");

            _rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            _playerLayer = LayerMask.NameToLayer("Character");
        }

        private void Start() {
            _playerPos = GameObject.FindGameObjectWithTag("Player").transform;

            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            currentVelocity = _rb.velocity;

            var pos = transform.position;
            _detectPlayerFollowHit = Physics2D.Raycast(pos, Vector2.right, enemyData.FollowDistance, _playerLayer);
            _detectPlayerAttackHit = Physics2D.Raycast(pos, Vector2.right, enemyData.AttackDistance, _playerLayer);

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion


        #region Set Functions
        public void SetVelocityX(float velocity) {
            _rb.velocity = new Vector2(velocity, currentVelocity.y);
            currentVelocity = _rb.velocity;
        }

        public void FreezePlayer() => _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        public void UnFreezePlayer() => _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        #endregion


        #region Check Functions
        public void CheckIfShouldFlip() {
            var dirToPlayer = _playerPos.position - transform.position;
            var alignX = Vector2.Dot(dirToPlayer, transform.right);

            if (alignX < 0.5f) {
                Flip();
            }
        }

        public bool CheckIfCanAttackPlayer() {
            return _detectPlayerAttackHit;
        }

        public bool CheckIfCanFollowPlayer() {
            return _detectPlayerFollowHit;
        }
        #endregion


        #region Misc Functions
        public float GetFacingDirection => transform.localScale.x;

        private void Flip() {
            var localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        }

        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion


        #region Debug section
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * enemyData.FollowDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * enemyData.AttackDistance);
        }
        #endregion
    }
}