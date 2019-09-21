using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed {get; set;}= 1.5f; 
    private Vector3 dir = new Vector3();
    public Vector3 Velocity {get; set;}= new Vector3();
    // private GameObject paddle;
    // private SceneController sceneController;
    private SceneController SceneController {get; set;}
    // protected static SphericalPaddle SphericalPdl {get; private set;}
    void Start()
    // void Awake()
    // public void Init()
    {
        // Debug.Log("Ball.Init: entered");
        // paddle = GameObject.Find("spherical_paddle_proto");
        // Debug.Log($"velocity={Velocity}");
        // sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        SceneController = GameObject.Find("GameMaster").GetComponent<SceneController>();
        // var sphericalPaddle_go = GameObject.Find("SphericalPaddle"); 
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddleProto>();
        // SphericalPdl = GameObject.FindWithTag("SphericalPaddle").GetComponent<SphericalPaddle>();

        init();
        Utils.DrawLine(Vector3.zero, Velocity, Color.green, 30.0f);
        // Utils.DrawLine(Vector3.zero, new Vector3(1, 1, 1), Color.blue, 30.0f);
    }

    void Update()
    {
        Vector3 posDelta = Time.deltaTime * Velocity;
        transform.position += posDelta;
        
    }

    public void init() {
        // transform.position = new Vector3(0, 2, 5);

        // dir = (paddle.transform.position - transform.position).normalized;
        // Velocity = dir *= speed;

        // Debug.Log("Ball.init: now calling sceneController.initBall");
        // sceneController.initBall();
        // Debug.Log("SceneController.initBall: entered");
        float rndOffset = SceneController.SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SceneController.SphericalPaddle.transform.position.x + rndOffset; 
        var sp = SceneController.SphericalPaddle;
        Vector3 tgtPoint = new Vector3(tgtPointX, sp.transform.position.y, sp.transform.position.z);

        transform.position = new Vector3(0, 2, 5);

        var dir = (tgtPoint - transform.position).normalized;
        Velocity = dir *= speed;
        // Debug.Log($"SceneController.initBall: Velocity={Velocity}");
    }
    public void PaddleHit(Vector3 angleDelta) {
        // Debug.Log($"Ball.PaddleHit: entered");

        // velocity.x = -velocity.x;
        // velocity.z = -velocity.z;
        // Debug.Log($"Ball.PaddleHit: magnitude vel.pre={Velocity.magnitude}");
        // Velocity = new Vector3(
        //     (Velocity.x + angleDelta.x), 
        //     Velocity.y + angleDelta.y, 
        //     -(Velocity.z + angleDelta.z));
        // Debug.Log($"Ball.PaddleHit: magnitude vel.post={Velocity.magnitude}");
        Velocity = new Vector3(
            (-Velocity.x + angleDelta.x), 
            -(Velocity.y + angleDelta.y), 
            -(Velocity.z + angleDelta.z));
    }

    // Send the ball back towards the game center
    public void BoundaryHit(SphericalPaddle paddle, Vector3 hitPoint) {
        // Debug.Log($"Ball.BoundaryHit: paddle.pos={paddle.transform.position}");
        // Debug.Log($"Ball.BoundaryHit: hitPoint={hitPoint}");
        // dir = -1 * (hitPoint - paddle.transform.position).normalized;
        // dir = -1 * (hitPoint - paddle.Pivot.position).normalized;
        dir = -1 * (transform.position - paddle.Pivot.position).normalized;
        // dir.y -= 2.0f;
        Velocity = dir *= speed;

        // Utils.DrawLine(hitPoint, hitPoint + 3.0f * Velocity, Color.green, 60);
        Utils.DrawLine(hitPoint, hitPoint + 3.0f * Velocity, Color.green, 60);

    }
    private void OnTriggerEnter(Collider other)
    {
        SceneController.TriggerDispatcher(other);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"Ball.OnTrigerEnter: other.tag={other.tag}");
    //     if (other.tag == "SphericalPaddle") {
    //         Vector3 pc = SphericalPdl.transform.position;
    //         Vector3 bounceAngleDelta = other.transform.position - pc;
    //         BallPaddleHit(bounceAngleDelta);
    //     }
    // }
}
