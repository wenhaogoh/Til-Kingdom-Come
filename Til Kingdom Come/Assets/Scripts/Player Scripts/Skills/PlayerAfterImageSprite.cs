using System;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class PlayerAfterImageSprite : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private float timeActivated;
        private float alpha;
        // starting alpha value
        private float alphaSet = 1f;
        // how much the alpha is decreased
        private float alphaMultiplier = 0.96f;
    
        private Color color;

        private void OnEnable()
        {
            alpha = alphaSet;
            timeActivated = Time.time;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void InitializeValues(Sprite sprite, Transform playerTransform)
        {
            transform.position = playerTransform.position;
            transform.rotation = playerTransform.rotation;
            spriteRenderer.sprite = sprite;
        }
        
        private void Update()
        {
            alpha *= alphaMultiplier;
            color = new Color(1f, 1f, 1f, alpha);
            spriteRenderer.color = color;
        }

        public bool IsAlphaZero()
        {
            return Math.Abs(alpha) < Mathf.Epsilon;
        }
    }
}