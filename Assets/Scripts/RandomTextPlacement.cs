using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class RandomTextPlacement : MonoBehaviour
{
    public TextMeshProUGUI[] textElements;

    private void Awake()
    {
        textElements = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void PlaceTextSomewhere(string text)
    {
        // Select a random text from the array

        // Select a random TextMeshProUGUI element
        TextMeshProUGUI randomTextElement = textElements[Random.Range(0, textElements.Length)];

        // Assign the random text to the selected TextMeshProUGUI element
        randomTextElement.text = text;

        // Set a random position on the screen
        float randomX = Random.Range(0f, Screen.width);
        float randomY = Random.Range(0f, Screen.height);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
        randomTextElement.rectTransform.position = randomPosition;
    }
}