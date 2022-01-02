using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace Orbits
{
    class Galaxy
    {
        public List<Mass> Masses = new List<Mass>();
        public List<Orbital> Orbitals = new List<Orbital>();

        public void Update(float delta)
        {
            foreach (var orbital in Orbitals)
            {
                orbital.Update(Masses, delta);
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var mass in Masses)
            {
                mass.Draw(window);
            }

            foreach (var orbital in Orbitals)
            {
                orbital.Draw(window);
            }
        }
    }

    internal class Orbital
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public void Update(List<Mass> masses, float delta)
        {
            foreach (var mass in masses)
            {
                var posDiff = mass.Position - Position;
                var posDiffDirection = posDiff / posDiff.Length();
                var posDiffMagnitude = posDiff.Length();
                var squareMagnitude = posDiffMagnitude * posDiffMagnitude;
                var inverseSquareMagnitude = 1 / squareMagnitude;

                Velocity += mass.Mass_ * posDiffDirection * inverseSquareMagnitude * delta;
            }

            Position += Velocity * delta;
        }

        public void Draw(RenderWindow window)
        {
            var radius = 2f;
            var circle = new CircleShape(radius);
            circle.Position = new Vector2f(Position.X, Position.Y) - new Vector2f(radius, radius);
            var speed = Velocity.Length();
            circle.FillColor = Color.Black.LerpSaturated(Color.Cyan, speed/350);
            window.Draw(circle);
        }
    }

    internal class Mass
    {
        public Vector2 Position;
        public float Mass_;

        public void Draw(RenderWindow window)
        {
            var radius = (float) Math.Max(1f, Math.Sqrt(Mass_) / 100f);
            var circle = new CircleShape(radius);
            circle.Position = new Vector2f(Position.X, Position.Y) - new Vector2f(radius, radius);
            circle.FillColor = Color.Red;
            window.Draw(circle);
        }
    }
}
