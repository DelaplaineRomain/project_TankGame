using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Stats for player
    [SerializeField]
    private float moveSpeed = 1.0f;
    private float moveSpeedFactor = 1.5f;
    private int SpeedTime = 5;
    [SerializeField]
    private float rotationSpeed = 1.0f;
    [HideInInspector]
    public int PlayerId;

    // Player input settings
    [HideInInspector]
    public PlayerXsettings CurrentPlayerSettings;

    // Reference and variables to manage the movement
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;

    // All reference for audio management
    public AudioSource MovementAudio;               // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    private float MovementStartingVolume = 0.5f;    // Default volume of the tank movement at the begining of the game
    public AudioClip EngineIdling;                  // Audio to play when the tank isn't moving.
    public AudioClip EngineDriving;                 // Audio to play when the tank is moving.
    public float PitchRange = 0.2f;
    private float OriginalPitch;


    private void Awake()
    {
        // Register the starting folume of explosion audio
        MovementStartingVolume = MovementAudio.volume;

        // Set up the audio according to the audio factor
        SetUpAudio();
    }
    public void SetUpAudio()
    {
        // Adjust audio of all audio source :
        MovementAudio.volume = MovementStartingVolume * GameManager.VolumeFactor;
        MovementAudio.Play();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Default UserInputSettings 
        if (PlayerId == 1 && UserInputSettings.Player1Settings == null)
        {
            UserInputSettings.Player1Settings = new PlayerXsettings(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space);
        }
        else if (PlayerId == 2 && UserInputSettings.Player2Settings == null)
        {
            UserInputSettings.Player2Settings = new PlayerXsettings(KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad4, KeyCode.Keypad0);
        }

        OriginalPitch = MovementAudio.pitch;
    }

    void Update()
    {
        // Movement check
        GetInput();

        // Sound management
        EngineAudio();
    }

    private void EngineAudio()
    {
        // Play the correct audio depending on the movement of the tank
        if (Mathf.Abs(horizontalInput) < 0.1f && Mathf.Abs(verticalInput) < 0.1f) // no movement
        {
            // ... and if the audio source is currently playing the driving clip...
            if (MovementAudio.clip == EngineDriving)
            {
                // ... change the clip to idling and play it.
                MovementAudio.clip = EngineIdling;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }
        }
        else // movement
        {
            if (MovementAudio.clip == EngineIdling)
            {
                // ... change the clip to driving and play.
                MovementAudio.clip = EngineDriving;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }
        }
    }

    // Get horizontal and vertical input
    private void GetInput()
    {
        if (PlayerId == 1)
        {
            CurrentPlayerSettings = UserInputSettings.Player1Settings;
        }
        else if (PlayerId == 2)
        {
            CurrentPlayerSettings = UserInputSettings.Player2Settings;
        }

        // if (Input.GetKeyDown(CurrentPlayerSettings.Right))
        // {
        //     horizontalInput = 1;
        // }
        // else if (Input.GetKeyDown(CurrentPlayerSettings.Left))
        // {
        //     horizontalInput = -1;
        // }
        // else if (Input.GetKeyDown(CurrentPlayerSettings.Up))
        // {
        //     verticalInput = 1;
        // }
        // else if (Input.GetKeyDown(CurrentPlayerSettings.Down))
        // {
        //     verticalInput = -1;
        // }
        // else if (Input.GetKeyUp(CurrentPlayerSettings.Left) || Input.GetKeyUp(CurrentPlayerSettings.Right))
        // {
        //     horizontalInput = 0;
        // }
        // else if (Input.GetKeyUp(CurrentPlayerSettings.Up) || Input.GetKeyUp(CurrentPlayerSettings.Down))
        // {
        //     verticalInput = 0;
        // }

        if (Input.GetKeyDown(CurrentPlayerSettings.Right))
        {
            horizontalInput += 1;
        }
        if (Input.GetKeyDown(CurrentPlayerSettings.Left))
        {
            horizontalInput -= 1;
        }
        if (Input.GetKeyDown(CurrentPlayerSettings.Up))
        {
            verticalInput += 1;
        }
        if (Input.GetKeyDown(CurrentPlayerSettings.Down))
        {
            verticalInput -= 1;
        }
        if (Input.GetKeyUp(CurrentPlayerSettings.Left))
        {
            horizontalInput += 1;
        }
        if (Input.GetKeyUp(CurrentPlayerSettings.Right))
        {
            horizontalInput -= 1;
        }
        if (Input.GetKeyUp(CurrentPlayerSettings.Up))
        {
            verticalInput -= 1;
        }
        if (Input.GetKeyUp(CurrentPlayerSettings.Down))
        {
            verticalInput += 1;
        }
    }

    // FixedUpdate is called once every physics update
    void FixedUpdate()
    {
        // Movement application
        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);


        // Rotate application
        float turn = horizontalInput * rotationSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    public void PU_IncreaseSpeed()
    {
        moveSpeed *= moveSpeedFactor;
        Invoke("PU_DecreaseSpeed", SpeedTime);
    }

    public void PU_DecreaseSpeed()
    {
        moveSpeed /= moveSpeedFactor;
    }

}
