using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMapController : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    private static int selectedMap = 0;
    public static int GetMap()
    {
        return selectedMap;
    }
    public static void SetMap(int i)
    {
        selectedMap = i;
    }
    private void Start()
    {
        image.sprite = sprites[selectedMap];
    }
    public void NextMap()
    {
        selectedMap++;
        if (selectedMap >= sprites.Count)
        {
            selectedMap = 0;
        }
        image.sprite = sprites[selectedMap];
    }
    public void PreviousMap()
    {
        selectedMap--;
        if (selectedMap < 0)
        {
            selectedMap = sprites.Count - 1;
        }
        image.sprite = sprites[selectedMap];
    }
}
