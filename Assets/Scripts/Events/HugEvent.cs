using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HugEvent : IEvent
{


    public void Accept(PlayerHealth player)
    {
        if (player.body.heart < player.body.heartMax)
        {
            player.body.heart += 1;
        }
        GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<GameController>().UpdateGUI();
    }

    public void Decline(PlayerHealth player)
    {
        return;
    }

    public bool IsBig()
    {
        return true;
    }

    public string Prompt()
    {
        return "You sound very friendly. And I think we could both use a little warmness. Soooo ... Do you want a hug ?\nIt will warm up your heart ! <3";
    }
}