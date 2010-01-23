using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.FMOD;
using MinGH.ChartImpl;
using System.Threading;

namespace MinGH.GameScreen.SinglePlayer
{
    public static class AudioInitializer
    {
        public static void InitaliazeAudio(GameEngine.FMOD.System system, ChartSelection chartSelection, Chart mainChart, RESULT result,
                                           Channel musicChannel, Channel bassChannel, Channel guitarChannel, Channel drumChannel,
                                           Sound musicStream, Sound bassStream, Sound guitarStream, Sound drumStream)
        {
            GameEngine.FMOD.Factory.System_Create(ref system);
            system.init(32, INITFLAGS.NORMAL, (IntPtr)null);

            // Load up the audio tracks.
            // NOTE: I assume the audio tracks for midi files are lowercase (e.g. song.ogg)
            //       so if a file is not found, I also assume it is cause the audio track is
            //       capatalized (e.g. Song.ogg).  
            if (mainChart.chartInfo.musicStream != null)
            {
                string musicLocation = chartSelection.directory + "\\" + mainChart.chartInfo.musicStream;
                result = system.createSound(musicLocation, MODE.CREATESTREAM, ref musicStream);
                if (result == RESULT.ERR_FILE_NOTFOUND)
                {
                    result = system.createSound(chartSelection.directory + "\\Song.ogg", MODE.HARDWARE, ref musicStream);
                }
                result = system.playSound(CHANNELINDEX.FREE, musicStream, true, ref musicChannel);
            }
            if (mainChart.chartInfo.guitarStream != null)
            {
                string guitarLocation = chartSelection.directory + "\\" + mainChart.chartInfo.guitarStream;
                result = system.createSound(guitarLocation, MODE.CREATESTREAM, ref guitarStream);
                if (result == RESULT.ERR_FILE_NOTFOUND)
                {
                    result = system.createSound(chartSelection.directory + "\\Guitar.ogg", MODE.CREATESTREAM, ref guitarStream);
                }
                result = system.playSound(CHANNELINDEX.FREE, guitarStream, true, ref guitarChannel);
            }
            if (mainChart.chartInfo.bassStream != null)
            {
                string bassLocation = chartSelection.directory + "\\" + mainChart.chartInfo.bassStream;
                result = system.createSound(bassLocation, MODE.CREATESTREAM, ref bassStream);
                if (result == RESULT.ERR_FILE_NOTFOUND)
                {
                    result = system.createSound(chartSelection.directory + "\\Rhythm.ogg", MODE.CREATESTREAM, ref bassStream);
                }
                result = system.playSound(CHANNELINDEX.FREE, bassStream, true, ref bassChannel);
            }
            if (mainChart.chartInfo.drumStream != null)
            {
                string drumLocation = chartSelection.directory + "\\" + mainChart.chartInfo.drumStream;
                result = system.createSound(drumLocation, MODE.CREATESTREAM, ref drumStream);
                if (result == RESULT.ERR_FILE_NOTFOUND)
                {
                    result = system.createSound(chartSelection.directory + "\\Drums.ogg", MODE.CREATESTREAM, ref drumStream);
                }
                result = system.playSound(CHANNELINDEX.FREE, drumStream, true, ref drumChannel);
            }

            // I wished that sleeping by 2 seconds would give the pc time to syncronize the audio
            // before I let it loose, but it still desyncs.  Not as much as without though...i think.
            Thread.Sleep(2000);

            if (mainChart.chartInfo.musicStream != null)
            {
                result = musicChannel.setPaused(false);
            }
            if (mainChart.chartInfo.guitarStream != null)
            {
                result = guitarChannel.setPaused(false);
            }
            if (mainChart.chartInfo.bassStream != null)
            {
                result = bassChannel.setPaused(false);
            }
            if (mainChart.chartInfo.drumStream != null)
            {
                result = drumChannel.setPaused(false);
            }
        }
    }
}
