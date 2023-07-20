using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Data/CratePhysicsDataSo")]
    public class CratePhysicsDataSo : ScriptableObject {
        [Header("RigidBody")]
        [SerializeField] private float defaultMass;
        [SerializeField] private float defaultGravity;
        [SerializeField] private float freeFallGravity;
        [SerializeField] private float carryThrowGravity;

        [Header("Physics Material 2D")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;
        [SerializeField] private PhysicsMaterial2D groundFriction;

        #region getters
        public float DefaultMass => defaultMass;
        public float DefaultGravity => defaultGravity;
        public float FreeFallGravity => freeFallGravity;
        public float CarryThrowGravity => carryThrowGravity;

        public PhysicsMaterial2D LowFriction => lowFriction;
        public PhysicsMaterial2D HighFriction => highFriction;
        public PhysicsMaterial2D GroundFriction => groundFriction;
        #endregion
    }
}