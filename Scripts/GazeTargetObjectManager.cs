using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AsagiHandyScripts
{

    public class GazeTargetObjectManager : MonoBehaviour
    {
        [System.Serializable]
        public class TimElapsedEvent : UnityEvent<float> { }

        [SerializeField]
        float progressingTime = 4f;

        [SerializeField]
        bool isProcessingOnceAtTime = true;

        [SerializeField]
        UnityEvent seenEvent = new UnityEvent();

        [SerializeField]
        UnityEvent unSeenEvent = new UnityEvent();

        [SerializeField]
        TimElapsedEvent seeingEvent = new TimElapsedEvent();

        [SerializeField]
        TimElapsedEvent gazeProgressingEvent = new TimElapsedEvent();

        [SerializeField]
        UnityEvent gazeProgressedEvent = new UnityEvent();

        GazeInputManager gazeInputManager;

        bool isSeen = false;
        bool processed = false;
        float timeWhenSeen = 0;

        float progressState = 0;
        public float ProgressState { get { return progressState; } }

        void Start()
        {
            gazeInputManager = FindObjectOfType(typeof(GazeInputManager)) as GazeInputManager;
            gazeInputManager.GazeIn += Seen;
            gazeInputManager.GazeOut += UnSeen;
        }

        // Update is called once per frame
        void Update()
        {
            if (isSeen)
            {
                if (!isProcessingOnceAtTime || (isProcessingOnceAtTime && !processed))
                {
                    float elapsedTime = Time.time - timeWhenSeen;
                    progressState = Mathf.Min(1f, elapsedTime / progressingTime);
                    seeingEvent.Invoke(elapsedTime);
                    gazeProgressingEvent.Invoke(progressState);

                    if (progressState >= 1f)
                    {
                        gazeProgressedEvent.Invoke();
                        processed = true;
                    }
                }
            }
            else
            {
                progressState = 0;
            }
        }

        public void Seen(object sender, GazeInputManager.GazeInputEventArgs args)
        {
            if (args.target.GetInstanceID() == gameObject.GetInstanceID() && !isSeen)
            {
                seenEvent.Invoke();
                timeWhenSeen = Time.time;
                isSeen = true;
            }
        }

        public void UnSeen(object sender, GazeInputManager.GazeInputEventArgs args)
        {
            if ( args.target.GetInstanceID() == gameObject.GetInstanceID())
            {
                unSeenEvent.Invoke();
                isSeen = false;
                processed = false;
            }
        }

        private void OnDestroy()
        {
            gazeInputManager.GazeIn -= Seen;
            gazeInputManager.GazeOut -= UnSeen;
        }
    }
}