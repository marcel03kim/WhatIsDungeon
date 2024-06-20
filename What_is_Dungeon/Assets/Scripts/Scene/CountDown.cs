using UnityEngine;
using System.Collections;

public class CountDown : MonoBehaviour
{

    private int Timer = 0;

    public GameObject Num_A;   //1번
    public GameObject Num_B;   //2번
    public GameObject Num_C;   //3번
    public GameObject Num_GO;
    public GameObject BackGround;



    void Start()
    {

        //시작시 카운트 다운 초기화
        Timer = 0;



        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_GO.SetActive(false);
        BackGround.SetActive(false);



    }

    void Update()
    {

        //게임 시작시 정지
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }


        //Timer 가 90보다 작거나 같을경우 Timer 계속증가

        if (Timer <= 120)
        {
            Timer++;

            // Timer가 30보다 작을경우 3번켜기
            if (Timer < 40)
            {
                Num_C.SetActive(true);
                BackGround.SetActive(true);
            }

            // Timer가 30보다 클경우 3번끄고 2번켜기
            if (Timer > 40)
            {
                Num_C.SetActive(false);
                Num_B.SetActive(true);
            }

            // Timer가 60보다 작을경우 2번끄고 1번켜기
            if (Timer > 80)
            {
                Num_B.SetActive(false);
                Num_A.SetActive(true);
            }

            //Timer 가 90보다 크거나 같을경우 1번끄고 GO 켜기 LoadingEnd () 코루틴호출
            if (Timer >= 120)
            {
                Num_A.SetActive(false);
                Num_GO.SetActive(true);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f; //게임시작
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