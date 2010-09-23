using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;
using MinGH.GameScreen;
using MinGH.GameScreen.SinglePlayer;

namespace MinGH.Interfaces
{
    /// <summary>
    /// Encompasses most of the logic to be done during the single player game for every
    /// update.
    /// </summary>
    public interface INoteUpdater
    {
        /// <summary>
        /// Updates the position of viewable notes and creates/destroys notes when necessary
        /// </summary>
        /// <param name="inputNotechart">The Notechart currently being played.</param>
        /// <param name="inputNoteIterator">Indicates the next note to be drawn.</param>
        /// <param name="physicalNotes">The 2D array of drawable notes.</param>
        /// <param name="viewportRectangle">The rectangle surrounding the game screen.</param>
        /// <param name="currStep">The number of pixels each note on screen must move for
        ///                        this current update.</param>
        /// <param name="themeSetting">The current theme setting of the game.</param>
        /// <param name="currentMsec">The current milisecond position the playing song is on.</param>
        /// <param name="spriteSheetSize">The size of an individual note on the sprite sheets (i.e. 100px)</param>
        /// <param name="playerInfo">The player's current status.</param>
        /// <param name="hitBox">The current hit window.</param>
        /// <param name="noteParticleEmitters">Used for the autohit functionality.</param>
        /// <param name="noteStartPosition">How far back on the fretboard a newly created note will be placed.</param>
        /// <param name="timeNotesTakeToPassHitmarker">How many miliseconds it takes for a note to pass the hitmarker.</param>
        /// <param name="currStepPerMilisecond">How many game space units a note must move per milisecond.</param>
        void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                         Note[,] physicalNotes, Rectangle viewportRectangle,
                         float currStep, double currentMsec,
                         int spriteSheetSize, PlayerInformation playerInfo,
                         HorizontalHitBox hitBox, NoteParticleEmitters noteParticleEmitters,
                         float noteStartPosition, float timeNotesTakeToPassHitmarker,
                         float currStepPerMilisecond);
    }
}
