using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PillNameParts
{
    public List<string> PillsGenerator;

    public List<string> pillPrefixes;
    public List<string> pillSuffixs;

    public void SeparateNames()
    {
        pillPrefixes = new List<string>();
        pillSuffixs = new List<string>();
        int i = 0;
        foreach(string name in PillsGenerator)
        {
            if (i < PillsGenerator.Count / 2)
            {
                pillPrefixes.Add(name);
            } else
            {
                pillSuffixs.Add(name);
            }
            i++;
        }
    }
}
