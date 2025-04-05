using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float loseTargetDistance;
    [SerializeField] private float obstacleAvoidanceRange;
    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private Vector3 zoneCenter;
    [SerializeField] private Vector3 zoneSize;

    private Vector3 wanderDirection;
    private Transform player;
    private enum State { Wandering, Chasing}
    private State currentState = State.Wandering;

	private Vector3 currentDirection;

	private void Start()
	{
		currentDirection = transform.forward;

		wanderDirection = Random.onUnitSphere;
		player = GameObject.FindGameObjectWithTag("Submarine").transform;
	}

	private void Update()
	{
		StayInZone();

		switch (currentState)
		{
			case State.Wandering:
				Wander();
				DetectPlayer();
				break;
			case State.Chasing:
				Chase();
				LosePlayerCheck();
				break;
		}

		AvoidObstacles();
		MoveForward();
	}

	void Wander()
	{
		wanderDirection = Vector3.Slerp(wanderDirection, Random.onUnitSphere, Time.deltaTime * .5f);
		RotateTowards(transform.position + wanderDirection);
	}

	void Chase()
	{
		if (player != null)
			RotateTowards(player.position);
	}

	void DetectPlayer()
	{
		if(player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
		{
			currentState = State.Chasing;
		}
	}

	void LosePlayerCheck()
	{
		if(player != null && Vector3.Distance(transform.position, player.position) > loseTargetDistance)
		{
			currentState = State.Wandering;
		}
	}

	Vector3 avoidanceForce = Vector3.zero;

	void AvoidObstacles()
	{
		Vector3 futurePos = transform.position + transform.forward * speed * 1.0f;
		RaycastHit hit;

		Vector3[] directions = {
		transform.forward,
		(transform.forward + transform.right).normalized,
		(transform.forward - transform.right).normalized,
		(transform.forward + transform.up).normalized,
		(transform.forward - transform.up).normalized,
		Vector3.down, // NEW - direct downward check
        Vector3.up    // NEW - direct upward check
    };

		foreach (var dir in directions)
		{
			if (Physics.SphereCast(futurePos, 1f, dir, out hit, obstacleAvoidanceRange, obstacleMask))
			{
				Vector3 awayFromObstacle = hit.normal * (obstacleAvoidanceRange - hit.distance);
				avoidanceForce = Vector3.Lerp(avoidanceForce, awayFromObstacle, Time.deltaTime * 5f);
				return;
			}
		}

		// decay avoidance force
		avoidanceForce = Vector3.Lerp(avoidanceForce, Vector3.zero, Time.deltaTime * 2f);
	}

	void RotateTowards(Vector3 targetPosition)
	{
		Vector3 desiredDirection = (targetPosition - transform.position).normalized;
		Vector3 finalDirection = (desiredDirection + avoidanceForce).normalized;

		// Smooth the steering direction
		currentDirection = Vector3.Slerp(currentDirection, finalDirection, Time.deltaTime * 2f);

		if (currentDirection == Vector3.zero)
			return;

		Quaternion lookRotation = Quaternion.LookRotation(currentDirection);
		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			lookRotation,
			rotationSpeed * Time.deltaTime * 100f
		);
	}

	void MoveForward()
	{
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	void StayInZone()
	{
		Vector3 min = zoneCenter - zoneSize * 0.5f;
		Vector3 max = zoneCenter + zoneSize * 0.5f;

		Vector3 steer = Vector3.zero;
		Vector3 pos = transform.position;
		float edgeBuffer = 5f; // start turning back this far from the edge

		if (pos.x < min.x + edgeBuffer) steer.x = 1;
		else if (pos.x > max.x - edgeBuffer) steer.x = -1;

		if (pos.y < min.y + edgeBuffer) steer.y = 1;
		else if (pos.y > max.y - edgeBuffer) steer.y = -1;

		if (pos.z < min.z + edgeBuffer) steer.z = 1;
		else if (pos.z > max.z - edgeBuffer) steer.z = -1;

		if (steer != Vector3.zero)
		{
			wanderDirection = Vector3.Slerp(wanderDirection, steer.normalized, Time.deltaTime * 2f);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(zoneCenter, zoneSize);

		if (!Application.isPlaying) return;

		Gizmos.color = Color.red;

		// Visualize forward cast from future position
		Vector3 futurePos = transform.position + transform.forward * speed * 1.0f;

		Vector3[] directions = {
		transform.forward,
		(transform.forward + transform.right).normalized,
		(transform.forward - transform.right).normalized,
		(transform.forward + transform.up).normalized,
		(transform.forward - transform.up).normalized,
		Vector3.down,
		Vector3.up
	};

		foreach (var dir in directions)
		{
			Gizmos.DrawRay(futurePos, dir * obstacleAvoidanceRange);
			Gizmos.DrawWireSphere(futurePos + dir * obstacleAvoidanceRange, 1f); // end point
		}

		// Optional: show detection radius
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);

		// Optional: show line to player if chasing
		if (player != null && currentState == State.Chasing)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, player.position);
		}
	}
#endif
}
