using UnityEngine;

namespace TrackSpawner.Core
{
    public class Obstacle : MonoBehaviour
    {
        public int DelayPoints;
        public MeshRenderer MeshRenderer;

        private void Start() => ChangeMeshColor(MeshRenderer);

        // Define a static method to change the color of the first material to a random vibrant color
        public static void ChangeMeshColor(MeshRenderer meshRenderer)
        {
            if (meshRenderer == null || meshRenderer.materials.Length == 0)
            {
                Debug.LogError("MeshRenderer or materials not found!");
                return;
            }

            // Get the first material of the MeshRenderer
            Material material = meshRenderer.materials[0];

            // Generate a random vibrant color
            Color randomColor = GetRandomVibrantColor();

            // Set the color of the material
            material.color = randomColor;
        }

        // Helper method to generate a random vibrant color
        private static Color GetRandomVibrantColor()
        {
            // You can customize the range of colors based on your preference
            float r = Random.Range(0.5f, 1.0f);
            float g = Random.Range(0.5f, 1.0f);
            float b = Random.Range(0.5f, 1.0f);

            return new Color(r, g, b);
        }
    }
}
