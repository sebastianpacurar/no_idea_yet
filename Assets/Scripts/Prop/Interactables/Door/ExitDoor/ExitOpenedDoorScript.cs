using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prop.Interactables.Door.ExitDoor
{
    public class ExitOpenedDoorScript : MonoBehaviour
    {
        [SerializeField] private Light2D light2D;
        [SerializeField] private float duration = 0.5f;

        [Header("Shades of grey")]
        [SerializeField] private Color startColor = new(0.8f, 0.8f, 0.8f, 1f);
        [SerializeField] private Color targetColor = new(0.2f, 0.2f, 0.2f, 1f);

        private void Update()
        {
            var t = Mathf.PingPong(Time.time, duration) / duration;
            light2D.color = Color.Lerp(startColor, targetColor, t);
        }
    }
}