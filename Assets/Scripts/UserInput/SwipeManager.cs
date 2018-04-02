using UnityEngine;
using System.Collections.Generic;
using System.Linq;
 
class CardinalDirection
{
    public static readonly Vector2 Up        = new Vector2 (0, 1);
    public static readonly Vector2 Down      = new Vector2 (0, -1);
    public static readonly Vector2 Right     = new Vector2 (1, 0);
    public static readonly Vector2 Left      = new Vector2 (-1, 0);
    public static readonly Vector2 UpRight   = new Vector2 (1, 1);
    public static readonly Vector2 UpLeft    = new Vector2 (-1, 1);
    public static readonly Vector2 DownRight = new Vector2 (1, -1);
    public static readonly Vector2 DownLeft  = new Vector2 (-1, -1);
}
 
public enum Swipe
{
    None,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
};

public enum Gesture 
{
    None,
    Pinch,
    Rotate
}
 
public class SwipeManager : MonoBehaviour
{
    #region Inspector Variables
 
    [Tooltip("Min swipe distance (inches) to register as swipe")]
    [SerializeField] float minSwipeLength = 0.5f;
 
    [Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
    [SerializeField] bool triggerSwipeAtMinLength = false;
 
    [Tooltip("Whether to detect eight or four cardinal directions")]
    [SerializeField] bool useEightDirections = false;
 
    #endregion
 
    const float eightDirAngle = 0.906f;
    const float fourDirAngle  = 0.5f;
    const float defaultDPI    = 72f;
    const float dpcmFactor    = 2.54f;
 
    static Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2> ()
    {
        { Swipe.Up,       CardinalDirection.Up},
        { Swipe.Down,     CardinalDirection.Down},
        { Swipe.Right,    CardinalDirection.Right},
        { Swipe.Left,     CardinalDirection.Left},
        { Swipe.UpRight,  CardinalDirection.UpRight},
        { Swipe.UpLeft,   CardinalDirection.UpLeft},
        { Swipe.DownRight,CardinalDirection.DownRight},
        { Swipe.DownLeft, CardinalDirection.DownLeft}
    };
 
    public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);
    public delegate void OnGestureDetectedHandler(Gesture gestureType, float value);
    public delegate void OnTapDetectedHandler(Vector3 tapPosition);

    static OnSwipeDetectedHandler _OnSwipeDetected;
    public static event OnSwipeDetectedHandler OnSwipeDetected
    {
        add {
            _OnSwipeDetected += value;
            autoDetect = true;
        }
        remove {
            _OnSwipeDetected -= value;
        }
    }

    static OnGestureDetectedHandler _OnGestureDetected;
    public static event OnGestureDetectedHandler OnGestureDetected {
        add {
            _OnGestureDetected += value;
            autoDetect = true;
        }
        remove {
            _OnGestureDetected -= value;
        }
    }

    static OnTapDetectedHandler _OnTapDetected;
    public static event OnTapDetectedHandler OnTapDetected {
        add {
            _OnTapDetected += value;
            autoDetect = true;
        }
        remove {
            _OnTapDetected -= value;
        }
    }

    public static Vector2 swipeVelocity;
 
    static float dpcm;
    static float swipeStartTime;
    static float swipeEndTime;
    static bool autoDetect = true;
    static bool swipeStarted;
    static Swipe swipeDirection;
    static SwipeManager instance;
    static List<InputData> inputs = new List<InputData>();

    void Awake ()
    {
        instance = this;
        float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
        dpcm = dpi / dpcmFactor;
    }
 
    void Update ()
    {
        if (autoDetect) {
            ClearOldInputs();
            if (GetTouchInput() || GetKeyboardInput()) {
                DetectSwipe();
                DetectGesture();
                DetectTap();
            } else {
                swipeStarted = false;
            }
        } else {
            swipeDirection = Swipe.None;
        }
    }

