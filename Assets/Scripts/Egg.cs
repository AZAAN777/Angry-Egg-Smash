using System.Collections;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public enum EggType { Red, White }
    public EggType type;

    public Sprite redHappyOpen, redHappyClosed, redAngryOpen, redAngryClosed;
    public Sprite whiteHappyOpen, whiteHappyClosed, whiteAngryOpen, whiteAngryClosed;

    private SpriteRenderer sr;
    private float blinkTimer = 0f;
    private float blinkInterval = 0.5f;
    private float blinkDuration = 0.2f;
    private bool isDead = false;
    private bool isAngry = false;

    public float fallSpeed = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetDefaultOpenSprite();
    }

    void Update()
    {
        if (isDead) return;

        // Move egg down
        transform.Translate(Vector2.down * GameManager.Instance.eggFallSpeed * Time.deltaTime);

        // Blink
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            StartCoroutine(Blink());
            blinkTimer = 0f;
        }

        // If egg falls below screen
        if (transform.position.y < -5f)
        {
            isDead = true;

            if (type == EggType.Red)
            {
                isAngry = true;
                sr.sprite = redAngryOpen;
                GameManager.Instance.RedEggMissed();
                Destroy(gameObject, 0.4f);
            }
            else
            {
                Destroy(gameObject); // No penalty for white falling
            }
        }
    }

    IEnumerator Blink()
    {
        if (type == EggType.Red)
        {
            sr.sprite = isAngry ? redAngryClosed : redHappyClosed;
            yield return new WaitForSeconds(blinkDuration);
            sr.sprite = isAngry ? redAngryOpen : redHappyOpen;
        }
        else
        {
            sr.sprite = isAngry ? whiteAngryClosed : whiteHappyClosed;
            yield return new WaitForSeconds(blinkDuration);
            sr.sprite = isAngry ? whiteAngryOpen : whiteHappyOpen;
        }
    }

    void OnMouseDown()
    {
        if(isDead || Time.timeScale == 0f || GameManager.Instance.isGameOver) return;
        isDead = true;

        if (type == EggType.Red)
        {
            sr.sprite = redHappyOpen;
            GameManager.Instance.RedEggPopped();

            // Convert world position to screen point
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            GameObject splash = Instantiate(GameManager.Instance.redEggSplashPrefab, GameManager.Instance.canvasTransform);
            splash.GetComponent<RectTransform>().position = screenPos;

            Destroy(splash, 0.2f);
        }
        else
        {
            isAngry = true;
            sr.sprite = whiteAngryOpen;
            GameManager.Instance.GameOver();
        }

        Destroy(gameObject);
    }



    void SetDefaultOpenSprite()
    {
        sr.sprite = type == EggType.Red ? redHappyOpen : whiteHappyOpen;
    }
}
