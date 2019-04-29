using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class EffectData
{
    public string name;
    public string displayName;
    public string description;
    public int duration;
    public bool permanent;
    public string stateName;
    public State state;
    public int baseValue;
    public string type;
    public enum State { bonus , malus, neutral, critical };


    public void Build()
    {
        System.Enum.TryParse<State>(stateName, out State state);
        this.state = state;
    }

    public override string ToString()
    {
        return name + displayName + " : " + description + ", " + type + ", " + baseValue;
    }

   
}
