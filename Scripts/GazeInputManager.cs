using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsagiHandyScripts
{
    public class GazeInputManager : MonoBehaviour
    {
        public class GazeInputEventArgs : EventArgs
        {
            public GameObject target;
            public Vector3 point;
            public Vector3 normal;

            public GazeInputEventArgs(GameObject _target, Vector3 _point, Vector3 _normal)
            {
                target = _target;
                point = _point;
                normal = _normal;
            }
        }

        public event EventHandler<GazeInputEventArgs> GazeIn;
        public event EventHandler<GazeInputEventArgs> GazeOut;

        [SerializeField]
        Transform eye;

        [SerializeField]
        LayerMask rayMask;

        [SerializeField]
        float rayRadius = 0.1f;

        [SerializeField]
        float rayMaxDistance = 3f;

        [SerializeField]
        bool isDebug = false;

        GazeInputEventArgs gazeTarget = null;
        public GazeInputEventArgs GazeTarget { get { return gazeTarget; } }
        public float RayMaxDistance { get { return rayMaxDistance; } }

        // Use this for initialization
        void Start()
        {
            if (isDebug)
            {
                LineRenderer line = gameObject.AddComponent<LineRenderer>();
                line.positionCount = 2;
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                line.useWorldSpace = false;
                line.SetPositions(new Vector3[] { Vector3.down * 0.5f, Vector3.forward * rayMaxDistance });
            }
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.SphereCast(eye.position, rayRadius, eye.forward, out hitInfo, rayMaxDistance, rayMask.value))
            {
                GazeInputEventArgs newArgs = new GazeInputEventArgs(hitInfo.collider.gameObject, hitInfo.point, hitInfo.normal);
                if (gazeTarget != null && gazeTarget.target.GetInstanceID() != hitInfo.collider.gameObject.GetInstanceID() && GazeOut != null)
                    if(GazeOut != null)
                        GazeOut(this, gazeTarget);
                
                if (gazeTarget == null || gazeTarget.target.GetInstanceID() != hitInfo.collider.gameObject.GetInstanceID())
                    if (GazeIn != null)
                        GazeIn(this, newArgs);
                
                gazeTarget = newArgs;
            }
            else
            {
                if (gazeTarget != null)
                {
                    if (GazeOut != null)
                        GazeOut(this, gazeTarget);

                    gazeTarget = null;
                }
            }
        }
    }
}