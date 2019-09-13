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
    public Transform pivot;
    public SphericalCoordinates sc;

    private float lastPolar;
    private float lastElevation;
    public float Radius {get; private set;} 
    void Start()
    {
        Radius = 1.0f;
        // sc = new SphericalCoordinates(transform.position, 3f, 10f, 0f, Mathf.PI * 2f, 0f, Mathf.PI / 4f);
        sc = new SphericalCoordinates(transform.position, 0f, 6f, 
            -Mathf.PI * 1f, Mathf.PI / 1f,  
            -Mathf.PI / 1f, Mathf.PI / 1f);
        sc.loopElevation = true;
        sc.loopPolar = true;
        // Initialize position
        transform.position = sc.toCartesian + pivot.position;
        // transform.position = sc.toCartesian;
        // transform.Rotate(0, Mathf.PI / 4f, 0);

        // transform.Rotate(0, (-sc.polar + Mathf.PI / 2f) * Mathf.Rad2Deg, 0);
        // var a = (-sc.polar + Mathf.PI) * (Mathf.PI / 2f);
        this.lastPolar = sc.polar;
        this.lastElevation = sc.elevation;

        // transform.RotateAround(Vector3.zero, Vector3.up, 180f);
        // transform.RotateAroundLocal( Vector3.up, 180f);
        // transform.Rotate( Vector3.up, 180f);
        Debug.Log($"Start: forward={transform.forward}");

            // transform.LookAt( pivot.transform , Vector3.up );
    }

    void Update()
    {
        // transform.Rotate( Vector3.up, 180f);
        // transform.position = sc.Rotate( h * rotateSpeed * Time.deltaTime, v * rotateSpeed * Time.deltaTime ).toCartesian + pivot.position;
        var axis = movePaddle.GetAxis(SteamVR_Input_Sources.RightHand);

        // float polarAngleDelta = 0.0f;
        // float elevationAngleDelta = 0.0f;
        // horizontal plane
        // if (Mathf.Abs(axis.x) > Mathf.Abs(axis.y))
        // {
        //     transform.position = sc.Rotate( -axis.x * rotateSpeed * Time.deltaTime, 0 ).toCartesian + pivot.position;
        // }
        // else if (Mathf.Abs(axis.y) > Mathf.Abs(axis.x))
        // {
        //     transform.position = sc.Rotate( 0, -axis.y * rotateSpeed * Time.deltaTime ).toCartesian + pivot.position;
        // }
        var cx = axis.x;
        var cy = axis.y;
 
		var h = cx*cx > cy*cy ? cx : cy;
		// var v = cy*cy > mv*mv ? kv : mv;
 
		// if( h*h > .1f || v*v > .1f )
		if( h*h > .1f )
        {
            transform.position = sc.Rotate( cx * rotateSpeed * Time.deltaTime, cy * rotateSpeed * Time.deltaTime ).toCartesian + pivot.position;

            // transform.Rotate(axis.y, axis.x + Mathf.PI / 2f, 0);
            // transform.Rotate(axis.y, axis.x, 0);
            /* 
            float polarDelta = sc.polar - this.lastPolar;
            Debug.Log($"polarDelta={polarDelta}");
            transform.Rotate(0, -polarDelta * Mathf.Rad2Deg, 0);
            this.lastPolar = sc.polar;

            float elevationDelta = sc.elevation - this.lastElevation;
            transform.Rotate(0, -polarDelta * Mathf.Rad2Deg, 0);
            this.lastPolar = sc.polar;
            */

            // transform.LookAt( pivot.position, Vector3.up );
            // transform.forward = -transform.forward;
            // transform.LookAt( pivot.position + transform.forward, Vector3.up );
            // transform.LookAt( pivot.transform , Vector3.up );
            // transform.LookAt( pivot.transform.forward , Vector3.up );
            // if (transform.forward.z < 0) {
            //     // transform.forward.z = -transform.forward.z;
            //     transform.forward = new Vector3(transform.forward.x, transform.forward.y, -transform.forward.z);
            //     Debug.Log($"SphericalPaddle.Update.if: forward={transform.forward}");
            // }
            transform.LookAt( pivot.transform , Vector3.up );
            transform.Rotate( Vector3.up, 180f);

        }

        // transform.position = sc.Rotate( -axis.x * rotateSpeed * Time.deltaTime, -axis.y * rotateSpeed * Time.deltaTime ).toCartesian;
        // transform.position = sc.Rotate( -axis.x * rotateSpeed * Time.deltaTime, 0 ).toCartesian + pivot.position;

        // transform.position = sc.Rotate( 0, -axis.y * rotateSpeed * Time.deltaTime ).toCartesian + pivot.position;

        // transform.position = sc.Rotate( -axis.x * rotateSpeed * Time.deltaTime, -axis.y * rotateSpeed * Time.deltaTime ).toCartesian + pivot.position;


        // Debug.Log($"sc={sc.ToString()}");
        // Debug.Log($"SphericalPaddle.Update: forward={transform.forward}");

        if (Input.GetKeyDown("space"))
        {
            transform.position = sc.Rotate(0f, 0f).toCartesian;
        }
        
    }
}
