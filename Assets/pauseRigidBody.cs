using UnityEngine;
using System.Collections;

public class pauseRigidBody : MonoBehaviour {

    public Vector3 savedVelocity;
    private bool paused = false;
    private Rigidbody body;

	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                onResume();
            }
            else
            {
                onPause();
            }
        }
        if (paused)
        {
            body.velocity = body.velocity = new Vector3(0, 0, 0);
        }
	}

    public void onPause()
    {
        paused = true;
        savedVelocity = body.velocity;
        body.velocity = new Vector3(0, 0, 0);
    }

    public void onResume()
    {
        paused = false;
        body.velocity = savedVelocity;
    }
}
