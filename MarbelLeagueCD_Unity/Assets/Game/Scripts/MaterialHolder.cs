using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    [SerializeField]
    Material[] materials;
    private Material tmpMaterial;
    public Material ReturnMaterial(string materialName)
    {
        //Return a material by name
        switch(materialName)
        {
            case "Yellow":   tmpMaterial = materials[0];     break;
            case "Red":     tmpMaterial = materials[1];     break;
        }
        return tmpMaterial;
    }
}
