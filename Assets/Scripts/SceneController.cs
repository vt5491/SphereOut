using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // public static SphericalPaddleProto SphericalPdl {get; private set;}
    public static SphericalPaddle SphericalPaddle {get; private set;}
    public static Ball Ball {get; private set;}
    // void Start()
    void Awake()
    {
        // Debug.Log("SceneController.Awake: entered");
        // var sphericalPaddle_go = GameObject.Find("spherical_paddle_proto"); 
        var sphericalPaddle_go = GameObject.Find("SphericalPaddle"); 
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddleProto>();
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddle>();
        SphericalPaddle = GameObject.FindWithTag("SphericalPaddle").GetComponent<SphericalPaddle>();

        Ball = GameObject.Find("Ball").GetComponent<Ball>();
        
    }

    void Update()
    {
        
    }

    public void DoItController() {
        Debug.Log("SceneController.DoItController: about to call sphericalPaddle.DoIt");
        // SphericalPaddle.DoIt();
    }

    public void initBall() {
        // point the velocity to somewhere within the spherical paddle's
        // radius;
        // Debug.Log("SceneController.initBall: entered");
        float rndOffset = SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SphericalPaddle.transform.position.x + rndOffset; 
        var sp = SphericalPaddle;
        Vector3 tgtPoint = new Vector3(tgtPointX, sp.transform.position.y, sp.transform.position.z);

        Ball.transform.position = new Vector3(0, 2, 5);

        var dir = (tgtPoint - Ball.transform.position).normalized;
        Ball.Velocity = dir *= Ball.speed;
        // Debug.Log($"SceneController.initBall: Velocity={Ball.Velocity}");
    }

    // public void BallPaddleHit(Collider other) {
    //     if (other.tag == "Ball") {
    //         // SphericalPaddle sp = SphericalPaddle;
    //         // Vector3 pc = SphericalPdl.GetPaddleCtr();
    //         Vector3 pc = SphericalPdl.transform.position;
    //         Vector3 bounceAngleDelta = other.transform.position - pc;
    //         Ball.BallPaddleHit(bounceAngleDelta);
    //     }
    // }
    // private void OnTriggerEnter(Collider other)
    public void TriggerDispatcher(Collider other)
    {
        Debug.Log($"SceneController.OnTrigerEnter: other.tag={other.tag}, other.pos={other.transform.position}");
        if (other.tag == "SphericalPaddle") {
            Vector3 paddleCenter = SphericalPaddle.transform.position;
            // Vector3 pc = SphericalPaddle.Pivot.transform.position;
            Vector3 bounceAngleDelta = other.transform.position - paddleCenter;
            Ball.PaddleHit(bounceAngleDelta);
        }
        else if ( other.transform.parent.tag == "Boundary" && other.transform.position != null ) {
            Ball.BoundaryHit(SphericalPaddle, other.transform.position);
        }
        else if ( other.transform.parent.tag == "Brick") {
            Ball.BoundaryHit(SphericalPaddle, other.transform.position);
            Debug.Log("SceneController.TriggerDispatcher: Brick Hit");
        }
    }


}
