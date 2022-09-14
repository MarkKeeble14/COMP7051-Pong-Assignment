using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlGoalie : MonoBehaviour
{
    private Transform ball;
    public float speed;
    private ControlGoalie playerGoalie;

    // Start is called before the first frame update
    void Start()
    {
        playerGoalie = GetComponent<ControlGoalie>();
        speed = playerGoalie.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ball)
        {
            BallController bc = FindObjectOfType<BallController>();
            if (bc)
                ball = bc.transform;
            return;
        }

        if (transform.position.z >= playerGoalie.maxZ
            && ball.transform.position.z > transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, 0, playerGoalie.maxZ);
        }
        else if (transform.position.z <= -playerGoalie.maxZ
            && ball.transform.position.z < transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, 0, -playerGoalie.maxZ);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, 0, ball.transform.position.z),
                speed / 1.75f * Time.deltaTime);
        }
    }
}
