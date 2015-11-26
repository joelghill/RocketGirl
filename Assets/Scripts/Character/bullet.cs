using UnityEngine;
using System.Collections;
using System;

public class bullet : MonoBehaviour , IKillable, IPauseable {
    public float speed = 5;
    bool hostile = false;
    public float power = 10;
    public Rigidbody rb;
	public SpriteRenderer render;

    private Vector3 savedVelocity;
    private bool paused = false;

	// Use this for initialization
	void Start () {
        //if (rb == null)
            //rb = gameObject.GetComponent<Rigidbody>();
        //put bullet close to camera
        //Vector3 pos = this.transform.position;
        //this.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z + 1);
        //setVelocity(speed, 0);
		if(render == null)
			render = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (paused) return;
        onHit();
	}

	public void setBody(Rigidbody x){
		rb = x;
	}

	void OnBecameInvisible(){
		Destroy(gameObject);
	}
	
	public void setVelocity(float speed, float rotation)
    {
		rb.velocity = new Vector3 (speed, 0, 0);
    }

    public void makeHostile(bool value)
    {
        hostile = value;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void onHit()
    {
        Vector3 pos = this.transform.position;
        Vector3 hitCheck = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
        RaycastHit hit;

        if (Physics.Raycast(hitCheck, Camera.main.transform.forward, out hit))
        {
            IDamageable<float> damageable = hit.collider.gameObject.GetComponent<IDamageable<float>>();
			if (damageable != null && hit.collider.gameObject.tag != "Player")
            {
                damageable.takeDamage(power);
                Die();
            }
        }
    }

    public void onPause()
    {
        paused = true;
        savedVelocity = rb.velocity;
        rb.velocity = new Vector3(0, 0, 0);
    }

    public void onResume()
    {
        paused = false;
        rb.velocity = savedVelocity;
    }
}
