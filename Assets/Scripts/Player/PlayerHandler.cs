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
    public bool dodging = false, returning = false;
    public Transform returnPoint;
    public List<GameObject> dodgePoints = new List<GameObject>();
    public float glitchDistance = 0.2f, dodgeDistance = 5f, movementSpeed = 5f;
    int dodgeDirection;
    [Header("Jumping and Gravity")]
    public float jumpSpeed = 50.0f, gravity = -10.0f;
    public bool jumping = false;
    Vector3 velocity;
    [Header("Sliding")]
    float slideTimeStamp, slideTime = 1.5f;
    bool sliding;
    [Header("Gem Collection")]
    public Text gemsDisplay;
    int gems;
    float gemArmourValue = 0.25f;
    [Header("Armour")]
    public bool armour;
    float armourPercent;
    public Slider armourCollectionPercentDisplay;
    [Header("Power Ups")]
    public bool invincible;
    public float invincibleTimeStamp;
    public float timeLimit = 20f;
    [Header("Test")]
    MeshRenderer charMesh;
    [Header("Annimation")]
    public Animator playerAnimator;
    public bool isGrounded;
    public LayerMask groundLayerMask;
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
            this.gameObject.SetActive(false);
        }
    }
    void CollisionWithDeathObject(Transform death)
    {
        if (invincible)
        {
            Destroy(death.gameObject);
        }
        else if (armour)
        {
            armour = false;
            armourPercent = 0;
            armourCollectionPercentDisplay.value = armourPercent;
            Destroy(death.gameObject);
        }
        else
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Death":
                CollisionWithDeathObject(other.transform);
                break;
            case "Gem":
                GemCollection(other.transform);
                break;
            case "Power Up":
                Invincibility();
                Destroy(other.gameObject);
                break;
            //Added by oscar
            case "Hazard":
                HazardInteraction(other.gameObject);
                break;
        }
    }
    void Start()
    {
        charControl = gameObject.GetComponent<CharacterController>();
        charMesh = gameObject.GetComponent<MeshRenderer>();
        transform.position = returnPoint.position;
        playerAnimator = gameObject.GetComponent<Animator>();
    }
    #endregion
    void Update()
    {
        Debug.Log(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"));
        if (charControl.isGrounded && !sliding/*playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running")*/)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                //Dodge(-1);
                playerAnimator.Play("Dodge Left");
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                playerAnimator.Play("Dodge Right");
                //Dodge(1);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                playerAnimator.Play("Slide");
                //Slide();
            }
            if (Input.GetButtonDown("Jump"))
            {
                playerAnimator.Play("Jump");
                Debug.Log("jump");
                //velocity.y = jumpSpeed;
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
        if (charControl.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
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
    }
}