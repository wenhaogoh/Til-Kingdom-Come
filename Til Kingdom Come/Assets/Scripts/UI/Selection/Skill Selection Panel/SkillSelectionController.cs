using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillSelectionController : MonoBehaviour
{
    public static int playerOneSkill;
    public static int playerTwoSkill;
    public SkillListController skillList;
    private List<GameObject> skillCells;
    private int skillCellsCount;
    private KeyCode playerOneLeft;
    private KeyCode playerOneRight;
    private KeyCode playerTwoLeft;
    private KeyCode playerTwoRight;
    // Start is called before the first frame update
    private void Awake()
    {
        playerOneLeft = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", "A"));
        playerOneRight = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", "D"));
        playerTwoLeft = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", "LeftArrow"));
        playerTwoRight = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", "RightArrow"));
    }
    private void Start()
    {
        playerOneSkill = 0;
        playerTwoSkill = 0;
        skillCells = skillList.GetSkillCells();
        skillCellsCount = skillCells.Count;
        UpdateSkillCellPointers();
    }
    private void Update()
    {
        if (Input.GetKeyDown(playerOneLeft))
        {
            DecreasePlayerOne();
            UpdateSkillCellPointers();
        }
        else if (Input.GetKeyDown(playerOneRight))
        {
            IncreasePlayerOne();
            UpdateSkillCellPointers();
        }

        if (Input.GetKeyDown(playerTwoLeft))
        {
            DecreasePlayerTwo();
            UpdateSkillCellPointers();
        }
        else if (Input.GetKeyDown(playerTwoRight))
        {
            IncreasePlayerTwo();
            UpdateSkillCellPointers();
        }
    }
    private void IncreasePlayerOne()
    {
        playerOneSkill++;
        if (playerOneSkill >= skillCellsCount)
        {
            playerOneSkill = 0;
        }
    }
    private void IncreasePlayerTwo()
    {
        playerTwoSkill++;
        if (playerTwoSkill >= skillCellsCount)
        {
            playerTwoSkill = 0;
        }
    }
    private void DecreasePlayerOne()
    {
        playerOneSkill--;
        if (playerOneSkill < 0)
        {
            playerOneSkill = skillCellsCount - 1;
        }
    }
    private void DecreasePlayerTwo()
    {
        playerTwoSkill--;
        if (playerTwoSkill < 0)
        {
            playerTwoSkill = skillCellsCount - 1;
        }
    }
    private void UpdateSkillCellPointers()
    {
        if (playerOneSkill == playerTwoSkill)
        {
            for (int i = 0; i < skillCellsCount; i++)
            {
                if (i == playerOneSkill)
                {
                    skillCells[i].GetComponent<SkillCellController>().SelectedByBothPlayers();
                }
                else
                {
                    skillCells[i].GetComponent<SkillCellController>().NotSelected();
                }
            }
        }
        else
        {
            for (int i = 0; i < skillCellsCount; i++)
            {
                if (i == playerOneSkill)
                {
                    skillCells[i].GetComponent<SkillCellController>().SelectedByPlayerOne();
                }
                else if (i == playerTwoSkill)
                {
                    skillCells[i].GetComponent<SkillCellController>().SelectedByPlayerTwo();
                }
                else
                {
                    skillCells[i].GetComponent<SkillCellController>().NotSelected();
                }
            }
        }
    }
}
