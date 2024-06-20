using UnityEngine;
using System.Collections;

public class CountDown : MonoBehaviour
{

    private int Timer = 0;

    public GameObject Num_A;   //1��
    public GameObject Num_B;   //2��
    public GameObject Num_C;   //3��
    public GameObject Num_GO;
    public GameObject BackGround;



    void Start()
    {

        //���۽� ī��Ʈ �ٿ� �ʱ�ȭ
        Timer = 0;



        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_GO.SetActive(false);
        BackGround.SetActive(false);



    }

    void Update()
    {

        //���� ���۽� ����
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }


        //Timer �� 90���� �۰ų� ������� Timer �������

        if (Timer <= 120)
        {
            Timer++;

            // Timer�� 30���� ������� 3���ѱ�
            if (Timer < 40)
            {
                Num_C.SetActive(true);
                BackGround.SetActive(true);
            }

            // Timer�� 30���� Ŭ��� 3������ 2���ѱ�
            if (Timer > 40)
            {
                Num_C.SetActive(false);
                Num_B.SetActive(true);
            }

            // Timer�� 60���� ������� 2������ 1���ѱ�
            if (Timer > 80)
            {
                Num_B.SetActive(false);
                Num_A.SetActive(true);
            }

            //Timer �� 90���� ũ�ų� ������� 1������ GO �ѱ� LoadingEnd () �ڷ�ƾȣ��
            if (Timer >= 120)
            {
                Num_A.SetActive(false);
                Num_GO.SetActive(true);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f; //���ӽ���
            }
        }

    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Num_GO.SetActive(false);
        BackGround.SetActive(false);
    }

}