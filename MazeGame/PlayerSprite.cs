using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace MazeGame
{
    public class PlayerSprite : DrawableGameComponent
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IPlayer _player;
        private float _rotation;
        private Vector2 _previousPos;

        private Vector2 _playerPostion;
        private Texture2D _playerTexture;
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Rectangle _spriteSize;

        public Texture2D PathTexture { get; set; }

        public PlayerSprite(Game game, IPlayer player) : base(game)
        {
            this._player = player;
            this._game = game;
            this._rotation = _player.GetRotation();
        }

        private void updatePathPos()
        {
            int x = (_player.Position.X * _playerTexture.Width) + _playerTexture.Width / 2;
            int y = (_player.Position.Y * _playerTexture.Height) + _playerTexture.Height / 2;
            _previousPos = new Vector2(x, y);
        }

        public override void Initialize()
        {
            InputManager.Instance.AddKeyHandler(Keys.Left, () =>
            {
                updatePathPos();
                _player.TurnLeft();
                _rotation = _player.GetRotation();
                logger.Info("Player turned left on: " + _player.Position);
            });

            InputManager.Instance.AddKeyHandler(Keys.Right, () =>
            {
                updatePathPos();
                _player.TurnRight();
                _rotation = _player.GetRotation();
                logger.Info("Player turned right on: " + _player.Position);
            });

            InputManager.Instance.AddKeyHandler(Keys.Up, () =>
            {
                logger.Info("Player attemping to move forwards on: " + _player.Position);
                updatePathPos();
                _player.MoveForward();
                _playerPostion.X = (_player.Position.X * _playerTexture.Width) + _playerTexture.Width / 2;
                _playerPostion.Y = (_player.Position.Y * _playerTexture.Height) + _playerTexture.Height / 2;
                logger.Info("Player moved forward to: " + _player.Position);
            });

            InputManager.Instance.AddKeyHandler(Keys.Down, () =>
            {
                logger.Info("Player attemping to move backwards on: " + _player.Position);
                updatePathPos();
                _player.MoveBackward();
                _playerPostion.X = (_player.Position.X * _playerTexture.Width) + _playerTexture.Width / 2;
                _playerPostion.Y = (_player.Position.Y * _playerTexture.Height) + _playerTexture.Height / 2;
                logger.Info("Player moved backward to: " + _player.Position);
            });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _playerTexture = _game.Content.Load<Texture2D>("playerSprite");
            _spriteSize = new Rectangle(0, 0, _playerTexture.Width, _playerTexture.Height);

            _playerPostion = new Vector2((_player.Position.X * _playerTexture.Width) + _playerTexture.Width / 2, (_player.Position.Y * _playerTexture.Height) + _playerTexture.Height / 2);
            _previousPos = _playerPostion;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            logger.Debug("Path texture being re-drawn on: " + _previousPos);
            _spriteBatch.Draw(PathTexture, _previousPos, _spriteSize, Color.White, 0f, new Vector2(_spriteSize.Size.X / 2, _spriteSize.Size.Y / 2), 1f, SpriteEffects.None, 1f);
            logger.Debug("Player texture being drawn on: " + _playerPostion);
            _spriteBatch.Draw(_playerTexture, _playerPostion, _spriteSize, Color.White, _rotation, new Vector2(_spriteSize.Size.X/2,_spriteSize.Size.Y/2), 1f, SpriteEffects.None, 1f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
