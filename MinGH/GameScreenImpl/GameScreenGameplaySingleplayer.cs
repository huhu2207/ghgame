using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.ChartImpl;
using MinGH.GameStringImpl;
using Microsoft.Xna.Framework.Content;
using FMOD;
using System;

namespace MinGH.GameScreenImpl
{
    /// <summary>
    /// Contains all functionality and data to display a single player session of MinGH
    /// </summary>
    class GameScreenGameplaySingleplayer : IGameScreen
    {
        SpriteBatch spriteBatch;  // Draws the shapes
        Rectangle viewportRectangle;  // The window itself
        Texture2D backgroundTex;  // The background texture
        SpriteFont game_font;  // The font the game will use
        gameObject[,] Notes;  // Will hold every note currently on the screen
        const int maxNotesOnscreen = 50;  // Maximum amount of a single note (i.e. how many reds per frame)
        const double noteVelocityMultiplier = 0.7;  // Current speed in which the notes will move
        // The number of miliseconds to speed up the notes so they appear on time
        // NOTE: 490 is the magic number constant for a 1.0 multiplier, it gets adjusted to the current multiplier in Initialization()
        double noteVelocityConstant = 490;
        int[] noteIterators;  // These iterators are used to keep track of which note to observe next

        Chart mainChart;  // Create the chart file
        GameStringManager strManager = new GameStringManager();  // Stores each string and its position on the screen

        // Variables related to the audio playing and note syncing
        private FMOD.System system = new FMOD.System();
        private FMOD.Channel channel = new FMOD.Channel();
        private FMOD.Sound sound = new FMOD.Sound();
        uint currentMsec = 0;
        bool audioIsPlaying = false;  // So we don't play the song again every single update

        public void Initialize(GraphicsDeviceManager graphics)
        {
            // Setup the strings
            Initialize_Functions.Initialize_Strings(ref strManager, graphics.GraphicsDevice.Viewport.Width,
                                                    graphics.GraphicsDevice.Viewport.Height);
            // Initialize some variables
            noteIterators = new int[5];
            Notes = new gameObject[5, maxNotesOnscreen];

            // Create the sprite bacth
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            // Setup the note velocity constant (HYPERSPEEDS!!)
            noteVelocityConstant /= noteVelocityMultiplier;
        }

        public void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            game_font = content.Load<SpriteFont>("Arial");  // Load the font
            mainChart = new Chart("chart.chart");  // Setup the chart
            backgroundTex = content.Load<Texture2D>("Backgrounds\\GH_Background");

            // Setup the notes appearance and velocity
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:  // Green Notes
                            Notes[i, j] = new gameObject(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(6, 32, 86, 36));
                            break;
                        case 1:  // Red Notes
                            Notes[i, j] = new gameObject(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(108, 32, 86, 36));
                            break;
                        case 2:  // Yellow Notes
                            Notes[i, j] = new gameObject(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(207, 32, 86, 36));
                            break;
                        case 3:  // Blue Notes
                            Notes[i, j] = new gameObject(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(305, 32, 86, 36));
                            break;
                        case 4:  // Orange Notes
                            Notes[i, j] = new gameObject(content.Load<Texture2D>("Sprites\\GH_Sprites"),
                                          new Rectangle(404, 32, 86, 36));
                            break;
                    }
                    Notes[i,j].velocity = new Vector2(0.0f, (float)noteVelocityMultiplier);
                }
            }

            // Add the "Song Title" and "Artist Name" to the string manager
            strManager.Set_String(2, "Song Title:\n" + mainChart.chartInfo.songName);
            strManager.Set_String(3, "Artist Name:\n" + mainChart.chartInfo.artistName);
            
            // Setup the window
            viewportRectangle = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
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

            channel.getPosition(ref currentMsec, TIMEUNIT.MS);

            Misc_Functions.updateNotes(mainChart.Note_Charts[0].greenNotes,
                                        0, ref noteIterators[0], ref Notes, viewportRectangle,
                                        gameTime, noteVelocityMultiplier, 86, currentMsec + noteVelocityConstant);
            Misc_Functions.updateNotes(mainChart.Note_Charts[0].redNotes,
                                        1, ref noteIterators[1], ref Notes, viewportRectangle,
                                        gameTime, noteVelocityMultiplier, 86, currentMsec + noteVelocityConstant);
            Misc_Functions.updateNotes(mainChart.Note_Charts[0].yellowNotes,
                                        2, ref noteIterators[2], ref Notes, viewportRectangle,
                                        gameTime, noteVelocityMultiplier, 86, currentMsec + noteVelocityConstant);
            Misc_Functions.updateNotes(mainChart.Note_Charts[0].blueNotes,
                                        3, ref noteIterators[3], ref Notes, viewportRectangle,
                                        gameTime, noteVelocityMultiplier, 86, currentMsec + noteVelocityConstant);
            Misc_Functions.updateNotes(mainChart.Note_Charts[0].orangeNotes,
                                        4, ref noteIterators[4], ref Notes, viewportRectangle,
                                        gameTime, noteVelocityMultiplier, 86, currentMsec + noteVelocityConstant);

            strManager.Set_String(0, "Current MSEC:\n" + Convert.ToString(currentMsec));
            strManager.Set_String(1, "End MSEC:\n" + Convert.ToString(mainChart.chartInfo.chartLengthMiliseconds));

            // Stop playing when chart is over
            if (currentMsec > mainChart.chartInfo.chartLengthMiliseconds)
            {
                channel.stop();
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //Draw the background
            spriteBatch.Draw(backgroundTex, viewportRectangle, Color.White);

            // Draw every string in str_manager
            strManager.DrawStrings(spriteBatch, game_font);

            //Draw the notes
            for (int i = 0; i < Notes.GetLength(0); i++)
            {
                for (int j = 0; j < Notes.GetLength(1); j++)
                {
                    if (Notes[i, j].alive)
                    {
                        spriteBatch.Draw(Notes[i, j].sprite, Notes[i, j].position, Notes[i, j].spriteSheetPosition, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
