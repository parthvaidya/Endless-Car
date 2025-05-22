
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class Car : MonoBehaviour
{

    [SerializeField] Rigidbody rb;


    [SerializeField] Transform gameModel;

    [SerializeField] ExplosionHandler explosionHandler;


    [SerializeField] float accelerationIncreaseRate = 0.1f;
    [SerializeField] float maxSpeedIncreaseRate = 0.5f;

    [SerializeField] float maxAccelerationLimit = 15f;
    [SerializeField] float maxVelocityLimit = 150f;

    [SerializeField] private TextMeshProUGUI crashText;


    float accelerationMultiplier = 6;
    float brakeMultiplier = 15;
    float steerMultiplier = 5;
    float MaxSteerVelocity = 2;
    float MaxForwardVelocity = 60;
    

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

        IncreaseSpeedOverTime();
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
        {
            SoundManager.Instance.PlayBrake();
            Brake();
        }
            


        Steer();

        if (rb.velocity.z <= 0)
            rb.velocity = Vector3.zero;

        if (rb.velocity.z > 5)
            SoundManager.Instance.PlayEngineRunning();
        else
            SoundManager.Instance.PlayEngineIdle();
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
            float normalizedX = rb.velocity.x / MaxSteerVelocity;

            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            //rb.velocity = new Vector3(normalizedX * MaxSteerVelocit, 0, rb.velocity.z);

            Vector3 targetVelocity = new Vector3(normalizedX * MaxSteerVelocity, 0, rb.velocity.z);
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2);

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

        //input = Vector2.ClampMagnitude(inputVector, 1f);
    }

    IEnumerator SlowDownTime()
    {
        while(Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);


        while(Time.timeScale < 1.0f)
        {
            Time.timeScale += Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
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

        explosionHandler.Explode(velocity * 5);

        isExploded = true;

        GameManager.Instance.CarExploded();

        SoundManager.Instance.PlayCarCrash();

        

        StartCoroutine(SlowDownTime());

        if (crashText != null)
            SoundManager.Instance.PlayCarDeath();
            crashText.gameObject.SetActive(true);
    }

    public float GetCurrentSpeed()
    {
      
        return rb.velocity.magnitude * 3.6f;
    }
    void IncreaseSpeedOverTime()
    {
        if (accelerationMultiplier < maxAccelerationLimit)
        {
            accelerationMultiplier += accelerationIncreaseRate * Time.deltaTime;
        }

        if (MaxForwardVelocity < maxVelocityLimit)
        {
            MaxForwardVelocity += maxSpeedIncreaseRate * Time.deltaTime;
        }
    }


}
