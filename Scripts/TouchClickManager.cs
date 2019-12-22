using System;
using UnityEngine;

namespace AsagiHandyScripts
{
    public class TouchClickManager : MonoBehaviour
    {
        public class BaseTouchClickEvent : EventArgs
        {
            public float time;
            public Vector2 position;
            public bool isMouse;

            public BaseTouchClickEvent(float _time, Vector2 _pos, bool _isMouse)
            {
                time = _time;
                position = _pos;
                isMouse = _isMouse;
            }
        }

        public class TapEventArgs : BaseTouchClickEvent
        {
            public TapEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) {}
        }

        public event EventHandler<TapEventArgs> Tap;

        public class TapHoldEventArgs : BaseTouchClickEvent
        {
            public float holdTime;

            public TapHoldEventArgs(float _time, float _holdTime, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse)
            {
                holdTime = _holdTime;
            }
        }

        public event EventHandler<TapHoldEventArgs> TapHold;

        public class TapHoldInEventArgs : BaseTouchClickEvent
        {
            public TapHoldInEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) {}
        }

        public event EventHandler<TapHoldInEventArgs> TapHoldIn;

        public class TapHoldOutEventArgs : BaseTouchClickEvent
        {
            public TapHoldOutEventArgs(float _time, Vector2 _pos, bool _isMouse) : base (_time, _pos, _isMouse) {}
        }

        public event EventHandler<TapHoldOutEventArgs> TapHoldOut;

        public class TapHoldCancelEventArgs : BaseTouchClickEvent
        {
            public TapHoldCancelEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) { }
        }

        public event EventHandler<TapHoldCancelEventArgs> TapHoldCancel;

        public class TouchInEventArgs : BaseTouchClickEvent
        {
            public TouchInEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) { }
        }

        public event EventHandler<TouchInEventArgs> TouchIn;

        public class TouchOutEventArgs : BaseTouchClickEvent
        {
            public TouchOutEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) { }
        }

        public event EventHandler<TouchOutEventArgs> TouchOut;

        public class SwipeEventArgs : BaseTouchClickEvent
        {
            public Vector2 moves;

            public SwipeEventArgs(float _time, Vector2 _pos, Vector2 _moves, bool _isMouse) : base(_time, _pos, _isMouse)
            {
                moves = _moves;
            }
        }

        public event EventHandler<SwipeEventArgs> Swipe;

        public class SwipeStartEventArgs : BaseTouchClickEvent
        {
            public SwipeStartEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) { }
        }

        public event EventHandler<SwipeStartEventArgs> SwipeStart;

        public class SwipeEndEventArgs : BaseTouchClickEvent
        {
            public SwipeEndEventArgs(float _time, Vector2 _pos, bool _isMouse) : base(_time, _pos, _isMouse) { }
        }

        public event EventHandler<SwipeEndEventArgs> SwipeEnd;

        #region public Variables
        public bool Tapped { get; private set; }
        public bool Swiped { get; private set; }
        public bool Held { get; private set; }
        public bool Cancelled { get; private set; }
        #endregion

        #region serialized fields
        [SerializeField]
        float tapToSwipeMoves = 0.1f;
        [SerializeField]
        float timeHoldDetection = 1.0f;
        [SerializeField]
        bool debug;
        #endregion

        #region private variables
        float tapStartTime = 0;
        Vector2 tapStartPos = Vector2.zero;
        Vector2 tapNowPos = Vector2.zero;
        float lastTime;
        #endregion

        // Use this for initialization
        void Start()
        {
            lastTime = Time.time;
            Tapped = false;
            Swiped = false;
            Held = false;
            Cancelled = false;
        }

        // Update is called once per frame
        void Update()
        {
            bool mouseClick = Input.GetMouseButton(0);
            bool isTouch = Input.touchCount > 0;
            bool anyTap = (mouseClick || isTouch);

            if (!Cancelled && anyTap)
            {
                float timeNow = Time.time;
                Vector2 lastTouchPos = tapNowPos;
                tapNowPos = GetPointerPosition(mouseClick);

                DebugLog("tap now: " + tapNowPos.ToString());

                float distanceTapPos = Vector2.Distance(tapNowPos, tapStartPos);

                if (!Tapped)
                {
                    tapStartPos = tapNowPos;
                    tapStartTime = Time.time;
                    Tapped = true;

                    if (TouchIn != null)
                        TouchIn(this, new TouchInEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("TouchIn : TouchClickManager : " + timeNow);
                }
                else if (!Held && distanceTapPos < tapToSwipeMoves && timeNow - tapStartTime >= timeHoldDetection)
                {
                    Held = true;
                    if (TapHoldIn != null)
                        TapHoldIn(this, new TapHoldInEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("TapHoldIn : TouchClickManager : " + timeNow);
                }
                else if (!Held && !Swiped && distanceTapPos >= tapToSwipeMoves)
                {
                    Swiped = true;
                    if (SwipeStart != null)
                        SwipeStart(this, new SwipeStartEventArgs(timeNow, tapStartPos, mouseClick));

                    DebugLog("SwipeStart : TouchClickManager : " + timeNow);
                }
                else if (Swiped)
                {
                    if (Swipe != null)
                        Swipe(this, new SwipeEventArgs(timeNow, tapNowPos, tapNowPos - lastTouchPos, mouseClick));

                    DebugLog("Swiping : TouchClickManager : " + timeNow);
                }
                else if (Held)
                {
                    if (TapHold != null)
                        TapHold(this, new TapHoldEventArgs(timeNow, timeNow - tapStartTime, tapNowPos, mouseClick));

                    DebugLog("Holding : TouchClickManager : " + timeNow);
                }
                else if (Held && !Swiped && distanceTapPos >= tapToSwipeMoves)
                {
                    Tapped = false;
                    Swiped = false;
                    Held = false;
                    Cancelled = true;

                    if (TapHoldCancel != null)
                        TapHoldCancel(this, new TapHoldCancelEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("TapHoldCancelled : TouchClickManager : " + timeNow);
                }
            }
            else if (!anyTap && (Tapped || Swiped || Held) && !Cancelled)
            {
                float timeNow = Time.time;

                if (TouchOut != null)
                    TouchOut(this, new TouchOutEventArgs(timeNow, tapNowPos, mouseClick));

                DebugLog("TouchOut : TouchClickManager : " + timeNow);

                if (Swiped)
                {
                    if (SwipeEnd != null)
                        SwipeEnd(this, new SwipeEndEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("SwipeIsEnd : TouchClickManager : " + timeNow);
                }
                else if (Held)
                {
                    if (TapHoldOut != null)
                        TapHoldOut(this, new TapHoldOutEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("TapHeld : TouchClickManager : " + timeNow);
                }
                else if (Tapped)
                {
                    if (Tap != null)
                        Tap(this, new TapEventArgs(timeNow, tapNowPos, mouseClick));

                    DebugLog("Tapped : TouchClickManager : " + timeNow);
                }

                ResetFlag();
            }
            else if (Cancelled && !anyTap)
            {
                Cancelled = false;
            }
        }

        public Vector2 GetPointerPosition(bool isMouse)
        {
            if (isMouse)
                return Input.mousePosition;
            else
                return Input.GetTouch(0).position;
        }

        void ResetFlag()
        {
            Tapped = false;
            Swiped = false;
            Held = false;
            Cancelled = false;
        }

        void DebugLog(string str)
        {
            if (debug)
                Debug.Log(str);
        }
    }
}