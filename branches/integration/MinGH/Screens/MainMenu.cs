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
    class MainMenu : Screen
    {
        Text screenTitle;
        ListBox menuList;

        public MainMenu()
            : base("MainMenu")
        {
        }

        public override void Initialize(bool addToManagers)
        {
            base.Initialize(addToManagers);

            screenTitle = new Text();
            screenTitle.DisplayText = "Main Menu";
            Vector2 titlePosition = PositionHelper.percentToCoordSprite(0.0f, 50.0f);
            screenTitle.Y = titlePosition.Y;
            screenTitle.HorizontalAlignment = HorizontalAlignment.Center;
            TextManager.AddText(screenTitle);

            GuiManager.IsUIEnabled = true;
            menuList = GuiManager.AddListBox();
            menuList.ScaleX = 10;
            menuList.ScaleY = 10;
            menuList.ScrollBarVisible = false;
            Vector2 menuPosition = PositionHelper.percentToCoordGUI(50.0f, 50.0f);
            menuList.Y = menuPosition.Y;
            menuList.AddItem("Single Player", GameState.SongSelection);
            menuList.AddItem("Options", GameState.Options);
            menuList.AddItem("Quit", GameState.QuitGame);
            
            menuList.HighlightItem("Single Player");
            menuList.CallClick();

            if(addToManagers)
                AddToManagers();
        }

        public override void AddToManagers()
        {
            
            base.AddToManagers();
        }

        public override void Activity(bool firstTimeCalled)
        {
            
            bool escapeKeyPressed = InputManager.Keyboard.KeyPushed(Keys.Escape);
            if (escapeKeyPressed)
            {
                ScreenChange newEvent = new ScreenChange(GameState.QuitGame);
                EventBus<ScreenChange>.instance.fireEvent(this, newEvent);
            }

            bool enterKeyPressed = InputManager.Keyboard.KeyPushed(Keys.Enter);
            if (enterKeyPressed)
            {
                // Get selected item
                GameState newState = (GameState)menuList.GetFirstHighlightedObject();
                ScreenChange newEvent = new ScreenChange(newState);
                EventBus<ScreenChange>.instance.fireEvent(this, newEvent);
            }

            base.Activity(firstTimeCalled);
        }

        public override void Destroy()
        {
            GuiManager.RemoveWindow(menuList);
            TextManager.RemoveText(screenTitle);
            base.Destroy();
        }
    }
}
