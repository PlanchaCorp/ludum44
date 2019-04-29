using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillGenerator
{
    private const float AMPLITUDE_DENOMINATEUR = 8;
    private const float FREQUENCY = 39;
    private readonly List<int> LOCATIONS = new List<int>(){ -2, -1, 0, 1, 2 };
    private const int MAX_VALUE = 4;
    private const int MIN_VALUE = -4;

    private int nonUsedLevelPoints;
    private Sprite[] textures;

    List<PillData> alreadyCreatedPills;

    public PillGenerator(Sprite[] pillTextures)
    {
        textures = pillTextures;
        alreadyCreatedPills = new List<PillData>();
    }

    static void RandomizeArray(List<float> array)
    {
        for (int i = array.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i);
            float tmp = array[i];
            array[i] = array[r];
            array[r] = tmp;
        }
    }

    static void InvertArray(List<float> array)
    {
        for (int i = array.Count - 1; i > 0; i--)
        {
            array[i] = -array[i];
        }
    }

    public List<PillData> GenerateTwoPills(int playerLevel)
    {
        List<PillData> pills = new List<PillData>();
        for (int i = 0; i < 2; i++)
        {
            pills.Add(GeneratePill(playerLevel));
        }
        return pills;
    }

    public PillData GeneratePill(int playerLevel)
    {
        PillData pill;
        do
        {
            pill = new PillData();
            pill.level = playerLevel;
            int effectLevel = UnityEngine.Random.Range(0, (int)((float)pill.level));
            pill = PillStatDecorator(pill, pill.level - effectLevel);
            pill = PillEffectDecorator(pill, effectLevel);
        } while (Mathf.Abs(pill.brain) + Mathf.Abs(pill.heart)  + Mathf.Abs(pill.pulmon)  + Mathf.Abs(pill.muscles)  + Mathf.Abs(pill.intestine) == 0);
        pill.cost = playerLevel * UnityEngine.Random.Range(100, 200);
        pill = PillImageDecorator(pill);
        pill = PillNameDecorator(pill);
        pill = PillDescriptionDecorator(pill);
        return pill;
    }

    private PillData PillEffectDecorator(PillData pill, int effectLevel)
    {
        List<string> possibleEffectTypes = new List<string>();
        foreach(KeyValuePair<string, int> organValue in new Dictionary<string, int>() {
            { "brain", pill.brain },
            { "heart", pill.heart },
            { "lungs", pill.pulmon },
            { "stomac", pill.intestine },
            { "muscles",  pill.muscles }
        })
        {
            for(int i = 0; i < Mathf.Abs(organValue.Value); i++)
            {
                possibleEffectTypes.Add(organValue.Key);
            }
        }
        string possibleOrgan = (possibleEffectTypes.Count > 0) ? possibleEffectTypes[UnityEngine.Random.Range(0, possibleEffectTypes.Count - 1)] : "";
        List<EffectData> possibleEffects = GameContext.effects.Effects.FindAll(e => e.type == possibleOrgan);
        EffectData randomEffect = (possibleEffects.Count > 0) ? possibleEffects[UnityEngine.Random.Range(0, possibleEffects.Count - 1)] : null;
        if (randomEffect != null && randomEffect.baseValue <= Mathf.Abs(effectLevel))
        {
            if (randomEffect.permanent)
            {
                nonUsedLevelPoints = effectLevel - Mathf.Abs(randomEffect.baseValue);
            } else
            {
                int divider = effectLevel != 0 ? effectLevel : 1;
                randomEffect.duration = Mathf.Abs(randomEffect.baseValue) / divider;
                nonUsedLevelPoints = Mathf.Abs(randomEffect.baseValue) % divider;
            }
        }
        pill.secondaryEffect = new List<EffectData>();
        if (randomEffect != null)
        {
            pill.secondaryEffectID = new string[1];
            pill.secondaryEffectID[0] = randomEffect.name;
            pill.secondaryEffect.Add(randomEffect);
        } else
        {
            pill.secondaryEffectID = new string[0];
        }
        return pill;
    }

    private PillData PillImageDecorator(PillData pill)
    {

        pill.image = UnityEngine.Random.Range(0, textures.Length - 1);
        return pill;
    }

    private PillData PillStatDecorator(PillData pill, int statLevel)
    {
        float amplitude = (float)statLevel / AMPLITUDE_DENOMINATEUR;
        List<float> values = new List<float>();
        LOCATIONS.ForEach(l => values.Add(Mathf.Cos(l * FREQUENCY) * amplitude));
        for (int i = 0; i < values.Count; i++)
        {
            values[i] = values[i] + UnityEngine.Random.Range(-amplitude, amplitude);
            values[i] = Mathf.Round(values[i]);
            if (values[i] > MAX_VALUE)
            {
                values[i] = MAX_VALUE;
            }
            else if (values[i] < MIN_VALUE)
            {
                values[i] = MIN_VALUE;
            }
        }
        RandomizeArray(values);

        int valuesSum = 0;
        values.ForEach(v => valuesSum += (int)v);
        while (valuesSum > amplitude)
        {
            valuesSum -= (int)values[0];
            values[0] = values[0] > 0 ? -values[0] : values[0];
            valuesSum += (int)values[0];
            RandomizeArray(values);
        }
        if (valuesSum > 3)
        {
            InvertArray(values);
        }

        pill.brain = (int)values[0];
        pill.heart = (int)values[1];
        pill.pulmon = (int)values[2];
        pill.intestine = (int)values[3];
        pill.muscles = (int)values[4];
        return pill;
    }

    private PillData PillNameDecorator(PillData pill)
    {
        string displayName = "", name = "";
        PillNameParts pillNames = GameContext.pillsNameParts;
        do
        {
            string prefix = pillNames.pillPrefixes[UnityEngine.Random.Range(0, pillNames.pillPrefixes.Count - 1)];
            string suffix = pillNames.pillSuffixs[UnityEngine.Random.Range(0, pillNames.pillSuffixs.Count - 1)];
            displayName = prefix + suffix;
            name = displayName.Replace(" ", "");
        } while (alreadyCreatedPills.Exists(p => p.name == name));
        pill.displayName = displayName;
        pill.name = name;
        return pill;
    }

    private PillData PillDescriptionDecorator(PillData pill)
    {
        List<string> possibleDescriptionTypes = new List<string>();
        foreach (KeyValuePair<string, int> organValue in new Dictionary<string, int>() {
            { "brain", pill.brain },
            { "heart", pill.heart },
            { "lungs", pill.pulmon },
            { "stomac", pill.intestine },
            { "muscles",  pill.muscles }
        })
        {
            for (int i = 0; i < Mathf.Abs(organValue.Value); i++)
            {
                possibleDescriptionTypes.Add(organValue.Key + ((organValue.Value > 0) ? "+" : "-"));
            }
        }
        string possibleOrgan = (possibleDescriptionTypes.Count > 0) ? possibleDescriptionTypes[UnityEngine.Random.Range(0, possibleDescriptionTypes.Count - 1)] : "";
        List<DescriptionData> possibleDescriptions = new List<DescriptionData>();
        Debug.Log("possibleType : " + possibleOrgan);
        switch (possibleOrgan)
        {
            case "brain+":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.brain > 0);
                break;
            case "heart+":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.heart > 0);
                break;
            case "lungs+":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.pulmon > 0);
                break;
            case "stomac+":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.intestine > 0);
                break;
            case "muscles+":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.muscles > 0);
                break;
            case "brain-":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.brain < 0);
                break;
            case "heart-":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.heart < 0);
                break;
            case "lungs-":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.pulmon < 0);
                break;
            case "stomac-":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.intestine < 0);
                break;
            case "muscles-":
                possibleDescriptions = GameContext.descriptions.FindAll(d => d.muscles < 0);
                break;
        }
        pill.description = (possibleDescriptions.Count > 0) ? possibleDescriptions[UnityEngine.Random.Range(0, possibleDescriptions.Count - 1)].description : "What could possibly go wrong ?";
        return pill;
    }
}
