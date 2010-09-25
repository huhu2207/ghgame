using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.GameScreen;
using MinGH.ChartImpl;

namespace MinGH.Fretboard
{
    public interface IFretboard
    {
        void loadContent(GameConfiguration gameConfiguration, Texture2D laneSeparatorTexture, Texture2D hitMarkerTexture,
                                Effect effect, Matrix viewMatrix, Matrix projectionMatrix, int noteSpriteSheetSize,
                                GraphicsDeviceManager graphics, Game game);

        void unloadContent();

        void update(IKeyboardInputManager keyboardInputManager, Rectangle viewportRectangle,
                           GameConfiguration gameConfiguration, Effect effect, uint currentMsec,
                           GraphicsDeviceManager graphics, int noteSpriteSheetSize, GameTime gameTime);

        void draw(GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix);

        ChartInfo getChartInfo();

        PlayerInformation getPlayerInfo();
    }
}
