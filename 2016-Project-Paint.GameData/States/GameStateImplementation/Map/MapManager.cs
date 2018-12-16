using _2016_Project_Paint.GameData.Config;
using _2016_Project_Paint.GameData.States.GameStateImplementation.Entities;
using _2016_Project_Paint.Graphic;
using _2016_Project_Paint.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2016_Project_Paint.GameData.States.GameStateImplementation.Map
{
    public class MapManager
    {
        private bool _textureUpdating;
        private RenderTarget2D _renderTarget;
        private MySpriteBatch _spritebatch;
        private Random _random;

        public MapManager()
        {
            _renderTarget = new RenderTarget2D(GraphicManager.Instance.GraphicsDevice, Settings.ScreenWidth, Settings.ScreenHeight);

            _spritebatch = new MySpriteBatch(GraphicManager.Instance.GraphicsDevice);

            _random = new Random();
        }

        public void AddColor(Texture2D texture, Rectangle sourceRectangle, Rectangle rectangle, Color color)
        {
            _textureUpdating = true;

            var renderTarget = new RenderTarget2D(GraphicManager.Instance.GraphicsDevice, Settings.ScreenWidth, Settings.ScreenHeight);

            GraphicManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicManager.Instance.GraphicsDevice.Clear(Color.Black);

            _spritebatch.Begin();

            _spritebatch.Draw(_renderTarget, Vector2.Zero, Color.White);

            _spritebatch.Draw(texture, rectangle, sourceRectangle, color, 
                (float)_random.NextDouble(), new Vector2(0.5f, 0.5f), 
                SpriteEffects.None, 0);

            _spritebatch.End();

            GraphicManager.Instance.GraphicsDevice.SetRenderTarget(null);

            //_textureGame = (Texture2D)renderTarget;

            _renderTarget.Dispose();
            _renderTarget = renderTarget;
            //_renderTarget.Dispose();
            //spritebatch.Dispose();

            _textureUpdating = false;
        }

        public void Update(double time)
        {

        }

        public void Draw(MySpriteBatch spritebatch)
        {
            if (!_textureUpdating)
            {
                spritebatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            }
        }

        public Score ComputeScore(Color red, Color blue, Color green, Color yellow)
        {
            Score score = new Score();

            double total = 0;
            double redCount = 0;
            double blueCount = 0;
            double greenCount = 0;
            double yellowCount = 0;

            var texture = (Texture2D)_renderTarget;

            Color[] pixels = new Color[texture.Width * texture.Height];

            texture.GetData(pixels);

            for(var i=0; i < pixels.Length; i++)
            {
                var color = pixels[i];
                if(color == red)
                {
                    redCount++;
                }
                else if (color == blue)
                {
                    blueCount++;
                }
                else if (color == green)
                {
                    greenCount++;
                }
                else if (color == yellow)
                {
                    yellowCount++;
                }
            }

            total = redCount + blueCount + greenCount + yellowCount;

            if (total != 0)
            {
                score.Red = redCount / total * 100;
                score.Blue = blueCount / total * 100;
                score.Green = greenCount / total * 100;
                score.Yellow = yellowCount / total * 100;
            }

            return score;
        }
    }
}
