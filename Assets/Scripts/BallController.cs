using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float minXVelocityOnStart = 120;
    public float maxXVelocityOnStart = 150;
    public float minZVelocityOnStart = 50;
    public float maxZVelocityOnStart = 75;
    public float ballSpeedIncrementOnCollide = 1.02f;
    public int curBallSpeedIncrements;
    public int maxBallSpeedIncrements = 50;
    private float speed = 1f;
    private bool paused;
    private Vector3 startingVelo;

    private int xDirection = 1;
    private int zDirection = 1;

    public Material ballMat;
    private Rigidbody rb;
    private GameManager gameManager;

    private void OnApplicationQuit()
    {
        ballMat.SetColor("_EmissionColor", Color.white);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void Pause()
    {
        paused = true;
        rb.isKinematic = true;
    }

    public void Unpause()
    {
        paused = false;
        rb.isKinematic = false;
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public void Jumpstart()
    {
        curBallSpeedIncrements = 0;
        float xVelo = Random.Range(minXVelocityOnStart, maxXVelocityOnStart);
        if (Random.Range(0, 2) == 0)
        {
            xVelo *= -1;
        }
        float zVelo = Random.Range(minZVelocityOnStart, maxZVelocityOnStart);
        if (Random.Range(0, 2) == 0)
        {
            zVelo *= -1;
        }
        startingVelo = new Vector3(xVelo, 0, zVelo);
        rb.velocity = startingVelo;
    }

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, transform.localScale.y, size);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            zDirection *= -1;
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            xDirection *= -1;
        }

        // Ball get's faster every collision (up to a max)
        if (curBallSpeedIncrements < maxBallSpeedIncrements)
        {
            rb.velocity = rb.velocity * ballSpeedIncrementOnCollide;
            curBallSpeedIncrements++;
        }

        // Scoring
        if (collision.gameObject.CompareTag("LeftGoal"))
        {
            gameManager.Player2Scored();
        }
        else if (collision.gameObject.CompareTag("RightGoal"))
        {
            gameManager.Player1Scored();
        }
    }

    private void Update()
    {
        if (paused)
            rb.velocity = Vector3.zero;
        else
            rb.velocity = new Vector3(
            startingVelo.x * xDirection * speed, 0,
            startingVelo.z * zDirection * speed);
    }
}
