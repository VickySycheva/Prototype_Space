using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Shield : MonoBehaviour
{
    private int levelShown = 1;
    private float rotationPerSecond = 0.1f;
    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        int currLevel = Mathf.FloorToInt(Hero.S.shieldlevel);
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            mat.mainTextureOffset = new Vector2 (0.2f * levelShown, 0);
        }

        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler (0, 0, rZ);
    }
}
