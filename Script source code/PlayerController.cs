using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpInitialSpeed = 5;
    private int health;
    private int maxHealth;
    public Text healthText;
    private int jumpNr;
    public Text jumpText;
    public Text wireCutterText;
    private int wireCutterNr;
    public Text deathText;
    public Text arrestText;
    bool arrested;
    public Text winText;
    public Text tutorialText;
    bool displayTutorial;
    

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        healthText.text = "Health: " + health.ToString();
        jumpNr = 5;
        jumpText.text = "Jumps :" + jumpNr.ToString();
        wireCutterNr = 0;
        wireCutterText.text = "Wire Cutters: " + wireCutterNr.ToString();
        deathText.text = "";
        arrested = false;
        arrestText.text = "";
        winText.text = "";
        tutorialText.text = "You are a person living in a dictatorship. You have to avoid the police and the barbed wire\nto escape to freedom. Because you are malnourished, you can only jump a limited number of times.\n" +
            "There are pickups to increase your number of jumps. Barbed wire takes 80% of your health,\nunless you have a wire cutter, in which case the barbed wire will dissapear." +
            "\nThere are health and wire cutter pickups around the level. You have to avoid the police,\nif they touch you, you are arrested and it's game over. You win if you arrive at the end of the level.\nPress Escape to make this dissapear.";
        displayTutorial = true;
    }


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKeyDown("escape"))
        {
            displayTutorial = false;
        }

        if(!displayTutorial)
        {
            tutorialText.text = "";
        }

        if (arrested)
        {
            arrestText.text = "You have been arrested!";
        }
        else if (health > 0)
        {
            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded && (jumpNr > 0))
            {
                velocity.y = jumpInitialSpeed;
                jumpNr--;
                jumpText.text = "Jumps :" + jumpNr.ToString();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * .5f;
                }
            }

            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));

            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            targetVelocity = move * maxSpeed;
        }
        else
        {
            deathText.text = "You are dead!\nGame Over!";
        }




    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Health Pickup"))
        {
            if (health < maxHealth)
            {
                other.gameObject.SetActive(false);
                health += 10;
                if(health>maxHealth)
                {
                    health = maxHealth;
                }
                healthText.text = "Health: " + health.ToString();
            }
        }
        else if (other.gameObject.CompareTag ("Jump Pickup"))
        {
            other.gameObject.SetActive(false);
            jumpNr++;
            jumpText.text = "Jumps :" + jumpNr.ToString();
        }
        else if (other.gameObject.CompareTag("Jump Pickup 2"))
        {
            other.gameObject.SetActive(false);
            jumpNr+=2;
            jumpText.text = "Jumps :" + jumpNr.ToString();
        }
        else if (other.gameObject.CompareTag ("Wire Cutter Pickup"))
        {
            other.gameObject.SetActive(false);
            wireCutterNr++;
            wireCutterText.text = "Wire Cutters: " + wireCutterNr.ToString();
        }
        else if(other.gameObject.CompareTag("Barbed Wire")&& wireCutterNr==0)
        {
            health -= 80;
            healthText.text = "Health: " + health.ToString();
            
        }
        else if (other.gameObject.CompareTag("Barbed Wire") && wireCutterNr>0)
        {
            
            wireCutterNr--;
            wireCutterText.text = "Wire Cutters: " + wireCutterNr.ToString();
            other.gameObject.SetActive(false);
            
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            arrested = true;
        }
        else if (other.gameObject.CompareTag("Winner"))
        {
            winText.text = "You won!";
        }
    }
}


    