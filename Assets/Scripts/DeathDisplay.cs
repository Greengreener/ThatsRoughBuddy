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
    void Start()
    {
        HUDPanel.SetActive(false);//set HUD Panel inactive
        //set the text elements to display the players score
        gems.text = "Gems: " + PlayerHandler.gems.ToString();
        distance.text = "Distance: " + PlayerHandler.distance.ToString();
    }
}
