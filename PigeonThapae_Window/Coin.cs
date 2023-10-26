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
    internal class Coin
    {
        static public int worth = 1;
        public int todayworth;
        private Vector2 pos;
        private float time_pick = 0;

        public Coin(int frameCount, int frameRow, int framesPerSec, Vector2 pos)
        {
            this.pos = pos;
            todayworth = worth;

        }

        public void Coindraw(SpriteBatch _batch,SpriteFont font)
        {
            _batch.DrawString(font, "+"+todayworth, pos, Color.White);
        }

        public bool selfdestroy(float elapsed)
        {
            time_pick += elapsed;
            if(time_pick >= 3)
            {
                return true;
            }
            return false;
        }
    }
}
