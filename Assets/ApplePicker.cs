using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ApplePicker : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject basketPrefab;
    public int numBaskets = 4;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;

    [Header("UI Elements")]
    public TextMeshProUGUI roundText;
    public GameObject restartButton;

    private int currentRound = 1;

    void Start()
    {
        basketList = new List<GameObject>();

        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }

        UpdateRoundText();
        restartButton.SetActive(false);
    }

    public void AppleMissed()
    {
        // Destroy all falling apples
        GameObject[] appleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tempGO in appleArray)
        {
            Destroy(tempGO);
        }

        // Destroy all falling branches
        GameObject[] branchArray = GameObject.FindGameObjectsWithTag("Branch");
        foreach (GameObject tempGO in branchArray)
        {
            Destroy(tempGO);
        }

        // Remove a basket
        if (basketList.Count > 0)
        {
            int basketIndex = basketList.Count - 1;
            GameObject basketGO = basketList[basketIndex];
            basketList.RemoveAt(basketIndex);
            Destroy(basketGO);

            currentRound++;
            UpdateRoundText();
        }

        // Check if there are no baskets left
        if (basketList.Count == 0)
        {
            roundText.text = "Game Over";
            restartButton.SetActive(true);
            Time.timeScale = 0f; // Stops game movement
        }
    }


    void UpdateRoundText()
    {
        roundText.text = "Round " + currentRound;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
