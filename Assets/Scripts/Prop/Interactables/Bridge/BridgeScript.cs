using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Prop.Interactables.Bridge
{
    public class BridgeScript : MonoBehaviour
    {
        [SerializeField] private GameObject startPoint;
        [SerializeField] private GameObject endPoint;

        [SerializeField] private GameObject plankPrefab;
        [SerializeField] private Transform planksContainer;

        private List<GameObject> _planks;
        private Dictionary<int, GameObject[]> _segments;


        private void Awake()
        {
            _planks = new List<GameObject>();
            _segments = new Dictionary<int, GameObject[]>();
        }

        private void Start()
        {
            GeneratePlanks();
            SetSegments();
            SetHingeProps();
        }


        // instantiate plank object based on position difference and add to list
        private void GeneratePlanks()
        {
            var start = startPoint.transform.position.x;
            var count = endPoint.transform.position.x - start;

            for (var i = 1; i <= count; i++)
            {
                var pos = new Vector3(start + i, startPoint.transform.position.y, 1);
                var plank = Instantiate(plankPrefab, pos, Quaternion.identity, planksContainer);
                _planks.Add(plank);
            }
        }


        // set all hinge properties for the relevant objects
        private void SetHingeProps()
        {
            var firstPlank = _planks[0];
            var lastPlank = _planks[^1];

            // attach first plank to startPoint 
            SetHingeProps(firstPlank, startPoint);

            // attach each current plank to the previous plank
            for (var i = 1; i < _planks.Count; i++)
            {
                SetHingeProps(_planks[i], _planks[i - 1]);
            }

            // attach endPoint to last plank
            SetHingeProps(endPoint, lastPlank);
        }


        // break the list into 3 different segments
        private void SetSegments()
        {
            var ratio = Mathf.FloorToInt(_planks.Count / 3);
            var arr = _planks.ToArray();
            _segments[1] = arr[..ratio];
            _segments[2] = arr[ratio..(ratio * 2)];
            _segments[3] = arr[(ratio * 2)..];
        }

        // set all hinge joint properties. the current obj is connected to the previous obj
        private void SetHingeProps(GameObject current, GameObject previous)
        {
            var currHj = current.GetComponent<HingeJoint2D>();
            var currRb = current.GetComponent<Rigidbody2D>();


            var previousRb = previous.GetComponent<Rigidbody2D>();
            currHj.connectedBody = previousRb;

            currHj.enableCollision = true;
            currHj.autoConfigureConnectedAnchor = true;

            currHj.useLimits = true;

            // set lower and upper angle limits
            currHj.limits = new JointAngleLimits2D
            {
                // all _planks use lower -30f and upper 5f. endPoint uses lower 5f and upper -30f
                min = current.Equals(endPoint) ? 5f : -30f,
                max = current.Equals(endPoint) ? -30f : 5f,
            };

            // if plank is in the middle segment, then make bridge easier to break
            if (_segments[2].Contains(current))
            {
                currHj.breakForce = 7500f;
                currRb.mass = 7.5f;
            } else
            {
                currHj.breakForce = 10000f;
                currRb.mass = 15f;
            }
        }
    }
}