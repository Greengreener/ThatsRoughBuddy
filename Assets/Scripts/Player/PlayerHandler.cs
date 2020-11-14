using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerHandler : MonoBehaviour
{
    #region Variables
    [Header("Dodging")]
    CharacterController charControl;
    bool dodging = false, returning = false;//used in update to determine if the player is dodging
    [SerializeField]
    Transform returnPoint;//the centre point for the player, used to return them to the centre after dodging
    [SerializeField]
    float glitchDistance = 0.5f/*used in Update to check if the player is at the returnPoint*/, dodgeDistance = 3f/*the distance from the return point that the dodge ends*/, movementSpeed = 5f/*the speed the player moves while doding*/;
    int dodgeDirection;//used in Dodge() to determine the direction the player is dodging
    [Header("Jumping and Gravity")]
    [SerializeField]
    float jumpSpeed = 8.0f, gravity = 10.0f;
    Vector3 velocity;
    [Header("Sliding")]
    float slideTimeStamp, slideTime = 1.5f;
    bool sliding;
    [SerializeField]
    [Header("Gem Collection")]
    Text gemsDisplay;
    public static int gems;
    float gemArmourValue = 0.25f;
    [Header("Armour")]
    bool armour;
    float armourPercent;
    [SerializeField]
    Slider armourCollectionPercentDisplay;
    [Header("Power Ups")]
    bool invincible;
    float invincibleTimeStamp;
    float timeLimit = 20f;
    [Header("Test")]
    MeshRenderer charMesh;
    [Header("Annimation")]
    Animator playerAnimator;
    bool isGrounded;
    [SerializeField]
    LayerMask groundLayerMask;
    [Header("Distance")]
    float speed = 5f;
    public static float distance;
    [SerializeField]
    [Header("Death")]
    public GameObject deathDisplayPanel;
    [Header("Pause")]
    [SerializeField]
    GameObject pauseMenuPanel;
    public static bool paused;
    #endregion
    #region Functions
    void Dodge(int direction)
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
    void Invincibility()
    {
        invincible = true;
        invincibleTimeStamp = Time.time;
    }
    void GemCollection(Transform gem)
    {
        gems++;
        gemsDisplay.text = "Gems: " + gems.ToString();
        if (!armour)
        {
            armourPercent += gemArmourValue;
            if (armourPercent >= 1)
            {
                armour = true;
            }
            armourCollectionPercentDisplay.value = armourPercent;
        }
        Destroy(gem.gameObject);
    }
    public void PauseToggle()//To be used by resume button in pause menu and in Update()
    {
        if (paused)
        {
            Time.timeScale = 1;//resume game time
            pauseMenuPanel.SetActive(false);//set the pause menu inactive
            paused = false;//set paused false
        }
        else
        {
            Time.timeScale = 0;//stop game time
            pauseMenuPanel.SetActive(true);//set the pause menu active
            paused = true;//set paused true
        }
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
            deathDisplayPanel.SetActive(true);
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
            case "Hazard":                              //
                HazardInteraction(other.gameObject);    //
                break;                                  //
            //Added by Oscar                            /\
        }
    }
    void Start()
    {
        //Set reference variables on the player object
        charControl = gameObject.GetComponent<CharacterController>();
        charMesh = gameObject.GetComponent<MeshRenderer>();
        playerAnimator = gameObject.GetComponent<Animator>();
        //Set reference variables on other objects by tag
        //returnPoint = GameObject.FindGameObjectWithTag("ReturnPoint").transform;
        //gemsDisplay = GameObject.FindGameObjectWithTag("GemDisplay").GetComponent<Text>();
        //armourCollectionPercentDisplay = GameObject.FindGameObjectWithTag("ArmourDisplay").GetComponent<Slider>();
        //Set players position to middle position
        transform.position = returnPoint.position;
        //Reset display variables to 0 for play session
        distance = 0;
        gems = 0;
    }
    #endregion
    void Update()
    {
        if (/*charControl.isGrounded ||*/ Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayerMask) && !sliding && !dodging && !returning)
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
            if (Input.GetButtonDown("Jump") || Input.GetAxis("Vertical") > 0)
            {
                velocity.y += jumpSpeed;
            }
        }
        if(Input.GetButtonDown("Cancel"))
        {
            PauseToggle();
        }
        if (sliding && Time.time - slideTimeStamp > slideTime)
        {
            playerAnimator.StopPlayback();
            sliding = false;
            playerAnimator.enabled = false;
        }
        if (invincible)
        {
            if (Time.time - invincibleTimeStamp > timeLimit)
            {
                invincible = false;
            }
        }
        if (charControl.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        if (dodging)
        {
            charControl.Move(Vector3.right * dodgeDirection * movementSpeed * Time.deltaTime);
            if (Vector3.Distance(returnPoint.position, transform.position) > dodgeDistance)
            {
                dodging = false;
                returning = true;
            }
        }
        else if (returning)
        {
            charControl.Move(Vector3.right * dodgeDirection * -1 * movementSpeed * Time.deltaTime);
            if (Vector3.Distance(returnPoint.position, transform.position) < glitchDistance)
            {
                returning = false;
                transform.position = returnPoint.position;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
            charControl.Move(velocity * Time.deltaTime);
        }
        distance += speed * Time.deltaTime;
    }
}