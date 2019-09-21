using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SphericalPaddle : MonoBehaviour
{
    [Header("Action Mappings")]
    public SteamVR_Action_Vector2 movePaddle;
    [Header("General Parms")]
    public float rotateSpeed = 1f;
    // public Transform pivot {get; set;}
    public Transform Pivot;
    public SphericalCoordinates sc;
    private SphericalPaddleInternal sphericalPaddleInternal; 

    // private float lastPolar;
    // private float lastElevation;
    // public float Radius {get; private set;} 
    public float Radius {get; set;} 
    public float paddleHeight {get; set;}
    public float paddleWidth {get; set;}
    void Start()
    {
        Debug.Log($"SphericalPaddle.Start: entered");
        Radius = 1.0f;
        sc = new SphericalCoordinates(transform.position, 0f, 6f, 
            -Mathf.PI * 1f, Mathf.PI / 1f,  
            -Mathf.PI / 1f, Mathf.PI / 1f);
        sc.loopElevation = true;
        sc.loopPolar = true;
        // Initialize  position
        transform.position = sc.toCartesian + Pivot.position;
        // this.lastPolar = sc.polar;
        // this.lastElevation = sc.elevation;

        // Debug.Log($"Start: forward={transform.forward}");
        sphericalPaddleInternal = new SphericalPaddleInternal();
        // Debug.Log($"SphericalPaddle.Start: sphericalPaddleInternal.ThetaOut={sphericalPaddleInternal.ThetaOut()}");
        // paddle width and height are just the size of the bounding box
        var boundingBox = this.GetComponent<BoxCollider>();
        paddleWidth = boundingBox.size.x;
        paddleHeight = boundingBox.size.y;

        Debug.Log($"SphericalPaddle: pwidth={paddleWidth}, ph={paddleHeight}");

    }

    void Update()
    {
        var axis = movePaddle.GetAxis(SteamVR_Input_Sources.RightHand);
        var cx = axis.x;
        var cy = axis.y;
 
		var h = cx*cx > cy*cy ? cx : cy;
 
		if( h*h > .1f )
        {
            transform.position = sc.Rotate( cx * rotateSpeed * Time.deltaTime, cy * rotateSpeed * Time.deltaTime ).toCartesian + Pivot.position;

            transform.LookAt( Pivot.transform , Vector3.up );
            transform.Rotate( Vector3.up, 180f);

        }

        if (Input.GetKeyDown("space"))
        {
            transform.position = sc.Rotate(0f, 0f).toCartesian;
        }
        
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"SphericalPaddle.OnTrigerEnter: other.tag={other.tag}");
    // }

    // public void doIt() {
    //     Debug.Log($"SphericalPaddle.doIt: Radius={Radius}, pos={transform.position]");
    // }
    public Vector3 VelocityOut(Vector3 collisionPoint, Vector3 velocityIn, string strategy) {
        Debug.Log($"SphericalPaddle.VelocityOut: Radius={Radius}, pos={transform.position}");
        // return 1f;
        var r = Radius;
        // Note: Vert and Horiz are "logical" not physical.  Thus vert is always "up" and horizontial is
        // "sideways" even if the paddle is up at the poles.
        // delta is distance from the center.
        // convert collision point into spherical coords.
        // sc = new SphericalCoordinates(collisionPoint, 0f, 6f, 
        //     -Mathf.PI * 1f, Mathf.PI / 1f,  
        //     -Mathf.PI / 1f, Mathf.PI / 1f);
        // SphericalCoordinates collisonPointSc = sc.Clone();
        // SphericalCoordinates collisonPointSc = Instantiate(sc);
        // collisonPointSc = sc.FromCartesian(collisionPoint - Pivot.position);
        SphericalCoordinates collisonPointSc = new SphericalCoordinates(collisionPoint - Pivot.position);
        Debug.Log($"sc={sc.ToString()}");
        Debug.Log($"cpSc={collisonPointSc.ToString()}");
        // SphericalCoordinates collisionPointSc = new SphericalCoordinates();
        // collisionPointSc.SetRadius(sc.radius);
        // collisionPointSc.SetRadius(sc.radius);
        // float deltaHeight = paddleHeight / 2 - 
        var deltaElevationAngle = collisonPointSc.elevation - sc.elevation;

        var paddleHeightAngle = paddleHeight / (Radius *  Mathf.PI / 2.0f);
        Debug.Log($"deltaElevation={deltaElevationAngle * Mathf.Rad2Deg}, paddleHeightAngle={paddleHeightAngle * Mathf.Rad2Deg}");
        // 90 for tips, 0 at center
        var vertGapAngle = (Mathf.PI / 2.0f) * deltaElevationAngle / paddleHeightAngle; 
        Debug.Log($"vertGapAngle={vertGapAngle * Mathf.Rad2Deg}");

        return new Vector3(0,0,0);

    }
}

public class SphericalPaddleInternal {
    // public float ThetaOut() {
    //     return 1f;
    // }
}
