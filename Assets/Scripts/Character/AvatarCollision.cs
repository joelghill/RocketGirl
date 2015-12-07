using UnityEngine;
using System.Collections;

public class AvatarCollision : MonoBehaviour {

    public enum CollisionDirection {HORIZONTAL, VERTICAL};

	protected Vector3 VertColPos;
	protected Vector3 HorColPos;
	protected float height;

    public SpriteRenderer spriteRenderer;
    private Vector3 position;

    public float xMargin = 0;
    public float yMargin = 0;

    public bool drawPrimaryCollisionPoints = false;
    public bool drawSecondaryCollisionPoints = false;

    private Bounds startBounds;

	// Use this for initialization
	void Start () {
        startBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        position = transform.position;
	}

    public float Right()
    {
        return transform.position.x + (spriteRenderer.bounds.size.x / 2) - xMargin;

    }

    public float Left()
    {
        return transform.position.x - (spriteRenderer.bounds.size.x / 2) + xMargin;

    }

    public float Top()
    {
        return transform.position.y + (spriteRenderer.bounds.size.y / 2) + yMargin;

    }

    public float Bottom()
    {
		return transform.position.y - (startBounds.size.y / 2); //+ yMargin;

    }


    public GameObject collideTop()
	{

        Vector3[] points = getTopPrimaryPoints();
        Vector3[] points2 = getTopSecondaryPoints();

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.up, CollisionDirection.VERTICAL);
    }

	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public GameObject collideLeft(){

        Vector3[] points = getLeftPrimaryPoints();
        Vector3[] points2 = getLeftSecondaryPoints();

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.right*(-1), CollisionDirection.HORIZONTAL);
    }
	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public GameObject collideRight(){

        Vector3[] points = getRightPrimaryPoints();
        Vector3[] points2 = getRightSecondaryPoints();
        //check collision
        return twoLevelCollisionCheck(points, points2, transform.right, CollisionDirection.HORIZONTAL);

    }
	
	/*
	 * Sends raycast bellow the character. If it hits an object, the character is grounded
	 */
     /// <summary>
     /// Determines whether or not the player os grounded. Checks for collision with sprite, then uses Sprite collider to resolved collision.
     /// </summary>
     /// <returns>True or false</returns>
	public GameObject collideBottom()
	{

        Vector3[] points =getBottomPrimaryPoints();
        Vector3[] points2 = getBottomSecondaryPoints();

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.up * (-1), CollisionDirection.VERTICAL);
	}

    /// <summary>
    /// Function to check for collision using a series of check points.
    /// </summary>
    /// <param name="pointList">List of points to send rays to.</param>
    /// <param name="direction">Direction to send the ray in.</param>
    /// <param name="distance">Optional. Usually distance from center of object to border in direction of collision check.</param>
    /// <returns>A list of sprite colliders of the other object.</returns>
    public object[] checkCollisionList(Vector3[] pointList, Vector3 direction, float distance = 0.0f)
    {
        RaycastHit hit;
        ArrayList colliders = new ArrayList();
        for (int i = 0; i < pointList.Length; i++)
        {
            bool collide;
            if(distance != 0.0)
            {
                collide = Physics.Raycast(pointList[i], direction, out hit, distance);
            }
            else
            {
                collide = Physics.Raycast(pointList[i], direction, out hit);
				if(isPlayerHidden() && collide){
					//Debug.Log("Player is hidden");
					collide = false;
				}
            }
            if (collide)
            {
                colliders.Add(hit.collider.gameObject);
            }
        }
        return colliders.ToArray();
    }

    /// <summary>
    /// A collision check with 2 level verification.
    /// The First check uses rays from camera to player.
    /// If first check returns false, use alternative rays from player.
    /// This is to ensure the player still collides with objects when hidden from view.
    /// </summary>
    /// <param name="firstPoints">First set of points to send rays from. Usually at camera</param>
    /// <param name="secondPoints">Second set. Usually at player</param>
    /// <param name="secondaryDirection"> Direction to send secondary rays from. Usually left, right, up or down.</param>
    /// <returns></returns>
    public GameObject twoLevelCollisionCheck(Vector3[] firstPoints, Vector3[] secondPoints, Vector3 secondaryDirection, CollisionDirection orientation)
    {
        //The allowed distance of the ray for secondary collision check.
        float distance;
        //if it's not up or down it's left or right
        if(secondaryDirection == transform.up || secondaryDirection == -1 * transform.up)
        {
            //vertical collision from center to bottom or top of sprite
            distance = getVerticalCollisionDistance();
        }
        else
        {
            //horizontal collision is distance from center to left or right side.
            distance = getHorizontalCollisionDistance();
        }



        //check intial point list for valid collsion
        object[] o = checkCollisionList(firstPoints, Camera.main.transform.forward);
        GameObject collide = null;

        //
        if (o != null)
        {
            //resolve if there was a valid collision with other sprite.
            for(int i = 0; i < o.Length; i++)
            {
                GameObject ob = (GameObject)o[i];
                SpriteCollider sc = ob.GetComponent<SpriteCollider>();
                /*if (sc != null && sc.getVertCollision(gameObject, transform.position.y))
                {
                    collide = ob;
                    break;
                }*/
                if(orientation == CollisionDirection.VERTICAL)
                {
                    if (sc != null && sc.getVertCollision(gameObject, Bottom()))
                    {
                        collide = ob;
                        break;
                    }
                }
                else // then horizonal
                {
                    if (sc != null && sc.getHorzCollision(gameObject))
                    {
                        collide = ob;
                        break;
                    }
                }
                    
            }
        }
        //if there was no valid collision, check secondary
        //this is to avoid player falling through platforms when behind non-colliding objects.
        if (o == null || !collide)
        {
            o = checkCollisionList(secondPoints, secondaryDirection, distance);
            if (o != null)
            {
                //resolve if there was a valid collision with other sprite.
                for (int i = 0; i < o.Length; i++)
                {
                    GameObject ob = (GameObject)o[i];
                    SpriteCollider sc = ob.GetComponent<SpriteCollider>();
                    if(orientation == CollisionDirection.VERTICAL)
                    {
                        if (sc != null && sc.getVertCollision(gameObject, Bottom()))
                        {
                            collide = ob;
                            break;
                        }
                    }
                    else
                    {
                        if (sc.getHorzCollision(gameObject))
                        {
                            collide = ob;
                            break;
                        }
                    }

                }
            }
            if (o == null || !collide)
            {
                return null;
            }
        }
        //shouldn't actually get to this point....
        return collide;
    }

    private void drawPrimaryPoints()
    {
        if (!drawPrimaryCollisionPoints) return;
        Vector3[] topPoints = getTopPrimaryPoints();
        Vector3[] bottomPoints = getBottomPrimaryPoints();
        Vector3[] rightPoints = getRightPrimaryPoints();
        Vector3[] leftPoints = getLeftPrimaryPoints();

        setZ(rightPoints, transform.position.z -1);
        setZ(leftPoints, transform.position.z-1);
        setZ(topPoints, transform.position.z-1);
        setZ(bottomPoints, transform.position.z-1);

        for (int i =0; i < topPoints.Length -1; i++)
        {
            Debug.DrawLine(topPoints[i], topPoints[i + 1], Color.red, 0.0f, true);
            Debug.DrawLine(bottomPoints[i], bottomPoints[i + 1], Color.red, 0.0f, true);
            Debug.DrawLine(leftPoints[i], leftPoints[i + 1], Color.red, 0.0f, true);
            Debug.DrawLine(rightPoints[i], rightPoints[i + 1], Color.red, 0.0f, true);
        }

    }

    private void drawSecondaryPoints()
    {
        if (!drawSecondaryCollisionPoints) return;
        Vector3[] topPoints = getTopSecondaryPoints();
        Vector3[] bottomPoints = getBottomSecondaryPoints();
        Vector3[] rightPoints = getRightSecondaryPoints();
        Vector3[] leftPoints = getLeftSecondaryPoints();

        Vector3[] secondtopPoints = getTopLinePoints(topPoints);
        Vector3[] secondbottomPoints = getBottomLinePoints(bottomPoints);
        Vector3[] secondrightPoints = getRightLinePoints(rightPoints);
        Vector3[] secondleftPoints = getLeftLinePoints(leftPoints);

        for (int i = 0; i < topPoints.Length; i++)
        {
            Debug.DrawLine(topPoints[i], secondtopPoints[i], Color.white, 0.0f, true);
            Debug.DrawLine(bottomPoints[i], secondbottomPoints[i], Color.white, 0.0f, true);
            Debug.DrawLine(leftPoints[i], secondleftPoints[i], Color.white, 0.0f, true);
            Debug.DrawLine(rightPoints[i], secondrightPoints[i], Color.white, 0.0f, true);
        }

    }

    private Vector3[] getRightLinePoints(Vector3[] points)
    {
        Vector3[] secondPoints = (Vector3[])points.Clone();
        for(int i = 0; i < secondPoints.Length; i++)
        {
            secondPoints[i].x = secondPoints[i].x + getHorizontalCollisionDistance();
        }
        return secondPoints;
    }

    private Vector3[] getBottomLinePoints(Vector3[] points)
    {
        Vector3[] secondPoints = (Vector3[])points.Clone();
        for (int i = 0; i < secondPoints.Length; i++)
        {
            secondPoints[i].y = secondPoints[i].y - getVerticalCollisionDistance();
        }
        return secondPoints;
    }

    private Vector3[] getTopLinePoints(Vector3[] points)
    {
        Vector3[] secondPoints = (Vector3[])points.Clone();
        for (int i = 0; i < secondPoints.Length; i++)
        {
            secondPoints[i].y = secondPoints[i].y + getVerticalCollisionDistance();
        }
        return secondPoints;
    }

    private Vector3[] getLeftLinePoints(Vector3[] points)
    {
        Vector3[] secondPoints = (Vector3[])points.Clone();
        for (int i = 0; i < secondPoints.Length; i++)
        {
            secondPoints[i].x = secondPoints[i].x - getHorizontalCollisionDistance();
        }
        return secondPoints;
    }

    private void setZ(Vector3[] points, float z)
    {
        for(int i = 0; i < points.Length; i++)
        {
            points[i].z = z;
        }
    }

    private Vector3[] getTopPrimaryPoints()
    {
        Vector3 pos = this.transform.position;

        Vector3 rayTop = new Vector3(pos.x, Top(), Camera.main.transform.position.z);
        Vector3 rayTopLeft = new Vector3(Left() + 0.2f, Top(), Camera.main.transform.position.z);
        Vector3 rayTopRight = new Vector3(Right() - 0.2f, Top(), Camera.main.transform.position.z);

        Vector3[] points = { rayTop, rayTopLeft, rayTopRight };
        return points;
    }

    private Vector3[] getTopSecondaryPoints()
    {
        Vector3 pos = this.transform.position;

        Vector3 rayTop2 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 rayTopLeft2 = new Vector3(Left() + 0.2f, pos.y, pos.z);
        Vector3 rayTopRight2 = new Vector3(Right() - 0.2f, pos.y, pos.z);

        Vector3[] points2 = { rayTop2, rayTopLeft2, rayTopRight2 };
        return points2;
    }

    private Vector3[] getLeftPrimaryPoints()
    {
        Vector3 pos = this.transform.position;

        Vector3 rayLeft = new Vector3(Left(), pos.y, Camera.main.transform.position.z);
        Vector3 rayLeftUp = new Vector3(Left(), Top() - 0.2f, Camera.main.transform.position.z);
        Vector3 rayLeftDown = new Vector3(Left(), Bottom() + 0.2f, Camera.main.transform.position.z);

        Vector3[] points = { rayLeft, rayLeftUp, rayLeftDown };
        return points;
    }

    private Vector3[] getLeftSecondaryPoints()
    {
        Vector3 pos = this.transform.position;
        Vector3 rayLeft2 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 rayLeftUp2 = new Vector3(pos.x, Top() - 0.2f, pos.z);
        Vector3 rayLeftDown2 = new Vector3(pos.x, Bottom() + 0.2f, pos.z);

        Vector3[] points2 = { rayLeft2, rayLeftUp2, rayLeftDown2 };
        return points2;
    }

    private Vector3[] getRightPrimaryPoints()
    {
        Vector3 pos = this.transform.position;

        Vector3 rayRight = new Vector3(Right(), pos.y, Camera.main.transform.position.z);
        Vector3 rayRightUp = new Vector3(Right(), Top() - 0.2f, Camera.main.transform.position.z);
        Vector3 rayRightDown = new Vector3(Right(), Bottom() + 0.2f, Camera.main.transform.position.z);

        Vector3[] points = { rayRight, rayRightUp, rayRightDown };
        return points;
    }

    private Vector3[] getRightSecondaryPoints()
    {
        Vector3 pos = this.transform.position;
        //second set of points
        Vector3 rayRight2 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 rayRightUp2 = new Vector3(pos.x, Top() - 0.2f, pos.z);
        Vector3 rayRightDown2 = new Vector3(pos.x, Bottom() + 0.2f, pos.z);

        Vector3[] points2 = { rayRight2, rayRightUp2, rayRightDown2 };
        return points2;
    }

    private Vector3[] getBottomPrimaryPoints()
    {
        //avatar position
        Vector3 pos = this.transform.position;

        //first set of points
        Vector3 rayBottom = new Vector3(pos.x, Bottom(), Camera.main.transform.position.z);
        Vector3 rayBottomLeft = new Vector3(Left() + 0.2f, Bottom(), Camera.main.transform.position.z);
        Vector3 rayBottomRight = new Vector3(Right() - 0.2f, Bottom(), Camera.main.transform.position.z);

        Vector3[] points = { rayBottom, rayBottomLeft, rayBottomRight };
        return points;
    }

    private Vector3[] getBottomSecondaryPoints()
    {
        Vector3 pos = this.transform.position;
        //second set of points
        Vector3 rayBottom2 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 rayBottomLeft2 = new Vector3(Left() + 0.2f, pos.y, pos.z);
        Vector3 rayBottomRight2 = new Vector3(Right() - 0.2f, pos.y, pos.z);

        Vector3[] points2 = { rayBottom2, rayBottomLeft2, rayBottomRight2 };
        return points2;
    }

    private float getVerticalCollisionDistance()
    {
        //float distance = startBounds.size.y / 2 + yMargin;
		float distance = spriteRenderer.bounds.size.y / 2 + yMargin;
        return distance;
    }

    private float getHorizontalCollisionDistance()
    {
        float distance = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        return distance;
    }

    // Update is called once per frame
    void Update () {
        drawPrimaryPoints();
        drawSecondaryPoints();
	}

	public bool isPlayerHidden(){

		RaycastHit hit;
		bool hidden;
		Vector3 pos = transform.position;
		Vector3 origin = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
		bool collide = Physics.Raycast(origin, Camera.main.transform.forward, out hit);

		if(collide){
			if(hit.collider.gameObject == gameObject){
				return false;
			}
		}
		return true;
	}
}
