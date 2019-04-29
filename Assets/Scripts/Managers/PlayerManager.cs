using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private readonly GameObject effectList;
    private readonly GameObject brain;
    private readonly GameObject heart;
    private readonly GameObject intestine;
    private readonly GameObject pulmon;
    private readonly GameObject muscle;
    private readonly Dictionary<int, string> organKeys;
    private readonly Dictionary<int, string> heartKeys;
    public int level;

    public PlayerHealth player { get; }

    public PlayerManager()
    {
        player = new PlayerHealth();
        level = 5;

        effectList = GameObject.FindGameObjectWithTag("EffectList");
        brain = GameObject.FindGameObjectWithTag("Brain");
        heart = GameObject.FindGameObjectWithTag("Heart");
        intestine = GameObject.FindGameObjectWithTag("Intestine");
        pulmon = GameObject.FindGameObjectWithTag("Pulmon");
        muscle = GameObject.FindGameObjectWithTag("Muscle");

        organKeys = new Dictionary<int, string>()
        {
            { 0, "Black" },
            { 1, "Red" },
            { 2, "Orange" },
            { 3, "Yellow" },
            { 4, "LightGreen" },
            { 5, "DarkGreen" }
        };
        heartKeys = new Dictionary<int, string>()
        {
            { 0, "Black" },
            { 1, "Red" },
            { 2, "Yellow" },
            { 3, "DarkGreen" }
        };
    }

    public bool EatPill(PillData pill)
    {
        level++;
        if (!player.ApplyPillStat(pill.bodyEffect, ref player.currentEffects, player.effectList))
        {
            return false;
        }
        if (pill.secondaryEffect != null)
        {
            player.AddEffects(pill.secondaryEffect);
        }
        player.currency += pill.cost;
        return true;
    }

    public void DecreaseSideEffects()
    {
        player.DecreaseEffects(1);
    }

    public void UpdateGUI()
    {
        // Body organs display
        foreach(GameObject organ in new List<GameObject>() { brain, intestine, pulmon, muscle })
        {
            foreach (Transform image in organ.transform)
            {
                image.gameObject.SetActive(false);
            }
        }
        brain.transform.Find("UI_Brain_" + organKeys[(int)(player.body.brain/ (float)player.body.brainMax * (organKeys.Count - 1))]).gameObject.SetActive(true);
        intestine.transform.Find("UI_Intestine_" + organKeys[(int)(player.body.intestine / (float)player.body.intestineMax * (organKeys.Count - 1))]).gameObject.SetActive(true);
        pulmon.transform.Find("UI_Pulmon_" + organKeys[(int)(player.body.pulmon / (float)player.body.pulmonMax * (organKeys.Count - 1))]).gameObject.SetActive(true);
        muscle.transform.Find("UI_Muscle_" + organKeys[(int)(player.body.muscles / (float)player.body.musclesMax * (organKeys.Count - 1))]).gameObject.SetActive(true);
        foreach (Transform image in heart.transform)
        {
            image.gameObject.SetActive(false);
        }
        heart.transform.Find("UI_Heart_" + heartKeys[(int)(player.body.heart / (float)player.body.heartMax * (heartKeys.Count - 1))]).gameObject.SetActive(true);
        // Effects display
        
    }
    public void Transplant(ShopItem item)
    {
        int price = Mathf.CeilToInt( item.cost * (Mathf.Pow(1.5f,player.transplantationCount)));
        if (player.currency < price)
        {
            return;
        }
        player.transplantationCount++;
        player.currency -= price;
        Debug.Log("transplate " + item.name);
        player.body.Reset(item.name, player.transplantationCount);
    }
}
