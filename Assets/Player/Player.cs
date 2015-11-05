using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour, IDamageable<float>, IKillable {

    public IControllable avatar;
    public float health = 100;
    float direction = 1f;
    // Use this for initialization
    void Start () {
        if(avatar == null)
        {
            avatar = gameObject.GetComponent<IControllable>();
        }
	}
	
	void playerMove(){

		/*
		 * Retrieves axis from analog stick, then checks for keyboard input
		 */ 
		float axis = Input.GetAxis ("Horizontal");
		if (Input.GetKey ("d")) {
			axis = 1.0f;
		} else if (Input.GetKey ("a")) {
			axis = -1.0f;
		}

		avatar.move (axis);
        direction = axis;

	}

	/*
	 * Tells when jump starts based on when the jump button is pressed, and when the 
	 * jump ends based on when the jump button is released.
	 */ 
	void playerJump(){
        if (Input.GetButtonDown("Jump"))
        {
            avatar.jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            avatar.doneJump();
        }
		//jump (Input.GetButtonDown ("Jump"), Input.GetButtonUp ("Jump"));

	}

	void playerShoot(){
		if (Input.GetKeyDown ("e")) {
			print ("Shots Fired!");
			avatar.shoot(0);
		}
	}

    // Update is called once per frame
    void Update () {
		playerMove ();
		playerJump ();
		playerShoot ();
		print (transform.rotation.ToString());
    }

    public void takeDamage(float damage)
    {
        health = health - damage;
        if(health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Die()
    {
        //animate
        //make sound
        //reset scene?
        Destroy(this.gameObject);
    }
}
