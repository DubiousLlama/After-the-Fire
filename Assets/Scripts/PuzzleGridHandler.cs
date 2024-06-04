using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Audio;
using TMPro;
using UnityEditor;
using GridHandler;

/*
 * This script handles the internal logic of the plant grid. It allows the player to place plants on a grid and calculates the mana generated by the plants. You should attach a new instance of the script to each puzzle in the game.
 * 
 * Available functions:
 * int CalculateMana() - Calculates the total mana generated by all plants on the grid.
 * bool Place(string content) - Places a plant on the grid at the player's current position. Since we do not have a player yet, this function is untested. String must be a key in the plantRef dictionary.
 * void PlaceAbsolute(int x, int y, string content) - Places a plant on the grid at the specified position.
 * void PrintGrid() - Prints the contents of the grid to the console.
 * void GetContent(int x, int y) - Returns the content of the grid at the specified position.
 * 
 */

namespace GridHandler
{

    [ExecuteInEditMode]
    public class PuzzleGridHandler : MonoBehaviour
    {
        // public string[,] setup = { { "soil", "soil", "soil" } };

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

        [Header("Prefabs")]
        public GameObject fertileSoil;
        public GameObject moonglow;
        public GameObject starleafTree;
        public GameObject pinepalm;
        public GameObject bloomberry;

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

        public bool isSolved = false;


        // Awake is called before the first frame update and before all Start functions
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

