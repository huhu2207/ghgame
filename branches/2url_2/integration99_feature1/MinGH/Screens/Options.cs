using System;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screen;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Enum;
using MinGH.Events;
using MinGH.Util;
using Microsoft.Xna.Framework;
using FlatRedBall.Graphics;

namespace MinGH.Screens
{
    class Options : Screen
    {
        ListBox listBox;
        Text screenTitle;

        public Options()
            : base("Options")
        {
        }

        public override void Initialize(bool addToManagers)
        {
            base.Initialize(addToManagers);

            screenTitle = new Text();
            screenTitle.DisplayText = "Options";
            Vector2 titlePosition = PositionHelper.percentToCoordSprite(0.0f, 50.0f);
            screenTitle.Y = titlePosition.Y;
            screenTitle.HorizontalAlignment = HorizontalAlignment.Center;
            TextManager.AddText(screenTitle);

            GuiManager.IsUIEnabled = true;
            listBox = GuiManager.AddListBox();
            listBox.ScaleX = 20;
            listBox.ScaleY = 12;
            // The first argument to AddItem is the text to show for the item
            // The second argument is the object that the Item will store. 
            // The second argment is optional, but we'll use it to reference
            // the background color that we want to use.
            listBox.AddItem("Red", Color.Red);
            listBox.AddItem("Blue", Color.Blue);
            listBox.AddItem("Yellow", Color.Yellow);

            for (int i = 0; i < 200; i++)
                listBox.AddItem("BLAAA" + i);

            if(addToManagers)
                AddToManagers();
        }

        public override void AddToManagers()
        {
            
            base.AddToManagers();
        }

        public override void Activity(bool firstTimeCalled)
        {
            Boolean escapeKeyPressed = InputManager.Keyboard.KeyPushed(Keys.Escape);

            if (escapeKeyPressed)
            {
                ScreenChange newEvent = new ScreenChange(GameState.MainMenu);
                EventBus<ScreenChange>.instance.fireEvent(this, newEvent);
            }

            base.Activity(firstTimeCalled);
        }

        public override void Destroy()
        {
            TextManager.RemoveText(screenTitle);
            GuiManager.RemoveWindow(listBox);
            base.Destroy();
        }
    }
}
