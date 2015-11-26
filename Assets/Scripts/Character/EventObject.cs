using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventObject : MonoBehaviour, IEvent {

    public MessageBoard message;
    public BoxCollider2D eventCollider;

	// Use this for initialization
	void Start () {
        if(eventCollider == null)
        {
            eventCollider = gameObject.GetComponent<BoxCollider2D>();
           
        }
        if(eventCollider != null && !eventCollider.isTrigger)
        {
            eventCollider.isTrigger = true;
        }

        if(message != null)
        {
            message.enabled = false;
        }

        //adjust position of message board
        Vector3 pos = transform.position;
        message.transform.position = new Vector3(pos.x, pos.y + 2, Camera.main.transform.position.z +5);
	}

    void OnTriggerEnter(Collider other)
    {
        //if not facing same direction as camera, is not active
        if (transform.forward != Camera.main.transform.forward &&
            transform.forward != -1*Camera.main.transform.forward)
        {
            return;
        }
            if (other.gameObject.tag == "Player")
        {
            onSpriteCollisionEnter(other.gameObject);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onSpriteCollisionExit(other.gameObject);
        }

    }

    public void onSpriteCollisionEnter(GameObject other)
    {
        Debug.Log("SHOW MESSAGE");
        //adjust position of message board
        Vector3 pos = transform.position;
        message.transform.position = new Vector3(pos.x, pos.y + 2, Camera.main.transform.position.z + 5);
        message.show();
        //throw new NotImplementedException();
    }

    public void onSpriteCollisionExit(GameObject other)
    {
        //throw new NotImplementedException();
        message.hide();
        Debug.Log("HIDE MESSAGE");
    }



    // Update is called once per frame
    void Update () {
	
	}
}
