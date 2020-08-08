using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Attack : Skill, ICharge
    {
        private LayerMask playerLayerMask;
        private Charge charge;
        private int maxCharge = 3;
        private float chargeTime = 4f;
        private int damage = 20;
        private int finalComboDamage = 40;
        private float reactionDelay = 0.3f;
        private float attackDistance = 4.5f;
        private float moveDistance = 12f;
        private Combo combo = new Combo();
        public Sprite[] sprites = new Sprite[3];

        public GameObject finalComboParticle;
        private float finalComboParticleXOffset = 4f;
        private CinemachineImpulseSource cinemachineImpulseSource;

        private void Awake()
        {
            name = "Attack";
            cooldown = chargeTime;
            charge = new Charge(maxCharge, chargeTime);
            playerLayerMask = 1 << 8;
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            Charging();
            combo.UpdateDecay();
            icon = sprites[(int) combo.CurrentCombo];
        }

        public override void Cast(Player player)
        { 
            if (charge.IsChargeEmpty())
            {
                return;
            }

            // Activate dark mask when charge is full
            if (charge.IsFullyCharged())
            {
                nextAvailableTime = Time.time + chargeTime;
            }
            charge.DecreaseCharge();
            combo.SetDecay();
            switch (combo.CurrentCombo)
            {
                case (Combo.ComboNumber.One):
                    StartCoroutine(ComboOneAnimDelay(player));
                    break;
                case (Combo.ComboNumber.Two):
                    StartCoroutine(ComboTwoAnimDelay(player));
                    break;
                case (Combo.ComboNumber.Three):
                    StartCoroutine(ComboThreeAnimDelay(player));
                    break;
            }
        }

        #region COMBOS

        private IEnumerator ComboOneAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(reactionDelay);
            DetectHit(player, damage);
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
            player.combatState = Player.CombatState.NonCombat;
        }
        
        private IEnumerator ComboTwoAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger("Attack 2");
            var velocity = player.rb.velocity;
            player.rb.velocity = player.IsFacingLeft()
                ? new Vector2(-moveDistance, velocity.y)
                : new Vector2(moveDistance, velocity.y); 
            yield return new WaitForSeconds(reactionDelay);
            DetectHit(player, damage);
            yield return new WaitForSeconds(AnimationTimes.instance.Attack2Anim - reactionDelay);
            player.combatState = Player.CombatState.NonCombat;
        }
        
        private IEnumerator ComboThreeAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger("Attack 3");
            var velocity = player.rb.velocity;
            player.rb.velocity = player.IsFacingLeft()
                ? new Vector2(-moveDistance, velocity.y)
                : new Vector2(moveDistance, velocity.y); 
            yield return new WaitForSeconds(reactionDelay);
            DetectHit(player, finalComboDamage);
            yield return new WaitForSeconds(AnimationTimes.instance.Attack3Anim - reactionDelay);

            // Final combo particles
            var finalComboParticleOffset = player.IsFacingLeft()
                ? new Vector3(-finalComboParticleXOffset, 0)
                : new Vector3(finalComboParticleXOffset, 0); 
            Instantiate(finalComboParticle, player.transform.position + finalComboParticleOffset,
                Quaternion.identity);
            cinemachineImpulseSource.GenerateImpulse();
            player.combatState = Player.CombatState.NonCombat;
        }

        #endregion

        private void DetectHit(Player player, int damage)
        {
            var playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            var direction = player.IsFacingLeft()
                ? new Vector2(-1, 0) // left
                : new Vector2(1, 0); // right
            var yOffset = 2f;
            var xOffset = 0f;
            var tempOffset = player.IsFacingLeft()
                ? new Vector2(-xOffset, yOffset)
                : new Vector2(xOffset, yOffset);
            // Debug.DrawRay(playerPosition + tempOffset, direction * attackDistance, Color.red, 3);
            RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPosition + tempOffset, direction, attackDistance, playerLayerMask);
            foreach (var hit2D in rayCasts)
            {
                Player target = hit2D.collider.GetComponent<Player>();
                if (target == player) continue;
                if (target.combatState == Player.CombatState.Blocking &&
                    Player.IsOpponentFacingPlayer(player, target))
                {
                    // successful block
                    target.SuccessfulBlock();
                }
                else if (target.combatState == Player.CombatState.Rolling)
                {
                    // successful roll, play sound effect
                }
                else if (target.combatState == Player.CombatState.Dead)
                {
                    // ??? play sound effect   
                }
                else
                {
                    // add sfx
                    target.TakeDamage(damage);
                }
                
            }
            combo.UpdateCombo();
        }
        
        private void Charging()
        {
            if (!charge.IsFullyCharged())
            {
                if (Time.time >= nextAvailableTime)
                {
                    charge.IncreaseCharge();
                    if (!charge.IsFullyCharged())
                    {
                        nextAvailableTime = Time.time + chargeTime;
                    }
                }

            }
        }

        public int GetCurrentCharge()
        {
            return charge.GetCurrentCharge();
        }
    }
}
