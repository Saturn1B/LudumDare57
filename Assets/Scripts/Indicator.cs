using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
	[SerializeField] private Transform subTransform;
	[SerializeField] private Vector3 direction;
	[SerializeField] private float maxDistance;
	//[SerializeField] private Image indicatorLight;
	//[SerializeField] private Color colorOff, colorOn;
	[SerializeField] private Material matOff, matOn;
	[SerializeField] private MeshRenderer indicatorLight;
	[SerializeField] private float baseBlinkSpeed;
	public float detectionScale = 1f;
	[SerializeField] private LayerMask layerMask;
	private float blinkSpeed;

	bool isBlinking = false;

	//private void Update()
	//{
	//	if(detector.colliderDetected.Count > 0 && !isBlinking)
	//	{
	//		isBlinking = true;
	//		StartCoroutine(Blink());
	//	}

	//	if (detector.colliderDetected.Count <= 0 && isBlinking)
	//	{
	//		isBlinking = false;
	//	}

	//	if (isBlinking)
	//	{
	//		float minDistance = float.MaxValue;
	//		Collider closestCol = null;
	//		foreach (var col in detector.colliderDetected)
	//		{
	//			if (col == null) continue;

	//			Vector3 closestPoint = col.ClosestPoint(subTransform.position);
	//			float distance = Vector3.Distance(subTransform.position, closestPoint);

	//			Vector3 dir = (closestPoint - subTransform.position).normalized;
	//			Debug.DrawRay(subTransform.position, dir * 10, colorOn, 1);

	//			if (distance < minDistance)
	//			{
	//				minDistance = distance;
	//				closestCol = col;
	//			}
	//		}
	//		blinkSpeed = baseBlinkSpeed * minDistance;
	//	}
	//}

	private void Update()
	{
		Vector3 origin = subTransform.position;

		Vector3 halfExtents = (new Vector3(5, 5, 7) * detectionScale) / 2;
		//float maxDistance = 10;

		//Quaternion orientation = Quaternion.LookRotation(transform.TransformDirection(direction), transform.up);

		DebugDrawBoxCast(origin, halfExtents, transform.TransformDirection(direction), subTransform.rotation, maxDistance, Color.blue);

		if(Physics.BoxCast(origin, halfExtents, transform.TransformDirection(direction), out RaycastHit hit, subTransform.rotation, maxDistance, layerMask))
		{
			float t = 1f - (hit.distance / maxDistance);
			blinkSpeed = baseBlinkSpeed / t;
			blinkSpeed = Mathf.Clamp(blinkSpeed, 0.01f, 5);
			if (!isBlinking)
			{
				isBlinking = true;
				StartCoroutine(Blink());
			}
		}
		else
		{
			if (isBlinking)
			{
				isBlinking = false;
				StopAllCoroutines();
				indicatorLight.material = matOff;
			}
		}
	}

	private IEnumerator Blink()
	{
		indicatorLight.material = matOn;

		yield return new WaitForSeconds(blinkSpeed / 10);

		indicatorLight.material = matOff;

		yield return new WaitForSeconds(blinkSpeed / 10);

		if(isBlinking)
			StartCoroutine(Blink());
	}

	void DebugDrawBoxCast(Vector3 origin, Vector3 halfExtents, Vector3 direction, Quaternion rotation, float distance, Color color)
	{
		// Move to end point
		Vector3 castCenter = origin + direction.normalized * distance;

		// Get corner offsets from center
		Vector3 right = rotation * Vector3.right * halfExtents.x;
		Vector3 up = rotation * Vector3.up * halfExtents.y;
		Vector3 forward = rotation * Vector3.forward * halfExtents.z;

		// Compute all 8 corners at start and end
		Vector3[] startCorners = new Vector3[8];
		Vector3[] endCorners = new Vector3[8];

		int i = 0;
		for (int x = -1; x <= 1; x += 2)
		{
			for (int y = -1; y <= 1; y += 2)
			{
				for (int z = -1; z <= 1; z += 2)
				{
					Vector3 cornerOffset = x * right + y * up + z * forward;
					startCorners[i] = origin + cornerOffset;
					endCorners[i] = castCenter + cornerOffset;
					i++;
				}
			}
		}

		// Draw start and end box edges
		for (int j = 0; j < 4; j++)
		{
			Debug.DrawLine(startCorners[j], startCorners[(j + 1) % 4], color);              // bottom
			Debug.DrawLine(startCorners[j + 4], startCorners[4 + ((j + 1) % 4)], color);    // top
			Debug.DrawLine(startCorners[j], startCorners[j + 4], color);                    // vertical

			Debug.DrawLine(endCorners[j], endCorners[(j + 1) % 4], color);
			Debug.DrawLine(endCorners[j + 4], endCorners[4 + ((j + 1) % 4)], color);
			Debug.DrawLine(endCorners[j], endCorners[j + 4], color);

			Debug.DrawLine(startCorners[j], endCorners[j], color);
			Debug.DrawLine(startCorners[j + 4], endCorners[j + 4], color);
		}
	}
}
