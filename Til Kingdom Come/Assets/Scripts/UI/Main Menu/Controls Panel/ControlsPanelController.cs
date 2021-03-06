﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Controls_Panel
{
    public class ControlsPanelController : MonoBehaviour
    {
        public enum Messages
        {
            Select, Choose, Duplicate, Assign
        }
        private GameObject currentKey;
        private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
        [Header("Player 1")]
        public TextMeshProUGUI p1left, p1right, p1roll, p1attack, p1block, p1skill;
        [Header("Player 2")]
        public TextMeshProUGUI p2left, p2right, p2roll, p2attack, p2block, p2skill;
        [Header("Messages")]
        public GameObject[] messages;
        void Start()
        {
            keys.Add("P1Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", "A")));
            keys.Add("P1Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", "D")));
            keys.Add("P1Roll", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Roll", "S")));
            keys.Add("P1Attack", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Attack", "F")));
            keys.Add("P1Block", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Block", "G")));
            keys.Add("P1Skill", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Skill", "H")));

            keys.Add("P2Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", "LeftArrow")));
            keys.Add("P2Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", "RightArrow")));
            keys.Add("P2Roll", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Roll", "DownArrow")));
            keys.Add("P2Attack", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Attack", "Slash")));
            keys.Add("P2Block", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Block", "Period")));
            keys.Add("P2Skill", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Skill", "Comma")));

            p1left.text = keys["P1Left"].ToString();
            p1right.text = keys["P1Right"].ToString();
            p1roll.text = keys["P1Roll"].ToString();
            p1attack.text = keys["P1Attack"].ToString();
            p1block.text = keys["P1Block"].ToString();
            p1skill.text = keys["P1Skill"].ToString();

            p2left.text = keys["P2Left"].ToString();
            p2right.text = keys["P2Right"].ToString();
            p2roll.text = keys["P2Roll"].ToString();
            p2attack.text = keys["P2Attack"].ToString();
            p2block.text = keys["P2Block"].ToString();
            p2skill.text = keys["P2Skill"].ToString();
        }
        void OnGUI()
        {
            if (currentKey != null)
            {
                Event e = Event.current;
                if (e.isKey)
                {
                    // check for duplicate keys
                    bool isDuplicate = false;
                    foreach (var key in keys)
                    {
                        if (key.Key != currentKey.name && key.Value == e.keyCode)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                    // check for KeyCode.None
                    bool isNone = e.keyCode == KeyCode.None;

                    if (!isDuplicate && !isNone)
                    {
                        keys[currentKey.name] = e.keyCode;
                        currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                        currentKey = null;
                        EnableMessage((int) Messages.Select);
                    }
                    else
                    {
                        EnableMessage((int) Messages.Duplicate);
                    }
                }
            }
        }
        public void ChangeKey(GameObject clicked)
        {
            if (currentKey == null)
            {
                currentKey = clicked;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = '"'.ToString();
                EnableMessage((int) Messages.Choose);
            }
            else
            {
                if (currentKey == clicked)
                {
                    EnableMessage((int) Messages.Choose);
                }
                else
                {
                    EnableMessage((int) Messages.Assign);
                }
            }
        }
        public void SaveKeys()
        {
            foreach (var key in keys)
            {
                PlayerPrefs.SetString(key.Key, key.Value.ToString());
            }

            PlayerPrefs.Save();
        }

        private void EnableMessage(int index)
        {
            int len = messages.Length;
            for (int i = 0; i < len; i++)
            {
                if (i == index)
                {
                    GameObject message = messages[i];
                    message.SetActive(true);
                    ErrorMessageController err = message.GetComponent<ErrorMessageController>();
                    if (err != null)
                    {
                        err.Begin();
                    }
                }
                else
                {
                    messages[i].SetActive(false);
                }
            }
        }
    }
}
