﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Selection.Map_Selection_Panel
{
    public class SetMapController : MonoBehaviour
    {
        public Image image;
        public List<Sprite> sprites = new List<Sprite>();
        private static int selectedMap = 0;
        public static int GetMap()
        {
            return selectedMap;
        }
        public static void SetMap(int map)
        {
            selectedMap = map;
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
}
