using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMovement : MonoBehaviour {

    int index = 0;
    public int totallevels = 2;
    public float yOffset = 1f;

    void Start() {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (index < totallevels - 1)
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            if (index == 0)
            {
                SceneManager.LoadScene("Menu");
            }
            else if (index == 1) {
                Debug.Log("You Pressed the Quit Button");
                Application.Quit();
            }




        }
    }
}
