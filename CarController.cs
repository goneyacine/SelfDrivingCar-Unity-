using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float movementSpeed = 2;
    //rotation rate per second in angles
    public float rotationRate = 1;
    //min and max angles in degrees 
    public Vector2 rotationRange = new Vector2(-45,45);
    private Rigidbody2D rb;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftArrow) && Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z + (rotationRate * Time.fixedDeltaTime))) <= Mathf.Sin(Mathf.Deg2Rad *rotationRange.y))
        transform.eulerAngles += new Vector3(0,0,rotationRate * Time.fixedDeltaTime);
        else if(Input.GetKey(KeyCode.RightArrow) && Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z - (rotationRate * Time.fixedDeltaTime))) >= Mathf.Sin(Mathf.Deg2Rad *rotationRange.x))
        transform.eulerAngles -= new Vector3(0,0,rotationRate * Time.fixedDeltaTime);
        
        rb.velocity = new Vector2(-movementSpeed * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z),movementSpeed * Mathf.Cos(Mathf.Deg2Rad *transform.eulerAngles.z));
    }
}