            // runTest();
        }

        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            inventoryManager = FindObjectOfType<InventoryManager>();
            dialogue = gameObject.GetComponent<InteractibleGeneric>();
            if (dialogue != null)
            {
                dialogue.messages = dialogueMessages;
            } 
        }

        private void Update()
        {
            // Check if all requirements are met
            if (CalculateMana() >= manaRequired && !isSolved)
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
                            }
                        }
                        dialogue.ActivateDialogue();
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
                        }
                    }
                    if (dialogue != null)
                    {
                        dialogue.ActivateDialogue();
                    }
                    isSolved = true;
                    Debug.Log("Puzzle Solved!");
                }

            }
        }

        // Define the names and types of all plants that can be placed on the grid
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

        // Checks to see that the grid initializes correctly and that the CalculateMana function works
        public void setTile(int x, int y, GameObject tile)
        {
            # if UNITY_EDITOR
            // If running in edit mode
            if (!Application.isPlaying)
            {
                DestroyImmediate(tiles[y, x]);
                // Instantiate the objects as children of this gameobject
                tiles[y, x] = Instantiate(tile, this.transform);
                tiles[y, x].transform.position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, 0);
                return;
            }
            #endif

            Destroy(tiles[y, x]);
            // Instantiate the objects as children of this gameobject
            tiles[y, x] = Instantiate(tile, this.transform);
            tiles[y, x].transform.position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, 0);
        }

        private void runTest()
        {
            Debug.Log("Debug Tile (should be soil): " + grid.GetContent(0, 0)); // soil

            PlaceAbsolute(0, 0, "moonglow");
            PlaceAbsolute(1, 0, "starleaf tree");
            PlaceAbsolute(2, 0, "moonglow");

            PrintGrid();
            Debug.Log("Debug Score (Should be 10): " + CalculateMana());
        }

        // Loops over the grid. For each plant contained in the Plant dictionary, it calls the Score function of that plant object.
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

        public int calculateManaAtPosition(int i, int j)
        {   
            int mana = 0;

            string content = grid.GetContent(i, j);
            if (plantRef.TryGetValue(content, out Plant plant))
            {
                // Check if the tile is rich soil
                if (tiles[i, j].tag == "2x")
                {
                    mana += plant.Score(j, i, grid, plantRef) * 2;
                }
                else
                {
                    mana += plant.Score(j, i, grid, plantRef);
                    // Debug.Log("Plant: " + plant.name + " at " + j.ToString() + ", " + i.ToString() + " with score " + plant.Score(j, i, grid, plantRef));
                }
            }
            return mana;
        }

        public int countType(string plant)
        {
            // Count the number of grid cells matching the plant string
            int num = 0;
            for (int i = 0; i < grid.GetHeight(); i++)
            {
                for (int j = 0; j < grid.GetWidth(); j++)
                {
                    if (grid.GetContent(j, i) == plant)
                    {
                        num += 1;
                    }
                }
            }
            return num;
        }

        // Places a plant on the grid at the player's current position
        public bool Place(string content)
        {
            // Get the player's position
            Vector3 playerPosition = player.transform.position;

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


            // check if the content is a valid plant
            if (!plantRef.ContainsKey(content))
            {
                throw new ArgumentException("Invalid plant name.");
            }


            // Check if the tile is fertile soil
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


            Debug.Log("Debug Score: " + CalculateMana());
            // Play the long dig for trees and the short dig for bushes
            if (plantRef[content].type == "tree")
                audioManager.TriggerSFX("LongDig");
            else
                audioManager.TriggerSFX("ShortDig");
            return true;
        }

        // Places a plant on the grid at the specified position
        public void PlaceAbsolute(int x, int y, string content)
        {
            if (!plantRef.ContainsKey(content))
                throw new ArgumentException("Invalid plant name.");

            grid.SetContent(y, x, content);
            plants[y, x] = InstantiatePlant(content, x, y);
        }

        // Returns the content of the grid at the specified position
        public string GetContent(int x, int y)
        {
            return grid.GetContent(y, x);
        }

        // Prints the contents of the grid to the console
        public void PrintGrid()
        {
            for (int i = 0; i < grid.GetHeight(); i++)
                for (int j = 0; j < grid.GetWidth(); j++)
                    Debug.Log(j.ToString() + ", " + i.ToString() + ": " + grid.GetContent(i, j));
        }

        private GameObject InstantiatePlant(string plantName, int x, int y)
        {
            // Convert the grid position to a world position
            Vector3 position = new Vector3(this.transform.position.x + x * tileSize, this.transform.position.y + y * tileSize, -1);

            Vector3 treeOffset = new Vector3(0, 0.25f, 0);

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

        // Converts the player's position to a tile on the grid
        public Vector2 PositionToTile(Vector3 position)
        {
            Vector2 notInGrid = new Vector2(-1, -1);

            // Find the player's position relative to the corner of the grid
            float relativeX = position.x - this.transform.position.x + 0.5f * tileSize;
            float relativeY = position.y - this.transform.position.y + 0.5f * tileSize;

            // Check if the player is inside the grid
            if (relativeX < 0 || relativeX >= grid.GetWidth() * tileSize || relativeY < 0 || relativeY >= grid.GetHeight() * tileSize)
                return notInGrid;

            // Find the player's position in terms of tiles
            int tileX = (int)(relativeX / tileSize);
            int tileY = (int)(relativeY / tileSize);

            return new Vector2(tileX, tileY);
        }

        // Generates a grid of soil tiles.
        private GameObject[,] generateTiles()
        {
            GameObject[,] tiles = new GameObject[height, width];

            // Instantiate a grid of soil made up of the fertileSoil prefab
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

        public void Cleanup()
        {
            for (int i = this.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }      
        }
    }

    // In hindsight I think this maybe should have all been included in the main placement tracker script, but I'm too lazy to change it now.
    // This class proivides some basic useful functions for working with a grid beyond what I would get from a 2D array.
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

    // This really should be a parent class with subclasses for each type of plant, but I'm lazy so instead you get a switch statement.
    // Plants must have a name and a type. The Score function calculates the mana generated by the plant at the specified position.
    // When you add a new plant, you must add a case to the switch statement in the Score function.
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
                    // Get the number of adjacent bushes
                    int count = 0;

                    foreach (Vector2 dir in directions)
                        if (plantRef.TryGetValue(grid.GetContent(y + (int)dir.y, x + (int)dir.x), out Plant plant))
                            if (plant.type == "bush")
                                count++;
                    
                    return 2 * count;
                case "pinepalm":
                    // Return the number of adjacent plants with different names
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

                    // Return the number of adjacent empty tiles
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
                   //Also considered returning the number of adjacent bloomberry plants, but they have a way of taking over if you do that.
   
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
                    // Check if it has exactly one adjacent pairtwine
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


