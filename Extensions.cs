using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Orbits
{
    static class Extensions
    {
        public static Color LerpSaturated(this Color a, Color b, float amount)
        {
            if (amount < 0)
                return a;
            if (amount > 1)
                return b;

            return new Color(a.R.LerpByte(b.R, amount), a.G.LerpByte(b.G, amount), a.B.LerpByte(b.B, amount), a.A.LerpByte(b.A, amount));
        }

        private static byte LerpByte(this byte a, byte b, float amount)
        {
            return (byte) ((float)a + (((float)b - (float)a) * amount));
        }
    }
}
