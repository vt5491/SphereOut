using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private GameObject ball_go;
    private Ball ball;
    // void Start()
    void Awake()
    {
        ball_go = GameObject.FindGameObjectWithTag("Ball");
        ball = ball_go.GetComponent<Ball>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Wall.OnTrigerEnter: other.tag={other.tag}");
        // if (other.tag == "Ball") {
        //     ball.WallHit(other.transform.position);
        //     // mark it with a cube for debug purposes
        //     GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //     tmp.transform.position = other.transform.position;
        //     tmp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // }
    }
}
