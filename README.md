# projetoTECNICAS2

Este trabalho foi realizado por Gabriel Costa(25957) e Rafael Enes(25980).

Este projeto foi feito no ambito da disciplina de Técnicas de Desenvolvimento de Video Jogos.

O nosso jogo tem como inspiração o jogo ***Space Invaders***.

**Solution**:

![image](https://github.com/GabrielCosta123/projetoTECNICAS2/assets/120459962/26219475-10e9-4ea7-addd-ac05c51f1a09)


Na solução criamos as seguintes classes: 

->Enemy;

->Entity;

->KeyboardHelper(Retirado de http://cafecomengine.blogspot.com/search?updated-max=2014-09-04T14:37:00-03:00&max-results=7);

->Ship;

->Shoot;

->ShootEnemy;

Para além das classe ***default*** do Monogame(Game1 e Program).

Para jogar o jogo é bastante simples, no teclado a setas para a esquerda e para a direita fazem a nave ir para a esquerda ou para a direita,respetivamente.
Para atirar nos inimigos utiliza a tecla Space no teclado.



Classes:

Entity

```cs
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
```

Enemy

```cs
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
```

KeyboardHelper

```cs
public class KeyboardHelper
    {
        public KeyboardState State;
        public KeyboardState LastState;

        public void Update(GameTime gameTime)
        {
            LastState = State;
            State = Keyboard.GetState();
        }

        public bool IsDown(Keys key)
        {
            bool result = false;

            if (LastState.IsKeyDown(key) && State.IsKeyDown(key))
                result = true;

            return result;
        }

        public bool IsPress(Keys key)
        {
            bool result = false;

            if (LastState.IsKeyUp(key) && State.IsKeyDown(key))
                result = true;

            return result;
        }

        public bool IsUp(Keys key)
        {
            bool result = false;

            if (LastState.IsKeyUp(key) && State.IsKeyUp(key))
                result = true;

            return result;
        }
    }
```

Ship

```cs
class Ship : Entity
    {
        Shoot shoot;
        public List<Shoot> listShoot;
        

        public Ship(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("naveNormal");
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
```

Shoot

```cs
 class Shoot : Entity
    {
        public Shoot(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("shoot");
        }

        public Shoot(Shoot s) : base(s) { }

        public override void Update(GameTime gameTime)
        {
            Position -= new Vector2(0, 5);

            if (Position.Y < 0)
                Enabled = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
```

ShootEnemy

```cs
class ShootEnemy : Entity
    {
        public ShootEnemy(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("shoot");
        }

        public ShootEnemy(ShootEnemy se) : base(se) { }

        public override void Update(GameTime gameTime)
        {
            Position += new Vector2(0, 5);

            if (Position.Y > GameInstance.GraphicsDevice.Viewport.Height)
                Enabled = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
```
