using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private TextMeshProUGUI weekText;
    private Transform effectList;

    private int weekNumber;
    private List<string> effectsNames;

    // Start is called before the first frame update
    private void Start()
    {
        InitGameObjectReferences();
        weekNumber = 1;
        effectsNames = new List<string>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGUI();
    }

    /// <summary>
    /// Game objects initialization, if found
    /// </summary>
    private void InitGameObjectReferences()
    {
        GameObject weekGameObject = GameObject.FindGameObjectWithTag("WeekText");
        if (weekGameObject != null)
        {
            weekText = weekGameObject.GetComponent<TextMeshProUGUI>();
        }
        GameObject effectListGameObject = GameObject.FindGameObjectWithTag("EffectList");
        if (effectListGameObject != null)
        {
            effectList = effectListGameObject.transform;
        }
    }

    /// <summary>
    /// Update different GUI elements with data
    /// </summary>
    private void UpdateGUI()
    {
        if (weekText != null)
        {
            weekText.SetText("Week " + weekNumber);
        }
        else
        {
            Debug.LogWarning("Week text element is not defined !");
        }
        if (effectList != null)
        {
            bool firstItemKept = false;
            Transform effectListName = null;
            foreach (Transform child in effectList)
            {
                if (firstItemKept)
                {
                    Destroy(child.gameObject);
                } else
                {
                    effectListName = child;
                    firstItemKept = true;
                }
            }
            if (effectListName != null)
            {
                foreach (string effectName in effectsNames)
                {
                    GameObject newEffect = Instantiate(effectListName.gameObject);
                    newEffect.name = "EffectElement";
                    newEffect.GetComponent<TextMeshProUGUI>().SetText(effectName);
                    newEffect.transform.SetParent(effectList);
                    newEffect.transform.localScale = new Vector3(1, 1, 1);
                }
            } else
            {
                Debug.LogWarning("Effect list name element is not defined !");
            }
        }
        else
        {
            Debug.LogWarning("Effect list element is not defined !");
        }
    }
}
