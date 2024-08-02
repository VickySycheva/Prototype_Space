using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    public bool IsOnScreen { get; private set; }
    public float CamWidth { get; private set; }
    public float CamHeight { get; private set; }
    public bool OffRight { get; private set; }
    public bool OffLeft { get; private set; }
    public bool OffUp { get; private set; }
    public bool OffDown { get; private set; }

    public float Radius => radius;

    [SerializeField] private float radius = 1f;
    [SerializeField] private bool keepOnScreen = true;
    
    void Awake()
    {
        CamHeight = Camera.main.orthographicSize;
        CamWidth = CamHeight * Camera.main.aspect;
        IsOnScreen = true;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        OffRight = OffLeft = OffUp = OffDown = false;

        if(pos.x >= CamWidth - radius) 
        {
            pos.x = CamWidth - radius;
            OffRight = true;
        }
        if(pos.x <= -CamWidth + radius)
        {
            pos.x = -CamWidth + radius;
            OffLeft = true;
        }
        if(pos.y >= CamHeight - radius) 
        {
            pos.y = CamHeight - radius;
            OffUp = true;
        }
        if(pos.y <= -CamHeight + radius) 
        {
            pos.y = -CamHeight + radius;
            OffDown = true;
        }

        IsOnScreen = !(OffRight || OffLeft || OffUp || OffDown);
        if (keepOnScreen && !IsOnScreen)
        {
            transform.position = pos;
            IsOnScreen = true;
            OffRight = OffLeft = OffUp = OffDown = false;
        }
    }

    void OnDrawGizmos ()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(CamWidth*2, CamHeight*2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }

}
