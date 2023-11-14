using System.Collections.Generic;
using UnityEngine;

namespace TrackSpawner
{
    public class TrackSpawner : MonoBehaviour
    {
        public GameObject trackTilePrefab;
        public GameObject obstaclePrefab;
        public int numberOfTiles = 1000;
        public int minObstacleSize, maxObstacleSize;

        [Range(0, 100)]
        public int obstacleSpawnProbability = 20; // Adjust this to control obstacle spawn probability (percentage)

        private List<GameObject> tiles = new List<GameObject>();
        private float tileWidth = 1f;
        private float distanceBetweenTracks = 3f;
        private int numberOfTracks = 3;

        private Dictionary<int, int> trackObstacleCount = new Dictionary<int, int>();

        private void Start()
        {
            SpawnTracks();
            SpawnObstacles();
        }

        private void SpawnTracks()
        {
            for (int i = 0; i < numberOfTracks; i++)
            {
                for (int j = 0; j < numberOfTiles; j++)
                {
                    var instance = Instantiate(trackTilePrefab,
                        new Vector3(i * distanceBetweenTracks, 0, j * tileWidth), Quaternion.identity);
                    instance.transform.parent = transform;
                    tiles.Add(instance);
                }

                // Initialize obstacle count for each track
                trackObstacleCount.Add(i, 0);
            }
        }

        private void SpawnObstacles()
        {
            // Keep track of tracks with obstacles
            List<int> tracksWithObstacles = new List<int>();

            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                int trackIndex = tileIndex / numberOfTiles;

                // Skip spawning obstacles for the first 100 tiles of each track
                if (trackObstacleCount[trackIndex] < 100)
                {
                    trackObstacleCount[trackIndex]++;
                    continue;
                }

                // Randomly choose a track that doesn't have an obstacle
                int trackWithObstacle;
                do
                {
                    trackWithObstacle = Random.Range(0, numberOfTracks);
                } while (tracksWithObstacles.Contains(trackWithObstacle));

                if (Random.Range(0, 100) < obstacleSpawnProbability)
                {
                    float randomScale = Random.Range(minObstacleSize, maxObstacleSize);

                    // Calculate obstacle size based on the tile size and scale
                    float obstacleSize = tileWidth * randomScale;

                    // Check for collisions before spawning the obstacle
                    if (!ObstacleOverlap(tiles[tileIndex].transform.position, obstacleSize))
                    {
                        // Spawn obstacle on the track tile with the random scale
                        var obstacle = Instantiate(obstaclePrefab, tiles[tileIndex].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity,
                            tiles[tileIndex].transform);
                        obstacle.transform.localScale = new Vector3(1, 1, randomScale);

                        // Add the track with an obstacle to the list
                        tracksWithObstacles.Add(trackWithObstacle);

                        // Check if all tracks have obstacles, reset the list
                        if (tracksWithObstacles.Count == numberOfTracks)
                        {
                            tracksWithObstacles.Clear();
                        }
                    }
                }
            }
        }

        // Check if the obstacle would overlap with any existing obstacle
        private bool ObstacleOverlap(Vector3 position, float size)
        {
            Collider[] colliders = Physics.OverlapBox(position, new Vector3(size / 2, size / 2, size / 2));

            // If there are any colliders in the box, it means there's an overlap
            return colliders.Length > 0;
        }
    }
}
