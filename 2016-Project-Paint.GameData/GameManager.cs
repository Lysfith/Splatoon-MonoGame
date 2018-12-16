using _2016_Project_Paint.GameData.Enums;
using _2016_Project_Paint.Graphic;
using _2016_Project_Paint.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace _2016_Project_Paint.GameData
{
    public class GameManager
    {
        public SpriteFont Font;

        // Use this for initialization
        public void Start(Game game)
        {
            StateManager.Instance.SetGame(game);

            StateManager.Instance.SetGameState(EnumGameState.MainMenu);

            //StateManager.Instance.SetGameState(EnumGameState.Game);
#if true
            //StateManager.Instance.SetGameState(EnumGameState.Game);

            DebugGame.ShowFps();

#else

#endif

        }

        // Update is called once per frame
        public void Update(GameTime gameTime)
        {
            StateManager.Instance.Update(gameTime);

#if true
            DebugGame.Update();

            //_fps.text = "FPS : " + DebugGame.GetFps();
#endif

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Application.Quit();
            //}
        }

        public void Draw(GameTime gameTime, MySpriteBatch spritebatch)
        {
            StateManager.Instance.Draw(gameTime, spritebatch);

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Application.Quit();
            //}
        }
    }
}