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
        //set the text elements to display the players score
        gems.text = "Gems: " + PlayerHandler.gems.ToString();
        distanceDisplay = 0;
    }
    private void Update()
    {
        if(distanceDisplay < PlayerHandler.distance)
        {
            distanceDisplay += 0.5f;
            distance.text = "Distance: " + distanceDisplay.ToString();
        }
        if (gemDisplay < PlayerHandler.gems)
        {
            gemDisplay += 0.5f;
            distance.text = "Gems: " + gems.ToString();
        }
    }
}