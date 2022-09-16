using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGoalie : MonoBehaviour
{
    public string controllerAxis;
    public float speed;
    public float maxZ = 15;

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(0, 0, Input.GetAxisRaw(controllerAxis)) * Time.deltaTime * speed;
        if (transform.position.z + input.z > maxZ)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                maxZ);
        }
        else if (transform.position.z + input.z < -maxZ)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                -maxZ);
        }
        else
        {
            transform.position += input;
        }
    }
}
