using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using Maze;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System;
using MazeFile = MazeFromFile.MazeFromFile;
using MazeRecursion;
using MazeHuntKill;
using System.Diagnostics;

namespace MazeGame;

public class MazeGame : Game
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IMapProvider _mapProvider;
    private IMap _map;
    private string _filePath;

    private PlayerSprite _player;
    private InputManager _inputManager;

    private Texture2D _wallTexture;
    private Texture2D _goalTexture;
    private Texture2D _pathSprite;
    private SpriteFont _font;
    private Color _colorFontFile;
    private Color _colorFontRecur;
    private Color _colorFontHunt;

    private int _widthSprite;
    private int _heightSprite;
    private Rectangle _spriteSize;
    private int _pixelSize;
    private int _menuSize => 20 * _pixelSize;

    //Menu flags and choice controller
    private bool _isMapDrawn;
    private bool _notInMenu;
    private int _menuChoice;

    //Menu Size flags and width and height fields
    private bool _inSizeMenu;
    private int _recursiveWidth;
    private int _recursiveHeight;
    private bool _reachedSizeMenu;

    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //Setting a default pixel size to shrink the window screen
        _pixelSize = 32;

        //Default font colours
        _colorFontFile = Color.White;
        _colorFontRecur = Color.White;
    }

    private void chooseMap()
    {
        //Setting a default _filePath value to ensure there is always a file chosen
        _filePath = null;
        while (_filePath == null)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    _filePath = openFileDialog.FileName;
                }
            }
        }

        //Map creation for file provider
        try
        {
            logger.Info("Map Chosen: " + _filePath);
            _mapProvider = new MazeFile(_filePath);
            _map = new Map(_mapProvider);

            _map.CreateMap();
            logger.Info("Goal position: " + _map.Goal);
        } catch (Exception e)
        {
            logger.Error("Error occured" + e);
        }

        initRestOfMap();
    }

    private void recursiveMap()
    {
        //Map creation for recursive map algorithm
        try
        { 
            _mapProvider = MazeRecursionFactory.GetProvider();
            _map = new Map(_mapProvider);
            _map.CreateMap(_recursiveWidth, _recursiveHeight); 
            logger.Info("Goal position: " + _map.Goal);
        }
        catch (Exception e)
        {
            logger.Error("Error occured" + e);
        }

        initRestOfMap();
    }

    private void huntKillMap()
    {
        //Map creation for hunt kill map algorithm
        try
        {
            _mapProvider = MazeHuntKillFactory.GetProvider();
            _map = new Map(_mapProvider);
            _map.CreateMap(_recursiveWidth, _recursiveHeight);
            logger.Info("Goal position: " + _map.Goal);
        }
        catch (Exception e)
        {
            logger.Error("Error occured" + e);
        }

        initRestOfMap();
    }

    private void initRestOfMap()
    {
        // Init playerSprite
        _player = new PlayerSprite(this, _map.Player);
        _player.PathTexture = _pathSprite;
        logger.Info("Player starting position: " + _map.Player.Position);
        Components.Add(_player);

        //Setup window size based on map
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = _map.Width * _pixelSize;
        _graphics.PreferredBackBufferHeight = _map.Height * _pixelSize;
        _graphics.ApplyChanges();

        //Set bool isMapDrawn
        _isMapDrawn = false;
    }

    //Clean up old maze to setup new maze
    private void cleanup()
    {
        _recursiveWidth = 5;
        _recursiveHeight = 5;
        _reachedSizeMenu = false;
        GraphicsDevice.Clear(Color.Black);
        Components.Remove(_player);
        _player = null;
    }

    //Set default screen menu size
    private void defaultMenuSize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = _menuSize;
        _graphics.PreferredBackBufferHeight = _menuSize;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        //Init InputManager and flags
        _inputManager = InputManager.Instance;
        _inSizeMenu = false;
        _reachedSizeMenu = false;

        //Escape to quit
        _inputManager.AddKeyHandler(Keys.Escape, () =>
        {
            logger.Info("Program exit due to Key.Escape");
            this.Exit();
        });

        //Menu arrows
        _inputManager.AddKeyHandler(Keys.Up, () =>
        {
            if (!_notInMenu)
            {
                logger.Info("Up key pressed for menu");
                _menuChoice -= 1;
                if (_menuChoice < 0)
                {
                    _menuChoice = 0;
                }
            }
        });

        _inputManager.AddKeyHandler(Keys.Down, () =>
        {
            if (!_notInMenu)
            {
                logger.Info("Down key pressed for menu");
                _menuChoice += 1;
                if (_menuChoice > 2)
                {
                    _menuChoice = 2;
                }
            }
        });

        //Create Key handlers to select size
        _inputManager.AddKeyHandler(Keys.Up, () =>
        {
            if (_inSizeMenu)
            {
                logger.Info("Up key pressed for recursive size menu");
                _recursiveHeight += 2;
            }
        });

        _inputManager.AddKeyHandler(Keys.Down, () =>
        {
            if (_inSizeMenu)
            {
                logger.Info("Down key pressed for recursive size menu");
                if (_recursiveHeight - 2 < 5)
                {
                    _recursiveHeight = 5;
                }
                else
                {
                    _recursiveHeight -= 2;
                }
            }
        });

        _inputManager.AddKeyHandler(Keys.Left, () =>
        {
            if (_inSizeMenu)
            {
                logger.Info("Left key pressed for recursive size menu");
                if (_recursiveWidth - 2 < 5)
                {
                    _recursiveWidth = 5;
                }
                else
                {
                    _recursiveWidth -= 2;
                }
            }
        });

        _inputManager.AddKeyHandler(Keys.Right, () =>
        {
            if (_inSizeMenu)
            {
                logger.Info("Right key pressed for recursive size menu");
                _recursiveWidth += 2;
            }
        });

        //Choice enter
        _inputManager.AddKeyHandler(Keys.Enter, () =>
        {
            if (!_notInMenu)
            {
                _notInMenu = true;
                logger.Info("Enter key pressed for menu");
                if (_menuChoice == 0)
                {
                    chooseMap();
                }
                else if (_menuChoice == 1)
                {
                    _recursiveHeight = 5;
                    _recursiveWidth = 5;
                    _inSizeMenu = true;
                } else if (_menuChoice == 2)
                {
                    _recursiveHeight = 5;
                    _recursiveWidth = 5;
                    _inSizeMenu = true;
                }
            }
        });

        //Enter keyhandler runs only when in Size Menu
        _inputManager.AddKeyHandler(Keys.Enter, () =>
        {
            if (_inSizeMenu && _reachedSizeMenu)
            {
                _inSizeMenu = false;
                if (_menuChoice == 1)
                {
                    recursiveMap();
                } else
                {
                    huntKillMap();
                }
                
            }
        });

        //Default screen menu size
        defaultMenuSize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        //Getting width and height based on textures
        _wallTexture = Content.Load<Texture2D>("wallSprite");
        _widthSprite = _wallTexture.Width;
        _heightSprite = _wallTexture.Height;
        _spriteSize = new Rectangle(0, 0, _widthSprite, _heightSprite);

        _goalTexture = Content.Load<Texture2D>("goalSprite");
        _pathSprite = Content.Load<Texture2D>("pathSprite");
        _font = this.Content.Load<SpriteFont>("font");

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();

        //Changing choice colors
        if (_menuChoice == 0)
        {
            _colorFontFile = Color.Red;
            _colorFontRecur = Color.White;
            _colorFontHunt = Color.White;
        } else if (_menuChoice == 1)
        {
            _colorFontFile = Color.White;
            _colorFontRecur = Color.Red;
            _colorFontHunt = Color.White;
        } else if (_menuChoice == 2)
        {
            _colorFontFile = Color.White;
            _colorFontRecur = Color.White;
            _colorFontHunt = Color.Red;
        }

        //If player reached goal, then restart and set to default menu size
        if (_isMapDrawn && _map.IsGameFinished)
        {
            logger.Info("Player reached goal on: " + _map.Goal);
            cleanup();
            _isMapDrawn = false;
            _notInMenu = false;

            defaultMenuSize();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!_notInMenu)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "File Provider", new Vector2(10, 10), _colorFontFile);
            _spriteBatch.DrawString(_font, "Recursive", new Vector2(10, 70), _colorFontRecur);
            _spriteBatch.DrawString(_font, "Hunt Kill", new Vector2(10, 130), _colorFontHunt);
            _spriteBatch.DrawString(_font, "ESC to exit", new Vector2(10, 570), Color.White);
            _spriteBatch.End();
        }

        if (_notInMenu && _inSizeMenu)
        {
            _reachedSizeMenu = true;
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Width:" + _recursiveWidth, new Vector2(10, 140), Color.White);
            _spriteBatch.DrawString(_font, "Height:" + _recursiveHeight, new Vector2(300, 140), Color.White);
            _spriteBatch.DrawString(_font, "Use all 4 keys", new Vector2(10, 570), Color.White);
            _spriteBatch.End();
        }

        if (_notInMenu && !_inSizeMenu && !_isMapDrawn)
        {
            _spriteBatch.Begin();
            for (int y = 0; y < _map.MapGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _map.MapGrid.GetLength(1); x++)
                {
                    if (_map.MapGrid[y, x] == Block.Solid)
                    {
                        Vector2 pos = new Vector2(x * _widthSprite, y * _heightSprite);
                        _spriteBatch.Draw(_wallTexture, pos, _spriteSize, Color.White);
                        logger.Debug("Wall texture being drawn on: " + pos);
                    }
                    else if (x == _map.Goal.X && y == _map.Goal.Y)
                    {
                        Vector2 pos = new Vector2(x * _widthSprite, y * _heightSprite);
                        _spriteBatch.Draw(_goalTexture, pos, _spriteSize, Color.White);
                        logger.Debug("Goal texture being drawn on: " + pos);
                    }
                    else
                    {
                        Vector2 pos = new Vector2(x * _widthSprite, y * _heightSprite);
                        _spriteBatch.Draw(_pathSprite, pos, _spriteSize, Color.White);
                        logger.Debug("Path texture being drawn on: " + pos);
                    }
                }
            }
            _spriteBatch.End();
            _isMapDrawn = true;
        }
        base.Draw(gameTime);
    }
}
