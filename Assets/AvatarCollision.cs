using UnityEngine;
using System.Collections;

public class AvatarCollision : MonoBehaviour {

	protected Vector3 VertColPos;
	protected Vector3 HorColPos;
	protected float height;

	// Use this for initialization
	void Start () {
		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
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

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x + 0.5f > pos.x -0.5f){
				print ("bawls");
				transform.Translate(0.01f,0,0);
			}

			return sc.getHorzCollision(this.gameObject);
			
		} else if (leftUp) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x + 0.5f > pos.x -0.5f){
				print ("bawls");
				transform.Translate(0.01f,0,0);
			}

			return sc.getHorzCollision(this.gameObject);
			
		} else if (leftDown) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x + 0.5f > pos.x -0.5f){
				print ("bawls");
				transform.Translate(0.01f,0,0);
			}

			if(sc.gameObject.transform.position == VertColPos && sc.tag== "solid"){
				transform.Translate(0.05f,0,0);
				HorColPos = sc.transform.position;
				return sc.getHorzCollision(this.gameObject);
			}else{
				HorColPos = sc.transform.position;
				return sc.getHorzCollision(this.gameObject);
			}
			//return sc.getHorzCollision(this.gameObject);
			
		} else {

			HorColPos = new Vector3(0,0,-10000);
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

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x - 0.5f < pos.x +0.5f){
				transform.Translate(-0.01f,0,0);
			}

			return sc.getHorzCollision(this.gameObject);
			
		} else if (rightUp) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x - 0.5f < pos.x +0.5f){
				transform.Translate(-0.01f,0,0);
			}

			return sc.getHorzCollision(this.gameObject);
			
		} else if (rightDown) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();

			/*
			 * Position correction for the collision
			 */ 
			if((sc.getHorzCollision(this.gameObject)) && sc.transform.position.x - 0.5f < pos.x +0.5f){
				print ("bawls");
				transform.Translate(-0.01f,0,0);
			}

			/*
			 * A check for the rare occasion in which the character collides twice with the same object (bottom and top
			 * collision) causing it to hang up on the corner of a tile. If this happens the character will be translated 
			 * back slightly and be told to keep falling
			 */ 
			if(sc.gameObject.transform.position == VertColPos && sc.tag== "solid"){
				transform.Translate(-0.05f,0,0);
				HorColPos = sc.transform.position;
				return sc.getHorzCollision(this.gameObject);
			}else{
				HorColPos = sc.transform.position;
				return sc.getHorzCollision(this.gameObject);
			}
			
		} else {

			HorColPos = new Vector3(0,0,-10000);
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

			//if(sc.tag == "SemiSolid" && transform.position.y - height/2 < sc.transform.position.y + sc.gameObject.GetComponent<BoxCollider>().bounds.size.y/2){
				
			//	return false;
			//}

			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downLeft) {
			
			SpriteCollider sc = hit2.collider.gameObject.GetComponent<SpriteCollider>();
			 
			//if(sc.tag == "SemiSolid" && transform.position.y - height/2 < sc.transform.position.y + sc.gameObject.GetComponent<BoxCollider>().bounds.size.y/2){

			//	return false;
			//}

			if(sc.gameObject.transform.position == HorColPos){
				transform.Translate(0.1f,0,0);
				//collisionPosition = sc.transform.position;
				return false;
			}else{
				VertColPos = sc.transform.position;
				return sc.getVertCollision(this.gameObject, transform.position.y);
			}
			//return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else if (downRight) {
			
			SpriteCollider sc = hit3.collider.gameObject.GetComponent<SpriteCollider>();

			//if(sc.tag == "SemiSolid" && transform.position.y - height/2 < sc.transform.position.y + sc.gameObject.GetComponent<BoxCollider>().bounds.size.y/2){

			//	return false;
			//}

			/*
			 * A check for the rare occasion in which the character collides twice with the same object (bottom and top
			 * collision) causing it to hang up on the corner of a tile. If this happens the character will be translated 
			 * back slightly and be told to keep falling
			 */ 
			if(sc.gameObject.transform.position == HorColPos){
				transform.Translate(-0.1f,0,0);
				//collisionPosition = sc.transform.position;
				return false;
			}else{
				VertColPos = sc.transform.position;
				return sc.getVertCollision(this.gameObject, transform.position.y);
			}
			//return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else {


			return false;
			
		}
		
		//return !(Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit, rayDistance)); 
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
