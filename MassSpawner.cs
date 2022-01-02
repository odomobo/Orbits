using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Orbits
{
    class MassSpawner
    {
        public bool IsActive = false;
        private Mass _mass;

        private readonly RenderWindow _window;
        
        public MassSpawner(RenderWindow window)
        {
            _window = window;
        }

        public void Start(Vector2 origin)
        {
            IsActive = true;
            _mass = new Mass{Mass_ = 0f, Position = origin};
        }

        public void Draw(Vector2 mousePos)
        {
            if (IsActive)
            {
                Update(mousePos);
                _mass.Draw(_window);
            }
        }

        private void Update(Vector2 mousePos)
        {
            var diffVec = mousePos - _mass.Position;
            var diff = diffVec.LengthSquared();
            _mass.Mass_ = diff * 100f;
        }

        public bool TryComplete(Vector2 mousePos, out Mass mass)
        {
            if (IsActive)
            {
                Update(mousePos);
                IsActive = false;
                mass = _mass;
                return true;
            }

            mass = null;
            return false;
        }
    }
}
