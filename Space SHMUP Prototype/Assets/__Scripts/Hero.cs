using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    private float speed = 30;
    private float rollMult = -45;
    private float pitchMult = 30;
    private float gameRestartDelay = 2f;

    [SerializeField]
    private float _shieldLevel = 1;
    
    private GameObject lastTriggerGO = null;
    
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    public Weapon[] weapons;

    void Start()
    {
        if(S == null) 
        {
            S = this;
        }
        else 
        {
            Debug.LogError("Hero.Awake() - Attemted to assing second Hero.S!");
        }

        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    void Update()
    {
       float xAxis = Input.GetAxis("Horizontal");
       float yAxis = Input.GetAxis("Vertical");

       Vector3 pos = transform.position;
       pos.x += xAxis * speed * Time.deltaTime;
       pos.y += yAxis * speed * Time.deltaTime;
       transform.position = pos;

       transform.rotation = Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult, 0);

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    } 

    void OnTriggerEnter (Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGO) return;
        
        lastTriggerGO = go;

        if (go.tag == "Enemy")
        {
            shieldlevel--;
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print ("Triggered by non-Enemy: " + go.name);
        }
    }

    private void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        
        switch (pu.Type)
        {
            case WeaponType.shield:
                shieldlevel++;
                break;
            default:
                if (pu.Type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.Type);
                    }
                }
                else 
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.Type);
                }
                break;
        }
        
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldlevel
    {
        get
        {
            return(_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min (value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return weapons[i];
            }
        }
        
        return null;
    }

    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
