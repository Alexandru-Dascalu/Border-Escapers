using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : PhysicsObject
{
    public float walkWidth = 120.0f;
    

    //variables for the movement and turning
    private Rigidbody2D enemy;
    public float speed;
    public float maxSpeed;
    Vector2 move = new Vector2(1, 0);
    private float walk;
    bool facingLeft;
    



    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        walk = walkWidth;
        facingLeft = true;
    }

    void Update()
    {

        enemy.position += move * speed;

        enemy.velocity = (enemy.velocity.x > maxSpeed) ? new Vector2(maxSpeed, enemy.velocity.y) : enemy.velocity;
        enemy.velocity = (enemy.velocity.x < -maxSpeed) ? new Vector2(-maxSpeed, enemy.velocity.y) : enemy.velocity;
        walk--;
        if (walk == 0)
        {
             move.x *= -1;
             walk = walkWidth;
             if (facingLeft)
              {
                 transform.localRotation = Quaternion.Euler(0, 180, 0);
                 facingLeft = false;
              }
              else
              {
                 transform.localRotation = Quaternion.Euler(0, 0, 0);
                 facingLeft = true;
              }
        }
        
    }

    

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
        {
            move.x *= -1;
            walk = walkWidth;
        }
       
    }
}
