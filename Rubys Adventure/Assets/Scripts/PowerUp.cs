using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public AudioClip collectedClip;

    public int PowerUpTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
                controller.PowerUp(PowerUpTime);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
        }
    }
}
// Leana added this script