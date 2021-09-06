using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
	[SerializeField]
	GameObject[] enemyGameObjects;
    [SerializeField]
    public float distance, height, heightDamping, rotationDamping,transitionSpeed;
	private bool endgame;
    public GameObject trackedObject;
    private GameObject cachedObject;
	private Transform myTransform;
	private GameObject cachedTarget;
	private Transform targetTransform;
	int aiCount;

    // Start is called before the first frame update
    void Start()
    {
        //SET CAMERA
        if (cachedObject != gameObject)
		{
			cachedObject = gameObject;
			myTransform = gameObject.transform;
		}
        //SET TRACKED OBJECT
        if (cachedTarget != trackedObject)
		{
			cachedTarget = trackedObject;
			targetTransform = trackedObject.transform;
		}
    }
    void FixedUpdate()
    {
        // Calculate the current rotation angles
		var wantedRotationAngle = targetTransform.eulerAngles.y;
		var wantedHeight = targetTransform.position.y + height;
		var currentRotationAngle = myTransform.eulerAngles.y;
		var currentHeight = myTransform.position.y;
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		// Damp the height
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		myTransform.position = targetTransform.position;
		myTransform.position -= currentRotation * Vector3.forward * distance;
		// Set the height of the camera
		//myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);
        myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);
		// Always look at the target
		//myTransform.LookAt(targetTransform);
    }
	//Switch to other players
	public void SwitchTarget()
	{
		targetTransform = enemyGameObjects[aiCount].transform;
		aiCount++;
		if(aiCount>3)
		aiCount=0;
	}
}
