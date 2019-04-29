using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject effectTextObject;

    private Dictionary<string, GameObject> effectsTexts;

    public EffectGenerator()
    {
        effectsTexts = new Dictionary<string, GameObject>();
    }

    public void AddEffect(EffectData effect)
    {
        if (effectTextObject != null)
        {
            if (!effectsTexts.ContainsKey(effect.name))
            {
                effectsTexts.Add(effect.name, Instantiate(effectTextObject));
                effectsTexts[effect.name].name = "EffectText";
                effectsTexts[effect.name].GetComponent<TextMeshProUGUI>().SetText(effect.displayName);

                UnityEngine.UI.Image background = effectsTexts[effect.name].transform.Find("Background").GetComponent<UnityEngine.UI.Image>();
                switch(effect.state)
                {
                    case EffectData.State.bonus:
                    background.color = Color.green;
                        break;
                    case EffectData.State.malus:
                    background.color = Color.red;
                        break;
                    case EffectData.State.critical:
                    background.color = Color.black;
                        break;
                }
                effectsTexts[effect.name].transform.SetParent(transform);
                effectsTexts[effect.name].transform.localScale = new Vector3(1, 1, 1);
                effectsTexts[effect.name].transform.Find("ToolTip/EffectDescription").GetComponent<TextMeshProUGUI>().SetText(effect.description);

                effectsTexts[effect.name].transform.Find("ToolTip/EffectDuration").GetComponent<TextMeshProUGUI>().SetText(effect.stateName == "critical" ? "Critical" : (effect.permanent? "Prevention" : "Remain : " + effect.duration));
                Debug.Log("Gained effect : " + effect.name);
            }
        } else
        {
            Debug.LogWarning("EffectText is null and cannot be instantiated !");
        }
    }

    public void RemoveEffect(string effectKey)
    {
        Debug.Log("Removed effect : " + effectKey);
        GameObject.Destroy(effectsTexts[effectKey]);
        effectsTexts.Remove(effectKey);
    }

    public List<string> GetEffectNames()
    {
        List<string> effectsNames = new List<string>();
        foreach(GameObject effectText in effectsTexts.Values)
        {
            effectsNames.Add(effectText.name);
        }
        return effectsNames;
    }
}
