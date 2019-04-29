using UnityEngine;
using System.Collections;

public interface IEvent 
{
    bool IsBig();

    string Prompt();

    void Accept(PlayerHealth player);
    void Decline(PlayerHealth player);
}
