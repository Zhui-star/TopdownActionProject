using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCall : MonoBehaviour
{
    public float speed = 0.05f; //测试用
    private ParticleSystem ball;
    public ParticleSystem create;
    public Vector3 ballPosition;
    public Vector3 ballTargerPosition;
    public bool isBigEnough;
    public bool isSmall;
    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<ParticleSystem>();
        isSmall = true;
        //create = GetComponent<ParticleSystem>();
        ballTargerPosition = new Vector3(ball.transform.position.x + 2.5f, ball.transform.position.y, ball.transform.position.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.localScale.x <= 1 && ball.transform.localScale.y <= 1 && ball.transform.localScale.z <= 1)
            transform.Translate(new Vector3(1 * speed, 0, 0));

        //ball.transform.position = Vector3.Lerp(ball.transform.position, ballTargerPosition, Time.deltaTime * speed);

        if (isSmall)
        {
            Invoke("ExpandBall", 0.5f);
        }
      

        if (isBigEnough)
        {
            Destroy(gameObject);
            Create();
            isBigEnough = false;
        }
 
    }

    public void ExpandBall()
    {
        if (ball.transform.localScale.x <= 2.5f && ball.transform.localScale.y <= 2.5f && ball.transform.localScale.z <= 2.5f)
        {
           // speed = 0.02f;
            ball.transform.localScale = new Vector3(ball.transform.localScale.x + 0.02f,
                ball.transform.localScale.y + 0.02f, ball.transform.localScale.z + 0.02f);
        }
        else 
        {
            isSmall = false;
            isBigEnough = true;   
        }
    }

    public void Create()
    {
        Instantiate(create, ball.transform.position, Quaternion.identity);
    }
}
