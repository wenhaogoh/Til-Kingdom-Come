using System;
using Player_Scripts;
using Player_Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public IHealthBar entity;
    public Image healthBarFill;

    private void Update()
    {
        healthBarFill.fillAmount = entity.GetHealthRatio();
    }
}
