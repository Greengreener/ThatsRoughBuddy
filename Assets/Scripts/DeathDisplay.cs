using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject HUDPanel;//the players HUD display panel, set inactive on start
    [SerializeField]
    Text gems, distance;//text elements to display the distance travelled and the number of gems collected
    float distanceDisplay, gemDisplay;//used to display the players distance, counting up from 0 after their death
    void Start()
    {
        HUDPanel.SetActive(false);//set HUD Panel inactive
        //set the display floats to 0
        distanceDisplay = 0;
        gemDisplay = 0;
    }
    private void Update()
    {
        if(distanceDisplay < PlayerHandler.distance)//if the display float is less than distance
        {
            distanceDisplay++;//increase display float 
            distance.text = "Distance: " + distanceDisplay.ToString();//set text UI to equal display float
        }
        if (gemDisplay < PlayerHandler.gems)//if the display float is less than gems
        {
            gemDisplay++;//increase display float 
            gems.text = "Gems: " + gemDisplay.ToString();//set text UI to equal display float
        }
    }
}