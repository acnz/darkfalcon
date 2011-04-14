using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace DarkFalcon
{
    /// <summary>
    /// A game component.
    /// Has an associated rectangle.
    /// Accepts touch and click inside the rectangle.
    /// Has a state of IsTouching and IsClicked.
    /// </summary>
    public class _3DClickable
    {

        #region Fields
        Rectangle rectangle;
        public bool wasClicking;
        bool isClicking;
        public float alpha=1f;
        bool _on=true;
        PcView _game;

        #region Protected accessors
        public bool On
        {
            get { return _on; }
            set
            {
                if (value)
                    alpha = 1f;
                else
                    alpha = 0f;
                _on = value;
            }
        }
        public bool IsClicking { get { return isClicking; } }
        public bool IsClicked { get { return (wasClicking == true) && (isClicking == false); } }

        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }
        public PcView Game { get { return _game; } }
        #endregion
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The Game oject</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        public _3DClickable(PcView game)
        {
            _game = game;
        }
        #endregion

        #region Input handling
        /// <summary>
        /// Handles Input
        /// </summary>
        protected void HandleInput()
        {
            wasClicking = isClicking;
            isClicking = false;

            MouseState mouse = Mouse.GetState();
            var position = new Vector2(mouse.X,mouse.Y);

            Rectangle touchRect = new Rectangle((int)position.X, (int)position.Y ,1, 1);

                if (rectangle.Intersects(touchRect) && mouse.LeftButton == ButtonState.Pressed)
                    isClicking = true;
            }


        public virtual void Update()
        {
            HandleInput();
        }

        public virtual void Draw()
        {
        }
    }
        #endregion
    }



