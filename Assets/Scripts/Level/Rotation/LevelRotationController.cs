using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.Rotation
{
    public class LevelRotationController: MonoBehaviour
    {

        #region Private Fields

        private GameObjectRotation levelRotation;

        #endregion

        #region Public Fields

        public float RotationSpeed = 140f;

        public GameObject Level;

        public List<GameObject> NonRotatingEntities;

        public GameObject Player;

        #endregion

        /// <summary>
        /// Initialize Script
        /// </summary>
        void Start()
        {
            if(this.Player == null)
            {
                return;
            }

            if(this.NonRotatingEntities == null)
            {
                this.NonRotatingEntities = new List<GameObject>();
            }

            this.NonRotatingEntities.Add(this.Player);

            this.levelRotation = new GameObjectRotation(this.Level, this.Player.transform.position, 140, 5);

            this.levelRotation.NonRotating = this.NonRotatingEntities;

        }

        void Update()
        {
            if(this.Player == null || this.levelRotation == null)
            {
                return;
            }

            Direction PrimaryDirection = Direction.LEFT;

            bool leftRotateRequest = Input.GetButtonDown("Fire1");
            bool righRotateRequest = Input.GetButtonDown("Fire2");

            //if (Input.GetKeyDown(KeyCode.B)) {
            if (leftRotateRequest)
            {
                PrimaryDirection = Direction.LEFT;
            }
            else if (righRotateRequest)
            {
                PrimaryDirection = Direction.RIGHT;
            }

            bool startRotationNeeded = (righRotateRequest || leftRotateRequest) && this.levelRotation.IsRotating == false;

            if(startRotationNeeded)
            {
                this.levelRotation.StartRotation(PrimaryDirection);
            }
            
            this.levelRotation.UpdateRotationState();
        }
    }
}
