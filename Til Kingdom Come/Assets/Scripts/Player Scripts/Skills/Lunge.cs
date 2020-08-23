using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Lunge : Skill
    {
        private float lungeDistance = 150f;
        private int opponentKnockBackDistance = 5;
        private float channelDuration = 0.7f;
        private float lungeRange = 10f;
        private int damage = 50;
        private float yOffset = 2f;
        private float tempDrag = 15f;
        private LayerMask playerLayerMask;
        
        // After Image fields
        public GameObject afterImagePrefab;
        public Queue<GameObject> afterImagePool = new Queue<GameObject>();
        private float distanceBetweenImages = 1.5f;
        private float lastImageXpos;
        private bool isLunging;
        private int poolSize = 5;
        private float fadeDelay = 2.5f;

        private void Awake()
        {
            playerLayerMask = 1 << 8;
            GrowPool();
        }

        public override void Cast(Player player)
        {
            if (!CanCast()) return;
            
            AudioController.instance.PlaySoundEffect("Lunge");
            StartCoroutine(LungeAnimDelay(player));
            StartCoroutine(SpawnAfterImage(player));

        }

        private IEnumerator LungeAnimDelay(Player player)
        {
            // Increase drag temporarily
            var originalDrag = player.rb.drag;
            player.rb.drag = tempDrag;

            player.combatState = Player.CombatState.Combat;
            player.playerInput.DisableInput();
            player.anim.SetTrigger(name);
            yield return new WaitForSeconds(channelDuration);
            DetectHit(player);
            player.rb.AddForce(player.transform.right * lungeDistance, ForceMode2D.Impulse);
            yield return new WaitForSeconds(AnimationTimes.instance.LungeAnim - channelDuration);
            player.rb.velocity = Vector2.zero;
            player.combatState = Player.CombatState.NonCombat;
            player.playerInput.EnableInput();
            
            // Restore drag to default value
            yield return new WaitUntil(() => player.rb.velocity == Vector2.zero);
            player.rb.drag = originalDrag;
            
            yield return null;
        }

        private void GrowPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var afterImage = Instantiate(afterImagePrefab);
                AddToPool(afterImage);
            }
        }

        private void AddToPool(GameObject afterImage)
        {
            afterImage.SetActive(false);
            afterImagePool.Enqueue(afterImage);
        }

        private GameObject GetFromPool()
        {
            if (afterImagePool.Count == 0)
            {
                GrowPool();
            }

            var afterImage = afterImagePool.Dequeue();
            afterImage.SetActive(true);
            return afterImage;
        }

        private IEnumerator SpawnAfterImage(Player player)
        {
            var time = 0f;
            var animationTime = AnimationTimes.instance.LungeAnim;
            while (time < animationTime)
            {
                if (Mathf.Abs(player.transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    var afterImageGameObject = GetFromPool();
                    var afterImage = afterImageGameObject.GetComponent<PlayerAfterImageSprite>();
                    afterImage.InitializeValues(player.spriteRenderer.sprite, player.transform);
                    lastImageXpos = player.transform.position.x;
                    StartCoroutine(AfterImageFadeDelay(afterImageGameObject));
                }
                time += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        private IEnumerator AfterImageFadeDelay(GameObject afterImage)
        {
            yield return new WaitForSeconds(fadeDelay);
            AddToPool(afterImage);
            yield return null;
        }

        private void DetectHit(Player player)
        {
            var direction = player.IsFacingLeft()
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
            var rayOffset = player.transform.position + new Vector3(0, yOffset);
            Debug.DrawRay(rayOffset, direction * lungeRange, Color.green, 3);
            RaycastHit2D[] rayCasts = Physics2D.RaycastAll(rayOffset, direction, lungeRange, playerLayerMask);
            foreach (var hit2D in rayCasts)
            {
                var target = hit2D.collider.GetComponent<Player>();
                if (target == player) continue;
                if (target.combatState == Player.CombatState.Blocking &&
                    Player.IsOpponentFacingPlayer(player, target))
                {
                    // successful block
                    target.SuccessfulBlock();
                    target.KnockBack(opponentKnockBackDistance);
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
        }
        
    }
}