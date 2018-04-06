using UnityEngine;
using System.Collections.Generic;
using System.Linq;
 
public class InputController : MonoBehaviour
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

    #region Event setups
    public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);
    public delegate void OnTapDetectedHandler(Vector3 tapPosition);

    public delegate void OnInputStartedHandler(List<InputData> inputs);
    public delegate void OnInputHandler(List<InputData> inputs);
    public delegate void OnInputEndedHandler(List<InputData> inputs);

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

    static OnInputStartedHandler _OnInputStarted;
    public static event OnInputStartedHandler OnInputStarted {
        add {
            _OnInputStarted += value;
            autoDetect = true;
        }
        remove {
            _OnInputStarted -= value;
        }
    }

    static OnInputHandler _OnInput;
    public static event OnInputHandler OnInput {
        add {
            _OnInput += value;
            autoDetect = true;
        }
        remove {
            _OnInput -= value;
        }
    }

    static OnInputEndedHandler _OnInputEnded;
    public static event OnInputEndedHandler OnInputEnded {
        add {
            _OnInputEnded += value;
            autoDetect = true;
        }
        remove {
            _OnInputEnded -= value;
        }
    }

    #endregion

    public static Vector2 swipeVelocity;
 
    static float dpcm;
    static float swipeStartTime;
    static float swipeEndTime;
    static bool autoDetect = true;
    static bool swipeStarted;
    static Swipe swipeDirection;
    static InputController instance;
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
            DetectInputs();
            if (GetTouchInput() || GetKeyboardInput()) {
                DetectSwipe();
                DetectTap();
            } else {
                swipeStarted = false;
            }
        } else {
            swipeDirection = Swipe.None;
        }
    }

    static void DetectInputs() {
        if (inputs.Count == 0) return;
    
        foreach (InputData data in inputs) {
            if(data.phase == TouchPhase.Began) {
                if (_OnInputStarted != null) {
                    _OnInputStarted(inputs);
                    return;
                }
            }
            if (data.phase == TouchPhase.Ended) {
                if (_OnInputEnded != null) {
                    _OnInputEnded(inputs);
                    return;
                }
            }
        }

        if (_OnInput != null) {
            _OnInput(inputs);
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



    #region Helper Functions

    public static bool IsSwiping() { return swipeDirection != Swipe.None; }
    public static bool IsSwipingRight() { return IsSwipingDirection(Swipe.Right); }
    public static bool IsSwipingLeft() { return IsSwipingDirection(Swipe.Left); }
    public static bool IsSwipingUp() { return IsSwipingDirection(Swipe.Up); }
    public static bool IsSwipingDown() { return IsSwipingDirection(Swipe.Down); }
    public static bool IsSwipingDownLeft() { return IsSwipingDirection(Swipe.DownLeft); }
    public static bool IsSwipingDownRight() { return IsSwipingDirection(Swipe.DownRight); }
    public static bool IsSwipingUpLeft() { return IsSwipingDirection(Swipe.UpLeft); }
    public static bool IsSwipingUpRight() { return IsSwipingDirection(Swipe.UpRight); }

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
                } else {
                    data.phase = TouchPhase.Stationary;
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
            } else {
                InputData data = inputs[spaceKeyIndex];
                if (pressPos != data.startPosition) {
                    data.phase = TouchPhase.Moved;
                } else {
                    data.phase = TouchPhase.Stationary;
                }
                data.prevPosition = data.currentPosition;
                data.currentPosition = pressPos;
                inputs[spaceKeyIndex] = data;
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
        float angle = instance.useEightDirections ? eightDirAngle : fourDirAngle;
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
#region Helper Classes
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

class CardinalDirection {
    public static readonly Vector2 Up        = new Vector2(0, 1);
    public static readonly Vector2 Down      = new Vector2(0, -1);
    public static readonly Vector2 Right     = new Vector2(1, 0);
    public static readonly Vector2 Left      = new Vector2(-1, 0);
    public static readonly Vector2 UpRight   = new Vector2(1, 1);
    public static readonly Vector2 UpLeft    = new Vector2(-1, 1);
    public static readonly Vector2 DownRight = new Vector2(1, -1);
    public static readonly Vector2 DownLeft  = new Vector2(-1, -1);
}

public enum Swipe {
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

public enum Gesture {
    None,
    Pinch,
    Rotate
}

#endregion

