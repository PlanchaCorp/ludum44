using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DummyEvent : IEvent
{
    public void Accept(PlayerHealth player)
    {
        Debug.Log("accept");
    }

    public void Decline(PlayerHealth player)
    {
       Debug.Log("decline");
    }

    public bool IsBig()
    {
        return true;
    }

    public string Prompt()
    {
        return "Tes rideau sont moches";
    }
}