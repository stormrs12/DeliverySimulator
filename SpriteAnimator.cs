using SFML.Graphics;
using SFML.System;
using System;

namespace DeliverySimulator
{
    public class SpriteAnimator
    {
        public int Rows { get; private set; }
        public int FramesPerRow { get; private set; }
        public Vector2i FrameSize { get; private set; }

        public int CurrentFrame { get; set; }
        public int CurrentRow { get; set; }


        public int RowStart { get; set; }
        public int RowEnd { get; set; }

        public bool Playing = true;

        private float timePerFrame = 0;
        private float currentTime = 0;

        public SpriteAnimator(Texture spriteSheet, int framesPerRow, int rows, int fps = 1)
        {
            Rows = rows;
            FramesPerRow = framesPerRow;

            FrameSize = new Vector2i(
                (int)spriteSheet.Size.X / framesPerRow,
                (int)spriteSheet.Size.Y / rows
                );

            timePerFrame = 1.0f / fps;

            CurrentFrame = 0;
            CurrentRow = 0;
            RowStart = 0;
            RowEnd = framesPerRow;
        }

        public void ResetRowBounds()
        {
            RowStart = 0;
            RowEnd = FramesPerRow;
        }

        public void Start() 
        {
            currentTime = 0.0f;
            Playing = true;
        }

        public void Stop() 
        {
            currentTime = 0.0f;
            Playing = false;
        }

        public IntRect TextureRect => new IntRect(
            FrameSize.X * CurrentFrame,
            FrameSize.Y * CurrentRow,
            FrameSize.X,
            FrameSize.Y
            );

        public void Update(float delta)
        {
            currentTime += delta;

            if (currentTime >= timePerFrame && Playing)
            {
                currentTime = 0.0f;
                
                CurrentFrame++;

                if (CurrentFrame >= RowEnd) 
                {
                    CurrentFrame = RowStart;
                }
            }
        }
    }
}
