using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public enum AnimationDirection
    {
        RIGHT = 0,
        LEFT = 1
    }
    public interface IEntityAnimationController
    {
        void SetJumpAnimationState(bool IsJumping);

        void SetRunAnimationState(bool isRunning);

        void SetRunAnimationSpeed(float speed);

        void SetShootingAnimationState(bool isShooting);

        void SetFallAnimationState(bool isFalling);

        void SetLandingAnimationState(bool isLanding);

        void SetDirection(AnimationDirection direction);
    }
}

