using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace projetoTecnicas2
{
    class MenuScreen 
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private Texture2D _startButtonTexture;
        private Texture2D _exitButtonTexture;
        private Rectangle _startButtonRectangle;
        private Rectangle _exitButtonRectangle;

        public event EventHandler StartButtonClicked;
        public event EventHandler ExitButtonClicked;

        public MenuScreen(SpriteBatch spriteBatch, SpriteFont font, Texture2D startButtonTexture, Texture2D exitButtonTexture)
        {
            _spriteBatch = spriteBatch;
            _font = font;

            _startButtonTexture = startButtonTexture;
            _exitButtonTexture = exitButtonTexture;

            // Position and size of the buttons
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 20;
            int buttonY = 200;

            // Calculate button rectangles based on screen width and button dimensions
            int screenWidth = _spriteBatch.GraphicsDevice.Viewport.Width;
            int startX = (screenWidth - buttonWidth) / 2;

            _startButtonRectangle = new Rectangle(startX, buttonY, buttonWidth, buttonHeight);
            _exitButtonRectangle = new Rectangle(startX, buttonY + buttonHeight + buttonSpacing, buttonWidth, buttonHeight);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();

            if (_startButtonRectangle.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                OnStartButtonClicked();
            }
            else if (_exitButtonRectangle.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                OnExitButtonClicked();
            }
        }

        private void OnStartButtonClicked()
        {
            StartButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitButtonClicked()
        {
            ExitButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public void Draw()
        {
            _spriteBatch.Begin();

            // Draw the buttons
            _spriteBatch.Draw(_startButtonTexture, _startButtonRectangle, Color.White);
            _spriteBatch.Draw(_exitButtonTexture, _exitButtonRectangle, Color.White);

            _spriteBatch.End();
        }

    }
}
