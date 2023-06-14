using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace projetoTecnicas2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _backgroundTexture;
        private int score = 0;
        private SpriteFont font;
        private Song _song;
        private SoundEffect shootSound;
        private SoundEffect enemyDeathSound;
        //private Texture2D startButtonTexture;
        //private Texture2D exitButtonTexture;
        private float _volume = 0.5f;
        Ship ship;
        List<Enemy> listEnemys = new List<Enemy>();
        int lines = 1;
        int columns = 13;
        int distanceX = 50;
        int distanceY = 50;

        //private MenuScreen _menu;
        //private bool _showMenu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //_graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            //_graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            //_graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //_menu = new MenuScreen(_spriteBatch, font, startButtonTexture, exitButtonTexture);
            //_menu.StartButtonClicked += Menu_StartButtonClicked;
            //_menu.ExitButtonClicked += Menu_ExitButtonClicked;

            //_showMenu = true;

            base.Initialize();
        }

        /*private void Menu_StartButtonClicked(object sender, EventArgs e)
        {
            
            // TODO: Add your code to start the game
            _showMenu = false;

            // Enable the ship and set its initial position
            ship.Enabled = true;
            ship.Position = new Vector2(350, 400);

            // Reset the score
            score = 0;

            // Clear existing enemies
            listEnemys.Clear();

            // Initialize new enemies
            int posX = 0;
            int posY = distanceY;

            Random random = new Random();

            for (int l = 0; l <= lines; l++)
            {
                for (int c = 0; c <= columns; c++)
                {
                    Enemy enemy = new Enemy(this);
                    posX += distanceX;
                    enemy.Position = new Vector2(posX, posY);
                    enemy.Time = random.Next(1000, 10000);
                    listEnemys.Add(enemy);
                }
                posX = 0;
                posY += distanceY;
            }
        }

        private void Menu_ExitButtonClicked(object sender, EventArgs e)
        {
            Exit();
        }*/

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("File");

            //startButtonTexture = Content.Load<Texture2D>("startButton");
            //exitButtonTexture = Content.Load<Texture2D>("startButton");

            // Load background texture
            _backgroundTexture = Content.Load<Texture2D>("background");
            // Load sounds
            _song = Content.Load<Song>("sound");
            shootSound = Content.Load<SoundEffect>("8-bit-kit-lazer-5");
            enemyDeathSound = Content.Load<SoundEffect>("8-bit-kit-explosion-1");

            // Play background music
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(_song);

            // Create ship instance
            ship = new Ship(this);
            ship.Position = new Vector2(350, 400);

            int posX = 0;
            int posY = distanceY;

            Random random = new Random();

            for (int l = 0; l <= lines; l++)
            {
                for (int c = 0; c <= columns; c++)
                {
                    Enemy enemy = new Enemy(this);
                    posX += distanceX;
                    enemy.Position = new Vector2(posX, posY);
                    enemy.Time = random.Next(1000, 10000);
                    listEnemys.Add(enemy);
                }
                posX = 0;
                posY += distanceY;
            }

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (_showMenu)
            {
                _menu.Update();
            }
            else
            {*/
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                // Control volume
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    _volume += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    _volume -= 0.1f;
                }

                _volume = MathHelper.Clamp(_volume, 0.0f, 1.0f);
                MediaPlayer.Volume = _volume;


                // Play shoot sound
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    SoundEffectInstance instance = shootSound.CreateInstance();
                    instance.Play();
                }

                if (ship.Enabled)
                {
                    ship.Update(gameTime);
                }

                for (int x = 0; x <= listEnemys.Count - 1; x++)
                {
                    listEnemys[x].Collide(ship.listShoot);

                    if (listEnemys[x].Enabled == false)
                    {
                        SoundEffectInstance instance = enemyDeathSound.CreateInstance();
                        instance.Play();
                        listEnemys.RemoveAt(x);
                        score += 1;
                    }
                    else
                    {
                        listEnemys[x].Update(gameTime);
                        ship.Collide(listEnemys[x].listShoot);
                    }
                }
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            /*if (_showMenu)
            {
                _menu.Draw();
            }
            else
            {*/
                _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);

                _spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);

                if (ship.Visible)
                {
                    ship.Draw(_spriteBatch, gameTime);
                }

            foreach (Enemy e in listEnemys)
                {
                    e.Draw(_spriteBatch, gameTime);
                }
            //}
            if (ship.Visible == false)
            {
                Exit();
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
