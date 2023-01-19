using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    // Reference to the Game Manager
    private GameObject GameManagerObject;

    // The power up id
    public int PowerUpId = 0;

    private void Start() 
    {
        GameManagerObject = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Apply power Up
            other.gameObject.GetComponent<PlayerPowerManagement>().ApplyPowerUP(PowerUpId);

            // Destroy the power up object
            Destroy(this.gameObject);
            GameManagerObject.GetComponent<GameManager>().CallGeneratePowerUp();
        }
    }

}
