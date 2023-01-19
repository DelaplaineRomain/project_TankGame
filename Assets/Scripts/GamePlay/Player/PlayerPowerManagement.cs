using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerManagement : MonoBehaviour
{
    // Apply the power up to the player
    public void ApplyPowerUP (int PowerUpId)
    {
        // apply the correct power up
        switch (PowerUpId)
        {
            case 1:
                Power1();
                break;
            case 2:
                Power2();
                break;
            case 3:
                Power3();
                break;
            case 4:
                Power4();
                break;
            default :
                Debug.LogWarning("Power Up Id not recognized");
                break;
            
        }
    }

    // Power 1 : Give back the player his whole life
    private void Power1 ()
    {
        this.gameObject.GetComponent<PlayerResourceManagement>().ResetHealth();
        Debug.Log("Power 1 received");
    }
    
    // Power 2 : Give the player a temporarly shield : the shield break after x second
    private void Power2 ()
    {
        this.gameObject.GetComponent<PlayerResourceManagement>().PU_TurnOnShield();
        Debug.Log("Power 2 received");
    }

    // Power 3 : Increase the speed of the player 
    private void Power3()
    {
        this.gameObject.GetComponent<PlayerController>().PU_IncreaseSpeed();
        Debug.Log("Power 3 received");
    }

    // Power 3 : Switch the fire mode into auto fire with a higher rate of fire
    private void Power4()
    {
        this.gameObject.GetComponent<PlayerShootManagement>().PU_AutoFireOn();
        Debug.Log("Power 3 received");
    }
}
