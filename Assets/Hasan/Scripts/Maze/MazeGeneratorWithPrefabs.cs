using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorWithPrefabs : MonoBehaviour
{
    public int width = 51;
    public int height = 51;
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    private bool[,] maze;

    #region Cosmetic
    public GameObject[] cosmeticPrefabs;
    [Range(0f, 1f)] public float cosmeticChance = 0.1f;
    #endregion

    private List<GameObject> walls = new List<GameObject>();
    private bool startBlinkAndMove = false;

    private int phase = 1;
    private float blinkDuration = 0.5f;
    private float blinkInterval = 0.05f;
    private float moveFrequency = 0.1f;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        maze = new bool[width, height];

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(1, 1);
        maze[current.x, current.y] = true;
        stack.Push(current);

        System.Random rng = new System.Random();

        Vector2Int[] directions = {
            new Vector2Int(0, 2),
            new Vector2Int(2, 0),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0)
        };

        while (stack.Count > 0)
        {
            current = stack.Pop();
            List<Vector2Int> neighbours = new List<Vector2Int>();

            foreach (var dir in directions)
            {
                int nx = current.x + dir[0];
                int ny = current.y + dir[1];

                if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1)
                {
                    if (!maze[nx, ny])
                    {
                        neighbours.Add(new Vector2Int(nx, ny));
                    }
                }
            }

            if (neighbours.Count > 0)
            {
                stack.Push(current);
                var chosen = neighbours[rng.Next(neighbours.Count)];
                maze[chosen.x, chosen.y] = true;
                maze[(current.x + chosen.x) / 2, (current.y + chosen.y) / 2] = true;

                stack.Push(chosen);
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                if (maze[x, y])
                {
                    Instantiate(floorPrefab, position, Quaternion.identity, this.transform);

                    if (Random.value < cosmeticChance && cosmeticPrefabs.Length > 0)
                    {
                        int index = Random.Range(0, cosmeticPrefabs.Length);

                        Vector3[] cornerOffsets = new Vector3[]
                        {
                            new Vector3(-0.5f, 0.5f, 0),
                            new Vector3(0.5f, 0.5f, 0),
                            new Vector3(-0.5f, -0.5f, 0),
                            new Vector3(0.5f, -0.5f, 0)
                        };

                        Vector3 randomCorner = cornerOffsets[Random.Range(0, cornerOffsets.Length)];
                        Instantiate(cosmeticPrefabs[index], position + randomCorner, Quaternion.identity, this.transform);
                    }
                }
                else
                {
                    Instantiate(floorPrefab, position, Quaternion.identity, this.transform);
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, this.transform);
                    walls.Add(wall);
                }
            }
        }
    }

    IEnumerator WallBlinkAndMove()
    {
        while (startBlinkAndMove)
        {
            if (walls.Count > 0)
            {
                GameObject wall = walls[Random.Range(0, walls.Count)];
                Renderer wallRenderer = wall.GetComponent<Renderer>();

                float currentBlinkDuration = blinkDuration;
                float currentBlinkInterval = blinkInterval;
                float currentMoveFrequency = moveFrequency;

                if (phase >= 2)
                {
                    currentBlinkDuration = 0.25f; 
                    currentBlinkInterval = 0.03f; 
                    currentMoveFrequency = 0.05f; 
                }

                for (float t = 0; t < currentBlinkDuration; t += currentBlinkInterval)
                {
                    wallRenderer.material.color = Color.red;
                    yield return new WaitForSeconds(currentBlinkInterval);
                    wallRenderer.material.color = Color.white;
                    yield return new WaitForSeconds(currentBlinkInterval);
                }

                Vector3 direction = Vector3.zero;
                int moveDirection = Random.Range(0, 4);

                switch (moveDirection)
                {
                    case 0: direction = Vector3.up; break;
                    case 1: direction = Vector3.down; break;
                    case 2: direction = Vector3.left; break;
                    case 3: direction = Vector3.right; break;
                }

                wall.transform.position += direction;

                yield return new WaitForSeconds(currentMoveFrequency);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartBlinkAndMove()
    {
        startBlinkAndMove = true;
        StartCoroutine(WallBlinkAndMove());
    }

    public void ProgressPhase()
    {
        phase++;
    }
}
