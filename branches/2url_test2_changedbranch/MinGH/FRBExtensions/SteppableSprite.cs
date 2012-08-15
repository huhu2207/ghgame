using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall;

namespace MinGH.FRBExtensions
{
    public class SteppableSprite : Sprite
    {
        public SteppableSprite() : base()
        {
            spriteSheetStepX = 0;
            spriteSheetStepY = 0;
            pixelsPerSpriteSheetStepX = 0;
            pixelsPerSpriteSheetStepY = 0;
        }

        public int spriteSheetStepX
        {
            get
            {
                return _spriteSheetStepX;
            }
            set
            {
                _spriteSheetStepX = value;
                initalizeTextureCoords();
            }
        }
        protected int _spriteSheetStepX;

        public int spriteSheetStepY
        {
            get
            {
                return _spriteSheetStepY;
            }
            set
            {
                _spriteSheetStepY = value;
                initalizeTextureCoords();
            }
        }
        private int _spriteSheetStepY;

        public int pixelsPerSpriteSheetStepX
        {
            get
            {
                return _pixelsperSpriteSheetStepX;
            }
            set
            {
                _pixelsperSpriteSheetStepX = value;
                initalizeTextureCoords();
            }
        }
        private int _pixelsperSpriteSheetStepX;

        public int pixelsPerSpriteSheetStepY
        {
            get
            {
                return _pixelsperSpriteSheetStepY;
            }
            set
            {
                _pixelsperSpriteSheetStepY = value;
                initalizeTextureCoords();
            }
        }
        private int _pixelsperSpriteSheetStepY;

        private void initalizeTextureCoords()
        {
            float texHeight = this.Texture.Height;
            float texWidth = this.Texture.Width;

            float desiredTop = pixelsPerSpriteSheetStepY * spriteSheetStepY / texHeight;
            float desiredBottom = pixelsPerSpriteSheetStepY * (spriteSheetStepY + 1) / texHeight;
            float desiredLeft = pixelsPerSpriteSheetStepX * spriteSheetStepX / texWidth;
            float desiredRight = pixelsPerSpriteSheetStepX * (spriteSheetStepX + 1) / texWidth;

            this.TopTextureCoordinate = desiredTop;
            this.BottomTextureCoordinate = desiredBottom;
            this.LeftTextureCoordinate = desiredLeft;
            this.RightTextureCoordinate = desiredRight;
        }
    }
}
