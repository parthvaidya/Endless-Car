using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    [SerializeField] private ExplosionHandler explosionHandler;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        // Trigger explosion!
        if (explosionHandler != null)
        {
            explosionHandler.Explode(collision.relativeVelocity);
        }
        else
        {
            Debug.LogWarning("ExplosionHandler reference is missing!");
        }
    }
}
