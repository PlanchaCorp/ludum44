using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsImagesHandler : MonoBehaviour
{
    [SerializeField] Sprite[] textures;
    public Sprite[] GetTextures() { return textures; }

    [SerializeField] Sprite[] fruitTextures;
    public Sprite[] GetFruitTextures() { return fruitTextures; }

}
