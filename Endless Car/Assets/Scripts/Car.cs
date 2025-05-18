using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    float accelerationMultiplier = 3;
    float brakeMultiplier = 15;
    float steerMultiplier = 15;

    Vector2 input = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //accelerate
        if (input.y > 0)
            Accelerate();

        else
            rb.drag = 0.2f;

        //brake
        if (input.y < 0)
            Brake();

        Steer();
    }

    void Accelerate()
    {
        rb.drag = 0;
        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);

    }

    void Brake()
    {
        if(rb.velocity.z <= 0)
        {
            return;
        }

        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            rb.AddForce(rb.transform.right * steerMultiplier * input.x);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }
}
