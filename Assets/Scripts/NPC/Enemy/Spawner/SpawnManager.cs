using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace NPC.Enemy.Spawner
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private float minIntensity;
        private Light2D _globalLight;

        private void Start()
        {
            _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
        }

        // if Late Night then enable spawn points, else disable them
        private void Update()
        {
            if (_globalLight.intensity <= minIntensity)
            {
                SetChildrenToActive(true);
            } else if (_globalLight.intensity >= minIntensity)
            {
                SetChildrenToActive(false);
            }
        }

        // set all spawn points active status in hierarchy to false or true
        private void SetChildrenToActive(bool value)
        {
            foreach (Transform child in transform)
            {
                // if value is true but the first child is active, then skip
                // if value is false but the first child is inactive, then skip
                // all children can be either active or inactive at the same time
                if ((value && child.gameObject.activeInHierarchy) || (!value && !child.gameObject.activeInHierarchy)) return;

                child.gameObject.SetActive(value);
            }
        }
    }
}