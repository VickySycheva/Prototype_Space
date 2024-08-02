using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    public float PowerUpDropChance => powerUpDpopChance;

    public Vector3 pos 
    {
        get 
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    [SerializeField] private float speed = 10f;
    [SerializeField] private float health = 10;
    [SerializeField] protected float showDamageDuration = 0.1f;
    [SerializeField] private float powerUpDpopChance = 1f;
    [SerializeField] protected bool showingDamage = false;

    protected BoundsCheck bndCheck;

    private Material[] materials;
    private Color[] originalColors;

    protected float damageDoneTime;
    private bool notifiedOfDestruction = false;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++ )
        {
            originalColors[i] = materials[i].color;
        }
    }

    void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.OffDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
       Vector3 tempPos = pos;
       tempPos.y -= speed * Time.deltaTime;
       pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if(!bndCheck.IsOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-projectileHero");
                break;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
