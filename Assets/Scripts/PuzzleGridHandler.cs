using System;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using TMPro;
using GridHandler;

namespace GridHandler
{
    [ExecuteInEditMode]
    public class PuzzleGridHandler : MonoBehaviour
    {
        [Header("Grid Size")]
        public int height = 1;
        public int width = 3;

        [Header("Solve Requirements")]
        public int manaRequired = 10;
        public string plantRequired = "";
        public int plantRequiredAmount = 3;

        [Header("Solve Reward")]
        public PlantReward[] rewards;
        public string[] dialogueMessages;
        public string initialMessage; // Add this to hold the initial message
        public int initialMessageType;
        public int puzzleID; // Add this to identify the puzzle

        [Header("Prefabs")]
        public GameObject fertileSoil;
        public GameObject moonglow;
        public GameObject starleafTree;
        public GameObject pinepalm;
        public GameObject bloomberry;

        public PuzzleGridHandler puzzle2Handler; // Reference to Puzzle 2's handler

        private Dictionary<string, Plant> plantRef; // plant name -> plant object

        private Grid grid;
        private float tileSize = 1f; // size of each tile in the grid
        private GameObject player;

        private Transform location;
        public GameObject[,] tiles;
        private GameObject[,] plants;
        private AudioManager audioManager;
        private InventoryManager inventoryManager;
        private InteractibleGeneric dialogue;
        private DialogueManager dialogueManager;

        private InteractivePuzzle interactivePuzzle; // Reference to InteractivePuzzle script

        public bool isSolved = false;

        void Awake()
        {
            grid = new Grid(height, width);

            // instantiate the tile grid to match the data grid
            tiles = new GameObject[height, width];
            plants = new GameObject[height, width];

            // Get the location of this gamebobect
            location = this.transform;

            if (tiles == null)
            {
                tiles = generateTiles();
            }
            else
            {
                Cleanup();
                tiles = generateTiles();
            }

            if (player == null) 
                player = GameObject.Find("Player");

            plantRef = definePlants();
        }

        void Start()
        {
            // Debug.Log("PuzzleGridHandler Start called");

            audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null) Debug.LogError("AudioManager not found");

            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null) Debug.LogError("InventoryManager not found");

            dialogue = gameObject.GetComponent<InteractibleGeneric>();
            if (dialogue == null) Debug.LogError("InteractibleGeneric not found");

            dialogueManager = FindObjectOfType<DialogueManager>();

