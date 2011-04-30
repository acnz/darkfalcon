using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace DarkFalcon.gui
{
    /// <summary>
    /// A game component.
    /// Has an associated rectangle.
    /// Accepts touch and click inside the rectangle.
    /// Has a state of IsTouching and IsClicked.
    /// </summary>
    public class _Control : i_Control

    {

        #region Fields
        string name = string.Empty;
        Vector2 position = Vector2.Zero;
        Vector2 size = Vector2.Zero;
        string text = "";
        bool enabled = true;
        bool visible = true;
        string tooltipText;
        public float alpha=1f;
        bool isDisposed=true;
        PcView _game;
        hud owner;
        internal MouseState mNew;
        internal MouseState mOld;
        internal SpriteBatch spriteBatch;
        internal SpriteFont Font;
        internal bool a1, a2, wasPressed, wasReleased;

        public Rectangle area = Rectangle.Empty;

        #region Public accessors
        public PcView Game { get { return _game; } }



        public string Name { get { return name; } set { name = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                area.Width = (int)size.X;
                area.Height = (int)size.Y;
            }
        }
        public string Text { get { return text; } set { text = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }
        public bool IsDisposed { get { return isDisposed; } set { isDisposed = value; } }
        public hud Owner { get { return owner; } set { owner = value; } }
        public string ToolTipText { get { return tooltipText; } set { tooltipText = value; } }

        public float Top { get { return position.Y; } set { position.Y = value; } }
        public float Left { get { return position.X; } set { position.X = value; } }
        public float Width
        {
            get { return size.X; }
            set
            {
                size.X = value;
                area.Width = (int)value;
            }
        }
        public float Height
        {
            get { return size.Y; }
            set
            {
                size.Y = value;
                area.Height = (int)value;
            }
        }
        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }

        EventHandler onMouseOver;
        EventHandler onMouseOut;
        EventHandler onPress;
        EventHandler onRelease;

        public EventHandler OnMouseOver { get { return onMouseOver; } set { onMouseOver += value; } }
        public EventHandler OnMouseOut { get { return onMouseOut; } set { onMouseOut += value; } }
        public EventHandler OnPress { get { return onPress; } set { onPress += value; } }
        public EventHandler OnRelease { get { return onRelease; } set { onRelease += value; } }

        #endregion
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The Game oject</param>
        /// <param name="targetRectangle">Position of the component on the screen</param>
        public _Control(hud pai) { 
            this._game = pai.Game;
        this.owner = pai;

        }
        public _Control(hud pai,string name)
        {
            this.owner = pai;
            this.name = name;
            this._game = pai.Game;
        }
        public _Control(hud pai, string name, Vector2 position)
        {
            this._game = pai.Game;
            this.owner = pai;
            this.name = name;
            this.position = position;
            
        }
        virtual public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            isDisposed = false;
            //this.owner.add(this);
            spriteBatch = Game.spriteBatch;
            Font = Game.hudf;
            area.X = (int)(Position.X);
            area.Y = (int)(Position.Y);
        }
        virtual public void Dispose()
        {
            isDisposed = true;
            this.owner.remove(this);
        }
        #endregion

        #region Input handling
        
        virtual public void Update()
        {
            mNew = Mouse.GetState();
            mOld = Owner.Game.prevMouse;
            a1 = Owner.area.Contains(area);
            a2 = area.Contains(mNew.X, mNew.Y);
            wasPressed = false;
            if ((mNew.LeftButton == ButtonState.Pressed) && (mOld.LeftButton == ButtonState.Released))
                wasPressed = true;
            else
                wasPressed = false;
            wasReleased = false;
            if ((mNew.LeftButton == ButtonState.Released) && (mOld.LeftButton == ButtonState.Pressed))
                wasReleased = true;
            else
                wasReleased = false;


       }

        public virtual void Draw()
        {
        }
    }
        #endregion
    }



