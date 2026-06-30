using UnityEngine;

public class GhostTrail : MonoBehaviour
{

    [SerializeField] private float fadeSpeed;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        Color color = sr.color;
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;


        if (color.a <= 0f)
            Destroy(gameObject);
    }
}
