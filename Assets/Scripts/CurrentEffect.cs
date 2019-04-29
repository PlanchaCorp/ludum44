using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CurrentEffect
{
    public EffectData effectData;
    public int duration;

    public CurrentEffect(EffectData effectData)
    {
        this.effectData = effectData;
        this.duration = effectData.duration;
    }

    public void Reset()
    {
        this.duration = effectData.duration;
    }

    public bool Decrease(int amount)
    {
        if (effectData.permanent)
        {
            return false;
        }
        duration -= amount;
        if (duration < 0)
        {
            return true;
        }
        return false;
    }
}
