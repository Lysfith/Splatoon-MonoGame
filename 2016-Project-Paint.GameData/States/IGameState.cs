using _2016_Project_Paint.Graphic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2016_Project_Paint.GameData.States
{
    public interface IGameState
    {
        void Start(Game game, object data = null);
        void End();
        void Pause();
        void Resume();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, MySpriteBatch spritebatch);
    }
}
