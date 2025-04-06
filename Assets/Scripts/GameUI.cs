using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
	[SerializeField] private GameObject winPanel, loosePanel;

	public void Win()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		winPanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void GameOver()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		loosePanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void MainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void Retry()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}
}