    static void DetectGesture() {
        if(inputs.Count == 2) {

            InputData touchZero = inputs[0];
            InputData touchOne  = inputs[1];

            Vector2 touchZeroPrevPos = touchZero.currentPosition - touchZero.prevPosition;
            Vector2 touchOnePrevPos  = touchOne.currentPosition - touchOne.prevPosition;

            if(touchZero.phase != TouchPhase.Moved && touchOne.phase != TouchPhase.Moved) {
                Debug.Log("No touches moving, returning");
                return;
            }
            //Its a pinch!
            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {
                Debug.Log("pinch");
                swipeStarted = true;
                float prevDist = Vector2.Distance(touchOne.prevPosition, touchZero.prevPosition);
                float curDist = Vector2.Distance(touchOne.currentPosition, touchZero.currentPosition);
                if (_OnGestureDetected != null) {
                    _OnGestureDetected(Gesture.Pinch, prevDist - curDist);
                }
                return;
            }
            // Its a rotation!
            if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {
                swipeStarted = true;
                float currentValue = touchOnePrevPos.x - touchZeroPrevPos.x;
                float normalized = (currentValue - (-100)) / (100 - (-100)) * 30;

                if (_OnGestureDetected != null) {
                    _OnGestureDetected(Gesture.Rotate, normalized);
                }
            }

        }

    }

    static void DetectTap() {
        if (!swipeStarted && inputs.Count == 1 && inputs[0].phase == TouchPhase.Ended) {
            InputData inputData = inputs[0];
            if (Vector2.Distance(inputData.startPosition, inputData.currentPosition) < instance.minSwipeLength) {
                if (_OnTapDetected != null) {
                    _OnTapDetected(inputData.currentPosition);
                }
            }
        }
    }
 
    /// <summary>
    /// Attempts to detect the current swipe direction.
    /// Should be called over multiple frames in an Update-like loop.
    /// </summary>
    static void DetectSwipe () {
        if (inputs.Count == 1 && inputs[0].phase != TouchPhase.Ended) {
            InputData inputData  = inputs[0];
            Vector2 currentSwipe = inputData.currentPosition - inputData.startPosition;
            float swipeCm        = currentSwipe.magnitude / dpcm;

            if (swipeCm < instance.minSwipeLength) {
                // Swipe was not long enough, abort
                if (!instance.triggerSwipeAtMinLength) {
                    if (Application.isEditor) {
                        Debug.Log("[SwipeManager] Swipe was not long enough.");
                    }
                    swipeDirection = Swipe.None;
                }
                return;
            }
            swipeStarted   = true;
            swipeEndTime   = Time.time;
            swipeVelocity  = currentSwipe * (swipeEndTime - inputData.startTime);
            swipeDirection = GetSwipeDirByTouch(currentSwipe);

            if (_OnSwipeDetected != null) {
                _OnSwipeDetected(swipeDirection, swipeVelocity);
            }
        }
    }
 
	public static bool IsSwiping			 () {	  return swipeDirection != Swipe.None; 			}
    public static bool IsSwipingRight        () {     return IsSwipingDirection(Swipe.Right);   	}
    public static bool IsSwipingLeft         () {     return IsSwipingDirection(Swipe.Left);    	}
    public static bool IsSwipingUp           () {     return IsSwipingDirection(Swipe.Up);          }
    public static bool IsSwipingDown         () {     return IsSwipingDirection(Swipe.Down);        }
    public static bool IsSwipingDownLeft     () {     return IsSwipingDirection(Swipe.DownLeft);    }
    public static bool IsSwipingDownRight    () {     return IsSwipingDirection(Swipe.DownRight);   }
    public static bool IsSwipingUpLeft       () {     return IsSwipingDirection(Swipe.UpLeft);      }
    public static bool IsSwipingUpRight      () {     return IsSwipingDirection(Swipe.UpRight);     }
 
    #region Helper Functions
 
