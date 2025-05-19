
using UnityEngine;


public class Car : MonoBehaviour
{

    [SerializeField] Rigidbody rb;


    [SerializeField] Transform gameModel;

    [SerializeField] ExplosionHandler explosionHandler;


    float accelerationMultiplier = 3;
    float brakeMultiplier = 15;
    float steerMultiplier = 5;
    float MaxSteerVelocit = 2;
    float MaxForwardVelocity = 30;
    

    bool isExploded = false;
    
    float startingPoint;
    float distanceTravelled = 0;
    public float DistanceTravelled => distanceTravelled;

    Vector2 input = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploded) return;

        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);

        distanceTravelled = transform.position.z - startingPoint;

        
    }

    
    private void FixedUpdate()
    {
        if (isExploded)
        {
            rb.drag = rb.velocity.z * 0.1f;
            rb.drag = Mathf.Clamp(rb.drag, 1.5f, 10);

            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));
            return;
        }

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

            //rb.velocity = new Vector3(normalizedX * MaxSteerVelocit, 0, rb.velocity.z);

            Vector3 targetVelocity = new Vector3(normalizedX * MaxSteerVelocit, 0, rb.velocity.z);
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2);

        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        //inputVector.Normalize();

        //input = inputVector;

        input = Vector2.ClampMagnitude(inputVector, 1f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");

        if (explosionHandler == null)
        {
            Debug.LogError("ExplosionHandler is not assigned!");
            return;
        }

        Vector3 velocity = rb.velocity;

        explosionHandler.Explode(velocity * 45);

        isExploded = true;
    }

    
}
