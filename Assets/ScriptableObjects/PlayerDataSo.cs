using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Data/PlayerData")]
    public class PlayerDataSo : ScriptableObject {
        [SerializeField] private float runSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float carrySpeed;
        [SerializeField] private float jumpForce;

        # region getters
        public float RunSpeed => runSpeed;
        public float SprintSpeed => sprintSpeed;
        public float CrouchSpeed => crouchSpeed;
        public float CarrySpeed => carrySpeed;
        public float JumpForce => jumpForce;
        #endregion
    }
}