using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HospitalController : MonoBehaviour
{
    [SerializeField] GameObject transplantationPanel;

    [SerializeField] List<ShopItem> items;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void UpdateGUI(PlayerHealth player)
    {
      foreach(string part in player.body.GetBodyPartHealth().Keys)
        {
           GameObject row = Create(items.Find(i => i.name == part), player);
           int price = Mathf.CeilToInt(items.Find(i => i.name == part).cost * (Mathf.Pow(2, player.transplantationCount)));
            if (player.currency < price)
            {
                row.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
            } else
            {
                row.GetComponentInChildren<UnityEngine.UI.Button>().interactable = true;
            }
           
        }  
    }

    private GameObject Create(ShopItem shopItem,PlayerHealth player)
    {
        int price = Mathf.CeilToInt(shopItem.cost * (Mathf.Pow(1.5f, player.transplantationCount)));
        if (this.transform.Find(shopItem.name) == null)
        {
            GameObject transaction = Instantiate(transplantationPanel, this.transform);
            transaction.name = shopItem.name;
            transaction.transform.Find("HeartTextPanel/HeartTitle").GetComponent<TextMeshProUGUI>().SetText(shopItem.displayName);
            transaction.transform.Find("HeartTextPanel/HeartDescription").GetComponent<TextMeshProUGUI>().SetText(shopItem.description);
            transaction.transform.Find("HeartIcon").GetComponent<UnityEngine.UI.Image>().sprite = shopItem.icon;
            transaction.transform.Find("HeartButtonTransplant/HeartButtonTextTransplant").GetComponent<TextMeshProUGUI>().SetText(parse(price));
            transaction.transform.Find("HeartButtonTransplant").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                () => OnClick(shopItem)
                );
            return transaction;
        }
        this.transform.Find(shopItem.name).transform.Find("HeartButtonTransplant/HeartButtonTextTransplant").GetComponent<TextMeshProUGUI>().SetText(parse(price));
        return transform.Find(shopItem.name).gameObject;

    }

    private void remove(string name)
    {
        if (this.transform.Find(name) == null)
        {
            return;
        }
        Destroy(this.transform.Find(name).gameObject);
    }

    internal string parse(int value)
    {
        if (value < 1000)
        {
            return String.Format("{000}", value);
        }
        if (value < 10000)
        {
            return String.Format("{0:#K0}", value / 100);
        }
        if (value < 1000000)
        {
            return String.Format("{000:#k}", value / 1000);
        }
        return value.ToString();
    }
    public void OnClick(ShopItem shopItem)
    {
        GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<GameController>().Transplant(shopItem);
    }
}
