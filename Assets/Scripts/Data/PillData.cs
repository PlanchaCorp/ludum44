using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PillData
{
    public string name;
    public string DisplayName;

    public string description;

    public int brain;
    public int heart;
    public int pulmon;
    public int muscles;
    public int intestine;

    public string unlock;

    public string[] secondaryEffectID;

    public override string ToString()
    {
        return "name = " + name
            + "Display Name = " + DisplayName
            + "description = " + description
            + "secondaryEffect" + secondaryEffectID.ToString() ;
    }
}
