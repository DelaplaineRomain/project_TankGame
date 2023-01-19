using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserInputSettings : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerX;
    private GameObject ButtonXPressed;
    public GameObject MessageBox;

    public static PlayerXsettings Player1Settings = null;
    public static PlayerXsettings Player2Settings = null;

    private PlayerXsettings CurrentPlayerSettings;

    private bool KeyListenerActive = false;

    private void Awake()
    {
        if (PlayerX.tag.Equals("1") && Player1Settings == null)
        {
            Player1Settings = new PlayerXsettings(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space);
        }
        else if (PlayerX.tag.Equals("2") && Player2Settings == null)
        {
            Player2Settings = new PlayerXsettings(KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad4, KeyCode.Keypad0);
        }
    }

    private void OnGUI()
    {
        if (KeyListenerActive)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                ButtonXPressed.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                MessageBox.SetActive(false);
                ChangeKeyListenerActive();
                if (PlayerX.tag == "1")
                {
                    Player1Settings.ChangeKeyCode(ButtonXPressed.tag, e.keyCode);
                }
                else if (PlayerX.tag == "2")
                {
                    Player2Settings.ChangeKeyCode(ButtonXPressed.tag, e.keyCode);
                } 
            }
        }
        
    }

    public void UpdateUserKeyInput (GameObject ButtonPressed)
    {
        ButtonXPressed = ButtonPressed;
        ChangeKeyListenerActive();
    }

    private void ChangeKeyListenerActive ()
    {
        KeyListenerActive = !KeyListenerActive;
    }

    private void OnEnable()
    {
        Debug.Log("Oneable start");
        if (PlayerX.tag == "1")
        {
            CurrentPlayerSettings = Player1Settings;
        }
        else if (PlayerX.tag == "2")
        {
            CurrentPlayerSettings = Player2Settings;
        }

        if (CurrentPlayerSettings == null)
        {
            Debug.Log("CurrentPlayerSettings is null");
        }

        foreach (Transform child in transform)
        {
            if (!child.name.Equals("Info"))
            {
                Transform CurrentButton = child.GetChild(0);
                if (child.tag == "Up")
                {
                    CurrentButton.GetComponent<Text>().text = CurrentPlayerSettings.Up.ToString();
                }
                else if (child.tag.Equals("Down"))
                {
                    CurrentButton.GetComponent<Text>().text = CurrentPlayerSettings.Down.ToString();
                }
                else if (child.tag.Equals("Right"))
                {
                    CurrentButton.GetComponent<Text>().text = CurrentPlayerSettings.Right.ToString();
                }
                else if (child.tag.Equals("Left"))
                {
                    CurrentButton.GetComponent<Text>().text = CurrentPlayerSettings.Left.ToString();
                }
                else if (child.tag.Equals("Shoot"))
                {
                    CurrentButton.GetComponent<Text>().text = CurrentPlayerSettings.Shoot.ToString();
                }
            }
        }
    }

}
