using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsagiHandyScripts
{
    public class GazeTargetMarkerManager : MonoBehaviour
    {
        [SerializeField]
        Transform eye;

        [SerializeField]
        GazeInputManager gazeInputManager;

        [SerializeField]
        GameObject marker;

        GameObject markerObj;
        Transform markerTransform;

        public Vector3 MarkerPos { get; private set; }
        public Vector3 MarkerDir { get; private set; }

        // Use this for initialization
        void Start()
        {
            if (eye == null)
                eye = Camera.main.transform;

            if (marker.scene.name == null) {
                markerObj = Instantiate(marker, transform);   
            } else {
                markerObj = marker;
            }

            markerTransform = markerObj.transform;
        }

        // Update is called once per frame
        void Update()
        {
            GazeInputManager.GazeInputEventArgs args = gazeInputManager.GazeTarget;
            MarkerPos = (args != null ? args.point : eye.position + eye.forward * gazeInputManager.RayMaxDistance);
            MarkerDir = (args != null ? -args.normal : eye.forward);
            markerTransform.position = MarkerPos;
            markerTransform.rotation = Quaternion.LookRotation(MarkerDir, eye.up);
        }
    }
}