namespace MinGH.Config
{
    /// <summary>
    /// Contains information on how the game screen should be laid out.
    /// </summary>
    public class ThemeSetting
    {
        public string backgroundDirectory { get; set; }
        public string noteSkinTexture { get; set; }
        public string fretboardTexture { get; set; }
        public string laneSeparatorTexture { get; set; }
        public string fretboardBorderTexture { get; set; }
        public string hitMarkerTexture { get; set; }
        public int laneSeparatorSize { get; set; }
        public int laneSizeGuitar { get; set; }
        public int laneSizeDrums { get; set; }
        public int fretboardBorderSize { get; set; }
        public int hitMarkerDepth { get; set; }
        public int hitMarkerSize { get; set; }
        public int fretboardDepth { get; set; }

        public ThemeSetting()
        {
            noteSkinTexture = ".\\Content\\Sprites\\DefaultSprites.png";
            backgroundDirectory = ".\\Content\\Backgrounds\\Default";
            fretboardTexture = ".\\Content\\FretboardDefault.png";
            laneSeparatorTexture = ".\\Content\\LaneSeparatorDefault.png";
            fretboardBorderTexture = ".\\Content\\LaneSeparatorDefault.png";
            hitMarkerTexture = ".\\Content\\Fretboards\\HitmarkerDefault.png";
            laneSeparatorSize = 3;
            laneSizeGuitar = 35;
            laneSizeDrums = 55;
            fretboardBorderSize = 6;
            hitMarkerDepth = 300;
            hitMarkerSize = 6;
            fretboardDepth = 1000;
        }
    }
}
