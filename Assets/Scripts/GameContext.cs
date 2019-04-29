using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameContext
{

    //private static readonly string pillsPath = "Assets/Resources/LD44_-_Experimental_Treatment_-_Pills.json";
    private static readonly string pillsPath = Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_PillsProcedural.json");
    private static readonly string effectsPath = Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Effects.json");
    private static readonly string doctorStatementPath = Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Statements.json");
    private static readonly string pillsNameGeneratorPath = Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_PillsGenerator.json");
    private static readonly string descriptionsGeneratorPath = Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Descriptions.json");

    public static PillsList pills;
    public static PillNameParts pillsNameParts;

    public static Dictionary<string, EffectData> criticalEffects;

    public static EffectList effects;


    public static DoctorList doctorDatas;
    public static DescriptionList descriptionDatas;

    public static List<DoctorData> doctorStatements;
    public static List<IEvent> events;
    public static List<string> startStatements;
    public static List<string> endStatements;
    public static List<DescriptionData> descriptions;

    public static void Load()
    {
       
        foreach (EffectData effect in effects.Effects)
        {
            effect.Build();
            Debug.Log(effect);
        }
      
        foreach(PillData pill in pills.Pills)
        {
            pill.Build();
        }

        doctorStatements = doctorDatas.Statements;
        descriptions = descriptionDatas.Descriptions;

        criticalEffects = new Dictionary<string, EffectData>()
        {
            {"heart",effects.Effects.Find(e => e.name == "heartAttack")},
            {"brain",effects.Effects.Find(e => e.name == "stroke")},
            {"pulmon",effects.Effects.Find(e => e.name == "entérovirusD-68")},
            {"intestine",effects.Effects.Find(e => e.name == "ulcer")},
            {"muscles",effects.Effects.Find(e => e.name == "necrosis")}
        };
        startStatements = new List<string>()
        {
            "Hello and welcome into my office mister... miss...? Well, it does not matter, either you will be out of here, healthy and rich, or dead - in any cases, the chances of me seeing you again are pretty low!",
            "Here, we test some experimental treatment. You just have to choose one pill and wait !",
            "But be aware of your health state, as each pill might worsen it. You can monitor it at the bottom left of your patient sheet.",
            "Good luck, if I may! But remember, this is no hospital. Should you die, consider outside of the building."
        };
        endStatements = new List<string>()
        {
            "Clementine? You won the bet, the patient just died.",
            "Oh I never thought one could bend its limb like that while dying!",
            "Clementine? Could you bring me a stick? I am not sure the patient is dead yet.",
            "I hope you'll be luckier in the afterlife.",
            "Clementine? Could you get the patient's jacket? We can steal some money before the funeral parlour comes!",
            "Clementine? Bring me a mop! Yes the patient's dead, and I can confirm there is no muscle tone anymore.",
            "Clementine? Clementine? Oh, the battery must be dead... I could say \"so are you\" but I am alone so what's the point?",
            "Clementine? Bring a broom and a large stick! I am sure I can make a scarecrow out of this one!",
            "Clementine? Yes, this one is out, bring in the next.",
            "Clementine? Yes, with this ones death my evening is open! Your husband is out of town this week isn't he?",
            "What's that smell? You've been dead for 2 seconds and you already smell like a 10 years-old cadavre!",
            "Clementine? Call an ambulance, the patient jumped from the window! Now take those legs and help me out, no one's looking!"
        };

        events = new List<IEvent>()
        {
        };

        pillsNameParts.SeparateNames();
    }
}
