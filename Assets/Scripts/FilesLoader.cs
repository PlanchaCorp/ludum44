using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class FilesLoader : MonoBehaviour
{
    private static readonly Uri pillsPath = new Uri( Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_PillsProcedural.json"));
    private static readonly Uri effectsPath = new Uri(Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Effects.json"));
    private static readonly Uri doctorStatementPath = new Uri(Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Statements.json"));
    private static readonly Uri pillsNameGeneratorPath = new Uri(Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_PillsGenerator.json"));
    private static readonly Uri descriptionsGeneratorPath = new Uri(Path.Combine(Application.streamingAssetsPath, "LD44_-_Experimental_Treatment_-_Descriptions.json"));
    string data;
    
    // Use this for initialization
    void Start()
    {
       GetRequest(pillsPath.AbsoluteUri);
        GameContext.pills = JsonUtility.FromJson<PillsList>(data);
        Debug.Log(data);
        GetRequest(effectsPath.AbsoluteUri);
        GameContext.effects = JsonUtility.FromJson<EffectList>(data);
        Debug.Log(data);
       GetRequest(doctorStatementPath.AbsoluteUri);
        GameContext.doctorDatas = JsonUtility.FromJson<DoctorList>(data);
        Debug.Log(data);
       GetRequest(pillsNameGeneratorPath.AbsoluteUri);
        GameContext.pillsNameParts = JsonUtility.FromJson<PillNameParts>(data);
        Debug.Log(data);
       GetRequest(descriptionsGeneratorPath.AbsoluteUri);
        GameContext.descriptionDatas = JsonUtility.FromJson<DescriptionList>(data);
        Debug.Log(data);
        GameContext.Load();
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
           
            webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("request" + webRequest.downloadHandler.text);
             data =  webRequest.downloadHandler.text;
            }
        }
    }
}
