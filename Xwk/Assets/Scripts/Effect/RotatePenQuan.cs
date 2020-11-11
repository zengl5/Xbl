using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePenQuan : MonoBehaviour {
  public  float rotationSpeedX=90;
  public float rotationSpeedY = 0;
  public float rotationSpeedZ = 0;
  private Vector3 rotationVector;
	// Use this for initialization
	void Start () {
        rotationVector = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationVector * Time.deltaTime);
	}
}
