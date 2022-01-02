using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Orbits
{
    class Game
    {
        private readonly RenderWindow _window;
        private readonly Galaxy _galaxy;
        private readonly MassSpawner _massSpawner;
        private readonly OrbitalSpawner _orbitalSpawner;
        private const int FrameRate = 144;
        private readonly Font _openSans;

        public Game(uint width, uint height)
        {
            _window = new RenderWindow(new VideoMode(width, height), "Orbits", Styles.Titlebar | Styles.Close);

            _window.KeyPressed += KeyPressed;
            _window.Closed += Closed;
            _window.SetMouseCursor(new Cursor(Cursor.CursorType.Arrow));
            _window.MouseButtonPressed += MouseButtonPressed;
            _window.MouseButtonReleased += MouseButtonReleased;

            _galaxy = new Galaxy();

            _massSpawner = new MassSpawner(_window);
            _orbitalSpawner = new OrbitalSpawner(_window);
            _openSans = new Font("./OpenSans-Regular.ttf");
        }

        public void Run()
        {
            // clears up to 10 buffers, in practice, we only have to clear maybe 3 in the case of triple buffers
            int maxNumberOfBuffers = 10;
            for (int i = 0; i < maxNumberOfBuffers; i++)
            {
                _window.Clear(Color.White);
                _window.Display();
            }

            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                //_window.Clear(Color.White);
                FadeSlightly();
                DrawInstructions();

                int loopCount = 100; // more = slower but more precise
                for (int i = 0; i < loopCount; i++)
                {
                    _galaxy.Update(1f / (FrameRate * loopCount));
                }

                _massSpawner.Draw(GetMousePos());
                _galaxy.Draw(_window);
                _orbitalSpawner.Draw(GetMousePos());

                _window.Display();

                // we should probably factor in the actual amount of time taken, but meh
                Thread.Sleep((int)(1000f / FrameRate));
            }
        }

        private void FadeSlightly()
        {
            var fadeRect = new RectangleShape(new Vector2f(_window.Size.X, _window.Size.Y));
            fadeRect.FillColor = new Color(255, 255, 255, 10);
            _window.Draw(fadeRect);
        }

        private void DrawInstructions()
        {
            var instructions = new Text("Spawn Orbiters by: left click, drag, release.\nSpawn masses by: right click, drag, release.", _openSans, 16);
            instructions.Position = new Vector2f(10, 10);

            // this is silly, but clear the space behind the instructions
            instructions.FillColor = Color.White;
            for (int i = 0; i < 10; i++)
                _window.Draw(instructions);

            // then actually draw the instructions
            instructions.FillColor = Color.Black;
            _window.Draw(instructions);
        }

        #region Callbacks

        private void MouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                TryStartSpawnOrbital();
            }
            else if (e.Button == Mouse.Button.Right)
            {
                TryStartSpawnMass();
            }
        }

        private void MouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                TryFinishSpawnOrbital();
            }
            else if (e.Button == Mouse.Button.Right)
            {
                TryFinishSpawnMass();
            }
        }

        private void KeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
        }

        private void Closed(object? sender, EventArgs e)
        {
            _window.Close();
        }

        #endregion Callbacks

        #region Spawners
        private void TryStartSpawnOrbital()
        {
            if (_massSpawner.IsActive)
                return;

            _orbitalSpawner.Start(GetMousePos());
        }

        private void TryStartSpawnMass()
        {
            if (_orbitalSpawner.IsActive)
                return;

            _massSpawner.Start(GetMousePos());
        }

        private void TryFinishSpawnOrbital()
        {
            if (_massSpawner.IsActive)
                return;

            if (_orbitalSpawner.TryComplete(GetMousePos(), out var orbital))
            {
                _galaxy.Orbitals.Add(orbital);
            }
        }

        private void TryFinishSpawnMass()
        {
            if (_orbitalSpawner.IsActive)
                return;

            if (_massSpawner.TryComplete(GetMousePos(), out var mass))
            {
                _galaxy.Masses.Add(mass);
            }
        }

        #endregion Spawners

        private Vector2 GetMousePos()
        {
            var pos = Mouse.GetPosition(_window);
            return new Vector2(pos.X, pos.Y);
        }
    }
}
