using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float minGroundNormalY = 0.65f;

    /*modifier that enables you to change how each hard object is pulled down
     * by gravity*/
    public float gravityModifier = 1f;
    public float minGoundNormalyAxis = 0.65f;
    
    protected Vector2 targetVelocity;
    protected Vector2 velocity;

    //the vector of the normal force that the ground pushes the object by
    protected Vector2 groundNormal;

    //the rigid body of this object
    protected Rigidbody2D rb2d;

    //the minimum distance threshold ofr moving
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    //array to store info the objects detected by the ray cast
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    //a filter used when detecting contact objects before the object actually moves
    protected ContactFilter2D contactFilter;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    // a flag that tells if the object is on the ground or not
    protected bool grounded;

    /*Method that is called every time the script is activated. */
    void OnEnable()
    {
        /*Get the rigid body component of this object.*/
        rb2d = GetComponent<Rigidbody2D>();
    }

    /*Set the contact filter to use triggers colliders to detect contacts.*/
    void Start()
    {
        contactFilter.useTriggers = true;
    }

    //empty method so it can be overriden
    protected virtual void ComputeVelocity()
    {

    }
    
    //reset the target velocity between each frame
    void Update()
    {
        targetVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        /*Add to the velocity vector the vector representing the pull of gravity
         * in time since the last frame. */
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

        //set the horizontal component of the objects velocity to the target velocity
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        /*the vector representing how much the object should move by in the
         * next frame*/
        Vector2 deltaPosition = velocity * Time.deltaTime;

        //how much the vector should move by horizontally
        Vector2 move = moveAlongGround * deltaPosition.x;

        //move the object horizontally
        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        //the length of the vector
        float distance = move.magnitude;

        //only move if the distance is not neglijible
        if (distance>minMoveDistance)
        {
            /*Count how many objects would collide with this object if moved forward just once.*/
            int count= rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            //clear the buffer list and add the objects detected
            hitBufferList.Clear();
            for (int i=0; i<count;i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i=0; i<hitBufferList.Count;i++)
            {
                //get the normal vector of the point on the object hit by the ray
                Vector2 currentNormal = hitBufferList[i].normal;

                //if the vertical component is larger than the threshhold, ground the object
                if (currentNormal.y>minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        
        }

        rb2d.position = rb2d.position + move.normalized*distance;
    }
}
