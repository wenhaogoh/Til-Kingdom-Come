using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillSelectionController : MonoBehaviour
{
    private static int playerOneSkill;
    private static int playerTwoSkill;
    public SkillListController skillList;
    private List<GameObject> skillCells;
    private int skillCellsCount;
    private KeyCode playerOneLeft;
    private KeyCode playerOneRight;
    private KeyCode playerTwoLeft;
    private KeyCode playerTwoRight;
    private bool multiplayer = false;
    private bool isMasterClient;
    public static int GetPlayerOneSkill()
    {
        return playerOneSkill;
    }
    public static int GetPlayerTwoSkill()
    {
        return playerTwoSkill;
    }
    public void EnableMultiplayerMode(bool isMasterClient)
    {
        this.multiplayer = true;
        this.isMasterClient = isMasterClient;
    }
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
        if (multiplayer)
        {
            if (isMasterClient)
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
            }
            else
            {
                if (Input.GetKeyDown(playerOneLeft))
                {
                    DecreasePlayerTwo();
                    UpdateSkillCellPointers();
                }
                else if (Input.GetKeyDown(playerOneRight))
                {
                    IncreasePlayerTwo();
                    UpdateSkillCellPointers();
                }
            }
        }
        else
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
        if (multiplayer)
        {
            if (isMasterClient)
            {
                for (int i = 0; i < skillCellsCount; i++)
                {
                    if (i == playerOneSkill)
                    {
                        skillCells[i].GetComponent<SkillCellController>().SelectedByPlayerOne();
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
                    if (i == playerTwoSkill)
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
        else
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
}
