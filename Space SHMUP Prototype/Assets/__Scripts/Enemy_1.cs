using UnityEngine;

public class Enemy_1 : Enemy
{
    private float waveWith = 4; //ширина синусоиды в метрах
    private float waveRotY = 45;
    private float x0; //начальное значение координаты х
    private float birthTime;

    void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age;
        float sin = Mathf.Sin(theta);

        tempPos.x = x0 + waveWith * sin;
        pos = tempPos;
        
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        
        base.Move();
    }
}
