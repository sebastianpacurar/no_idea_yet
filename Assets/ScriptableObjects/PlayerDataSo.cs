using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Data/PlayerData")]
    public class PlayerDataSo : ScriptableObject {
        [Header("Physics")]
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float jumpForce;

        [Header("Crate Interaction Physics")]
        [SerializeField] private float carrySpeed;
        [SerializeField] private float aimSpeed;
        [SerializeField] private Vector2 minMaxAimRange;

        # region getters
        public float RunSpeed => runSpeed;
        public float SprintSpeed => sprintSpeed;
        public float CrouchSpeed => crouchSpeed;
        public float JumpForce => jumpForce;

        public float CarrySpeed => carrySpeed;
        public float AimSpeed => aimSpeed;
        public Vector2 MinMaxAimRange => minMaxAimRange;
        #endregion
    }
}