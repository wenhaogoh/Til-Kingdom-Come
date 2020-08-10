﻿using System;
using System.Collections;
using Photon.Pun;
using Player_Scripts.Interfaces;
using Player_Scripts.Skills;
using UnityEngine;

namespace Player_Scripts
{
    public class Player : Entity, IDamageable
    {
        public static int totalPlayers = 0;
        private PhotonView pv;
        private int playerNo;
        public enum CombatState { NonCombat, Blocking, Rolling, Hurt, Combat, Dead}

        private const int MAXHEALTH = 100;

        #region UNITY COMPONENTS

        public Rigidbody2D rb;
        public SpriteRenderer spriteRenderer;
        public Animator anim;

        #endregion
        
        #region PLAYER VARIABLES

        private float moveSpeed = 4f;
        public PlayerInput playerInput;
        private bool invulnerable = false;
        public CombatState combatState = CombatState.NonCombat;
        private ReskinAnimation reSkinAnimation;
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;

        #endregion

        #region PARTICLE EFFECTS

        public GameObject bloodSplatter;
        private float bloodSplatterYOffset = 1.5f;
        public GameObject sparks;
        private float sparksYOffset = 1.5f;

        #endregion

        #region SKILLS

        public Skill[] skills = new Skill[4];

        #endregion

        #region HURT
        private float hurtDuration = 0.4f;
        private float hurtInterval = 0.2f;
        private float hurtDistance = 8f;
        private float hurtStunDuration = 0.2f;
        #endregion
        private void Awake()
        {
            if (GameManager.IsMultiplayer())
            {
                pv = GetComponent<PhotonView>();
            }
            
            // Set player number
            totalPlayers++;
            playerNo = totalPlayers;

            // Initialize components
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerInput = gameObject.AddComponent<PlayerInput>();
            reSkinAnimation = GetComponentInChildren<ReskinAnimation>();
            
            playerInput.SetInput(playerNo);
            reSkinAnimation.spriteSheetName = $"Player{playerNo}";
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;

            SetMaxHealth(MAXHEALTH);
            RefillCurrentHealth();
        }

        private void Update()
        {
            if (GameManager.IsMultiplayer())
            {
                if (!pv.IsMine) return;
            }
            if (combatState == CombatState.Dead) return;

            if (combatState == CombatState.NonCombat)
            {
                ListenForMovement();
                ListenForAttack();
                ListenForBlock();
                ListenForRoll();
                ListenForSkill();
            } 
            else if (combatState == CombatState.Hurt)
            {
                ListenForMovement();
                ListenForBlock();
                ListenForRoll();
            }

        }

        #region LISTENERS

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
                if (GameManager.IsMultiplayer())
                {
                    pv.RPC("RPCAttack", RpcTarget.All);
                }
                else
                {
                    skills[0].Cast(this);
                }
            }
        }

        private void ListenForBlock()
        {
            if (playerInput.AttemptBlock)
            {
                if (GameManager.IsMultiplayer())
                {
                    pv.RPC("RPCBlock", RpcTarget.All);
                }
                else
                {
                    skills[1].Cast(this);
                }
            }
        }

        private void ListenForRoll()
        {
            if (playerInput.AttemptRoll)
            {
                if (GameManager.IsMultiplayer())
                {
                    pv.RPC("RPCRoll", RpcTarget.All);
                }
                else
                {
                    skills[2].Cast(this);
                }
            }
        }

        private void ListenForSkill()
        {
            if (playerInput.AttemptSkill)
            {
                if (GameManager.IsMultiplayer())
                {
                    pv.RPC("RPCSkill", RpcTarget.All);
                }
                else
                {
                    skills[3].Cast(this);
                }
            }
        }

        #endregion
        
        #region RPC METHODS

        [PunRPC]
        private void RPCAttack()
        {
            skills[0].Cast(this);
        }
        
        [PunRPC]
        private void RPCBlock()
        {
            skills[1].Cast(this);
        }
        
        [PunRPC]
        private void RPCRoll()
        {
            skills[2].Cast(this);
        }
        
        [PunRPC]
        private void RPCSkill()
        {
            skills[3].Cast(this);
        }
        
        #endregion
        
        private void RotateLeft()
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        private void RotateRight()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        #region DIRECTION CHECKS

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
            var playerPosition = player.transform.position.x;
            var opponentPosition = opponent.transform.position.x;
            return (playerPosition < opponentPosition && opponent.IsFacingLeft()) ||
                   (playerPosition > opponentPosition && opponent.IsFacingRight());
        }

        public static bool IsPlayerFacingOpponent(Player player, Player opponent)
        {
            var playerPosition = player.transform.position.x;
            var opponentPosition = opponent.transform.position.x;
            return (opponentPosition > playerPosition && player.IsFacingRight()) ||
                   (opponentPosition < playerPosition && player.IsFacingLeft());
        }

        #endregion

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
            playerInput.DisableInput();
            anim.SetBool("Death", true);
            Instantiate(bloodSplatter, transform.position + new Vector3(0, bloodSplatterYOffset), transform.rotation);
            GameManager.onPlayerDeath.Invoke(playerNo);
        }

        private IEnumerator Hurt()
        {
            combatState = CombatState.Hurt;
            // enable god mode
            invulnerable = true;
            // start hurt animation
            var animationRoutine = StartCoroutine(HurtAnimation(hurtInterval));
            // stun player
            StartCoroutine(HurtStun(hurtStunDuration));
            // knock player up
            HurtKnockUp(hurtDistance);
            yield return new WaitForSeconds(hurtDuration);
            // stop hurt animation
            StopCoroutine(animationRoutine);
            spriteRenderer.color = Color.white;
            // disable god mode
            invulnerable = false;
            combatState = CombatState.NonCombat;
        }
        private IEnumerator HurtAnimation(float interval)
        {
            while (true)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(interval);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(interval);
            }
        }
        private IEnumerator HurtStun(float duration)
        {
            playerInput.DisableInput();
            yield return new WaitForSeconds(duration);
            playerInput.EnableInput();
        }
        private void HurtKnockUp(float distance)
        {
            var velocity = rb.velocity;
            rb.velocity = new Vector2(velocity.x, distance);
        }

        public void AddSkill(GameObject skillPrefab)
        {
            int length = skills.Length;
            for (int i = 0; i < length; i++)
            {
                if (skills[i] == null)
                {
                    var skill = Instantiate(skillPrefab, Vector3.zero, Quaternion.identity);
                    skill.transform.parent = transform;
                    skills[i] = skill.GetComponent<Skill>();
                    return;
                }
            }
        }

        public void SuccessfulBlock()
        {
            Instantiate(sparks, transform.position + new Vector3(0, sparksYOffset), transform.rotation);
        }

        public void ResetPlayer()
        {
            PlayerInput.onDisableInput.Invoke();
            disableInvulnerability();

            rb.velocity = Vector2.zero;

            anim.SetBool("Death", false);
            anim.SetInteger("state", 0);
            
            foreach (Skill skill in skills)
            {
                skill.ResetCooldown();
                if (skill is ICharge)
                {
                    ICharge chargeSkill = skill as ICharge;
                    chargeSkill.RefillCharges();
                }
            }

            RefillCurrentHealth();
            combatState = CombatState.NonCombat;
            
            // Reset rotation and position
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
        }

        public void enableInvulnerability()
        {
            invulnerable = true;
        }
        public void disableInvulnerability()
        {
            invulnerable = false;
        }

        public int GetPlayerNo()
        {
            return playerNo;
        }
    }
}