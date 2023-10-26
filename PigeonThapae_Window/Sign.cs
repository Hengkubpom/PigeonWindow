using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonThapae_Window
{
    internal class Sign
    {
        public float time;
        private Vector2 area;
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        public Vector2 pos;
        public Rectangle hitbox;
        public Sign(Texture2D asset, Vector2 pos,float timesign, Vector2 area)
        {
            anim.Load(asset,1,1,1,1);
            this.pos = pos;
            time = timesign;
            this.area = area;
        }

        public bool time_out(float elapsed)
        {
            time -= elapsed;
            if(time <= 0)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch _batch)
        {
            hitbox = new Rectangle((int)pos.X-(int)(area.X/2), (int)pos.Y-(int)(area.Y / 2), (int)area.X, (int)area.Y);
            anim.DrawFrame(_batch,pos);
        }
    }
}
