using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{

    public static float GetImpactForce(this Collision2D collision)
    {
        float impulse = 0f;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            impulse += (contact.normalImpulse);
            //Debug.Log(contact.collider.name);
        }

        return impulse / Time.fixedDeltaTime;
    }
}
