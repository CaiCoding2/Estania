using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour {

    public float Speed;
  

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
        

        if (transform.position.y > 10)
        {
            transform.position = new Vector3(transform.position.x, -11f, transform.position.z);
        }

    }
}
