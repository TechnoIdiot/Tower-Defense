using Raylib_CsLo;
using System.Numerics;
using TurboMapReader;

namespace Tower_Defense
{
    /// <summary>
    ///  GameLoop runs everything gameplay.
    ///  TODO: Space out enemy spawning, I don't know why a single bullet can also kill every enemy :/
    ///  TODO: Add a timer for grace periods between rounds or make a button to start the rounds.
    ///  There are more TODOs spread around the project
    /// </summary>
    internal class GameLoop
    {
        // Game variables
        int hp;
        int money;
        int round;
        int deadEnemies = 0;
        bool hacks = false;
        bool lostGame;
        // On both of these 0 = easy, 1 = medium, 2 = hard
        int[] hpDiffs = { 150, 100, 50 };
        int[] startingMoney = { 500, 400, 200 };

        // Map variables
        TiledMap map;
        Texture textureAtlas;

        // Entities
        List<Troop> troops = new List<Troop>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Bullet> bullets = new List<Bullet>();

        /// <summary>
        /// A function to prepare for the game start. Returns GameState.GameLoop after it has done initializing everything.
        /// </summary>
        /// <returns></returns>
        public GameState GameInit()
        {
#if DEBUG
            hacks = true;
#endif
            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.DrawText("Select difficulty", 250, 100, 100, Raylib.WHITE);

            bool easy = RayGui.GuiButton(new Rectangle(425, 400, 100, 100), "Easy");
            bool normal = RayGui.GuiButton(new Rectangle(575, 400, 100, 100), "Normal");
            bool hard = RayGui.GuiButton(new Rectangle(725, 400, 100, 100), "Hard");

            if (easy)
            {
                hp = hpDiffs[0];
                money = startingMoney[0];
                lostGame = false;
                round = 0;
                deadEnemies = 0;
                return GameState.GameLoop;
            }
            else if (normal)
            {
                hp = hpDiffs[1];
                money = startingMoney[1];
                lostGame = false;
                round = 0;
                deadEnemies = 0;
                return GameState.GameLoop;
            }
            else if (hard)
            {
                hp = hpDiffs[2];
                money = startingMoney[2];
                lostGame = false;
                round = 0;
                deadEnemies = 0;
                return GameState.GameLoop;
            }

            bool backToMenu = RayGui.GuiButton(new Rectangle(10, 600, 100, 100), "Exit");
            if (backToMenu)
                return GameState.MainMenu;

            if (map == null)
            {
                map = MapReader.LoadMapFromFile("towermapsmaller.tmj");
                map.PrintToConsole();
                Image imageAtlas = Raylib.LoadImage("towerDefense_tilesheet.png");
                textureAtlas = Raylib.LoadTextureFromImage(imageAtlas);
            }

            // Returns gameinit to loop the difficulty selection.
            return GameState.GameInit;
        }

