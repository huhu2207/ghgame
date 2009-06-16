using System;
using IrrKlang;
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
        const double Note_Velocity = 1.5;  // Current speed in which the notes will move (higher = slower)
        int[] note_iterators;  // These iterators are used to keep track of which note to observe next
        int bpm_iterator = 0;  // Keeps track of what bpm change is to come next
        
        Chart main_chart;  // Create the chart file
        double current_tick = 0.0;  // Tracks the current tick the chart is on
        double ticks_per_msecond = 0.0;  // How many ticks pass per milisecond
        GameStringManager str_manager = new GameStringManager();  // Stores each string and its position on the screen

        ISoundEngine audio_engine = new ISoundEngine();  // start the sound engine with default parameters
        double game_offset = 0.0;  // This alters where the notes will be when they are expected to be "hit"
        bool audioIsPlaying = false;  // So we don't play the song again every single update

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
            // Setup the strings
            Initialize_Functions.Initialize_Strings(ref str_manager, graphics.GraphicsDevice.Viewport.Width,
                                                    graphics.GraphicsDevice.Viewport.Height);
            // Initialize some variables
            note_iterators = new int[5];
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

            // Add the "Song Title" and "Artist Name" to the string manager
            str_manager.Set_String(2, "Song Title:\n" + main_chart.Song_Name);
            str_manager.Set_String(3, "Artist Name:\n" + main_chart.Artist_Name);

            // Add an offset to the current tick so the song doesn't start too fast
            current_tick = (main_chart.Offset * (main_chart.BPM_Changes[0].Value * 192 / 60000));

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

            // Update the current tpms (ticks per milisecond).  Speed up if running slow.
            if (gameTime.IsRunningSlowly)
            {
                Misc_Functions.Update_TPMS(current_tick, ref bpm_iterator, main_chart.BPM_Changes,
                                           gameTime, ref ticks_per_msecond);
                current_tick += ticks_per_msecond;
            }
            else
            {

                // Start the song at the specified time
                if ((gameTime.TotalGameTime.TotalSeconds >= game_offset) && (audioIsPlaying == false))
                {
                    audio_engine.Play2D("guitar.ogg", true);
                    audioIsPlaying = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Green_Notes,
                                            0, ref note_iterators[0], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity);
                Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Red_Notes,
                                            1, ref note_iterators[1], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity);
                Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Yellow_Notes,
                                            2, ref note_iterators[2], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity);
                Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Blue_Notes,
                                            3, ref note_iterators[3], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity);
                Misc_Functions.Update_Notes(current_tick, main_chart.Note_Charts[0].Orange_Notes,
                                            4, ref note_iterators[4], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity);

                Misc_Functions.Update_TPMS(current_tick, ref bpm_iterator, main_chart.BPM_Changes,
                                               gameTime, ref ticks_per_msecond);
                current_tick += ticks_per_msecond;

                str_manager.Set_String(0, "Current Tick:\n" + Convert.ToString(current_tick));
                str_manager.Set_String(1, "TPMS:\n" + Convert.ToString(ticks_per_msecond));
            }

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

            // Draw every string in str_manager
            str_manager.DrawStrings(spriteBatch, game_font);

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
