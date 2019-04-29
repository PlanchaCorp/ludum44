using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PillManager
{
    private readonly List<string> randomPillDescriptions;

    private GameObject leftPill;
    private GameObject middlePill;
    private GameObject rightPill;

    private List<PillData> pillPool;
    private List<PillData> possiblePillChoices;

    private PillGenerator generator;
    private PillImagesData pillImages;
    private PillImagesData fruitImages;
    public PillManager(PillImagesData pillImages, PillImagesData fruitImages)
    {
        pillPool = new List<PillData>();
        this.pillImages = pillImages;
        this.fruitImages = fruitImages;
        generator = new PillGenerator(pillImages.textures);

        GameObject[] pills = GameObject.FindGameObjectsWithTag("Pill");
        if (pills != null && pills.Length >= 3)
        {
            leftPill = pills[0].gameObject;
            middlePill = pills[1].gameObject;
            rightPill = pills[2].gameObject;
        }

        randomPillDescriptions = new List<string>()
        {
            "This one is of a pretty nice color.",
            "Don't let it fall down.",
            "Are you supposed to eat it ?",
            "Where did you buy it ?",
            "Sure. Why not.",
            "This should be ok, I guess."
        };
    }

    /// <summary>
    /// Consume a given pill and updates other pills requirements
    /// </summary>
    /// <param name="position">Position of the eaten pill</param>
    /// <returns>Pill consumed</returns>
    public PillData ConsumePill(int position)
    {
        PillData pill = possiblePillChoices[position];
        pill.alreadyTaken = true;
        Debug.Log("Taken pill " + pill.name);
        // Old unlock system
        /*for (int i = 0; i < lockedPills.Count; i++)
        {
            PillData lockedPill = lockedPills[i];
            if (lockedPill.requirements != null && lockedPill.requirements.Contains(pill.name))
            {
                lockedPill.requirements.Remove(pill.name);
                if (lockedPill.requirements.Count == 0)
                {
                    UnlockPill(lockedPill.name);
                }
            }
        }*/
        return pill;
    }

    /// <summary>
    /// Fetch three pills and put them into available pills pool
    /// </summary>
    public void FetchPills(int level)
    {
        List<PillData> newPills = generator.GenerateTwoPills(level);
        newPills.ForEach(p => p.Build());
        pillPool.AddRange(newPills);
        possiblePillChoices = new List<PillData>();
        if (pillPool.Count <= 3)
        {
            newPills = generator.GenerateTwoPills(level);
            newPills.ForEach(p => p.Build());
            pillPool.AddRange(newPills);
        }
        while (possiblePillChoices.Count < 3)
        {
            int randomPillIndex = UnityEngine.Random.Range(0, pillPool.Count - 1);
            PillData randomPill = pillPool[randomPillIndex];
            if (!string.IsNullOrEmpty(randomPill.name) && !possiblePillChoices.Contains(randomPill))
            {
                possiblePillChoices.Add(randomPill);
            }
        }
    }

    /// <summary>
    /// Unlock a given pill for later pulls
    /// </summary>
    /// <param name="pillName">Pill name</param>
    /*private void UnlockPill(string pillName)
    {
        PillData pill = null;
        lockedPills.RemoveAll(s => lockedPills.FindAll(lockedPill => {
            if (lockedPill.name.Equals(pillName))
            {
                pill = lockedPill;
            }
            return lockedPill.name.Equals(pillName);
        }).Contains(s));
        if (pill != null)
        {
            Debug.Log("Pill unlocked : " + pill.name);
            unlockedPills.Add(pill);
        }
    }*/

    /// <summary>
    /// Update the GUI with active pills
    /// </summary>
    public void UpdateGUI(List<CurrentEffect> effects)
    {
        if (possiblePillChoices != null && possiblePillChoices.Count >= 3 && leftPill != null && middlePill != null && rightPill != null)
        {
            bool paralysed = effects.Exists(e => e.effectData.name == "paralysis");
            int paralysisDisabledPill = UnityEngine.Random.Range(0, 2);
            UpdatePillGUI(leftPill, possiblePillChoices[0], effects, paralysisDisabledPill == 0 && paralysed);
            UpdatePillGUI(middlePill, possiblePillChoices[1], effects, paralysisDisabledPill == 1 && paralysed);
            UpdatePillGUI(rightPill, possiblePillChoices[2], effects, paralysisDisabledPill == 2 && paralysed);
        }
    }

    /// <summary>
    /// Update a pill GUI
    /// </summary>
    private void UpdatePillGUI(GameObject pillObject, PillData pillData, List<CurrentEffect> effects, bool disabled)
    {
        Debug.Log(pillData);
        // Search Animator
        Animator animatorPillsSelection = GameObject.FindGameObjectWithTag("Pills").GetComponent<Animator>();
        animatorPillsSelection.SetTrigger("Apparate");
        // get all textures

        // Get ours
        Sprite ours = (effects.Exists(e => e.effectData.name == "severeHallucination")) ? fruitImages.textures[UnityEngine.Random.Range(0, fruitImages.textures.Length - 1)] : pillImages.textures[pillData.image];

        // I take my component
        pillObject.GetComponentInChildren<UnityEngine.UI.Image>().sprite = ours;

        // Disabling button if needed
        pillObject.GetComponentInChildren<UnityEngine.UI.Button>().interactable = !disabled;
        // Changing pill texts
        TextMeshProUGUI[] pillTexts = pillObject.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI pillText in pillTexts)
        {
            if (pillText.gameObject.tag == "PillName")
            {
                pillText.SetText(GetPillName(pillData));
            }
            else if (pillText.gameObject.tag == "PillDescription")
            {
                pillText.SetText(GetPillDescription(pillData));
            }
        }
        // Changing pill bonus/malus icons colors
        foreach (OrganIconChanger iconChanger in pillObject.transform.GetComponentsInChildren<OrganIconChanger>())
        {
            iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.alreadyTaken;
            switch (iconChanger.name)
            {
                case "HeartIcon":
                    iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.heart != 0;
                    iconChanger.ChangeColor(pillData.alreadyTaken ? pillData.heart : 0);
                    break;
                case "LungsIcon":
                    iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.pulmon != 0;
                    iconChanger.ChangeColor(pillData.alreadyTaken ? pillData.pulmon : 0);
                    break;
                case "BrainIcon":
                    iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.brain != 0;
                    iconChanger.ChangeColor(pillData.alreadyTaken ? pillData.brain : 0);
                    break;
                case "IntestineIcon":
                    iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.intestine != 0;
                    iconChanger.ChangeColor(pillData.alreadyTaken ? pillData.intestine : 0);
                    break;
                case "MuscleIcon":
                    iconChanger.GetComponent<UnityEngine.UI.Image>().enabled = pillData.muscles != 0;
                    iconChanger.ChangeColor(pillData.alreadyTaken ? pillData.muscles : 0);
                    break;
            }
        }

    }

    /// <summary>
    /// Generate name based on pill data, in case there is an effect to apply before
    /// </summary>
    /// <param name="pill">Pill</param>
    /// <returns>Pill name</returns>
    private string GetPillName(PillData pill)
    {
        if (PlayerHealth.IsBlind())
        {
            return "???";
        }
        else
        {
            return pill.displayName;
        }
    }

    /// <summary>
    /// Generate description based on pill data, in case there is an effect to apply before
    /// </summary>
    /// <param name="pill">Pill</param>
    /// <returns>Pill description</returns>
    private string GetPillDescription(PillData pill)
    {
        if (PlayerHealth.IsBlind())
        {
            return randomPillDescriptions[UnityEngine.Random.Range(0, randomPillDescriptions.Count - 1)];
        } else
        {
            return pill.description;
        }
    }
}
