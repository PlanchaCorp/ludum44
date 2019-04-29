using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganIconChanger : MonoBehaviour
{
    public void ChangeColor(int bonus)
    {
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        if (bonus < 0)
        {
            image.color = Color.red;
        } else if (bonus > 0)
        {
            image.color = Color.green;
        } else
        {
            image.color = Color.grey;
        }
    }
}
