using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxMultiplier;
    private float backgroundLength;
    private float xPos;
    private float startCam;

    public void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPos = transform.position.x;
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;
        startCam = cam.transform.position.x;
    }

    public void Update()
    {
        float camTravel = cam.transform.position.x - startCam;

        // Senin o ters çevrilmiþ düz mantýk hýz hesabýn
        float distanceToMove = camTravel * (1 - parallaxMultiplier);

        // Kameranýn bu katmana göre ne kadar ileri gittiðini net buluyoruz
        float distanceMoved = camTravel * parallaxMultiplier;

        transform.position = new Vector3(xPos + distanceToMove, transform.position.y);

        // ASIL ÇÖZÜM: Kameranýn bu katmandaki koordinatý, bizim resmin merkezinden 
        // ileriye doðru tek bir resim boyundan fazla uzaklaþtýysa anýnda tetikle!
        if (distanceMoved - xPos > backgroundLength)
        {
            xPos += backgroundLength;
        }
        else if (distanceMoved - xPos < -backgroundLength)
        {
            xPos -= backgroundLength;
        }
    }
}