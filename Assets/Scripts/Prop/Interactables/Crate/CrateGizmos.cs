using UnityEngine;

namespace Prop.Interactables.Crate {
    public class CrateGizmos : MonoBehaviour {
        [SerializeField] private bool turnOn;
        private CrateRayCasts _rayCasts;

        private void Awake() {
            _rayCasts = GetComponent<CrateRayCasts>();
        }

        private void OnDrawGizmos() {
            var pos = transform.position;

            if (turnOn) {
                var origin = _rayCasts.GetRayOriginVector();

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(origin, new Vector3(pos.x - _rayCasts.distFromPlayerX, origin.y, pos.z));

                // right player detection
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, new Vector3((pos.x + _rayCasts.distFromPlayerX), origin.y, pos.z));

                // bottom ground detection
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(pos, new Vector3(pos.x, -_rayCasts.distFromGround, pos.z));
            }
        }
    }
}