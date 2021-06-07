using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    [SerializeField]
    private Tank player;

    [SerializeField]
    private Image partImage;
    
    [SerializeField]
    private Text durability;


    private void Update()
    {
        if (player.AdditionalPart != null)
            partImage.sprite = player.AdditionalPart.GetSprite(Team.Player);
        else
            partImage.sprite = null;

        SetDurability();
    }

    private void SetDurability()
    {
        durability.text = "Dur: " + player.Durability;
    }
}
