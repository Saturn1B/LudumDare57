using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float yawRotationSpeed;
	[SerializeField] private float pitchRotationSpeed;
	[SerializeField] private float pitchLimitAngle;

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
			transform.eulerAngles += Vector3.up * yawRotationSpeed * Time.deltaTime;
		else
			transform.eulerAngles -= Vector3.up * yawRotationSpeed * Time.deltaTime;
	}

	public void RotatePitch(bool isBackward)
	{
		float targetXRotation = transform.eulerAngles.x;

		if (!isBackward)
			targetXRotation -= pitchRotationSpeed * Time.deltaTime;
		else if (isBackward)
			targetXRotation += pitchRotationSpeed * Time.deltaTime;

		if (targetXRotation > 180)
			targetXRotation -= 360;

		targetXRotation = Mathf.Clamp(targetXRotation, -pitchLimitAngle, pitchLimitAngle);
		transform.rotation = Quaternion.Euler(targetXRotation, transform.eulerAngles.y, transform.eulerAngles.z);
	}
}
