using UnityEngine;

namespace Player_Scripts
{
    public class AnimationTimes : MonoBehaviour
    {
        public static AnimationTimes instance;

        public Animator anim;

        private float hurtAnim;
        private float attackAnim;
        private float attack2Anim;
        private float attack3Anim;
        private float blockAnim;
        private float rollAnim;
        private float confusionAnim;
        private float throwKnivesAnim;
        private float healAnim;
        private float fireBallAnim;
        private float lungeAnim;
        private float deathAnim;

        #region Getters

        public float HurtAnim => hurtAnim;
        public float AttackAnim => attackAnim;
        public float Attack2Anim => attack2Anim;
        public float Attack3Anim => attack3Anim;
        public float BlockAnim => blockAnim;
        public float RollAnim => rollAnim;
        public float ConfusionAnim => confusionAnim;
        public float ThrowKnivesAnim => throwKnivesAnim;
        public float HealAnim => healAnim;
        public float FireBallAnim => fireBallAnim;
        public float LungeAnim => lungeAnim;
        public float DeathAnim => deathAnim;

        #endregion

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            UpdateAnimClipTimes();
        }

        // updates animation times
        private void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "Hurt":
                        instance.hurtAnim = clip.length;
                        break;
                    case "Attack":
                        instance.attackAnim = clip.length;
                        break;
                    case "Attack 2":
                        instance.attack2Anim = clip.length;
                        break;
                    case "Attack 3":
                        instance.attack3Anim = clip.length;
                        break;
                    case "Block":
                        instance.blockAnim = clip.length;
                        break;
                    case "Roll":
                        instance.rollAnim = clip.length;
                        break;
                    case "Confusion":
                        instance.confusionAnim = clip.length;
                        break;
                    case "ThrowKnives":
                        instance.throwKnivesAnim = clip.length;
                        break;
                    case "Heal":
                        instance.healAnim = clip.length;
                        break;
                    case "Fireball":
                        instance.fireBallAnim = clip.length;
                        break;
                    case "Lunge":
                        instance.lungeAnim = clip.length;
                        break;
                    case "Death":
                        instance.deathAnim = clip.length;
                        break;
                }
            }
        }
    }
}