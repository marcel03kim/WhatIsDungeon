using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public float speed = 5f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        switch (gameObject.tag)
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
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);    //몬스터 이동 스크립트
        
        if(hp <= 0 )
        {
            Die();
        }
    }
    void Die()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Die();
        }
    }
}
