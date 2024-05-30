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
        
        // 게임이 일시정지 상태가 아닐 때만 이동
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
            collision.gameObject.GetComponent<PlayerController>().Hp -= 1;         //충돌한 오브젝트에서 플레이어컨트롤러 스크립트를 가져오고
            Die();                                                                  //그 스크립트에 있는 Hp에서 1을 뺀다
        }
        if(EnemyTag == "Boss" && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Hp -= 3;         //충돌한 오브젝트에서 플레이어컨트롤러 스크립트를 가져오고
            Die();
        }
    }
}
