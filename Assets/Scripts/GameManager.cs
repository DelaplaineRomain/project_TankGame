using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    System.Random random;

    // Map object
    [SerializeField]
    private bool UseMap;
    [SerializeField]
    private bool UsePlayer;
    [SerializeField]
    private GameObject PrefabMap;       // Object that will be load
    private GameObject CurrentMap;      // Object that we manipulate 

    // Spawn point of the map
    private Transform SpawnPointA;      // Object that we manipulate in the script
    private Transform SpawnPointB;      // Object that we manipulate in the script

    // List of power Up spawn point of the map
    private List<Transform> PU_Spawnpoints;

    // List of the different Power Up available
    private GameObject[] PowerUps;

    // Boolean to see if there is allready a power up activated 
    [HideInInspector]
    public bool PowerUpActive = false;

    // Time between each spawn of power up
    [SerializeField]
    private int TimeBetweenPU = 5;                    // time in second

    // Player object
    private GameObject playerA;         // Object that we manipulate in the script
    private GameObject playerB;         // Object that we manipulate in the script

    [SerializeField]
    private GameObject PrefabPlayer;    // model that will be load and displayed in the game

    // bool for game status : end or not
    [SerializeField]
    private bool GameStatus = false;

    // Id of the winner
    public static int winnerId;

    // Float factor for the volume
    //TODO : at the end put the volume factor back to 1 !!!
    public static float VolumeFactor = 1f;  // Factor between 0 and 1 that is used to increase or decrease the volume 

    // Reference for graphics
    private GameObject UserInterface;
    private Transform PauseMenuPanel;
    private Transform EndGameInterface;
    private TextMeshProUGUI WinnerDisplay;

    private void Awake()
    {
        random = new System.Random();

        if (UseMap)
        {
            SetUpMap();
            GetSpawnPoint();
            LoadPowerUp();
            Invoke("GeneratePowerUp",TimeBetweenPU);
        }

        if (UsePlayer)
        {
            SetUpPlayers();
        }

        UserInterface = GameObject.Find("UserInterface");
        EndGameInterface = UserInterface.transform.GetChild(0);
        PauseMenuPanel = UserInterface.transform.GetChild(1);
        WinnerDisplay = EndGameInterface.GetComponentInChildren<TextMeshProUGUI>();

        GameStatus = true;
        Time.timeScale = 1;
    }

    void Update()
    {
        CheckPlayerLife();

        // Checking for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void LateUpdate()
    {
        if (!GameStatus)
        {
            EndGameInterface.gameObject.SetActive(true);
            WinnerDisplay.SetText("PLAYER " + winnerId.ToString() + " WIN !!!");
        }
    }

    public void UpdateAudioVolume()
    {
        // update movement audio
        playerA.GetComponent<PlayerController>().SetUpAudio();
        playerB.GetComponent<PlayerController>().SetUpAudio();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenuPanel.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenuPanel.gameObject.SetActive(false);
    }

    private void GetSpawnPoint()
    {
        Transform SpawnObject = CurrentMap.transform.Find("Spawn");
        SpawnPointA = SpawnObject.GetChild(0).transform;
        SpawnPointB = SpawnObject.GetChild(1).transform;
    }

    private void LoadPowerUp() 
    {
        PU_Spawnpoints = new List<Transform>();

        // Retrieve the differents spawnpoints available for the PU
        Transform MapPUSpawn = CurrentMap.transform.Find("PU_spawn");
        if (MapPUSpawn != null)
        {
            foreach (Transform PU_Spawnpoint in MapPUSpawn)
            {
                PU_Spawnpoints.Add(PU_Spawnpoint);
            }

            // Retrieve the differents PU
            string path = "Prefabs/PowerUp";
            PowerUps = Resources.LoadAll<GameObject>(path);
        }else
        {
            Debug.LogWarning("The current map does not contain any spawnpoint for power up.");
        }
    }

    private void GeneratePowerUp ()
    {
        int spawnIndex = random.Next(0, PU_Spawnpoints.Count);
        int PowerUpIndex = random.Next(0, PowerUps.Length);
        Instantiate(PowerUps[PowerUpIndex], PU_Spawnpoints[spawnIndex].position, PU_Spawnpoints[spawnIndex].rotation, CurrentMap.transform);
        PowerUpActive = true;
    }

    public void CallGeneratePowerUp()
    {
        Invoke("GeneratePowerUp",TimeBetweenPU);
    }

    private void SetUpMap()
    {
        CurrentMap = Instantiate<GameObject>(PrefabMap, Vector3.zero, Quaternion.identity);
        CurrentMap.name = "CurrentMap";
    }

    private void SetUpPlayers()
    {
        int playerA_ID = 1;
        playerA = Instantiate<GameObject>(PrefabPlayer);
        PlayerResourceManagement playerA_Ressource = playerA.GetComponent<PlayerResourceManagement>();
        playerA_Ressource.PlayerId = playerA_ID;
        if (SpawnPointA == null)
        {
            Debug.Log("Spawn point A null, default location apllied");
            playerA.transform.SetPositionAndRotation(new Vector3(1f,1f,0f),Quaternion.identity);
        }
        else
        {
            playerA.transform.SetPositionAndRotation(SpawnPointA.position, SpawnPointA.rotation);
        }
        playerA.name = "Player1";

        int playerB_ID = 2;
        playerB = Instantiate<GameObject>(PrefabPlayer);
        PlayerResourceManagement playerB_Ressource = playerB.GetComponent<PlayerResourceManagement>();
        playerB_Ressource.PlayerId = playerB_ID;
        if (SpawnPointB == null)
        {
            Debug.Log("Spawn point B null, default location apllied");
            playerB.transform.SetPositionAndRotation(new Vector3(-1f, 1f, 0f), Quaternion.identity);
        }
        else
        {
            playerB.transform.SetPositionAndRotation(SpawnPointB.position, SpawnPointB.rotation);
        }
        playerB.name = "Player2";
    }

    void CheckPlayerLife()
    {
        if (playerA.GetComponent<PlayerResourceManagement>().IsDead)
        {
            GameStatus = false;
            winnerId = playerB.GetComponent<PlayerResourceManagement>().PlayerId;
        }
        else if (playerB.GetComponent<PlayerResourceManagement>().IsDead)
        {
            GameStatus = false;
            winnerId = playerA.GetComponent<PlayerResourceManagement>().PlayerId;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
