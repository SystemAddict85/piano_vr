using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class PianoController : MonoBehaviour
{
    private SteamVR_Behaviour_Pose pose;
    private GameObject thumbStick;
    private SteamVR_Input_Sources hand;

    [SerializeField]
    private Transform sphereToRotate;
    [SerializeField]
    private Transform joystick;

    [SerializeField]
    private float sphereRotateSpeed = 750f;
    [SerializeField]
    private float joystickMaxAngle = 60f;

    private ControllerMixerInterface mixerInterface;

    private GameObject currentButton;
    private AxisEventData currentAxis;

    private static bool readyToMove = true;

    private static Vector2 trackPos;

    private void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        hand = pose.inputSource;
        mixerInterface = GetComponentInParent<ControllerMixerInterface>();
    }

    private void Update()
    {

        CheckForMusicMods();

        if (!GameManager.Instance.isGameActive)
        {
            CheckForUI();
        }
        else
        {
            readyToMove = true;
        }


    }

    private void CheckForMusicMods()
    {
        if (hand == SteamVR_Input_Sources.LeftHand)
        {
            var pitchPos = SteamVR_Actions.default_Pitch.GetAxis(hand);

            if (pitchPos.sqrMagnitude > 0f)
            {
                RotateSphere(pitchPos);
            }
            RotateJoystick(pitchPos);
            mixerInterface.PitchChange(pitchPos);

        }
        else if (hand == SteamVR_Input_Sources.RightHand)
        {
            var modPos = SteamVR_Actions.default_Modulation.GetAxis(hand);

            if (modPos.sqrMagnitude > 0f)
            {
                RotateSphere(modPos);
            }
            RotateJoystick(modPos);
            mixerInterface.ModulationChange(modPos);
        }
    }

    private void CheckForUI()
    {
        if (readyToMove && hand == SteamVR_Input_Sources.RightHand)
        {
            trackPos = SteamVR_Actions.default_Modulation.axis;

            if (trackPos == Vector2.zero)
            {
                trackPos = SteamVR_Actions.default_Pitch.axis;

                // if both trackpads are empty, check if user is using a keyboard
                if (trackPos == Vector2.zero)
                    trackPos = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            currentAxis = new AxisEventData(EventSystem.current);
            currentButton = EventSystem.current.currentSelectedGameObject;


            if (trackPos.sqrMagnitude > 0f)//TODO Change this to when they touchdown on the touchpad
            {
                StartCoroutine(WaitToMoveUI());
                if (trackPos.y > 0.7f) // move up
                {
                    currentAxis.moveDir = MoveDirection.Up;
                    ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                }
                else if (trackPos.y < -0.7f) // move down
                {
                    currentAxis.moveDir = MoveDirection.Down;
                    ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                }
                else if (trackPos.x > 0.7f) // move right
                {
                    currentAxis.moveDir = MoveDirection.Right;
                    ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                }
                else if (trackPos.x < -0.7f) // move left
                {
                    currentAxis.moveDir = MoveDirection.Left;
                    ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                }
            }
        }
        if (SteamVR_Actions.default_LeftTrigger.stateDown || SteamVR_Actions.default_RightTrigger.stateDown || Input.GetKeyDown(KeyCode.C))
        {
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.submitHandler);
        }

        if (SteamVR_Actions.default_TrackPadClick.stateDown || Input.GetKeyDown(KeyCode.B))
        {
            LevelManager.Instance.ShowMenu();
        }

    }

    IEnumerator WaitToMoveUI()
    {
        readyToMove = false;
        yield return new WaitForSeconds(.2f);
        readyToMove = true;
    }

    private void RotateJoystick(Vector2 padPos)
    {
        joystick.localEulerAngles = new Vector3(padPos.y * joystickMaxAngle, 0, -padPos.x * joystickMaxAngle);
    }

    private void RotateSphere(Vector2 padPos)
    {
        sphereToRotate.Rotate(padPos * sphereRotateSpeed * Time.deltaTime, Space.World);
    }
}