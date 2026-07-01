using UnityEngine;

public class Diamond : Coin
{
    
    [SerializeField] private float amp = 0.1f;
    [SerializeField] private float fr = 4f;

    private float startPosY;
    void Start()
    {
        startPosY = transform.position.y;
    }

    
    void Update()
    {
        transform.position = new Vector2(transform.position.x, startPosY + Mathf.Sin(Time.time * fr) * amp);
    }
}
