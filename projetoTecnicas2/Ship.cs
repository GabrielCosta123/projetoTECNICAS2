using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace projetoTecnicas2
{
    class Ship : Entity
    {
        Shoot shoot;
        public List<Shoot> listShoot;

        public Ship(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("red_ship");
            shoot = new Shoot(game);
            listShoot = new List<Shoot>();
        }

        public override void Update(GameTime gameTime)
        {            
            //Verificação das teclas para ações da nave
            if (Keyboard.IsDown(Keys.Right))
            {
                Position += new Vector2(5, 0);
            }
            else if (Keyboard.IsDown(Keys.Left))
            {
                Position -= new Vector2(5, 0);
            }
                
            
            if(Keyboard.IsPress(Keys.Space))
            {
                Shoot tempShoot = new Shoot(shoot);
                tempShoot.Position = new Vector2(Position.X + (Texture.Width / 2), Position.Y);

                listShoot.Add(tempShoot);
            }

            for(int x = 0; x <= listShoot.Count - 1; x++)
            {
                if (listShoot[x].Enabled == false)
                    listShoot.RemoveAt(x);
                else
                    listShoot[x].Update(gameTime);
            }

            //Verificação da posição da nave com relação a tela
            if (Position.X < GameInstance.GraphicsDevice.Viewport.X)
                Position = new Vector2(0, Position.Y);
            else if (Position.X + Texture.Width > GameInstance.GraphicsDevice.Viewport.Width)
                Position = new Vector2(GameInstance.GraphicsDevice.Viewport.Width - Texture.Width, Position.Y);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            foreach(Shoot s in listShoot)
            {
                s.Draw(spriteBatch, gameTime);
            }
        }

        public void Collide(List<ShootEnemy> listShoot)
        {
            foreach (ShootEnemy se in listShoot)
            {
                Rectangle shootRectangle = new Rectangle((int)se.Position.X, (int)se.Position.Y, se.Texture.Width, se.Texture.Height);
                Rectangle enmRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

                if (shootRectangle.Intersects(enmRectangle))
                {
                    se.Enabled = false;
                    this.Enabled = false;
                    this.Visible= false;


                }
            }
        }
    }
}
