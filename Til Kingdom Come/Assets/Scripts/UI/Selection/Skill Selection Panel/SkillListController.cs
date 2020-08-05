using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_Scripts.Skills;
using TMPro;
using UnityEngine.UI;
public class SkillListController : MonoBehaviour
{
    public GameObject skillCellPrefab;
    public List<Skill> skills;
    private List<GameObject> skillCells = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var skill in skills)
        {
            BuildSkillCell(skill);
        }
    }

    private void BuildSkillCell(Skill skill)
    {
        GameObject skillCell = Instantiate(skillCellPrefab, transform);
        skillCells.Add(skillCell);

        TextMeshProUGUI skillName = skillCell.transform.Find("Skill Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI skillInfo = skillCell.transform.Find("Skill Info").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI skillCooldown = skillCell.transform.Find("Skill Cooldown").GetComponent<TextMeshProUGUI>();
        Image skillIcon = skillCell.transform.Find("Skill Icon").GetComponent<Image>();

        skillName.text = skill.GetName();
        skillInfo.text = skill.GetInfo();
        skillCooldown.text = "Cooldown: " + skill.GetCooldown().ToString() + "s";
        skillIcon.sprite = skill.GetIcon();
    }

    public List<GameObject> GetSkillCells()
    {
        return skillCells;
    }
}
