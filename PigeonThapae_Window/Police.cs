using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PigeonThapae_Window
{
    internal class Police
    {
        private Random rnd = new Random();
        private int speed = 2;
        private float elapsed_walk = 0;
        private bool flip = false;
        public Vector2 pos,target;
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        public Rectangle hitbox,hitboxtarget;
        private Texture2D texture;
        private float time_police;
        public bool time_out = false;
        private int walk = 0;
        private bool allow_effect = true;
        public Police(Texture2D asset, float time_police)
        {
            pos = new Vector2(1200, 350);
            anim.Load(asset, 7, 2, 10, 1);
            target = new Vector2(720, 450);
            texture = asset;
            this.time_police = time_police;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 50,90);
            hitboxtarget = new Rectangle((int)target.X, (int)target.Y, 10, 10);
        }

        public void selecttarget(List<kid> dek,float elapsed)
        {
            time_police -= elapsed;
            elapsed_walk += elapsed;
            anim.UpdateFrame(elapsed);
            if (elapsed_walk >= 2)
            {
                elapsed_walk = 0;
                
                if (dek.Count > 0 && time_police > 0)
                {
                    target = dek[rnd.Next(dek.Count)].pos;
                }
                else if(dek.Count <= 0 && time_police > 0)
                {
                    target = new Vector2(720, 450);
                }
                else if(time_police <= 0)
                {
                    target = new Vector2(1500, 450);
                    time_out = true;
                }
            }
        }
        public void update()
        {
            hitboxtarget = new Rectangle((int)target.X, (int)target.Y, 10, 10);
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 90, 90);
            if (!hitbox.Intersects(hitboxtarget))
            {
                if (pos.X > target.X)
                {
                    pos.X -= speed;
                    flip = true;
                }
                if (pos.X < target.X)
                {
                    pos.X += speed;
                    flip = false;
                }
                if (pos.Y > target.Y)
                {
                    pos.Y -= speed;
                }
                if (pos.Y < target.Y)
                {
                    pos.Y += speed;
                }
                anim.startrow = 2;
                anim.TimePerFrame = (float)1 / 10;
            }
            else
            {
                anim.startrow = 1;
                anim.TimePerFrame = (float)1 / 5;
            }
            if(pos.X < 1000 & allow_effect)
            {
                var instance = Game1.sEffect[5].CreateInstance();
                instance.Volume = 0.2f;
                instance.Play();
                allow_effect = false;
            }
        }

        public void Draw(SpriteBatch _batch)
        {
            anim.DrawFrame(_batch, pos, flip);
        }
    }
}
