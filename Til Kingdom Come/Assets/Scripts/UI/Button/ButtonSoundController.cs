using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    public void PlaySoundEffect()
    {
        AudioController.instance.PlaySoundEffect("Button");
    }
}
