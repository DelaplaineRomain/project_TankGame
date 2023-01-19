using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChoiceMenu : MonoBehaviour
{
    // Reference to the carousel
    [SerializeField]
    private GameObject CarouselGO;

    private int sceneID;

    public void StartGame()
    {
        sceneID = CarouselGO.GetComponent<CarouselManagement>().getSceneID();

        SceneManager.LoadScene("main_scene_map"+sceneID.ToString());
    }
}
