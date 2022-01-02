using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using SFML.Graphics;

namespace Orbits
{
    class OrbitalSpawner
    {
        public bool IsActive = false;
        private Orbital _orbital;

        private readonly RenderWindow _window;

        public OrbitalSpawner(RenderWindow window)
        {
            _window = window;
        }

        public void Start(Vector2 origin)
        {
            IsActive = true;
            _orbital = new Orbital { Velocity = new Vector2(), Position = origin };
        }

        public void Draw(Vector2 mousePos)
        {
            if (IsActive)
            {
                Update(mousePos);
                _orbital.Draw(_window);
            }
        }

        private void Update(Vector2 mousePos)
        {
            var diffVec = _orbital.Position - mousePos;
            _orbital.Velocity = diffVec * 1f;
        }

        public bool TryComplete(Vector2 mousePos, out Orbital orbital)
        {
            if (IsActive)
            {
                Update(mousePos);
                IsActive = false;
                orbital = _orbital;
                return true;
            }

            orbital = null;
            return false;
        }
    }
}
