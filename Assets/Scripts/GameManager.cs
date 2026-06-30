using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player player;

   public static GameManager gameManager;

    [Header("Coins")]
    public int coins;
    public int coinMultiplier=1;

    [Header("Score")]
    public int score = 0;
    public float scoreMultiplier = 1;

    private float internalScore;
    private void Awake()
    {
        GameManager.gameManager = this;
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        HandleScore();
    }

    private void HandleScore()
    {
        if (!player.isDead)
        {
            internalScore += Time.deltaTime * scoreMultiplier;
            score = Mathf.FloorToInt(internalScore);
        }
    }

    public void RestartLevel() => SceneManager.LoadScene(0);

}
