using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class BodyData
{
    private readonly int BRAIN_BASE = 5;
    private readonly int HEART_BASE = 3;
    private readonly int PULMON_BASE = 5;
    private readonly int MUSCLE_BASE = 5;
    private readonly int INTESTINE_BASE = 5;

    public int brain;
    public int heart;
    public int pulmon;
    public int muscles;
    public int intestine;
    public int brainMax;
    public int heartMax;
    public int pulmonMax;
    public int musclesMax;
    public int intestineMax;

    public BodyData()
    {
        brain = BRAIN_BASE;
        heart = HEART_BASE;
        pulmon = PULMON_BASE;
        muscles = MUSCLE_BASE;
        intestine = INTESTINE_BASE;
        brainMax = BRAIN_BASE;
        heartMax = HEART_BASE;
        pulmonMax = PULMON_BASE;
        musclesMax = MUSCLE_BASE;
        intestineMax = INTESTINE_BASE;
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                brain = BRAIN_BASE / 2;
                break;
            case 1:
                pulmon = PULMON_BASE / 2;
                break;
            case 2:
                muscles = MUSCLE_BASE / 2;
                break;
            case 3:
                intestine = INTESTINE_BASE / 2;
                break;
        }
    }

    public BodyData(int brain, int heart, int pulmon, int muscles, int intestine)
    {
        this.brain = brain;
        this.heart = heart;
        this.pulmon = pulmon;
        this.muscles = muscles;
        this.intestine = intestine;
    }

    public void ApplyPillEffect(BodyData pillEffectData, ref List<CurrentEffect> effects, GameObject effectListObject)
    {
        brain = ApplyIndividualEffect(brain, pillEffectData.brain, brainMax, ref effects, effectListObject, "brainPrevention", "brainProtection", "brainDefficiency");
        intestine = ApplyIndividualEffect(intestine, pillEffectData.intestine, intestineMax, ref effects, effectListObject, "stomacPrevention", "stomacProtection", "stomacDefficiency");
        heart = ApplyIndividualEffect(heart, pillEffectData.heart, heartMax, ref effects, effectListObject, "heartPrevention", "heartProtection", "heartDefficiency");
        muscles = ApplyIndividualEffect(muscles, pillEffectData.muscles, musclesMax, ref effects, effectListObject, "musclesPrevention", "muscleProtection", "muscleDefficiency");
        pulmon = ApplyIndividualEffect(pulmon, pillEffectData.pulmon, pulmonMax, ref effects, effectListObject, "lungsPrevention", "lungsProtection", "lungsDefficiency");
    }

    /// <summary>
    /// Return the new stat for a body stat after a pill
    /// </summary>
    /// <param name="organStat">Organ stat before the edit</param>
    /// <param name="pillEffectStat">Organ stat modifier from the pill</param>
    /// <param name="maxStat">Maximum stat the organ can reach</param>
    /// <returns>New stat</returns>
    private int ApplyIndividualEffect(int organStat, int pillEffectStat, int maxStat, ref List<CurrentEffect> effects, GameObject effectListObject, string preventionEffectName, string protectionEffectName, string defficiencyEffectName)
    {
        if (pillEffectStat < 0 && effects.Exists(e => e.effectData.name == protectionEffectName))
        {
            return organStat;
        }
        if (pillEffectStat > 0 && effects.Exists(e => e.effectData.name == defficiencyEffectName))
        {
            return organStat;
        }
        if (pillEffectStat < 0 && effects.Exists(e => e.effectData.name == preventionEffectName))
        {
            effectListObject.GetComponent<EffectGenerator>().RemoveEffect(preventionEffectName);
            effects.Remove(effects.Find(e => e.effectData.name == preventionEffectName));
            return organStat;
        }
        organStat += pillEffectStat;
        if (organStat < 0)
        {
            organStat = 0;
        }
        if (organStat > maxStat)
        {
            organStat = maxStat;
        }
        return organStat;
    }

    public List<EffectData> GetCriticalBodyPartEffect()
    {
      
        List<string> criticalBodyParts = GetBodyPartHealth().Where(k => k.Value == 0).Select(k => k.Key).ToList();
     
        return GameContext.criticalEffects
            .Where(k => criticalBodyParts.Contains(k.Key))
            .Select(k =>GameContext.criticalEffects
                .Where(ce => ce.Key == k.Key).First().Value).ToList();
    }

    public Dictionary<string, int> GetBodyPartHealth()
    {
        return new Dictionary<string, int>()
        {
            { "brain",brain },
            { "heart",heart },
            { "pulmon",pulmon },
            { "intestine",intestine },
            { "muscles",muscles }
        };
    }

   public void Reset(string bodyPart,int transplantationCount)
    {

        switch (bodyPart)
        {
            case "brain":
                brain = BRAIN_BASE + (5 * transplantationCount);
                brainMax = BRAIN_BASE + (5 * transplantationCount);
                break;
            case "heart":
                heart = HEART_BASE + (3 * transplantationCount);
                heartMax = HEART_BASE + (3 * transplantationCount);
                break;
            case "pulmon":
                pulmon = PULMON_BASE + (5 * transplantationCount);
                pulmonMax = PULMON_BASE + (5 * transplantationCount);
                break;
            case "intestine":
                intestine = INTESTINE_BASE + (5 * transplantationCount);
                intestineMax = INTESTINE_BASE + (5 * transplantationCount);
                break;
            case "muscles":
                muscles = MUSCLE_BASE + (5 * transplantationCount);
                musclesMax = MUSCLE_BASE + (5 * transplantationCount);
                break;

        }
    }

    public override string ToString()
    {
        return "BodyData : brain=" + brain + ",heart=" + heart + ",pulmon=" + pulmon + ",muscles=" + muscles + ",intestine=" + intestine;
    }
}