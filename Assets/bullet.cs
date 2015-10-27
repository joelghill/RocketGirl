using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour , IKillable {
    public float speed = 5;
    bool hostile = false;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody>();
        //put bullet close to camera
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z + 1);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void makeHostile(bool value)
    {
        hostile = value;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
