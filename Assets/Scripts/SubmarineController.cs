using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float yawRotationSpeed;
	[SerializeField] private float pitchRotationSpeed;

	public void Move(bool isBackward)
	{
		if(!isBackward)
			transform.position += transform.forward * movementSpeed * Time.deltaTime;
		else
			transform.position -= transform.forward * movementSpeed * Time.deltaTime;
	}

	public void RotateYaw(bool isBackward)
	{
		if (!isBackward)
			transform.Rotate(Vector3.up * yawRotationSpeed * Time.deltaTime);
		else
			transform.Rotate(Vector3.down * yawRotationSpeed * Time.deltaTime);
	}

	public void RotatePitch(bool isBackward)
	{
		if (!isBackward)
			transform.Rotate(Vector3.right * pitchRotationSpeed * Time.deltaTime);
		else
			transform.Rotate(Vector3.left * pitchRotationSpeed * Time.deltaTime);
	}
}
