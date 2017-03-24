using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolEngine.TileEngine;

namespace RagolEngine.SpriteClasses
{
    public class AnimatedSprite
    {
        #region Field Region

        Dictionary<AnimationKey, Animation> animations;

        AnimationKey currentAnimation;

        bool isAnimating;

        Texture2D texture;
        Vector2 position;
        Point point;
        Vector2 velocity;
        float speed = 2.0f;

        Rectangle positionRec;

        //An offset to make collisions look better
        int collisionRadius = 8;

        #endregion

        #region Property Region

        public AnimationKey CurrentAnimation
        {
            get { return currentAnimation; }

            set { currentAnimation = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }

            set { isAnimating = value; }
        }

        public int Width
        {
            get { return animations[currentAnimation].FrameWidth; }
        }

        public int Height
        {
            get { return animations[currentAnimation].FrameHeight; }
        }

        public float Speed
        {
            get { return speed; }

            set { speed = MathHelper.Clamp(speed, 1.0f, 16.0f); }
        }

        public Vector2 Position
        {
            get { return position; }

            set
            {
                position = value;
                SetPositionRec();
                Point = new Point((int)(position.X / 8), (int)(position.Y / 8));
            }
        }

        public Point Point
        {
            get { return point; }
            private set { point = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }

            set
            {
                velocity = value;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
            }
        }

        public Rectangle PositionRec
        {
            get { return positionRec; }
        }

        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey,
            Animation> animation)
        {
            texture = sprite;
            animations = new Dictionary<AnimationKey, Animation>();
            foreach (AnimationKey key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            //TODO: Make it so that sprites won't be drawn if they are not in camera view
            spriteBatch.Draw(texture, position, 
                animations[currentAnimation].CurrentFrameRect, Color.White);
        }


        public void LockToMap()
        {
            position.X = MathHelper.Clamp(position.X, 0, Map.MapWidthInPixels);
            position.Y = MathHelper.Clamp(position.Y, 0, Map.MapHeightInPixels);
        }

        private void SetPositionRec()
        {
            positionRec = new Rectangle((int)position.X + collisionRadius, (int)position.Y , Width - collisionRadius, Height - collisionRadius);
        }

        #endregion
    }

}
