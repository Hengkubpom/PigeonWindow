using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonThapae_Window
{
    internal class Ceffect
    {
        private Vector2 pos;
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        public Ceffect(Texture2D asset, Vector2 pos)
        {
            anim.Load(asset, 7, 1, 14, 1);
            this.pos = new Vector2(pos.X-12,pos.Y-12);
        }

        public void Draw(SpriteBatch _batch)
        {
            anim.DrawFrame(_batch, pos);
        }

        public bool update(float elapsed)
        {
            anim.UpdateFrame(elapsed);
            if (anim.IsEnd)
            {
                return true;
            }
            return false;
                
        }
    }
}
