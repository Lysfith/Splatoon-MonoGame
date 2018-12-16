using _2016_Project_Paint.GameData.Config;
using _2016_Project_Paint.GameData.Enums;
using _2016_Project_Paint.GameData.States.GameStateImplementation.Entities;
using _2016_Project_Paint.Graphic;
using _2016_Project_Paint.Utils;
using GameUILibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2016_Project_Paint.GameData.States
{
    class ScoreState : IGameState
    {
        private Score _score;
        private UI _scoreUi;

        public void Start(Game game, object data = null)
        {
            _score = (Score)data;

            var callbacks = new Dictionary<string, Action<object, EventArgs>>();

            callbacks.Add("Back_OnGainFocus", (sender, e) => {
                StateManager.Instance.SetGameState(EnumGameState.MainMenu);
            });

            _scoreUi = UI.Load("UIDescription/Score/Score.xml", game, new Dictionary<string, string>(), callbacks);
            _scoreUi.Initialize();
        }

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

       

        public void Update(GameTime gameTime)
        {
            if (_scoreUi != null)
            {
                _scoreUi.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, MySpriteBatch spritebatch)
        {

            if (_scoreUi != null)
            {
                _scoreUi.Draw(gameTime, spritebatch);
            }

            SpriteFont font = FontManager.Instance.GetFont("Arial-16");

            var measuring = font.MeasureString("Rank");

            spritebatch.DrawString(font, "Rank", new Vector2(Settings.ScreenWidth * 0.5f - measuring.X * 0.5f, Settings.ScreenHeight * 0.5f), Color.Yellow);

            var scores = new List<double>()
            {
                _score.Red,
                _score.Blue,
                _score.Green,
                _score.Yellow
            };

            bool redDrawed = false;
            bool blueDrawed = false;
            bool greenDrawed = false;
            bool yellowDrawed = false;

            var orderScores = scores.OrderByDescending(x => x);

            for(int i=0; i< orderScores.Count(); i++)
            {
                var score = orderScores.ElementAt(i);

                if(score == _score.Red && !redDrawed)
                {
                    spritebatch.DrawString(font, (i+1) + " : Red : " + _score.Red.ToString("0") + "%", 
                        new Vector2(Settings.ScreenWidth * 0.5f - 50, Settings.ScreenHeight * 0.5f + 50 + i * 20), Color.Yellow);
                    redDrawed = true;
                }
                else if (score == _score.Blue && !blueDrawed)
                {
                    spritebatch.DrawString(font, (i + 1) + " : Blue : " + _score.Blue.ToString("0") + "%", 
                        new Vector2(Settings.ScreenWidth * 0.5f - 50, Settings.ScreenHeight * 0.5f + 50 + i * 20), Color.Yellow);
                    blueDrawed = true;
                }
                else if (score == _score.Green && !greenDrawed)
                {
                    spritebatch.DrawString(font, (i + 1) + " : Green : " + _score.Green.ToString("0") + "%", 
                        new Vector2(Settings.ScreenWidth * 0.5f - 50, Settings.ScreenHeight * 0.5f + 50 + i * 20), Color.Yellow);
                    greenDrawed = true;
                }
                else if (score == _score.Yellow && !yellowDrawed)
                {
                    spritebatch.DrawString(font, (i + 1) + " : Yellow : " + _score.Yellow.ToString("0") + "%", 
                        new Vector2(Settings.ScreenWidth * 0.5f - 50, Settings.ScreenHeight * 0.5f + 50 + i * 20), Color.Yellow);
                    yellowDrawed = true;
                }
            }

            
        }
    }
}
