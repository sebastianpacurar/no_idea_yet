using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "CartPhysicsDataSO", menuName = "Data/CartPhysicsData")]
    public class CartPhysicsDataSo : ScriptableObject {

        [Header("Rigidbody")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float wheelRotationFactor;
        [SerializeField] private float massWhenStationary;
        [SerializeField] private float massWhenPushed;
        
        [Header("Physics Material 2D")]
        [SerializeField] private PhysicsMaterial2D lowFriction;
        [SerializeField] private PhysicsMaterial2D highFriction;
        
        
        #region getters
        public float MaxSpeed => maxSpeed;
        public float WheelRotationFactor => wheelRotationFactor;
        public float MassWhenStationary => massWhenStationary;
        public float MassWhenPushed => massWhenPushed;

        public PhysicsMaterial2D LowFriction => lowFriction;
        public PhysicsMaterial2D HighFriction => highFriction;
        #endregion
    }
}
