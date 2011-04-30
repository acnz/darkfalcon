        /*
xWinForms © 2007-2009
Eric Grossinger - ericgrossinger@gmail.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DarkFalcon.gui
{
    public class _ComboBox : _Control
    {
        _Textbox textbox;        
        _Button button;
        _Listbox listbox;
        
        public List<string> Items { get { return listbox.Items; } set { listbox.Items = value; } }

        public bool Opened { get { return listbox.Visible; } }
        public bool Locked { get { return textbox.Locked; } set { textbox.Locked = value; } }
        public new string Text { get { return textbox.Text; } set { textbox.Text = value; } }

        public EventHandler OnSelectionChanged = null;
        bool justOpened = false;

        string[] items;
        public _ComboBox(hud pai,string name, Vector2 position, int width, string[] items)
            : base(pai,name, position)
        {
            this.Name = name;
            this.Position = position;
            this.Width = width;
            this.items = items;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            base.Initialize(content, graphics);
            // TODO: load your content here
            textbox = new _Textbox(Owner,"combotext", Position, (int)Width);
            textbox.Font = this.Font;
            textbox.Owner = this.Owner;
            textbox.Locked = true;
            textbox.Initialize(content, graphics);

            button = new _Button(Owner,"btOpen","", Position + new Vector2(Width - 5, 0f), "combobox", 1f);
            button.Font = this.Font;
            button.Owner = this.Owner;
            button.OnPress = Button_OnPress;
            button.Initialize(content, graphics);

            listbox = new _Listbox(Owner, "combolist", Position + new Vector2(0, textbox.Height), (int)Width, 8 * Font.LineSpacing, items);
            listbox.Font = this.Font;
            listbox.Owner = this.Owner;
            listbox.Visible = false;
            listbox.OnChangeSelection = Listbox_OnChangeSelection;
            listbox.OnPress = Listbox_OnPress;
            listbox.Initialize(content, graphics);

            
        }

        public override void Dispose()
        {
            // TODO: dispose of your content here
            textbox.Dispose();
            button.Dispose();
            listbox.Dispose();

            base.Dispose();
        }

        private void Button_OnPress(object obj, EventArgs e)
        {
            if (!listbox.Visible)
                Open();
            else
                Close();
        }

        private void Open()
        {
            justOpened = true;
            listbox.Visible = true;
        }

        private void Close()
        {
            listbox.Visible = false;
        }

        private void Listbox_OnChangeSelection(object obj, EventArgs e)
        {
            string previousItem = textbox.Text;
            textbox.Text = listbox.SelectedItem;

            if (textbox.Text != previousItem && OnSelectionChanged != null)
                OnSelectionChanged(listbox.SelectedItem, null);

            Close();
        }

        private void Listbox_OnPress(object obj, EventArgs e)
        {
        }

        public override void Update()
        {
            base.Update();

            // TODO: Add your update logic here
            textbox.Update();
            button.Update();

            if (listbox.Visible)
            {
                listbox.Update();

                if (wasPressed )
                    if (!listbox.a2 && !justOpened)
                    Close();

                if (wasReleased)
                    justOpened = false;
            }
        }

        public override void Draw()
        {
            // TODO: Add your drawing code here
            textbox.Draw();
            button.Draw();
            DrawOverlay();
        }

        public void DrawOverlay()
        {
            if (listbox.Visible)
                listbox.Draw();
        }
    }
}
