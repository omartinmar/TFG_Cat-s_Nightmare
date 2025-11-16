using UnityEngine;


public class WaterRise : MonoBehaviour
{
    [SerializeField] private float tideSpeed;
    [SerializeField] private Rigidbody2D body;


    void FixedUpdate()
    {
        
        body.linearVelocity = new Vector3(0, tideSpeed * Time.fixedDeltaTime,0);
        
    }
}
