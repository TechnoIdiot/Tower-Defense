namespace Tower_Defense
{
    internal class Program
    {
        /// <summary>
        /// Entrance to the program, initializes spaghettiloop to start it up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            MainLoop spaghettiloop = new MainLoop();
            spaghettiloop.Init();
        }
    }
}