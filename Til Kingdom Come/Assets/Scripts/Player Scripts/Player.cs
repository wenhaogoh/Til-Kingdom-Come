using System;
using System.Collections;
using Player_Scripts.Interfaces;
using UnityEngine;

namespace Player_Scripts
{
    public class Player : Entity, IDamageable
    {
        public static int totalPlayers = 0;
        private int playerNo;
        public enum CombatState { NonCombat, Blocking, Rolling, Hurt, Combat, Dead}

        private const int MAXHEALTH = 100;

        #region UNITY COMPONENTS

        public Rigidbody2D rb;
        private SpriteRenderer sprite;
        public Animator anim;

        #endregion
        
        #region PLAYER VARIABLES

        private float moveSpeed = 4f;
        private PlayerInput playerInput;
        private bool invulnerable = false;
        public CombatState combatState = CombatState.NonCombat;

        #endregion

        #region SKILLS

        public Skill[] skills = new Skill[4];

        #endregion
        
        private void Awake()
        {
            totalPlayers++;
            playerNo = totalPlayers;

            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            playerInput = gameObject.AddComponent<PlayerInput>();
            playerInput.SetInput(playerNo);
            SetMaxHealth(MAXHEALTH);
            RefillCurrentHealth();
        }

        private void Update()
        {
            if (combatState == CombatState.Dead) return;

            if (combatState == CombatState.NonCombat)
            {
                ListenForMovement();
                ListenForAttack();
            } 
            else if (combatState == CombatState.Hurt)
            {
                ListenForMovement();
            }

        }

        private void ListenForMovement()
        {
            if (playerInput.AttemptRight && playerInput.AttemptLeft)
            {
                anim.SetInteger("state", 0);
            }
            else if (playerInput.AttemptRight)
            {
                anim.SetInteger("state", 1);
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                RotateRight();
            }
            else if (playerInput.AttemptLeft)
            {
                anim.SetInteger("state", 1);
                rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
                RotateLeft();
            }
            else
            {
                anim.SetInteger("state", 0);
            }
        }

        private void ListenForAttack()
        {
            if (playerInput.AttemptAttack)
            {
                skills[0].Cast(this);
            }
        }

        private void RotateLeft()
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        private void RotateRight()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public bool IsFacingLeft()
        {
            return Math.Abs(transform.rotation.eulerAngles.y - 180f) < Mathf.Epsilon;
        }
        
        public bool IsFacingRight()
        {
            return Math.Abs(transform.rotation.eulerAngles.y) < Mathf.Epsilon;
        }

        public static bool IsOpponentFacingPlayer(Player player, Player opponent)
        {
            return (player.transform.position.x < opponent.transform.position.x && opponent.IsFacingRight()) ||
                   (player.transform.position.x > opponent.transform.position.x && opponent.IsFacingLeft());
        }

        public void TakeDamage(int damage)
        {
            if (invulnerable) return;
            
            DecreaseHealth(damage);
            if (IsDead())
            {
                Die();
            }
            else
            {
                StartCoroutine(Hurt());
            }
        }

        private void Die()
        {
            anim.SetBool("Death", true);
        }

        private IEnumerator Hurt()
        {
            yield return null;
        }

        public void AddSkill(Skill skill)
        {
            int length = skills.Length;
            for (int i = 0; i < length; i++)
            {
                if (skills[i] == null)
                {
                    skills[i] = skill;
                    return;
                }
            }
        }
    }
}