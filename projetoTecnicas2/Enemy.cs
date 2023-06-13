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
    class Enemy : Entity
    {
        ShootEnemy shoot;
        public List<ShootEnemy> listShoot;
        int elapsedTime = 0;

        public int Time { get; set; }


        public Enemy(Game game): base(game) 
        {
            Texture = game.Content.Load<Texture2D>("enm");
            shoot=new ShootEnemy(game);
            listShoot= new List<ShootEnemy>();
            Time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if(elapsedTime > Time)
            {
                ShootEnemy se = new ShootEnemy(shoot);
                se.Position = new Vector2(Position.X + (Texture.Width / 2), Position.Y + Texture.Height);
                elapsedTime= 0;
                listShoot.Add(se);
            }
            for(int x = 0 ; x <= listShoot.Count -1 ; x++)
            {
                if (listShoot[x].Enabled == false)
                {
                    listShoot.RemoveAt(x);
                }
                else
                {
                    listShoot[x].Update(gameTime);
                }
            }
            

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            

            foreach(ShootEnemy se in listShoot)
            {
                se.Draw(spriteBatch, gameTime);
            }
            base.Draw(spriteBatch, gameTime);
        }

        public void Collide(List<Shoot> listShoot)
        {
            foreach(Shoot s in listShoot)
            {
                Rectangle shootRectangle = new Rectangle((int)s.Position.X,(int)s.Position.Y,s.Texture.Width,s.Texture.Height);
                Rectangle enmRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

                if(shootRectangle.Intersects(enmRectangle))
                {
                    s.Enabled = false;
                    this.Enabled = false;


                }
            }
        }

    }
}
