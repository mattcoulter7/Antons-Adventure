using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprietRotator : MonoBehaviour
{

    private Rigidbody2D rb;
    public Rigidbody2D mainObject;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mainObject.transform.position.x, mainObject.transform.position.y, transform.position.z);
        Vector2 pos = new Vector2 (transform.position.x, transform.position.y);


        // Determine which direction to rotate towards
        Vector2 targetDirection = mainObject.velocity - pos;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        
    }
}
