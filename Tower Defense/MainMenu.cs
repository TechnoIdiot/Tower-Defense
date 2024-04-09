using Raylib_CsLo;

namespace Tower_Defense
{
    /// <summary>
    /// I thought it would be cleaner if MainMenu had it's own class
    /// Also contains the Settings menu. 
    /// Uses similar logic to mainloop to change between menus.
    /// </summary>
    internal class MainMenu
    {
        enum MenuStates
        {
            main,
            settings,
            difficulty
        }
        MenuStates menuState;
        bool start;
        bool exit;
        bool settings;
        bool backToMenu;

        public GameState Update()
        {
            if (start)
            {
                return GameState.GameInit;
            }
            else if (exit)
            {
                return GameState.Exit;
            }
            else if (backToMenu)
            {
                backToMenu = false;
                menuState = MenuStates.main;
            }
            else if (settings)
            {
                menuState = MenuStates.settings;
            }
            return GameState.MainMenu;
        }

        public void Draw()
        {
            switch (menuState)
            {
                case MenuStates.main:
                    Raylib.ClearBackground(Raylib.BLACK);
                    Raylib.DrawText("Tower defense", 250, 100, 100, Raylib.WHITE);

                    exit = RayGui.GuiButton(new Rectangle(425, 400, 100, 100), "Exit");
                    start = RayGui.GuiButton(new Rectangle(575, 400, 100, 100), "Start");
                    settings = RayGui.GuiButton(new Rectangle(725, 400, 100, 100), "Settings");
                    break;

                case MenuStates.settings:
                    Raylib.ClearBackground(Raylib.BLACK);
                    Raylib.DrawText("Settings", 400, 100, 100, Raylib.WHITE);

                    backToMenu = RayGui.GuiButton(new Rectangle(575, 400, 100, 100), "Exit");
                    break;
            }
        }
    }
}
