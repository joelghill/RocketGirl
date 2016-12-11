using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class PlayerAnimationController : MonoBehaviour, IEntityAnimationController
    {
        private Animator playerAnimation;
        // Use this for initialization
        void Start()
        {
            this.playerAnimation = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDirection(AnimationDirection direction)
        {
            this.playerAnimation.SetFloat("Dir", (float)direction);
        }

        public void SetFallAnimationState(bool isFalling)
        {
            this.playerAnimation.SetBool("Falling", isFalling);
        }

        public void SetJumpAnimationState(bool IsJumping)
        {
            this.playerAnimation.SetBool("jumping", IsJumping);
        }

        public void SetLandingAnimationState(bool isLanding)
        {
            this.playerAnimation.SetBool("Landing", isLanding);
        }

        public void SetRunAnimationSpeed(float speed)
        {
            this.playerAnimation.SetFloat("runSpeed", speed);
        }

        public void SetRunAnimationState(bool isRunning)
        {
            this.playerAnimation.SetBool("Running", isRunning);
        }

        public void SetShootingAnimationState(bool isShooting)
        {
            this.playerAnimation.SetBool("shooting", isShooting);
        }
    }
}


