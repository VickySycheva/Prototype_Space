using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Material[] GetAllMaterials (GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }

        return mats.ToArray();
    }
}