        /// <summary>
        /// The game's update loop
        /// </summary>
        public void Update()
        {
            if (!lostGame)
            {
                foreach (Troop troop in troops)
                {
                    troop.Update();
                }
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.IsDead())
                    {
                        deadEnemies++;
                        continue;
                    }
                    enemy.Update();
                }
                foreach (Bullet bullet in bullets)
                {
                    if (bullet.HasHit())
                    {
                        continue;
                    }
                    bullet.Update();
                }
                if (Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON))
                {
                    CreateNewTroop(0);
                }
                if (hp == 0)
                {
                    bool lost = true;
                }
                if (deadEnemies == enemies.Count)
                {
                    NewRound();
                }
            }
        }

        /// <summary>
        /// The game's draw loop
        /// </summary>
        public void Draw()
        {
            if (!lostGame)
            {
                Raylib.ClearBackground(Raylib.BLACK);
                DrawMap();
                DrawUi();
                foreach (Troop troop in troops)
                {
                    troop.Draw();
                }
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.IsDead())
                    {
                        continue;
                    }
                    enemy.Draw();
                }
                foreach (Bullet bullet in bullets)
                {
                    if (bullet.HasHit())
                    {
                        continue;
                    }
                    bullet.Draw();
                }
                DrawHoveredTileBorder();
            }
        }

        /// <summary>
        /// Draws Ui to see player health, money and placable troops
        /// Also handles the Ui buttons logic
        /// </summary>
        void DrawUi()
        {
            if (hacks)
            {
                bool moneyHack = RayGui.GuiButton(new Rectangle(map.width * map.tilewidth + 10, 100, 100, 100), "Money hack");
                if (moneyHack)
                {
                    money += 1000;
                }
            }
            Raylib.DrawText($"Money: {money}", map.width * map.tilewidth + 10, 10, 30, Raylib.WHITE);
            Raylib.DrawText($"Basic troop cost: 150$", map.width * map.tilewidth + 10, 40, 20, Raylib.WHITE);
            Raylib.DrawText($"Round: {round}", map.width * map.tilewidth + 10, 70, 20, Raylib.WHITE);
        }

        /// <summary>
        /// The main function for drawing the map. This reads through the TiledMap Map layer by layer to know where and which tiles to place and sends that information to DrawTile().
        /// </summary>
        void DrawMap()
        {
            for (int layer = 0; layer < map.layers.Count; layer++)
            {
                int rows = map.layers[layer].height;
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < map.layers[layer].width; col++)
                    {
                        int tileId = map.layers[layer].data[row * map.layers[layer].width + col];
                        int x = col * map.tilewidth;
                        int y = row * map.tileheight;
                        if (map.layers[layer].data[row * map.layers[layer].width + col] != 0)
                            DrawTile(x, y, tileId - 1);
                    }
                }
            }
        }
        /// <summary>
        /// Uses the data from DrawMap() to draw the tiles on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="TileId"></param>
        void DrawTile(int x, int y, int TileId)
        {
            int tilesPerRow = map.tilesetFiles[0].columns;

            float rowf = TileId / tilesPerRow;

            int row = Convert.ToInt32(rowf);
            int column = TileId % tilesPerRow;

            int u = column * map.tilewidth;
            int v = row * map.tileheight;

            Raylib.DrawTextureRec(textureAtlas, new Rectangle(u, v, map.tilewidth, map.tileheight), new System.Numerics.Vector2(x, y), Raylib.WHITE);
        }

        /// <summary>
        /// Returns which tile the player is hovering over
        /// </summary>
        /// <returns></returns>
        Vector2 GetMouseTilePosition()
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            Vector2 mouseTile = new Vector2(MathF.Floor(mousePos.X / map.tilewidth), MathF.Floor(mousePos.Y / map.tileheight));
            return mouseTile;
        }

        /// <summary>
        /// Creates a new troop, troopType is used to know which kind of troop to make. 
        /// Also checks if the placing position is valid.
        /// </summary>
        /// <param name="troopType"></param>
        void CreateNewTroop(int troopType)
        {
            Vector2 mouseTilePosition = GetMouseTilePosition();
            bool canPlace = false;
            Troop newTroop;

            if (mouseTilePosition.X < map.width)
            {
                // If the selected tile is in Ground layer
                if (map.layers[0].data[(int)mouseTilePosition.Y * map.layers[0].width + (int)mouseTilePosition.X] != 0)
                {
                    if (troops.Any())
                    {
                        foreach (Troop troop in troops)
                        {
                            if (troop.GetTroopPosition() == mouseTilePosition * map.tilewidth)
                            {
                                canPlace = false;
                                break;
                            }
                            else
                                canPlace = true;
                        }
                    }
                    else
                        canPlace = true;
                }
            }

            if (canPlace)
            {
                switch (troopType)
                {
                    case 0:
                        newTroop = new Troop(mouseTilePosition * map.tilewidth, new Vector2(48, 48), 150, 2f, 30, 1, 120);
                        if (money > newTroop.GetTroopCost())
                        {
                            troops.Add(newTroop);
                            money -= newTroop.GetTroopCost();
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Draws a Red Ring around the tile that the player is hovering over.
        /// </summary>
        void DrawHoveredTileBorder()
        {
            if (GetMouseTilePosition().X < map.width)
                Raylib.DrawRectangleLines((int)GetMouseTilePosition().X * map.tilewidth, (int)GetMouseTilePosition().Y * map.tileheight, 64, 64, Raylib.RED);
        }

        /// <summary>
        /// If all enemies are dead then we spawn new ones
        /// TODO: Add more enemy troops and give them raritys based on their strength
        /// TODO: Make it so enemies automatically spawn on the start of the path
        /// </summary>
        void NewRound()
        {
            round++;
            money += round * deadEnemies;
            enemies.Clear();
            deadEnemies = 0;
            bullets.Clear();

            Random rnd = new Random();
            int enemiesToSpawn = rnd.Next(round / 2, round);
            for (int i = 0; i <= enemiesToSpawn; i++)
            {
                Enemy enemy = new Enemy(new Vector2(-48, (map.height / 2) * map.tileheight), new Vector2(48, 48), 1, 1);
                enemies.Add(enemy);
            }
        }
    }
}
