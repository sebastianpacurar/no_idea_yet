using UnityEngine;

namespace Prop.Uninteractables {
    public class LampPostSwing : MonoBehaviour {
        [SerializeField] private GameObject swingObject;
        [SerializeField] private float sinFreq;
        [SerializeField] private float sinAmplitude;

        private void Update() {
            // calculate the rotation based on Sin movement
            var rotZ = Mathf.Sin(Time.time * sinFreq) * sinAmplitude + swingObject.transform.localRotation.z;

            // apply rotation using Quaternion.Euler
            swingObject.transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }
}