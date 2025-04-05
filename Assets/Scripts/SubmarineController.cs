using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float yawRotationSpeed;
	[SerializeField] private float pitchRotationSpeed;
	[SerializeField] private float pitchLimitAngle;

	private Rigidbody rb;

	private float currentYaw = 0f;
	private float currentPitch = 0f;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		currentYaw = transform.eulerAngles.y;
		currentPitch = transform.eulerAngles.x;
	}

	public void Move(bool isBackward)
	{
		//if (!isBackward)
		//	transform.position += transform.forward * movementSpeed * Time.deltaTime;
		//else
		//	transform.position -= transform.forward * movementSpeed * Time.deltaTime;

		Vector3 direction = isBackward ? -transform.forward : transform.forward;
		rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	public void RotateYaw(bool isBackward)
	{
		//if (!isBackward)
		//	transform.eulerAngles += Vector3.up * yawRotationSpeed * Time.deltaTime;
		//else
		//	transform.eulerAngles -= Vector3.up * yawRotationSpeed * Time.deltaTime;

		float yawDelta = (isBackward ? -yawRotationSpeed : yawRotationSpeed) * Time.deltaTime;
		currentYaw += yawDelta;
		UpdateRotation();
	}

	public void RotatePitch(bool isBackward)
	{
		//float targetXRotation = transform.eulerAngles.x;

		//if (!isBackward)
		//	targetXRotation -= pitchRotationSpeed * Time.deltaTime;
		//else if (isBackward)
		//	targetXRotation += pitchRotationSpeed * Time.deltaTime;

		//if (targetXRotation > 180)
		//	targetXRotation -= 360;

		//targetXRotation = Mathf.Clamp(targetXRotation, -pitchLimitAngle, pitchLimitAngle);
		//transform.rotation = Quaternion.Euler(targetXRotation, transform.eulerAngles.y, transform.eulerAngles.z);

		float pitchDelta = (isBackward ? pitchRotationSpeed : -pitchRotationSpeed) * Time.deltaTime;
		currentPitch += pitchDelta;

		// Clamp pitch
		if (currentPitch > 180) currentPitch -= 360;
		currentPitch = Mathf.Clamp(currentPitch, -pitchLimitAngle, pitchLimitAngle);

		UpdateRotation();
	}

	private void UpdateRotation()
	{
		Quaternion newRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
		rb.MoveRotation(newRotation);

		rb.angularVelocity = Vector3.zero;
	}

	public void StopAllMovement()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
}
