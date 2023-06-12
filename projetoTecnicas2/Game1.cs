using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;

namespace projetoTecnicas2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Ship ship;
        List<Enemy> listEnemys=new List<Enemy>();
        int lines = 4;
        int columns = 13;
        int distanceX = 50;
        int distanceY = 50;


        public Game1()
        {


            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ship = new Ship(this);
            ship.Position = new Vector2(350, 400);

            int posX = 0;
            int posY = distanceY;

            Random random= new Random();

            for(int l=0 ; l <= lines ; l++) 
            {
                for(int c=0 ; c<=columns ; c++)
                {
                    Enemy enemy = new Enemy(this);
                    posX += distanceX;
                    enemy.Position= new Vector2(posX, posY);
                    enemy.Time = random.Next(1000, 20000);
                    listEnemys.Add(enemy);
                }
                posX= 0;
                posY+= distanceY;

            }
           
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if(ship.Enabled)
            {
                ship.Update(gameTime);
            }

            for(int x= 0 ; x <=listEnemys.Count -1 ; x++)
            {
                listEnemys[x].Collide(ship.listShoot);

                if(listEnemys[x].Enabled == false)
                {
                    listEnemys.RemoveAt(x);
                }
                else
                {
                    listEnemys[x].Update(gameTime);
                    ship.Collide(listEnemys[x].listShoot);
                }
            }
            
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

    

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (ship.Visible)
            {
                ship.Draw(_spriteBatch, gameTime);
            }

            if (ship.Visible == false)
            {
                Exit();                            
            }

            foreach(Enemy e in listEnemys)
            {
                e.Draw(_spriteBatch, gameTime);
            }
             
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}