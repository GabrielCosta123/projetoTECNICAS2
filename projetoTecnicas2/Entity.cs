using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Helper.Input;

namespace projetoTecnicas2
{
    class Entity
    {
        protected Game GameInstance { get; set; }
        public Texture2D Texture { get; set; }
        protected KeyboardHelper Keyboard { get; set; }
        public Vector2 Position { get; set; }
        public Color TheColor { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        public Entity(Game game)
        {
            GameInstance = game;
            Position = Vector2.Zero;
            Texture = null;
            TheColor = Color.White;
            Keyboard = new KeyboardHelper();
            Enabled = true;
            Visible = true;
        }

        //Construtor para clonagem
        public Entity(Entity e)
        {
            GameInstance = e.GameInstance;
            Position = e.Position;
            Texture = e.Texture;
            TheColor = e.TheColor;
            Keyboard = e.Keyboard;
            Enabled = e.Enabled;
            Visible = e.Visible;
        }

        public virtual void Update(GameTime gameTime)
        {
            Keyboard.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, TheColor);
        }
    }
}