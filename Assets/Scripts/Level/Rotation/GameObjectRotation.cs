using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.Rotation
{
    public enum Direction {
        LEFT = -1,
        RIGHT = 1
    };

    class GameObjectRotation
    {
        #region Private Fields

        private int currentPoint = 0;

        private int previousPoint;

        private Direction currentDirection;

        #endregion

        #region Constructor

        public GameObjectRotation(
            GameObject levelToRotate,
            Vector3 rotationPoint,
            float rotationSpeed = 144f, 
            float snapDelta = 5.00f)
        {
            this.LevelToRotate = levelToRotate;

            this.RotationPoint = rotationPoint;

            this.RotationSpeed = rotationSpeed;

            this.SnapDelta = snapDelta;

            this.RotationGoals = new List<int>()
            {              
                0,
                90,
                180,
                270,
            };

        }

        #endregion

        #region Public Properties

        public GameObject LevelToRotate
        {
            get;
            set;
        }

        public List<int> RotationGoals
        {
            get;
            private set;
        }

        public List<GameObject> NonRotating
        {
            get;
            set;
        }

        public Vector3 RotationPoint
        {
            get;
            set;
        }

        public bool IsRotating
        {
            get;
            private set;
        }

        public float RotationSpeed
        {
            get;
            set;
        }

        public float SnapDelta
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public void UpdateRotationState()
        {
            if(this.IsRotating == false)
            {
                return;
            }

            this.LevelToRotate.transform.RotateAround(this.RotationPoint, this.LevelToRotate.transform.up, this.RotationSpeed * Time.deltaTime * (int)this.currentDirection);

            foreach (var entity in this.NonRotating)
            {
                entity.transform.RotateAround(entity.transform.position, this.LevelToRotate.transform.up, -1 * this.RotationSpeed * Time.deltaTime * (int)this.currentDirection);
            }

            if (this.IsPastGoal())
            {
                this.IsRotating = false;
                this.SnapToGoal();

                return;
            }
        }

        public void StartRotation(Direction direction)
        {
            this.IsRotating = true;

            this.IncrementCurrentIndex(direction, this.RotationGoals.Count());

            this.currentDirection = direction;

            foreach (var entity in this.NonRotating)
            {
                entity.SendMessage("onPause");
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Takes current object rotation and sets it to goal rotation.
        /// </summary>
        /// <param name="direction">Direction of rotation. 1 or 0</param>
        void SnapToGoal()
        {
            float remaining = this.RotationGoals[this.currentPoint] - this.LevelToRotate.transform.rotation.eulerAngles.y;
            this.LevelToRotate.transform.RotateAround(this.RotationPoint, this.LevelToRotate.transform.up, remaining);

            foreach (var entity in this.NonRotating)
            {
                entity.transform.RotateAround(entity.transform.position, this.LevelToRotate.transform.up, -1 * remaining);
                entity.SendMessage("onResume");
            }
        }

        void IncrementCurrentIndex(Direction direction, int MaxNumberPoints)
        {
            this.previousPoint = this.currentPoint;
            this.currentPoint = this.currentPoint + (int)direction;
            this.currentPoint = CorrectIndexOverflow(this.currentPoint, MaxNumberPoints-1);
        }

        int CorrectIndexOverflow(int index, int maxIndex)
        {
            if (index < 0)
            {
                return maxIndex;
            }
            if (index > maxIndex)
            {
                return 0;
            }

            return index;
        }

        private bool IsPastGoal()
        {
            float goal = this.RotationGoals[this.currentPoint];

            if (goal == 0)
            {
                goal = 360;
            }

            float previousGoal = this.RotationGoals[this.previousPoint];

            float currentAngle = this.LevelToRotate.transform.rotation.eulerAngles.y;

            bool isPast = false;

            if((int)this.currentDirection == -1 )
            {
                if(previousGoal == 90)
                {
                    if(currentAngle < goal && currentAngle > 90)
                    {
                        isPast = true;
                    }
                }
                else if(currentAngle < goal)
                {
                    isPast = true;
                }
            }
            else
            {
                if (currentAngle > goal || currentAngle < previousGoal - 10 )
                {
                    isPast = true;
                }
            }

            return isPast;
        }

        #endregion Private Helper Methods
    }
}
