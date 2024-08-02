using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoundsCheck))]
public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [SerializeField] private GameObject[] prefabEnemies;
    [SerializeField] private float enemySpawnPerSecond = 0.5f;
    [SerializeField] private float enemyDefaultPadding = 1.5f;
    [SerializeField] private WeaponDefinition[] weaponDefinitions;
    [SerializeField] private PowerUp prefabPowerUp;

    private WeaponType[] powerUpFrequency = new WeaponType[] {
                                WeaponType.blaster, WeaponType.blaster, 
                                WeaponType.spread, WeaponType.shield  };
    private BoundsCheck bndcheck;

    public void ShipDestroyed (Enemy e)
    {
        if(UnityEngine.Random.value <= e.PowerUpDropChance)
        {
            int ndx = UnityEngine.Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            PowerUp pu = Instantiate(prefabPowerUp);
            pu.SetType(puType);
            
            pu.transform.position = e.transform.position;
        }
    }

    void Awake()
    {
        S = this;
        bndcheck = GetComponent<BoundsCheck>();
        Invoke (nameof(SpawnEnemy), 1f/enemySpawnPerSecond);

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy ()
    {
        int ndx = UnityEngine.Random.Range (0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]);

        float enemyPadding = enemyDefaultPadding;
        BoundsCheck enemyBounds = go.GetComponent<BoundsCheck>();
        if (enemyBounds != null)
        {
            enemyPadding = Mathf.Abs(enemyBounds.Radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndcheck.CamWidth + enemyPadding;
        float xMax = bndcheck.CamWidth - enemyPadding;
        pos.x = UnityEngine.Random.Range(xMin, xMax);
        pos.y = bndcheck.CamHeight + enemyPadding;
        go.transform.position = pos;

        Invoke (nameof(SpawnEnemy), 1f/enemySpawnPerSecond);
    }

    public void DelayedRestart (float delay)
    {
        Invoke(nameof(Restart), delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    static public WeaponDefinition GetWeaponDefinition (WeaponType wt)
    {
        if(WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }

        return new WeaponDefinition();
    }
}
