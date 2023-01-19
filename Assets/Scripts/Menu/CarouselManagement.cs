using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselManagement : MonoBehaviour
{
    // Reference of the different image in the carousel
    [SerializeField]
    private GameObject ImageGO_Next;
    [SerializeField]
    private GameObject ImageGO_Previous;
    [SerializeField]
    private GameObject ImageGO_Selected;

    // List of the different map's image that will be displayed
    private Texture2D[] MapTextures;
    private string Path;

    // Current index of the mp that is selected
    [HideInInspector]
    public int index = 0;

    private void Start() 
    {
        Path = "Menu/Map";
        MapTextures = Resources.LoadAll<Texture2D>(Path);

        UpdateCarousel();
    }

    public void UpdateCarousel()
    {
        ImageGO_Selected.GetComponent<RawImage>().texture = MapTextures[(index%MapTextures.Length+MapTextures.Length)%MapTextures.Length];
        ImageGO_Previous.GetComponent<RawImage>().texture = MapTextures[((index-1)%MapTextures.Length+MapTextures.Length)%MapTextures.Length];
        ImageGO_Next.GetComponent<RawImage>().texture = MapTextures[((index+1)%MapTextures.Length+MapTextures.Length)%MapTextures.Length];
    }

    public void Next()
    {
        index++;
        UpdateCarousel();
    }

    public void Previous()
    {
        index--;
        UpdateCarousel();
    }

    public int getSceneID()
    {
        return (index%MapTextures.Length+MapTextures.Length)%MapTextures.Length;
    }
}

