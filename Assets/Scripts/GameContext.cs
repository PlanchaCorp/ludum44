using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameContext
{
    
    private static readonly string pillsPath = "Assets/Resources/LD44_-_Experimental_Treatment_-_Pillule.json";
    private static readonly string effectsPath = "Assets/Resources/LD44_-_Experimental_Treatment_-_Effects.json";

    public static PillsList pills;

    public static EffectList effects;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        pills =  JsonUtility.FromJson<PillsList>(LoadJsonFile(pillsPath));
        effects =  JsonUtility.FromJson<EffectList>(LoadJsonFile(effectsPath));  

        foreach(PillData pill in pills.Pillule)
        {
            Debug.Log(pill.ToString());
        }
    }

    private static string LoadJsonFile(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            return sr.ReadToEnd();
        }
    }

}
