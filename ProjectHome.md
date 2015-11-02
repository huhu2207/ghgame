This project finally has a name: MinGH

This game's goal is to be as minimal as it can get while still retaining the general Guitar Hero interface.  This project is geared towards people with older systems that may not handle FoF, Frontman, etc. well.  Plus the more minimal, the easier it is on my newbie skills to make.

  * This project is written in C# using Visual C# Express 2008.
    * I am using the XNA libraries to make the game making learning curve a tad easier.

  * All executable releases are made with a 32-bit machine.

  * This project makes use of the FMOD audio library.
    * [FMOD Link](http://www.fmod.org/)

  * The Mercury Particle Engine is also used.
    * [Mercury Particle Engine Link](http://mpe.codeplex.com/)

  * For MIDI parsing, I use both Stephen Toub's Toub.Sound.Midi library (I couldn't find an official link), and for rare cases, [Leslie Sanford's MIDI toolkit](http://www.codeproject.com/KB/audio-video/MIDIToolkit.aspx).
    * Currently, Sanford's MIDI parser is only used when Toub's parser fails (GH2 midis seem to still do this) and is quite slow when operating on larger MIDIs like Freebird.

  * In the playable versions, "asdfg" are the fret buttons and "up" and "down" are the respective strum directions.  It is important that you use the two buttons for strumming as opposed to one.  For me at least, using one key for both strums lead to dropped notes.
    * For drums (and drumsToGuitar mode) use "zxcvb." Z = bass and xcvb = pads.
    * For now, you can use joy2key to enable gamepad play.

  * An XML configuration file is now used.  It must be in the same directory as the executable under the name "config.xml"

  * MinGH should be able to find every chart within the specified song directory, regardless of the song directory structure.

  * The sprite sheet I use for the notes has a sprite size of 100 X 100 (thus the whole sheet is about 500x600).  So keep that in mind if you want to make custom sprite sheets (make sure the note is in the center).  The 100x100 notes will be scaled to properly fit the background using values from config.xml.  Make special notice that the bass drum note is 400x100.