using UnityEngine;
using System.Collections;

public class AvatarCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	/*
	 * 
	 */
	public bool collideTop()
	{
		Vector3 pos = this.transform.position;
		
		Vector3 rayBottom = new Vector3(pos.x, pos.y + (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomLeft = new Vector3(pos.x-0.4f, pos.y + (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomRight = new Vector3(pos.x+0.4f, pos.y + (transform.lossyScale.y)/5, pos.z-1000);
		
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		
		bool downCenter = Physics.Raycast(rayBottom, Camera.main.transform.forward, out hit1);
		bool downLeft = Physics.Raycast(rayBottomLeft, Camera.main.transform.forward, out hit2);
		bool downRight = Physics.Raycast(rayBottomRight, Camera.main.transform.forward, out hit3);
		
		
		if (downCenter) {

			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downLeft) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downRight) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else {
			
			return false;
			
		}
		
		//return !(Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit, rayDistance)); 
	}
	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public bool collideLeft(){
		
		Vector3 pos = this.transform.position;
		
		Vector3 rayLeft = new Vector3(pos.x - 0.5f, pos.y, pos.z-1000);
		Vector3 rayLeftUp = new Vector3(pos.x - 0.5f, pos.y+0.5f, pos.z-1000);
		Vector3 rayLeftDown = new Vector3(pos.x - 0.5f, pos.y-0.5f, pos.z-1000);
		
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		
		bool left = Physics.Raycast(rayLeft, Camera.main.transform.forward, out hit1);
		bool leftUp = Physics.Raycast(rayLeftUp, Camera.main.transform.forward, out hit2);
		bool leftDown = Physics.Raycast(rayLeftDown, Camera.main.transform.forward, out hit3);
		
		
		if (left) {
			
			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else if (leftUp) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else if (leftDown) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else {
			
			return false;
			
		}
		
	}
	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public bool collideRight(){
		
		Vector3 pos = this.transform.position;
		
		Vector3 rayRight = new Vector3(pos.x + 0.5f, pos.y, pos.z-1000);
		Vector3 rayRightUp = new Vector3(pos.x + 0.5f, pos.y+0.5f, pos.z-1000);
		Vector3 rayRightDown = new Vector3(pos.x + 0.5f, pos.y-0.5f, pos.z-1000);
		
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		
		bool right = Physics.Raycast(rayRight, Camera.main.transform.forward, out hit1);
		bool rightUp = Physics.Raycast(rayRightUp, Camera.main.transform.forward, out hit2);
		bool rightDown = Physics.Raycast(rayRightDown, Camera.main.transform.forward, out hit3);
		
		
		if (right) {
			
			
			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			print(sc.getHorzCollision(this.gameObject).ToString());
			//transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else if (rightUp) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else if (rightDown) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return sc.getHorzCollision(this.gameObject);
			
		} else {
			
			return false;
			
		}
		
	}
	
	/*
	 * Sends raycast bellow the character. If it hits an object, the character is grounded
	 */
	public bool isGrounded()
	{
		Vector3 pos = this.transform.position;
		
		Vector3 rayBottom = new Vector3(pos.x, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomLeft = new Vector3(pos.x-0.4f, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomRight = new Vector3(pos.x+0.4f, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
		
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		
		bool downCenter = Physics.Raycast(rayBottom, Camera.main.transform.forward, out hit1);
		bool downLeft = Physics.Raycast(rayBottomLeft, Camera.main.transform.forward, out hit2);
		bool downRight = Physics.Raycast(rayBottomRight, Camera.main.transform.forward, out hit3);
		
		
		if (downCenter) {
			
			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downLeft) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downRight) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();
			//transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else {
			
			return false;
			
		}
		
		//return !(Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit, rayDistance)); 
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
