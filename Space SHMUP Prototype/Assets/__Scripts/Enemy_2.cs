using UnityEngine;

public class Enemy_2 : Enemy
{
    private float sinEccentricity = 0.6f;
    private float lifeTime = 10;
    private Vector3 p0;
    private Vector3 p1;
    private float birthTime;
    
    void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndCheck.CamWidth - bndCheck.Radius;
        p0.y = Random.Range(-bndCheck.CamHeight, bndCheck.CamHeight);

        p1 = Vector3.zero;
        p1.x = bndCheck.CamWidth + bndCheck.Radius;
        p1.y = Random.Range(-bndCheck.CamHeight, bndCheck.CamHeight);

        if(Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u>1) //если корабль существует дольше чем lifeTime
        {
            Destroy(this.gameObject);
            return;
        }

        u = u + sinEccentricity * (Mathf.Sin(u*Mathf.PI * 2));

        pos = (1-u)*p0 + u*p1;
    }
}
