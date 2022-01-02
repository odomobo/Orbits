using System;
using System.Threading;
using SFML.Graphics;
using SFML.Window;

namespace Orbits
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(1600, 1200);
            game.Run();
        }
    }
}
