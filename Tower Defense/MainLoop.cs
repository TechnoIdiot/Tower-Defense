using Raylib_CsLo;

namespace Tower_Defense
{
    /// <summary>
    /// Basically a game state handler. Spaghettiloop switches scenes and also handles Raylib initialization
    /// </summary>
    internal class MainLoop
    {
        MainMenu menu = new MainMenu();
        GameLoop game = new GameLoop();
        GameState state;

        /// <summary>
        /// Starts up Raylib
        /// </summary>
        public void Init()
        {
            Raylib.InitWindow(1280, 800, "Tower Defense");
            Raylib.SetTargetFPS(60);
            Raylib.InitAudioDevice();
            state = GameState.MainMenu;

            // Game loop
            while (!Raylib.WindowShouldClose())
            {
                Draw();
                Update();
            }
        }

        /// <summary>
        /// The main Draw() loop
        /// Current Draw states: Mainmenu, Gameloop
        /// </summary>
        private void Draw()
        {
            Raylib.BeginDrawing();
            switch (state)
            {
                case GameState.MainMenu:
                    menu.Draw();
                    break;

                case GameState.GameLoop:
                    game.Draw();
                    break;

            }
            Raylib.EndDrawing();
        }

        /// <summary>
        /// The main Update loop
        /// Current Update checks: Exit, Mainmenu, GameLoop, GameInit
        /// </summary>
        private void Update()
        {
            switch (state)
            {
                case GameState.Exit:
                    Raylib.CloseAudioDevice();
                    Raylib.CloseWindow();
                    break;

                case GameState.MainMenu:
                    state = menu.Update();
                    break;

                case GameState.GameLoop:
                    game.Update();
                    break;

                case GameState.GameInit:
                    state = game.GameInit();
                    break;
            }
        }
    }
    enum GameState
    {
        MainMenu,
        GameInit,
        GameLoop,
        Paused,
        Exit
    }
}
