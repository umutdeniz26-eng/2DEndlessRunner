using System.Collections;

using UnityEngine;

public class PunchTrap : Trap
{
    private BoxCollider2D hitCollider;
    [SerializeField] private float growDuration;
    [SerializeField] private float targetHeight;
    void Start()
    {
        hitCollider = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        
    }


    public void StartExpansion()
    {
        StartCoroutine(ColliderExpansion());
    }
    private IEnumerator ColliderExpansion()
    {
        
        float time = 0;
        float startHeight = hitCollider.size.y;

        while (time < growDuration)
        {
            time += Time.deltaTime;

            float progress = time / growDuration;
            float currentY=Mathf.Lerp(startHeight,targetHeight,progress);

            hitCollider.size=new Vector2(hitCollider.size.x,currentY);

            hitCollider.offset = new Vector2(hitCollider.offset.x, currentY / 2f);

            yield return null;
        }
         
    }
}
