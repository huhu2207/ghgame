using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using FMOD;

namespace MinGH
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MinGHMain : Microsoft.Xna.Framework.Game
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
        
        Chart main_chart;  // Create the chart file
        GameStringManager str_manager = new GameStringManager();  // Stores each string and its position on the screen

        private FMOD.System system = new FMOD.System();
        private FMOD.Channel channel = new FMOD.Channel();
        private FMOD.Sound sound = new FMOD.Sound();
        uint currentMsec = 0;

        bool audioIsPlaying = false;  // So we don't play the song again every single update
        Timer offset_timer = new Timer("Offset Timer");  // Holds the game from starting (while the audio plays) so the notes and audio are in sync.

        public MinGHMain()
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
            game_font = Content.Load<SpriteFont>("Arial");  // Load the font
            main_chart = new Chart("chart.chart");  // Setup the chart
            backgroundTex = Content.Load<Texture2D>("Backgrounds\\GH_Background");

            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Green Notes
                            Notes[i, j] = new gameObject(Content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(6, 32, 86, 36));
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new gameObject(Content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(108, 32, 86, 36));
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new gameObject(Content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(207, 32, 86, 36));
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new gameObject(Content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(305, 32, 86, 36));
                            break;
                        case 4:  // Orange Notes
                            Notes[i, j] = new gameObject(Content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(404, 32, 86, 36));
                            break;
                    }
                    Notes[i,j].velocity = new Vector2(0.0f, (float)Note_Velocity);
                }
            }

            // Add the "Song Title" and "Artist Name" to the string manager
            str_manager.Set_String(2, "Song Title:\n" + main_chart.chartInfo.songName);
            str_manager.Set_String(3, "Artist Name:\n" + main_chart.chartInfo.artistName);

            // Set the offset timer's end value so the notes do not start too fast (or slow)
            // TODO: Have the 830 magic number be dynamicaly created according to the note velocity setting
            offset_timer.end_value = (main_chart.chartInfo.offset * 1000) - 830;
            
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

            // Start the song immediately
            if (audioIsPlaying == false)
            {
                //audio_engine.Play2D("guitar.ogg", true);
                FMOD.Factory.System_Create(ref system);
                system.init(32, INITFLAGS.NORMAL, (IntPtr)null);
                system.createSound("./guitar.ogg", MODE.HARDWARE, ref sound);
                system.playSound(CHANNELINDEX.FREE, sound, false, ref channel);
                audioIsPlaying = true;
            }

            // Check if the timer is done and proceed accordingly
            if (offset_timer.is_up == false)
            {
                offset_timer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            else
            {

                channel.getPosition(ref currentMsec, TIMEUNIT.MS);

                Misc_Functions.SUpdate_Notes(main_chart.Note_Charts[0].greenNotes,
                                            0, ref note_iterators[0], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity, 86, currentMsec + 760);
                Misc_Functions.SUpdate_Notes(main_chart.Note_Charts[0].redNotes,
                                            1, ref note_iterators[1], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity, 86, currentMsec + 760);
                Misc_Functions.SUpdate_Notes(main_chart.Note_Charts[0].yellowNotes,
                                            2, ref note_iterators[2], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity, 86, currentMsec + 760);
                Misc_Functions.SUpdate_Notes(main_chart.Note_Charts[0].blueNotes,
                                            3, ref note_iterators[3], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity, 86, currentMsec + 760);
                Misc_Functions.SUpdate_Notes(main_chart.Note_Charts[0].orangeNotes,
                                            4, ref note_iterators[4], ref Notes, viewportRectangle,
                                            gameTime, Note_Velocity, 86, currentMsec + 760);
            }

            
            str_manager.Set_String(0, "Current MSEC:\n" + Convert.ToString(currentMsec));
            //str_manager.Set_String(1, "TPMS:\n" + Convert.ToString(ticks_per_msecond));

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
                        spriteBatch.Draw(Notes[i, j].sprite, Notes[i, j].position, Notes[i, j].spriteSheetPosition, Color.White);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
