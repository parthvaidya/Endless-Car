using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [SerializeField] GameObject originalObject;

    Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        Debug.Log("Found rigidbodies: " + rigidbodies.Length);

    }
    
    void Start()
    {
        //Explode(Vector3.forward);
    }



    public void Explode(Vector3 externalForce)
    {
        originalObject.SetActive(false);

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.transform.parent = null;

            rb.GetComponent<Collider>().enabled = true;

            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.AddForce(Vector3.up * 50 + externalForce* 0.2f, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

        }
    }




}
