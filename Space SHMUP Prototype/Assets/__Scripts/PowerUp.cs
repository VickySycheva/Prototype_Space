using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro), typeof(Rigidbody), typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    public WeaponType Type { get; private set; }

    private Vector2 rotMinMax = new Vector2 (15, 90);
    private Vector2 driftMinMax = new Vector2 (0.25f, 2);
    private float lifeTime = 6f;
    private float fadeTime = 4f;
    private GameObject cube;
    private TextMeshPro letter;
    private Vector3 rotPerSecond;
    private float birthTime;
    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMeshPro>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        transform.rotation = Quaternion.identity; // отсутствие поворота

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), 
                                    Random.Range(rotMinMax.x, rotMinMax.y),
                                    Random.Range(rotMinMax.x, rotMinMax.y));
        
        birthTime = Time.time;
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if (u >= 1)
        {
            Destroy(gameObject);
        }
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if(!bndCheck.IsOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public void SetType (WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition (wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        Type = wt;
    }

    public void AbsorbedBy (GameObject target)
    {
        Destroy(this.gameObject);
    }
}
