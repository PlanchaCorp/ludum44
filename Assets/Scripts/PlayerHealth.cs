using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth
{
    private static bool blindness = false;

    public int transplantationCount;

    public int currency;

    public BodyData body;

    public List<CurrentEffect> currentEffects;
    private List<EffectData> lastCriticalEffects;

    public GameObject effectList;

    public PlayerHealth()
    {
        effectList = GameObject.FindGameObjectWithTag("EffectList");
        currency = 0;
        transplantationCount = 0;
        body = new BodyData();
        currentEffects = new List<CurrentEffect>();
        lastCriticalEffects = new List<EffectData>();
    }

    public static bool IsBlind()
    {
        return blindness;
    }


    public bool ApplyPillStat(BodyData bodyData, ref List<CurrentEffect> effects, GameObject effectListObject)
    {
        body.ApplyPillEffect(bodyData, ref effects, effectListObject);
        if (body.GetCriticalBodyPartEffect().Exists(e => lastCriticalEffects.Contains(e)))
        {
            GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<GameController>().TriggerGameOver();
            return false;
        }
        lastCriticalEffects = body.GetCriticalBodyPartEffect();
        AddEffects(lastCriticalEffects);
        return true;
    }

    /// <summary>
    /// Add a list of effects to the player symptoms
    /// </summary>
    /// <param name="newEffects">List of new effects</param>
    public void AddEffects(List<EffectData> newEffects)
    {
        foreach (EffectData effect in newEffects)
        {
            CurrentEffect effectData = currentEffects.Find(elem => elem.effectData.name == effect.name);

            if (effectData != null)
            {
                effectData.Reset();
            }
            else
            {
                currentEffects.Add(new CurrentEffect(effect));
                ToggleEffect(effect.name, true);
                effectList.GetComponent<EffectGenerator>().AddEffect(effect);
            }
        }
        // Apply misunderstood effect
        bool misunderstanding = currentEffects.Exists(e => e.effectData.name == "misunderstanding");
        foreach (Transform transformObject in GameObject.FindGameObjectWithTag("EffectPanel").transform.Find("EffectList").transform)
        {
            transformObject.gameObject.SetActive(!misunderstanding);
        }
        GameObject.FindGameObjectWithTag("EffectPanel").transform.Find("MisunderstoodEffectList").gameObject.SetActive(misunderstanding);
    }

    /// <summary>
    /// Each turn, decrease duration of effects
    /// </summary>
    /// <param name="amount">Amount of time to reduce (weeks)</param>
    public void DecreaseEffects(int amount)
    {
        currentEffects.ForEach(currentEffect =>
        {
            currentEffect.Decrease(amount);
            if (currentEffect.duration < 0 && !currentEffect.effectData.permanent)
            {
                effectList.GetComponent<EffectGenerator>().RemoveEffect(currentEffect.effectData.name);
                ToggleEffect(currentEffect.effectData.name, false);
            }
        });
        currentEffects.RemoveAll(s => currentEffects.FindAll(e => e.duration < 0 && !e.effectData.permanent).Contains(s));
    }

    private void ToggleEffect(string effect, bool active)
    {
        switch (effect)
        {
            case "blindness":
                blindness = active;
                break;
        }
    }

    internal string getCurrency()
    {
        if (currency < 1000)
        {
            return String.Format("{000}", currency);
        }
        if (currency < 10000)
        {
            return String.Format("{0:#K0}", currency / 100);
        }
        if (currency < 1000000)
        {
            return String.Format("{000:#k}", currency / 1000);
        }
        return currency.ToString();
    }
}
