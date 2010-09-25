using System.Threading;
using GameEngine.FMOD;
using MinGH.ChartImpl;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Simple encapsulation of the logic necessary to initialize all possible audio
    /// streams for a specifiec game instance.
    /// </summary>
    public static class AudioInitializer
    {
        /// <summary>
        /// Initializes any valid streams/channels and plays them.
        /// </summary>
        public static void InitaliazeAudio(GameEngine.FMOD.System system, ChartSelection chartSelection, ChartInfo chartInfo, RESULT result,
                                           Channel musicChannel, Channel bassChannel, Channel guitarChannel, Channel drumChannel,
                                           Sound musicStream, Sound bassStream, Sound guitarStream, Sound drumStream)
        {
            // Load the tracks that are defined in the chart (i.e. not null) as paused
            // so we can unpause them all together to ensure better syncing of the tracks.
            if (chartInfo.musicStream != null)
            {
                string musicLocation = chartSelection.directory + "\\" + chartInfo.musicStream;
                result = system.createSound(musicLocation, MODE.CREATESTREAM, ref musicStream);
                result = system.playSound(CHANNELINDEX.FREE, musicStream, true, ref musicChannel);
            }
            if (chartInfo.guitarStream != null)
            {
                string guitarLocation = chartSelection.directory + "\\" + chartInfo.guitarStream;
                result = system.createSound(guitarLocation, MODE.CREATESTREAM, ref guitarStream);
                result = system.playSound(CHANNELINDEX.FREE, guitarStream, true, ref guitarChannel);
            }
            if (chartInfo.bassStream != null)
            {
                string bassLocation = chartSelection.directory + "\\" + chartInfo.bassStream;
                result = system.createSound(bassLocation, MODE.CREATESTREAM, ref bassStream);
                result = system.playSound(CHANNELINDEX.FREE, bassStream, true, ref bassChannel);
            }
            if (chartInfo.drumStream != null)
            {
                string drumLocation = chartSelection.directory + "\\" + chartInfo.drumStream;
                result = system.createSound(drumLocation, MODE.CREATESTREAM, ref drumStream);
                result = system.playSound(CHANNELINDEX.FREE, drumStream, true, ref drumChannel);
            }

            // Sleep for a little while to allow the FMOD system to catch up with itself.
            Thread.Sleep(200);

            // Play all the used tracks at once.
            if (chartInfo.musicStream != null)
            {
                result = musicChannel.setPaused(false);
            }
            if (chartInfo.guitarStream != null)
            {
                result = guitarChannel.setPaused(false);
            }
            if (chartInfo.bassStream != null)
            {
                result = bassChannel.setPaused(false);
            }
            if (chartInfo.drumStream != null)
            {
                result = drumChannel.setPaused(false);
            }
        }
    }
}
