using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Data/CratePhysicsDataSo")]
    public class CratePhysicsDataSo : ScriptableObject {
        [Header("Crate Identifier")]
        [SerializeField] private bool isSmallCrate;

        [Header("Attach Crate Pos Offset")]
        [SerializeField] private float posOffset;

        [Header("Min-MAx Throw Force Threshold")]
        [SerializeField] private Vector2 minMaxAimRange;

        [Header("RigidBody")]
        [SerializeField] private float defaultMass;
        [SerializeField] private float stackMass;
        [SerializeField] private float defaultGravity;
        [SerializeField] private float freeFallGravity;
        [SerializeField] private float carryThrowMass;
        [SerializeField] private float carryThrowGravity;

        [Header("Physics Material 2D")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;
        [SerializeField] private PhysicsMaterial2D groundFriction;
        [SerializeField] private PhysicsMaterial2D noFriction;

        #region getters
        public bool IsSmallCrate => isSmallCrate;

        public float PosOffset => posOffset;

        public Vector2 MinMaxAimRange => minMaxAimRange;

        public float DefaultMass => defaultMass;
        public float StackMass => stackMass;
        public float DefaultGravity => defaultGravity;
        public float FreeFallGravity => freeFallGravity;
        public float CarryThrowMass => carryThrowMass;
        public float CarryThrowGravity => carryThrowGravity;

        public PhysicsMaterial2D LowFriction => lowFriction;
        public PhysicsMaterial2D HighFriction => highFriction;
        public PhysicsMaterial2D GroundFriction => groundFriction;
        public PhysicsMaterial2D NoFriction => noFriction;
        #endregion
    }
}