using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Basket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText; // Assigned in Inspector
    [SerializeField] private GameObject restartButton;  // Assigned in Inspector
    [SerializeField] private ScoreCounter scoreCounter; // Assigned in Inspector

    public static int round = 1;

    void Start()
    {
        // Find and assign ScoreCounter if not set in Inspector
        if (scoreCounter == null)
        {
            GameObject scoreGO = GameObject.Find("ScoreCounter");
            if (scoreGO != null)
            {
                scoreCounter = scoreGO.GetComponent<ScoreCounter>();
            }
            else
            {
               // Debug.LogError("⚠️ ScoreCounter GameObject not found! Assign it in the Inspector.");
            }
        }

        // Ensure roundText is assigned
        if (roundText == null)
        {
            // Debug.LogError("⚠️ roundText is NULL! Assign it in the Inspector.");
        }

        // Ensure restartButton is assigned
        if (restartButton == null)
        {
            // Debug.LogError("⚠️ restartButton is NULL! Assign it in the Inspector.");
        }

        UpdateRoundText();
    }

    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Apple"))
        {
            Destroy(collidedWith);
            if (scoreCounter != null)
            {
                scoreCounter.score += 100;
                HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);
            }
            else
            {
               // Debug.LogError("⚠️ ScoreCounter is NULL! Cannot update score.");
            }
        }
        else if (collidedWith.CompareTag("Branch")) // Branch hitting the Basket
        {
            // Debug.Log("🚨 Branch hit the Basket! Removing a basket...");
            Destroy(collidedWith);

            // Call ApplePicker's AppleMissed() to remove a Basket
            ApplePicker applePicker = Camera.main.GetComponent<ApplePicker>();
            if (applePicker != null)
            {
                applePicker.AppleMissed();
            }
            else
            {
               // Debug.LogError("⚠️ ApplePicker script not found! Cannot remove a basket.");
            }
        }
    }

    public void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = "Round " + round;
        }
        else
        {
           // Debug.LogError("⚠️ roundText is NULL! Cannot update UI.");
        }
    }

    public void IncrementRound()
    {
        round++;
        UpdateRoundText();
    }

    void GameOver()
    {
        if (roundText != null)
        {
            roundText.text = "Game Over";  // Show "Game Over"
        }
        if (restartButton != null)
        {
            restartButton.SetActive(true);  // Show restart button
        }
        Time.timeScale = 0f;  // Stop game movement
    }
}
