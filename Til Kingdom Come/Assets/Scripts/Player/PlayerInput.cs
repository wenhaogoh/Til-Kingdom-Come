using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private KeyCode leftKey;
        private KeyCode rightKey;
        private KeyCode rollKey;
        private KeyCode attackKey;
        private KeyCode blockKey;
        private KeyCode skillKey;
        private bool attemptLeft;
        private bool attemptRight;
        private bool attemptRoll;
        private bool attemptAttack;
        private bool attemptBlock;
        private bool attemptSkill;

        public bool AttemptLeft => attemptLeft;
        public bool AttemptRight => attemptRight;
        public bool AttemptRoll => attemptRoll;
        public bool AttemptAttack => attemptAttack;
        public bool AttemptBlock => attemptBlock;
        public bool AttemptSkill => attemptSkill;

        private void Awake()
        {

        }

        private void Start()
        {
        }
        
        private void Update()
        {
            InputManager();
        }

        public void SetInput(int playerNo)
        {
            if (playerNo == 1)
            {
                leftKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", "A"));
                rightKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", "D"));
                rollKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Roll", "S"));
                attackKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Attack", "F"));
                blockKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Block", "G"));
                skillKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Skill", "H"));
            }
            else if (playerNo == 2)
            {
                leftKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", "LeftArrow"));
                rightKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", "RightArrow"));
                rollKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Roll", "DownArrow"));
                attackKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Attack", "Slash"));
                blockKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Block", "Period"));
                skillKey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Skill", "Comma"));
            }
        }
        
        private void InputManager()
        {
            attemptLeft = Input.GetKey(leftKey);
            attemptRight = Input.GetKey(rightKey);
            attemptRoll = Input.GetKeyDown(rollKey);
            attemptAttack = Input.GetKeyDown(attackKey);
            attemptBlock = Input.GetKeyDown(blockKey);
            attemptSkill = Input.GetKeyDown(skillKey);
        }
    }
}