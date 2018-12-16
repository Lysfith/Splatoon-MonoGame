using _2016_Project_Paint.GameData.Config;
using _2016_Project_Paint.GameData.States.GameStateImplementation.Entities;
using _2016_Project_Paint.GameData.States.GameStateImplementation.Enums;
using _2016_Project_Paint.GameData.States.GameStateImplementation.Map;
using _2016_Project_Paint.GameData.Utils;
using _2016_Project_Paint.Graphic;
using _2016_Project_Paint.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace _2016_Project_Paint.GameData.States
{
    class GameState : IGameState
    {
        private List<TouchLocation> _touches;
        private float _pinchDistance;

        private MapManager _mapManager;

        private int _rectWidth = 400;
        private int _rectHeight = 200;

        private Rectangle _rectRed; //Top
        private Rectangle _rectBlue; //Right
        private Rectangle _rectGreen; //Bottom
        private Rectangle _rectYellow; //Left

        private Vector2 _positionRed; //Top
        private Vector2 _positionBlue; //Right
        private Vector2 _positionGreen; //Bottom
        private Vector2 _positionYellow; //Left

        private double _lastBallRed; //Top
        private double _lastBallBlue; //Right
        private double _lastBallGreen; //Bottom
        private double _lastBallYellow; //Left

        private List<PaintBall> _paintBalls;

        private Random _random;

        private double _timeGame = 60.0;
        private double _timeCurrent = 0;
        private double _lastComputeScore = 0;
        private Score _lastScore;

        public void Start(Game game, object data = null)
        {
            _random = new Random();

            //Création de la map
            _touches = TouchPanel.GetState().ToList();

            _mapManager = new MapManager();

            TextureManager.Instance.GetTexture("Textures/Paint");

            _rectRed = new Rectangle(
                (int)(Settings.ScreenWidth * 0.5f - _rectWidth * 0.5f),
                0,
                _rectWidth,
                _rectHeight
                );

            _rectBlue = new Rectangle(
                (int)(Settings.ScreenWidth - _rectHeight),
                (int)(Settings.ScreenHeight * 0.5f - _rectWidth * 0.5f),
                _rectHeight,
                _rectWidth
                );

            _rectGreen = new Rectangle(
                (int)(Settings.ScreenWidth * 0.5f - _rectWidth * 0.5f),
                (int)(Settings.ScreenHeight - _rectHeight),
                _rectWidth,
                _rectHeight
                );

            _rectYellow = new Rectangle(
                0,
                (int)(Settings.ScreenHeight * 0.5f - _rectWidth * 0.5f),
                _rectHeight,
                _rectWidth
                );

            _positionRed = new Vector2(Settings.ScreenWidth * 0.5f, 0);
            _positionBlue = new Vector2(Settings.ScreenWidth, Settings.ScreenHeight * 0.5f);
            _positionGreen = new Vector2(Settings.ScreenWidth * 0.5f, Settings.ScreenHeight);
            _positionYellow = new Vector2(0, Settings.ScreenHeight * 0.5f);

            _paintBalls = new List<PaintBall>();
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
            var time = gameTime.ElapsedGameTime.TotalSeconds;

            _timeCurrent += time;
            _lastComputeScore += time;

            if (_timeCurrent >= _timeGame)
            {
                StateManager.Instance.SetGameState(Enums.EnumGameState.Score, _lastScore);
            }

            _lastBallRed += time;
            _lastBallBlue += time;
            _lastBallGreen += time;
            _lastBallYellow += time;

            var touches = TouchPanel.GetState().ToList();
            //touches.RemoveAll(x => x.Pressure == 0);
            foreach (var touch in touches)
            {
                //Move
                var team = GetTeam(touch.Position);

                if(team != EnumTeam.NONE)
                {
                    Color color = Color.Red;
                    Vector2 positionBall = touch.Position;
                    Vector2 positionCenter = _positionRed;
                    double lastBall = _lastBallRed;

                    switch (team)
                    {
                        case EnumTeam.BLUE:
                            color = Color.Blue;
                            positionCenter = _positionBlue;
                            lastBall = _lastBallBlue;
                            break;
                        case EnumTeam.GREEN:
                            color = Color.Green;
                            positionCenter = _positionGreen;
                            lastBall = _lastBallGreen;
                            break;
                        case EnumTeam.YELLOW:
                            color = Color.Yellow;
                            positionCenter = _positionYellow;
                            lastBall = _lastBallYellow;
                            break;
                    }

                    if (lastBall > 0.1)
                    {
                        Vector2 direction = positionBall - positionCenter;
                        direction.Normalize();
                        float distanceThrow = Vector2.DistanceSquared(positionBall, positionCenter);

                        switch (team)
                        {
                            case EnumTeam.RED:
                                _lastBallRed = 0;
                                distanceThrow /= 55;
                                break;
                            case EnumTeam.BLUE:
                                _lastBallBlue = 0;
                                distanceThrow /= 30f;
                                break;
                            case EnumTeam.GREEN:
                                _lastBallGreen = 0;
                                distanceThrow /= 55;
                                break;
                            case EnumTeam.YELLOW:
                                _lastBallYellow = 0;
                                distanceThrow /= 30f;
                                break;
                        }

                        _paintBalls.Add(new PaintBall()
                        {
                            Position = positionCenter,
                            Color = color,
                            IsDrawed = false,
                            Height = 32,
                            Width = 32,
                            Time = 1,
                            Velocity = direction * distanceThrow
                        });
                    }
                }

               
            }
            _touches = touches;

            //Update balls
            foreach(var ball in _paintBalls)
            {
                if(!ball.IsDrawed)
                {
                    ball.Time -= time;

                    if(ball.Time <= 0)
                    {
                        var textureSplashs = TextureManager.Instance.GetTexture("Textures/Splashs");

                        var rand = _random.Next(0, 10);

                        _mapManager.AddColor(
                           textureSplashs,
                           new Rectangle(rand * 64, 0, 64, 64),
                           new Rectangle((int)(ball.Position.X - 32), (int)(ball.Position.Y - 32), 64, 64),
                           ball.Color
                           );

                        ball.IsDrawed = true;
                    }

                    ball.Position += ball.Velocity * (float)time;
                }
            }

            _paintBalls.RemoveAll(x => x.IsDrawed);

            _mapManager.Update(time);

            if(_lastComputeScore > 0.2)
            {
                _lastScore = _mapManager.ComputeScore(Color.Red, Color.Blue, Color.Green, Color.Yellow);
                _lastComputeScore = 0;
            }
        }

        private EnumTeam GetTeam(Vector2 position)
        {
            if(_rectRed.Contains(position))
            {
                return EnumTeam.RED;
            }
            if (_rectBlue.Contains(position))
            {
                return EnumTeam.BLUE;
            }
            if (_rectGreen.Contains(position))
            {
                return EnumTeam.GREEN;
            }
            if (_rectYellow.Contains(position))
            {
                return EnumTeam.YELLOW;
            }

            return EnumTeam.NONE;
        }

        public void Draw(GameTime gameTime, MySpriteBatch spritebatch)
        {
            _mapManager.Draw(spritebatch);

            
            //Player Positions
            var texturePlayerPositionWidth = TextureManager.Instance.GetTexture("Textures/PlayerPosition_Width");
            var texturePlayerPositionHeight = TextureManager.Instance.GetTexture("Textures/PlayerPosition_Height");

            spritebatch.Draw(texturePlayerPositionWidth, _rectRed, Color.White);
            spritebatch.Draw(texturePlayerPositionHeight, _rectBlue, Color.White);
            spritebatch.Draw(texturePlayerPositionWidth, _rectGreen, Color.White);
            spritebatch.Draw(texturePlayerPositionHeight, _rectYellow, Color.White);



            //Draw balls
            var textureBall = TextureManager.Instance.GetTexture("Textures/Paint");
            foreach (var ball in _paintBalls)
            {
                if (!ball.IsDrawed)
                {
                    var position = ball.Position - new Vector2(16, 16);
                    spritebatch.Draw(textureBall, new Rectangle((int)position.X, (int)position.Y, 32, 32), ball.Color);
                }
            }

            SpriteFont font = FontManager.Instance.GetFont("Arial-10");

            spritebatch.DrawString(font, "Pinch : " + _pinchDistance, new Vector2(10, 90), Color.Yellow);

            if(_lastScore != null)
            {
                spritebatch.DrawString(font, "Time : " + TimeSpan.FromSeconds(_timeGame - _timeCurrent), 
                    new Vector2(Settings.ScreenWidth - 200, 10), Color.Yellow);
                spritebatch.DrawString(font, "Red : " + _lastScore.Red.ToString("0") + "%", new Vector2(Settings.ScreenWidth - 200, 30), Color.Yellow);
                spritebatch.DrawString(font, "Blue : " + _lastScore.Blue.ToString("0") + "%", new Vector2(Settings.ScreenWidth - 200, 50), Color.Yellow);
                spritebatch.DrawString(font, "Green : " + _lastScore.Green.ToString("0") + "%", new Vector2(Settings.ScreenWidth - 200, 70), Color.Yellow);
                spritebatch.DrawString(font, "Yellow : " + _lastScore.Yellow.ToString("0") + "%", new Vector2(Settings.ScreenWidth - 200, 90), Color.Yellow);
            }
        }
    }
}
