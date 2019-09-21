using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SceneController SceneController {get; set;}
    void Start()
    {
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        SceneController = GameObject.Find("GameMaster").GetComponent<SceneController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Brick.OnTriggerEnter: entered. other={other}");
        SceneController.TriggerDispatcher(other);
    }

}
