using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Data/EnemyData")]
    public class EnemyDataSo : ScriptableObject {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float followSpeed;
        [SerializeField] private float damage;
        // [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float attackDistance;
        [SerializeField] private float followDistance;
        [SerializeField] private Vector2 minMaxIdleTime;
        [SerializeField] private Vector2 minMaxWalkTime;

        # region getters
        public float MoveSpeed => moveSpeed;
        public float FollowSpeed => followSpeed;
        public float Damage => damage;
        public float AttackDistance => attackDistance;
        public float FollowDistance => followDistance;
        // public float TimeBetweenAttacks => timeBetweenAttacks;
        public Vector2 MinMaxIdleTime => minMaxIdleTime;
        public Vector2 MinMaxWalkTime => minMaxWalkTime;
        #endregion
    }
}