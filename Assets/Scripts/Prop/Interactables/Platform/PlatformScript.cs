using System.Collections.Generic;
using Prop.Interactables.Crate;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Prop.Interactables.Platform
{
    public class PlatformScript : MonoBehaviour
    {
        [SerializeField] private bool forSmallCrate;
        [SerializeField] private TextMeshProUGUI countTxt;
        public int requiredCrateNo;
        public bool targetReached;

        [Header("Debug")]
        [SerializeField] private List<GameObject> cratesInRange;

        private Light2D _light;

        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }


        private void Update()
        {
            targetReached = cratesInRange.Count >= requiredCrateNo;
            var res = requiredCrateNo - cratesInRange.Count;
            var txt = res >= 0 ? res.ToString() : "0";
            countTxt.text = txt;

            if (targetReached)
            {
                _light.color = Color.green;
                countTxt.color = Color.green;
            } else
            {
                _light.color = Color.yellow;
                countTxt.color = Color.yellow;
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Crate")) return;
            if (cratesInRange.Contains(other.gameObject)) return;

            // if cart is for small crate, and target crate is a small crate, then add to list
            if (CheckIfMatch(other.gameObject))
            {
                cratesInRange.Add(other.gameObject);
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Crate")) return;
            if (!cratesInRange.Contains(other.gameObject)) return;
            cratesInRange.Remove(other.gameObject);
        }


        // if platform is for small crate and is small crate or if platform is not for small crate and not small crate
        private bool CheckIfMatch(GameObject obj)
        {
            var smallCrate = obj.GetComponent<CrateScript>().IsSmallCrate;
            return (smallCrate && forSmallCrate) || (!smallCrate && !forSmallCrate);
        }
    }
}