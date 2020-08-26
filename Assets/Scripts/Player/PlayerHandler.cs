using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerHandler : MonoBehaviour
{
    #region Old Code
    //public List<GameObject> dodgePoints = new List<GameObject>();//the points the player moves to when dodging
    //public bool jumping = false;
    //public bool isGrounded;
    /*distanceDisplaythe text used to display the distance travelled by the player*/
    #endregion
    #region Variables
    [Header("Dodging")]
    CharacterController charControl;//the player character controller
    public bool dodging = false, returning = false;//used to move the player to the dodge postitions and to check if they are dodging for physics
    public Transform playerOrigin;//the players neutral position
    public float glitchDistance = 0.2f/*the distance used to check if the player has reached the return point*/, dodgeDistance = 5f/*the distance from the origin the player will dodge*/, movementSpeed = 5f/*the speed the player will move while dodging*/;
    int dodgeDirection;//used in Dodge() and Update() to determine which direction the player is dodging
    [Header("Jumping and Gravity")]
    public float jumpSpeed = 50.0f/*the speed the player jumps at, used in Update()*/, gravity = -10.0f;
    Vector3 velocity;//the players movement vector used in Update() for jumping and gravity
    [Header("Sliding")]
    float slideTimeStamp, slideTime = 1.5f;
    bool sliding;
    [Header("Gem Collection")]
    public int gems;//number of gems collected by player
    float gemArmourValue = 0.25f;//the percent that each gem will fill the armour meter to reharge their armour
    [Header("Armour")]
    public bool armour;//determines if the player has armour
    float armourPercent;//the percentage the armour has been recharged by gems being collected
    public Slider armourCollectionPercentDisplay;//the slider used to display the armour percentage
    [Header("Invincibility")]
    public bool invincible;//determines if the player is invincible
    public float invincibleTimeStamp;//the time stamp when the player last collected the power up, set in Invincibility() and checked in Update()
    public float timeLimit = 20f;//the time that the players power up will stay active, checked against invincibleTimeStamp in Update()
    [Header("Annimation")]
    public Animator playerAnimator;//the players animator
    public LayerMask groundLayerMask;
    [Header("Score/Display")]
    public Text gemsDisplay/*the text used to display the number of gems collected by the player*/;
    public float distance;//the distance travelled by the player
    public GameObject deathScreen;//UI panel that is actived when the player dies to display their score
    #endregion
    #region Functions
    void Dodge(int direction)//used to set dodging true, which is checked in Update()
    {
        dodging = true;
        dodgeDirection = direction;
    }
    void Slide()
    {
        Debug.Log("slide");
        sliding = true;
        playerAnimator.enabled = true;
        playerAnimator.Play("Slide");
        slideTimeStamp = Time.time;
    }
    void Invincibility()//used to set invincibility true, which is checked in Update(), also sets invincibleTimeStamp
    {
        invincible = true;
        invincibleTimeStamp = Time.time;
    }
    void Death()
    {
        deathScreen.SetActive(true);
    }
    void GemCollection(Transform gem)
    {
        gems++;//increases number of gems collected
        gemsDisplay.text = "Gems: " + gems.ToString();//sets the UI to display the new number of gems
        if (!armour)//if the player does not have armour
        {
            armourPercent += gemArmourValue;//increase the armourPercent by gemArmourValue
            if (armourPercent >= 1)
            {
                armour = true;
            }
            armourCollectionPercentDisplay.value = armourPercent;
        }
        Destroy(gem.gameObject);
    }
    //Added by Oscar \/
    void HazardInteraction(GameObject interacted)
    {
        
        if(invincible)
        {
            interacted.SetActive(false);
        }
        else if (armour)
        {
            armour = false;
            armourPercent = 0;
            armourCollectionPercentDisplay.value = armourPercent;
            interacted.SetActive(false);
        }
        else 
        {
            Death();
            this.gameObject.SetActive(false);
        }
    }
    //Added by Oscar /\
    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Gem":
                GemCollection(other.transform);
                break;
            case "Power Up":
                Invincibility();
                Destroy(other.gameObject);
                break;
            //Added by Oscar                            \/
            case "Hazard":                              
                HazardInteraction(other.gameObject);    
                break;                                
            //Added by Oscar                            /\
        }
    }
    void Start()
    {
        charControl = gameObject.GetComponent<CharacterController>();
        transform.position = playerOrigin.position;
        playerAnimator = gameObject.GetComponent<Animator>();
    }
    #endregion
    void Update()
    {
        Debug.Log(charControl.isGrounded);
        if (charControl.isGrounded/* && !sliding*/)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                Dodge(-1);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                Dodge(1);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                Slide();
            }
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("jump");
                velocity.y += jumpSpeed;
            }
        }
        /*if (sliding && Time.time - slideTimeStamp > slideTime)
        {
            playerAnimator.StopPlayback();
            sliding = false;
            playerAnimator.enabled = false;
        }*/
        if (invincible)
        {
            if (Time.time - invincibleTimeStamp > timeLimit)
            {
                invincible = false;
            }
        }
        if (dodging)
        {
            charControl.Move(Vector3.right * dodgeDirection * movementSpeed * Time.deltaTime);
            if (Vector3.Distance(playerOrigin.position, transform.position) > dodgeDistance)
            {
                dodging = false;
                returning = true;
            }
        }
        else if (charControl.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        else if (returning)
        {
            charControl.Move(Vector3.right * dodgeDirection * -1 * movementSpeed * Time.deltaTime);
            if (Vector3.Distance(playerOrigin.position, transform.position) < glitchDistance)
            {
                returning = false;
                transform.position = playerOrigin.position;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
            charControl.Move(velocity * Time.deltaTime);
        }
        distance += movementSpeed * Time.deltaTime;
    }
}