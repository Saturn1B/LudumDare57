using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectManager : MonoBehaviour
{
	private int totalCollectible;
	private int collected = 0;
	[SerializeField] private TMP_Text counterText;
	[SerializeField] private Indicator indicator;
	[SerializeField] private GameUI gameUI;

	private void Awake()
	{
		GetComponent<BoxCollider>().size = new Vector3(5, 5, 7) * indicator.detectionScale;

		GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
		totalCollectible = collectibles.Length;
		counterText.text = $"{collected} / {totalCollectible}";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Collectible"))
		{
			collected++;
			counterText.text = $"{collected} / {totalCollectible}";
			Destroy(other.gameObject);

			if(collected >= totalCollectible)
			{
				gameUI.Win();
			}
		}
	}
}
