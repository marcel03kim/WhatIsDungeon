using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }

    public float hp;
    //private float currentHp;
    //public Image healthBar;

    public float speed = 5f;
    Rigidbody rb;

    public string EnemyTag;
    public bool isStop;

    // Start is called before the first frame update
    void Start()
    {
        //currentHp = hp;
        //UpdateHealthBar();

        isStop = false;
        rb = GetComponent<Rigidbody>();

        switch (EnemyTag)
        {
            case ("Wave1"):
                hp = 50;
                break;
            case ("Wave2"):
                hp = 100;
                break;
            case ("Wave3"):
                hp = 150;
                break;
            case ("Boss"):
                hp = 300;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {

        // ������ �Ͻ����� ���°� �ƴ� ���� �̵�
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

        //UpdateHealthBar();

        if (hp <= 0)
        {
            Die();
        }
        //if (currentHp <= 0)
        //{
        //    Die();
        //}
    }
    //public void TakeDamage(float amount)
    //{
    //    currentHp -= amount;
    //    currentHp = Mathf.Clamp(currentHp, 0f, hp); // ü���� ������ ���� �ʵ��� ����

    //    UpdateHealthBar();

    //    if (currentHp <= 0f)
    //    {
    //        Die();
    //    }
    //}

    // ü�¹� ������Ʈ
    //private void UpdateHealthBar()
    //{
    //    if (healthBar != null)
    //    {
    //        healthBar.fillAmount = currentHp / hp;
    //    }
    //}
    public void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    void Die()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 1;         //�浹�� ������Ʈ���� �÷��̾���Ʈ�ѷ� ��ũ��Ʈ�� ��������
            Die();                                                                  //�� ��ũ��Ʈ�� �ִ� Hp���� 1�� ����
        }
        if (EnemyTag == "Boss" && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 3;         //�浹�� ������Ʈ���� �÷��̾���Ʈ�ѷ� ��ũ��Ʈ�� ��������
            Die();
        }
    }
}
