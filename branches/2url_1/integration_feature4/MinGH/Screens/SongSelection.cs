using System;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screen;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Enum;
using MinGH.Events;
using FlatRedBall.Graphics;
using Microsoft.Xna.Framework;
using MinGH.Util;

namespace MinGH.Screens
{
    class SongSelection : Screen
    {
        Text screenTitle;
        ListBox listBox;

        public SongSelection()
            : base("SongSelection")
        { }

        public override void Initialize(bool addToManagers)
        {
            base.Initialize(addToManagers);

            screenTitle = new Text();
            screenTitle.DisplayText = "Song Selection";
            Vector2 titlePosition = PositionHelper.percentToCoordSprite(0.0f, 80.0f);
            screenTitle.Y = titlePosition.Y;
            screenTitle.HorizontalAlignment = HorizontalAlignment.Center;
            TextManager.AddText(screenTitle);

            GuiManager.IsUIEnabled = true;
            listBox = GuiManager.AddListBox();
            listBox.ScaleX = 10;
            listBox.ScaleY = 20;

            for (int i = 0; i < 200; i++)
                listBox.AddItem("Song " + i);

            if(addToManagers)
                AddToManagers();
        }

        public override void AddToManagers()
        {
            // Feature Commit!!!
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
