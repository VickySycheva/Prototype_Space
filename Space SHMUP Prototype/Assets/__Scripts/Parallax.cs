using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject poi; // Корабль игрока
    [SerializeField] private GameObject[] panels; // Прокручиваемые панели переднего плана
    [SerializeField] private float scrollSpeed = -30f;
    [SerializeField] private float motionMult = 0.25f; // Степень реакции панелей на перемещение корабля

    private float panelHt; // Высота панелей
    private float depth; // pos.z панелей

    void Start()
    {
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;

        panels[0].transform.position = new Vector3 (0, 0, depth);
        panels[1].transform.position = new Vector3 (0, panelHt, depth);
    }

    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);

        if (poi != null)
        {
            tX = - poi.transform.position.x * motionMult;
        }

        panels[0].transform.position = new Vector3 (tX, tY, depth);

        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3 (tX, tY - panelHt, depth);
        }
        else
        {
            panels[1].transform.position = new Vector3 (tX, tY + panelHt, depth);
        }
    }
}
