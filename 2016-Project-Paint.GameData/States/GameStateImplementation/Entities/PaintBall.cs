using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2016_Project_Paint.GameData.States.GameStateImplementation.Entities
{
    public class PaintBall
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Velocity { get; set; }
        public double Time { get; set; }
        public bool IsDrawed { get; set; }
        public Color Color{ get; set; }

    }
}
