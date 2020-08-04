using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMapController : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    public static int selectedMap = 1;
    void Start()
    {
        int index = selectedMap - 1;
        image.sprite = sprites[index];
    }
    public void NextMap()
    {
        selectedMap++;
        if (selectedMap > sprites.Count)
        {
            selectedMap = 1;
        }
        int index = selectedMap - 1;
        image.sprite = sprites[index];
    }
    public void PreviousMap()
    {
        selectedMap--;
        if (selectedMap < 1)
        {
            selectedMap = sprites.Count;
        }
        int index = selectedMap - 1;
        image.sprite = sprites[index];
    }
}
