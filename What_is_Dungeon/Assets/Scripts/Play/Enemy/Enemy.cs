using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }

    public float maxHp;
    public float currentHp;
    public Image healthBar; // 각 에너미 별로 할당된 healthBar
    public GameObject HpBar;

    public float speed = 5f;
    Rigidbody rb;

    public string EnemyTag;
    public bool isStop;

    // Start is called before the first frame update
    void Start()
    {
        switch (EnemyTag)
        {
            case ("Wave1"):
                maxHp = 50;
                break;
            case ("Wave2"):
                maxHp = 100;
                break;
            case ("Wave3"):
                maxHp = 150;
                break;
            case ("Boss"):
                maxHp = 300;
                break;
        }

        currentHp = maxHp;
        UpdateHealthBar();

        isStop = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (isStop)
            {
                speed = 0;
            }
            else
            {
                speed = 5f;
                Move();
            }
        }

        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }
    void OnEnable()
    {
        if (HpBar != null)
        {
            HpBar.gameObject.SetActive(true);
        }
    }
    void OnDisable()
    {
        if (HpBar != null)
        {
            HpBar.gameObject.SetActive(false);
        }
    }
    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        UpdateHealthBar();

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (HpBar != null)
        {
            HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(90, 2, 0)); // 적의 위치를 따라다니도록 설정
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 1;
            Die();
        }
        if (EnemyTag == "Boss" && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 3;
            Die();
        }
    }

    
}
