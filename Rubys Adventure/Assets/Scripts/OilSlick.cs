using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
    public AudioClip collectedClip;

    public bool played = false;
    
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeSpeed(1);

            if (played == false)
            {
                controller.PlaySound(collectedClip);

                played = true;
            }
        }
    }
}
// Quinones added this script