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
    private int curBallSpeedIncrements;
    public int maxBallSpeedIncrements = 50;
    private Rigidbody rb;
    private GameManager gameManager;

    private Vector3 pausedVelo;
    private Vector3 pausedAngularVelo;
    private Vector3 startingVelo;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        // Reset Material to be white initially
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void Pause()
    {
        pausedVelo = rb.velocity;
        pausedAngularVelo = rb.angularVelocity;
        rb.isKinematic = true;
    }

    public void Unpause()
    {
        rb.velocity = pausedVelo;
        rb.angularVelocity = pausedAngularVelo;
        rb.isKinematic = false;
    }

    public void Jumpstart()
    {
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

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
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

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, transform.localScale.y, size);
    }
}
