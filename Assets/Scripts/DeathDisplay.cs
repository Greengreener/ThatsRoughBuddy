using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour
{
    GameObject HUDPanel;//the players HUD display panel, set inactive on start
    Text gems, distance;//text elements to display the distance travelled and the number of gems collected
    PlayerHandler player;//a reference to the player handler to get the number of gems collected and distance travelled
    void Start()
    {
        HUDPanel.SetActive(false);//set HUD Panel inactive
        //set the text elements to display the players score
    }
}
