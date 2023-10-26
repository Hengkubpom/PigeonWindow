using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonThapae_Window
{
    internal class food
    {
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        public Vector2 pos;
        public Rectangle hitbox;
        private float time_pick = 0;
        public food(Texture2D asset, int frameCount, int frameRow, int framesPerSec, Vector2 pos)
        {
            anim.Load(asset, frameCount, frameRow, framesPerSec);
            this.pos = pos;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 20, 20);
        }


        public bool selfdestroy(float elapsed)
        {
            time_pick += elapsed;
            if (time_pick >= 13)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch _batch)
        {
            anim.DrawFrame(_batch, pos);
        }
    }
}
