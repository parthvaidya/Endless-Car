using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField] Transform gameModel;
    float accelerationMultiplier = 3;
    float brakeMultiplier = 15;
    float steerMultiplier = 5;
    float MaxSteerVelocit = 2;
    float MaxForwardVelocity = 30;

    Vector2 input = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);
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

        if (rb.velocity.z <= 0)
            rb.velocity = Vector3.zero;
    }

    void Accelerate()
    {
        rb.drag = 0;
        if (rb.velocity.z >= MaxForwardVelocity)
            return;

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
            float speedBaseSteerLimit = rb.velocity.z / 5.0f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(rb.transform.right * steerMultiplier * input.x * speedBaseSteerLimit);
            float normalizedX = rb.velocity.x / MaxSteerVelocit;

            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            rb.velocity = new Vector3(normalizedX * MaxSteerVelocit, 0, rb.velocity.z);

        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }
}
