using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chart_View
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main_Game : Microsoft.Xna.Framework.Game
    {
        // Global Content
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  // Draws the shapes
        SpriteFont game_font;  // The font the game will use
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex;  // The background texture
        
        gameObject[,] Notes;  // Will hold every note currently on the screen
        const int Max_Notes_Onscreen = 50;  // Maximum amount of a single note (i.e. how many reds per frame)
        const int Note_Velocity = 9;  // Current speed in which the notes will move (hyperspeed)
        int[] note_iterators;  // These iterators are used to keep track of which note to observe next
        int bpm_iterator = 0;  // Keeps track of what bpm change is to come next
        
        Chart main_chart;  // Create the chart file
        double current_tick = 0.0;  // Tracks the current tick the chart is on
        double ticks_per_msecond = 0.0;  // How many ticks pass per milisecond
        List<GameString>  str_array;  // Stores each string and its position on the screen
        

        public Main_Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize some variables
            note_iterators = new int[5];
            str_array = new List<GameString>();
            Notes = new gameObject[5, Max_Notes_Onscreen];
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            game_font = Content.Load<SpriteFont>("Text");  // Load the font
            main_chart = new Chart("chart.chart");  // Setup the chart
            backgroundTex = Content.Load<Texture2D>("Backgrounds\\Chart_View_Background");

            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:
                            Notes[i,j] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note_Green"));
                            break;
                        case 1:
                            Notes[i,j] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note_Red"));
                            break;
                        case 2:
                            Notes[i,j] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note_Yellow"));
                            break;
                        case 3:
                            Notes[i,j] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note_Blue"));
                            break;
                        case 4:
                            Notes[i,j] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note_Orange"));
                            break;
                    }
                    Notes[i,j].velocity = new Vector2(0.0f, (float)Note_Velocity);
                }
            }

            // Setup the strings
            Initialize_Functions.Initialize_Strings(ref str_array, graphics.GraphicsDevice.Viewport.Width,
                                                    graphics.GraphicsDevice.Viewport.Height);
            str_array[2].value = "Song Title:\n" + main_chart.Song_Name;
            str_array[3].value = "Artist Name:\n" + main_chart.Artist_Name;

            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update the notes themselves (have to specifiy each note set)
            Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Green_Notes,
                                        0, ref note_iterators[0], ref Notes);
            Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Red_Notes,
                                        1, ref note_iterators[1], ref Notes);
            Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Yellow_Notes,
                                        2, ref note_iterators[2], ref Notes);
            Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Blue_Notes,
                                        3, ref note_iterators[3], ref Notes);
            Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Orange_Notes,
                                        4, ref note_iterators[4], ref Notes);

            // Update the current tpms (ticks per milisecond)
            Misc_Functions.Update_TPMS(current_tick, ref bpm_iterator, main_chart.BPM_Changes,
                                       gameTime.ElapsedGameTime.Milliseconds, ref ticks_per_msecond);

            // Update the living notes
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    if (Notes[i,j].alive == true)
                        Notes[i,j].position += Notes[i, j].velocity;

                    if (!viewportRectangle.Contains(new Point((int)Notes[i, j].position.X,
                       (int)Notes[i, j].position.Y)))
                        Notes[i, j].alive = false;
                }
            }

            current_tick += ticks_per_msecond;
            str_array[0].value = "Current Tick:\n" + Convert.ToString(current_tick);
            str_array[1].value = "TPMS:\n" + Convert.ToString(ticks_per_msecond);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw Every GameString in str_array
            foreach (GameString curr_str in str_array)
            {
                Vector2 curr_origin = game_font.MeasureString(curr_str.value) / 2;
                spriteBatch.DrawString(game_font, curr_str.value, curr_str.position, curr_str.color,
                                       0, curr_origin, 1.0f, SpriteEffects.None, 0.5f);
            }

            //Draw the notes
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    if (Notes[i,j].alive)
                    {
                        spriteBatch.Draw(Notes[i, j].sprite, Notes[i, j].position, Color.White);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
