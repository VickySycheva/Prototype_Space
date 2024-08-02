using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(BoundsCheck))]
public class Projectile : MonoBehaviour
{
    public Rigidbody Rigid { get; private set; }

    public WeaponType type
    {
        get 
        {
            return _type;
        }
        set
        {
            SetType( value );
        }
    }

    [SerializeField] private WeaponType _type;

    private BoundsCheck bndCheck;
    private Renderer rend;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        Rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (bndCheck.OffUp)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
