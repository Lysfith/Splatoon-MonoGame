using _2016_Project_Paint.GameData.Enums;
using _2016_Project_Paint.Graphic;
using GameUILibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2016_Project_Paint.GameData.States
{
    class MainMenuState : IGameState
    {
        private UI _mainMenuUi;

        public void End()
        {

        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Start(Game game, object data = null)
        {
            var callbacks = new Dictionary<string, Action<object, EventArgs>>();

            callbacks.Add("Quit_OnGainFocus", (sender, e) => {
                game.Exit();
            });

            callbacks.Add("NewGame_OnGainFocus", (sender, e) => {
                StateManager.Instance.SetGameState(EnumGameState.Game);
            });

            _mainMenuUi = UI.Load("UIDescription/MainMenu/MainMenu.xml", game, new Dictionary<string, string>(), callbacks);
            _mainMenuUi.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            if (_mainMenuUi != null)
            {
                _mainMenuUi.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, MySpriteBatch spritebatch)
        {
            if(_mainMenuUi != null)
            {
                _mainMenuUi.Draw(gameTime, spritebatch);
            }
        }
    }
}
