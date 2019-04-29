using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PillData
{
    public string name;
    public string displayName;
    public string brand;
    public int cost;

    public int brain;
    public int heart;
    public int pulmon;
    public int muscles;
    public int intestine;

    public string description;
    public int level;
    public string stateName;
    public string type;
    public int baseValue;

    public BodyData bodyEffect;
    public int image;
    

    public List<string> requirements;
    public string[] secondaryEffectID;

    public List<EffectData> secondaryEffect;
    public bool alreadyTaken;


    public void Build()
    {
        alreadyTaken = false;
        bodyEffect = new BodyData(brain, heart, pulmon, muscles, intestine);
        secondaryEffect = new List<EffectData>();
        secondaryEffect = GameContext.effects.Effects.FindAll(s => Array.Exists(secondaryEffectID, r => r == s.name));
    }

    public override string ToString()
    {
        return "Pill " + name
            + ";display=" + displayName
            + ";description=" + description
            + "; image" + image
            + ";stats=(" + brain + "," + heart + "," + pulmon + "," + muscles + "," + intestine + ")";
    }

    public PillData()
    {

    }
    public PillData(int brain, int heart, int pulmon, int muscles, int intestines, int level)
    {
        this.brain = brain;
        this.heart = heart;
        this.pulmon = pulmon;
        this.muscles = muscles;
        this.intestine = intestines;
        this.level = level;
    }
}

