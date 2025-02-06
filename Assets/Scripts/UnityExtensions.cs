using UnityEngine;

public static class UnityExtensions
{
    public static void RotateLerp(this Rigidbody2D rb, float rotationSpeed, float angle, float dt)
    {
        rb.SetRotation(Quaternion.Lerp(
            rb.transform.rotation,
            Quaternion.Euler(new Vector3(0f,0f,angle)),
            rotationSpeed*dt)); 
    }
}