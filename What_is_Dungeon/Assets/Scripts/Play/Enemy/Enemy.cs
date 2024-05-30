using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float speed = 5f;
    Rigidbody rb;
    public string EnemyTag;
    public bool isFrozen;

    // Start is called before the first frame update
    void Start()
    {
        isFrozen = false;
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
            if (!isFrozen)
            {
                speed = 5f;
                Move();
            }
            else
            {
                speed = 0;
            }
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    void Die()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 1;         //�浹�� ������Ʈ���� �÷��̾���Ʈ�ѷ� ��ũ��Ʈ�� ��������
            Die();                                                                  //�� ��ũ��Ʈ�� �ִ� Hp���� 1�� ����
        }
        if(EnemyTag == "Boss" && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 3;         //�浹�� ������Ʈ���� �÷��̾���Ʈ�ѷ� ��ũ��Ʈ�� ��������
            Die();
        }
    }
}
