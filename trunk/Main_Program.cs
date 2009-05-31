using System;
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
        
        gameObject[] Notes;  // Will hold every note currently on the screen
        const int Max_Notes_Onscreen = 100;  // Maximum amount of notes on screen (should never be reached)
        const int Note_Velocity = 9;  // Current speed in which the notes will move (hyperspeed)
        int[] note_iterators;  // These iterators are used to keep track of which note to observe next
        
        Chart main_chart;  // Create the chart file
        double current_tick = 0.0f;  // Tracks the current tick the chart is on
        double ticks_per_msecond = 0.0f;  // How many ticks pass per milisecond
        Timer ms_timer;  // Stores the time value

        string tmp;  // Used to display debug values
        string tmp2;
        

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
            Notes = new gameObject[Max_Notes_Onscreen];
            ms_timer = new Timer("Total Milisecond Timer");
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
            
            // This turns the bpm into bps (second) and then multiplies by the chart formats
            // default resolution (192) and finally turns the value into miliseconds.
            // -NOTE: I'm honestly not sure why I use 50 instead of 100
            ticks_per_msecond = (((double)main_chart.BPM_Changes[0].Value / 6000000.0) * 192.0) * 1.6;

            // Setup the notes appearance and velocity (both are uniform at the moment)
            for (int i = 0; i < Max_Notes_Onscreen; i++)
            {
                Notes[i] = new gameObject(Content.Load<Texture2D>("Sprites\\RB_Note"));
                Notes[i].velocity = new Vector2(0.0f, (float)Note_Velocity);
            }

            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);


            // TODO: use this.Content to load your game content here
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
            Misc_Functions.Update_Notes(ref current_tick, ref main_chart.Note_Charts[0].Green_Notes,
                                        0, ref note_iterators[0], ref Notes);
            Misc_Functions.Update_Notes(ref current_tick, ref main_chart.Note_Charts[0].Red_Notes,
                                        1, ref note_iterators[1], ref Notes);
            Misc_Functions.Update_Notes(ref current_tick, ref main_chart.Note_Charts[0].Yellow_Notes,
                                        2, ref note_iterators[2], ref Notes);
            Misc_Functions.Update_Notes(ref current_tick, ref main_chart.Note_Charts[0].Blue_Notes,
                                        3, ref note_iterators[3], ref Notes);
            Misc_Functions.Update_Notes(ref current_tick, ref main_chart.Note_Charts[0].Orange_Notes,
                                        4, ref note_iterators[4], ref Notes);

            // Update the living notes
            foreach (gameObject curr_note in Notes)
            {
                if (curr_note.alive == true)
                    curr_note.position += curr_note.velocity;

                if (!viewportRectangle.Contains(new Point((int)curr_note.position.X,
                        (int)curr_note.position.Y)))
                    curr_note.alive = false;
            }
            
            ms_timer.Update(gameTime.TotalGameTime.Milliseconds);  // Update the timer
            current_tick += ticks_per_msecond;
            tmp = Convert.ToString(ms_timer.current_time);
            tmp2 = Convert.ToString(current_tick);

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

            Vector2 FontOrigin = game_font.MeasureString(tmp) / 2;
            spriteBatch.DrawString(game_font, tmp, new Vector2(50f, 60f), Color.Black, 0, FontOrigin,
                                   1.0f, SpriteEffects.None, 0.5f);

            Vector2 FontOrigin2 = game_font.MeasureString(tmp) / 2;
            spriteBatch.DrawString(game_font, tmp2, new Vector2(50f, 30f), Color.Black, 0, FontOrigin2,
                                    1.0f, SpriteEffects.None, 0.5f);

            //Draw the notes
            foreach (gameObject curr_note in Notes)
            {
                if (curr_note.alive)
                {
                    spriteBatch.Draw(curr_note.sprite, curr_note.position, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
