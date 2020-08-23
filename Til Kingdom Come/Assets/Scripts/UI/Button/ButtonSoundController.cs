using Audio;
using UnityEngine;

namespace UI.Button
{
    public class ButtonSoundController : MonoBehaviour
    {
        public void PlaySoundEffect()
        {
            AudioController.instance.PlaySoundEffect("Button");
        }
    }
}