            interactivePuzzle = gameObject.GetComponent<InteractivePuzzle>();
            if (interactivePuzzle == null) Debug.LogError("InteractivePuzzle not found");
        }

        void Update()
        {
            if (!isSolved && CalculateMana() >= manaRequired)
            {
                if (plantRequired != "")
                {
                    if (countType(plantRequired) >= plantRequiredAmount)
                    {
                        // Reward the player
                        foreach (PlantReward reward in rewards)
                        {
                            for (int i = 0; i < reward.count; i++)
                            {
                                inventoryManager.AddItem(reward.item);
                                Debug.Log("Rewarding player with " + reward.item.name);
                            }
                        }

                        if (interactivePuzzle != null)
                        {
                            interactivePuzzle.TriggerDialogue();
                        }

                        isSolved = true;
                        Debug.Log("Puzzle Solved!");
                    }
                }
                else
                {
                    // Reward the player
                    foreach (PlantReward reward in rewards)
                    {
                        for (int i = 0; i < reward.count; i++)
                        {
                            inventoryManager.AddItem(reward.item);
                            Debug.Log("Rewarding player with " + reward.item.name);
                        }
                    }

                    if (interactivePuzzle != null)
                    {
                        interactivePuzzle.TriggerDialogue();
                    }

                    isSolved = true;
                    Debug.Log("Puzzle Solved!");
                }
            }
        }

        public void setTile(int x, int y, GameObject tile)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(tiles[y, x]);
                tiles[y, x] = Instantiate(tile, this.transform);
                tiles[y, x].transform.position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, 0);
                return;
            }
            #endif

            Destroy(tiles[y, x]);
            tiles[y, x] = Instantiate(tile, this.transform);
            tiles[y, x].transform.position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, 0);

            if (tile.tag == "infertileSoil")
            {
                grid.SetContent(y, x, "infertileSoil");
            }
        }

        public int CalculateMana()
        {
            int mana = 0;
            for (int i = 0; i < grid.GetHeight(); i++)
            {
                for (int j = 0; j < grid.GetWidth(); j++)
                {
                    mana += calculateManaAtPosition(i, j);
                }
            }
            return mana;
        }

        public bool CheckFull()
        {
            for (int i = 0; i < grid.GetHeight(); i++)
                for (int j = 0; j < grid.GetWidth(); j++)
                    if (grid.GetContent(i, j) == "soil")
                        return false;
            return true;
        }

        public int calculateManaAtPosition(int i, int j)
        {
            int mana = 0;
            string content = grid.GetContent(i, j);
            if (plantRef.TryGetValue(content, out Plant plant))
            {
                if (tiles[i, j].tag == "2x")
                {
                    mana += plant.Score(j, i, grid, plantRef) * 2;
                }
                else
                {
                    mana += plant.Score(j, i, grid, plantRef);
                }
            }
            return mana;
        }

        public int countType(string plant)
        {
            int num = 0;
            for (int i = 0; i < grid.GetHeight(); i++)
            {
                for (int j = 0; j < grid.GetWidth(); j++)
                {
                    if (grid.GetContent(i, j) == plant)
                    {
                        num += 1;
                    }
                }
            }
            return num;
        }

        public bool Place(string content)
        {
            Vector3 playerPosition = player.transform.position;
            if (isSolved)
            {
                return false;
            }

            Vector2 tile = PositionToTile(playerPosition);
            if (tile.x == -1 && tile.y == -1)
            {
                Debug.Log("Player is not in the grid.");
                audioManager.TriggerSFX("HitRock");
                return false;
            }

            if (content == "soil")
            {
                if (grid.GetContent((int)tile.y, (int)tile.x) == "soil")
                {
                    return false;
                }
                grid.SetContent((int)tile.y, (int)tile.x, "soil");
                DestroyImmediate(plants[(int)tile.y, (int)tile.x]);
                audioManager.TriggerSFX("ShortDig");
                return true;
            }

            if (!plantRef.ContainsKey(content))
            {
                throw new ArgumentException("Invalid plant name.");
            }

            if (tiles[(int)tile.y, (int)tile.x].tag != "Plantable" && tiles[(int)tile.y, (int)tile.x].tag != "2x")
            {
                Debug.Log("Tile is not fertile soil.");
                audioManager.TriggerSFX("HitRock");
                return false;
            }

            if (grid.GetContent((int)tile.y, (int)tile.x) != "soil")
            {
                grid.SetContent((int)tile.y, (int)tile.x, "soil");
                DestroyImmediate(plants[(int)tile.y, (int)tile.x]);
            }

            grid.SetContent((int)tile.y, (int)tile.x, content);
            plants[(int)tile.y, (int)tile.x] = InstantiatePlant(content, (int)tile.x, (int)tile.y);

            plants[(int)tile.y, (int)tile.x].transform.parent = this.transform;

            Debug.Log("Debug Score: " + CalculateMana());

            if (plantRef[content].type == "tree")
                audioManager.TriggerSFX("LongDig");
            else
                audioManager.TriggerSFX("ShortDig");

            if (CheckFull() && CalculateMana() < manaRequired)
            {
                if (puzzleID == 3 && puzzle2Handler != null && !puzzle2Handler.isSolved)
                {
                    dialogue.PlayMessage("I need to find Springberry before I can cast this spell. I might need to cast a spell somewhere else first.");
                }
                else
                {
                    dialogue.PlayMessage("This arrangement of plants isn't producing enough mana to cast a spell! I should try something else.");
                }
            }

            return true;
        }

        public void PlaceAbsolute(int x, int y, string content)
        {
            if (!plantRef.ContainsKey(content))
                throw new ArgumentException("Invalid plant name.");

            grid.SetContent(y, x, content);
            plants[y, x] = InstantiatePlant(content, x, y);
            plants[y, x].transform.parent = this.transform;
        }

        public string GetContent(int x, int y)
        {
            return grid.GetContent(y, x);
        }

        public void PrintGrid()
        {
            for (int i = 0; i < grid.GetHeight(); i++)
                for (int j = 0; j < grid.GetWidth(); j++)
                    Debug.Log(j.ToString() + ", " + i.ToString() + ": " + grid.GetContent(i, j));
        }

        public Vector2 PositionToTile(Vector3 position)
        {
            Vector2 notInGrid = new Vector2(-1, -1);

            float relativeX = position.x - this.transform.position.x + 0.5f * tileSize;
            float relativeY = position.y - this.transform.position.y + 0.5f * tileSize;

            if (relativeX < 0 || relativeX >= grid.GetWidth() * tileSize || relativeY < 0 || relativeY >= grid.GetHeight() * tileSize)
                return notInGrid;

            int tileX = (int)(relativeX / tileSize);
            int tileY = (int)(relativeY / tileSize);

            return new Vector2(tileX, tileY);
        }

        public void Cleanup()
        {
            for (int i = this.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }

        private void runTest()
        {
            Debug.Log("Debug Tile (should be soil): " + grid.GetContent(0, 0));

            PlaceAbsolute(0, 0, "moonglow");
            PlaceAbsolute(1, 0, "starleaf tree");
            PlaceAbsolute(2, 0, "moonglow");

            PrintGrid();
            Debug.Log("Debug Score (Should be 10): " + CalculateMana());
        }

        private GameObject[,] generateTiles()
        {
            GameObject[,] tiles = new GameObject[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tiles[i, j] = Instantiate(fertileSoil, this.transform);
                    tiles[i, j].transform.position = new Vector3(this.transform.position.x + j * tileSize, this.transform.position.y + i * tileSize, 0);
                }
            }

            return tiles;
        }

        private Dictionary<string, Plant> definePlants()
        {
            plantRef = new Dictionary<string, Plant>();
            plantRef.Add("moonglow", new Plant("moonglow", "bush"));
            plantRef.Add("starbloom", new Plant("starbloom", "bush"));
            plantRef.Add("starleaf tree", new Plant("starleaf tree", "tree"));
            plantRef.Add("pinepalm", new Plant("pinepalm", "bush"));
            plantRef.Add("bloomberry", new Plant("bloomberry", "bush"));

            return plantRef;
        }

        private GameObject InstantiatePlant(string plantName, int x, int y)
        {
            Vector3 position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, -1);

            Vector3 treeOffset = new Vector3(0, 0.35f, 0);
            Vector3 plantOffset = new Vector3(0, 0.12f, 0);
            Vector3 lowerPlantOffset = new Vector3(0, 0, -0.01f);

            position = position + lowerPlantOffset * -y;

            if (plants[y, x] != null)
                return null;

            switch (plantName)
            {
                case "moonglow":
                    return Instantiate(moonglow, position + plantOffset, Quaternion.identity);
                case "starleaf tree":
                    return Instantiate(starleafTree, position + treeOffset, Quaternion.identity);
                case "pinepalm":
                    return Instantiate(pinepalm, position + plantOffset, Quaternion.identity);
                case "bloomberry":
                    return Instantiate(bloomberry, position + plantOffset, Quaternion.identity);
                default:
                    return null;
            }
        }
    }

    class Grid
    {
        private string[,] grid;

        public Grid(int height, int width)
        {
            grid = new string[height, width];
            for (int i = 0; i < GetHeight(); i++)
                for (int j = 0; j < GetWidth(); j++)
                    grid[i, j] = "soil";
        }

        public void setGrid(string[,] grid)
        {
            if (grid.GetLength(0) != GetHeight() || grid.GetLength(1) != GetWidth())
                throw new ArgumentException("The new grid must have the same dimensions as the old grid.");

            this.grid = grid;
        }

        public int GetHeight()
        {
            return grid.GetLength(0);
        }

        public int GetWidth()
        {
            return grid.GetLength(1);
        }

        public string GetContent(int row, int col)
        {
            if (row < 0 || row >= GetHeight() || col < 0 || col >= GetWidth())
                return "out of bounds";

            return grid[row, col];
        }

        public void SetContent(int row, int col, string content)
        {
            if (row < 0 || row >= GetHeight() || col < 0 || col >= GetWidth())
                throw new ArgumentOutOfRangeException("Row or column is out of the grid's bounds.");

            grid[row, col] = content;
        }
    }

    class Plant
    {
        public string name;
        public string type;

        public Plant(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public int Score(int x, int y, Grid grid, Dictionary<string, Plant> plantRef)
        {
            Vector2[] directions = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

            switch (name)
            {
                case "moonglow":
                    return 3;
                case "starbloom":
                    return 0;
                case "starleaf tree":
                    int count = 0;

                    foreach (Vector2 dir in directions)
                        if (plantRef.TryGetValue(grid.GetContent(y + (int)dir.y, x + (int)dir.x), out Plant plant))
                            if (plant.type == "bush")
                                count++;

                    return 2 * count;
                case "pinepalm":
                    HashSet<string> names = new HashSet<string>();

                    foreach (Vector2 dir in directions)
                        if (plantRef.TryGetValue(grid.GetContent(y + (int)dir.y, x + (int)dir.x), out Plant plant))
                            if (plant.name != "soil" && plant.name != "pinepalm")
                                names.Add(plant.name);

                    if (names.Count > 1)
                        return 5;
                    else
                        return 0;

                case "bloomberry":
                    int empty = 0;

                    foreach (Vector2 dir in directions)
                    {
                        if (!plantRef.ContainsKey(grid.GetContent(y + (int)dir.y, x + (int)dir.x)))
                        {
                            empty++;
                        }
                    }
                    return 2 * empty;

                case "witchweed":
                    int friends = 0;

                    foreach (Vector2 dir in directions)
                    {
                        if (plantRef.TryGetValue(grid.GetContent(y + (int)dir.y, x + (int)dir.x), out Plant plant))
                        {
                            if (plant.name == "bloomberry")
                            {
                                friends++;
                            }
                        }
                    }
                    return 3 * friends;

                case "pairtwine":
                    int pairtwines = 0;

                    foreach (Vector2 dir in directions)
                    {
                        if (plantRef.TryGetValue(grid.GetContent(y + (int)dir.y, x + (int)dir.x), out Plant plant))
                        {
                            if (plant.name == "pairtwine")
                            {
                                pairtwines++;
                            }
                        }
                    }

                    if (pairtwines == 1)
                    {
                        return 4;
                    }
                    else
                    {
                        return 0;
                    }

                default:
                    return 0;
            }
        }
    }
}