    static void ClearOldInputs() {
        for (int i = 0; i < inputs.Count; i++) {
            if (inputs[i].phase == TouchPhase.Ended)
                inputs.Remove(inputs[i]);
        }
    }

    static bool GetTouchInput ()
    {
        if (Input.touches.Length > 0) {
            for (int i = 0; i < Input.touches.Length; i++) {
                Touch t = Input.GetTouch(i);
                int id = t.fingerId;
                if (t.phase == TouchPhase.Began) {                   
                    Vector2 pos = t.position;
                    swipeStartTime = Time.time;
                    inputs.Add(new InputData(id, swipeStartTime, pos));
                }
                else {
                    int index = GetInputDataIndexWithID(id);
                    InputData data = inputs[index];
                    data.phase = t.phase;
                    data.prevPosition = data.currentPosition;
                    data.currentPosition = t.position;
                    inputs[index] = data;
                } 
            }
            return true;
        }
        return false;
    }
 
    static bool GetKeyboardInput ()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld) return false;

        int mouseButtonID    = 1000;
        int spaceKeyID       = 1100;
        int mouseButtonIndex = GetInputDataIndexWithID(mouseButtonID);
        int spaceKeyIndex    = GetInputDataIndexWithID(spaceKeyID);

        if (Input.GetMouseButton(0)) {
            Vector2 pressPos = (Vector2)Input.mousePosition;
            if (mouseButtonIndex == -1) {
                swipeStartTime = Time.time;
                inputs.Add(new InputData(mouseButtonID, swipeStartTime, pressPos));
            } else {
                InputData data = inputs[mouseButtonIndex];
                if(pressPos != data.startPosition) {
                    data.phase = TouchPhase.Moved;
                }
                data.prevPosition = data.currentPosition;
                data.currentPosition = pressPos;
                inputs[mouseButtonIndex] = data;
            }
            return true;

        } else if(mouseButtonIndex != -1) {
            InputData data = inputs[mouseButtonIndex];
            Vector2 pressPos = (Vector2)Input.mousePosition;
            data.prevPosition = data.currentPosition;
            data.currentPosition = pressPos;
            data.phase = TouchPhase.Ended;
            inputs[mouseButtonIndex] = data;
            return true;
        }

        if (Input.GetKey(KeyCode.Space)) {
            Vector2 pressPos = Vector2.zero;
            if (spaceKeyIndex == -1) {
                swipeStartTime = Time.time;
                inputs.Add(new InputData(spaceKeyID, swipeStartTime, pressPos));
            } 
            return true;
        } else if (spaceKeyIndex != -1) {
            InputData data = inputs[spaceKeyIndex];
            data.phase = TouchPhase.Ended;
            inputs[spaceKeyIndex] = data;
            return true;
        }
        return false;
    }

    static int GetInputDataIndexWithID (int id) {
        for (int i = 0; i < inputs.Count; i++) {
            if (inputs[i].id == id) return i;
        }
        return -1;
    }
 
    static bool IsDirection (Vector2 direction, Vector2 cardinalDirection)
    {
        var angle = instance.useEightDirections ? eightDirAngle : fourDirAngle;
        return Vector2.Dot(direction, cardinalDirection) > angle;
    }
 
    static Swipe GetSwipeDirByTouch (Vector2 currentSwipe)
    {
        currentSwipe.Normalize();
        var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));
        return swipeDir.Key;
    }
 
    static bool IsSwipingDirection (Swipe swipeDir)
    {
        DetectSwipe();
        return swipeDirection == swipeDir;
    }
 
    #endregion
}

public struct InputData {
    public int id;
    public TouchPhase phase;
    public float startTime;
    public Vector2 startPosition;
    public Vector2 currentPosition;
    public Vector2 prevPosition;

    public InputData(int id, float startTime, Vector2 pos) {
        this.id = id;
        this.phase = TouchPhase.Began;
        this.startTime = startTime;
        this.startPosition = pos;
        this.currentPosition = pos;
        this.prevPosition = pos;
    }
}

