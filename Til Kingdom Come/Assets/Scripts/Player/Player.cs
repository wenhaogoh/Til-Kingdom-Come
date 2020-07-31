using System;
using System.Collections;
using Player.Interfaces;
using UnityEngine;
using UnityEngine.Timeline;

namespace Player
{
    public class Player : Entity, IDamageable
    {
        public static int totalPlayers = 0;
        private int playerNo;
        public enum CombatState { NonCombat, Blocking, Rolling, Hurt, Combat, Dead}

        #region UNITY COMPONENTS

        private Rigidbody2D rb;
        private SpriteRenderer sprite;
        private Animator anim;

        #endregion
        
        #region PLAYER VARIABLES

        private float moveSpeed = 4f;
        private PlayerInput playerInput;
        private bool invulnerable = false;
        public CombatState combatState = CombatState.NonCombat;

        #endregion

        #region SKILLS

        // public Attack attack;

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
        }

        private void Update()
        {
            if (combatState == CombatState.Dead) return;

            if (combatState == CombatState.NonCombat)
            {
                ListenForMovement();
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

        private void RotateLeft()
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        private void RotateRight()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public bool IsFacingRight()
        {
            return Math.Abs(transform.rotation.y - 180f) < Mathf.Epsilon;
        }
        
        public bool IsFacingLeft()
        {
            return Math.Abs(transform.rotation.y) < Mathf.Epsilon;
        }

        public void TakeDamage(int damage)
        {
            if (invulnerable) return;
            
            health.DecreaseHealth(damage);
            if (health.IsDead())
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
            
        }

        private IEnumerator Hurt()
        {
            yield return null;
        }
    }
}