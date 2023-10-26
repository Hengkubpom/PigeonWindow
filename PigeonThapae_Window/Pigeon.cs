using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PigeonThapae_Window
{
    internal class Pigeon
    {
        private int speed = 1;
        private bool fly = false;
        private int health = 4;
        public Vector2 pos,target,lastpos;
        private Random rnd = new Random();
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        public bool flip = false;
        public float elapsed2 = 0, elapsed = 0;
        public int time_walk;
        private int time_coin = 10;
        public Rectangle hitbox = new Rectangle();
        private Texture2D texture,flytexture;
        private bool allow_hitbox = true;
        public bool go_out = false;
        public List<Coin> coins = new List<Coin>();
        private int hungry = 5;
        private bool allow_effectfly = true;
        




        public Pigeon(Texture2D asset,Texture2D flyt, Vector2 pos)
        {
            this.pos = pos;
            anim.Load(asset, 4, 2, 4, 1);
            texture = asset;
            flytexture = flyt;
            hitbox = new Rectangle((int)pos.X,(int)pos.Y,40,40);
            target = new Vector2(rnd.Next(10,1100), rnd.Next(310, 600));
            time_walk = rnd.Next(4, 7);
        }

        public void Pigeondraw(SpriteBatch _batch,SpriteFont font)
        {
            if(allow_hitbox == true)
            {
                hitbox = new Rectangle((int)pos.X, (int)pos.Y, 40, 40);
            }
            if(hungry > 3 & !fly)
            {
                anim.DrawFrame(_batch, pos, flip, Color.White);
            }
            else if(hungry >= 2 & !fly)
            {
                anim.DrawFrame(_batch, pos, flip, Color.HotPink);
            }
            else if (hungry == 1 & !fly)
            {
                anim.DrawFrame(_batch, pos, flip, Color.Red);
            }
            else
            {
                anim.DrawFrame(_batch, pos, flip, Color.White);
            }
            foreach (Coin scoin in coins)
            {
                scoin.Coindraw(_batch,font);
            }
        }
        public void updatebird(int x, int y, float elapsed, List<food> bfood)
        {
            this.elapsed += elapsed;
            elapsed2 += elapsed;
            anim.UpdateFrame(elapsed);
            if (allow_hitbox == true)
            {
                if(hungry <= 3 && bfood.Count > 0)
                {
                    target = nearestfood(bfood);
                }
                else if (this.elapsed >= time_walk)
                {
                    target = new Vector2(rnd.Next(10, x), rnd.Next(310, y));
                    this.elapsed = 0;
                } 
            }
            else
            {
                if(pos.X >= target.X-5 && pos.X <= target.X + 5 && pos.Y >= target.Y-5 && pos.Y <= target.Y+5)
                {
                    allow_hitbox = true;
                    speed = speed/2;
                    
                }
            }
            if (elapsed2 >= time_coin)
            {
                hungry -= 1;
                coins.Add(new Coin(1, 1, 1, pos));
                elapsed2 = 0;

            }
            foreach (Coin scoin in coins)
            {
                if (scoin.selfdestroy(elapsed))
                {
                    Game1.money += scoin.todayworth;
                    coins.Remove(scoin);
                    break;
                }
            }
            if (hungry <= 0 || go_out)
            {
                speed = 2;
                go_out = true;
                fly = true;
                allow_hitbox = false;
                if (allow_effectfly)
                {
                    var instance = Game1.sEffect[3].CreateInstance();
                    instance.Volume = 1;
                    instance.Play();
                    allow_effectfly = false;
                }
                anim.changeall(flytexture, 9, 1, 10, 1);
                if (pos.X < 500)
                {
                    target = new Vector2(-100, -100);
                }
                else
                {
                    target = new Vector2(1400, -100);
                }
            }

        }
        public void move(Rectangle nonearea)
        {
            if(hitbox.Intersects(nonearea) & !fly)
            {
                pos = lastpos;
            }
            if (pos != target)
            {
                if (pos.X > target.X)
                {
                    pos.X -= speed;
                    flip = true;
                    if (!fly)
                    {
                        anim.startrow = 2;
                    }
                }
                if (pos.X < target.X)
                {
                    pos.X += speed;
                    flip = false;
                    if (!fly)
                    {
                        anim.startrow = 2;
                    }
                }
                if (pos.Y > target.Y)
                {
                    pos.Y -= speed;
                    if (!fly)
                    {
                        anim.startrow = 2;
                    }
                }
                if (pos.Y < target.Y)
                {
                    pos.Y += speed;
                    if (!fly)
                    {
                        anim.startrow = 2;
                    }
                }
            }
            else
            {
                anim.startrow = 1;
            }
            lastpos = pos;
        }

        public void checkintersect(Rectangle dekhitbox)
        {
            if (hitbox.Intersects(dekhitbox))
            {
                if(allow_hitbox == true)
                {
                    health -= 1;
                    speed = speed * 2;
                    if(pos.X < 500)
                    {
                        target = new Vector2(pos.X + rnd.Next(250,400), rnd.Next(310, 600));
                    }
                    else
                    {
                        target = new Vector2(pos.X - rnd.Next(250,400), rnd.Next(310, 600));
                    }
                    allow_hitbox = false;
                    if(health <= 0)
                    {
                        go_out = true;
                        fly = true;
                    }
                }
                hitbox = new Rectangle((int)target.X, (int)target.Y, texture.Width, texture.Height);
            }
        }


        public bool checkfood(Rectangle foodhitbox)
        {
            if (hitbox.Intersects(foodhitbox) && hungry < 5)
            {
                hungry = 5;
                return true;
            }
            return false;
        }

        protected Vector2 nearestfood(List<food> bfood)
        {
            float Xnear = 2000;
            Vector2 nearest = new Vector2(2000, 2000);
            foreach(food mini_food in bfood)
            {
                if (pos.X < mini_food.pos.X)
                {
                    if (mini_food.pos.X - pos.X < Xnear)
                    {
                        Xnear = mini_food.pos.X - pos.X;
                        nearest = mini_food.pos;
                    }
                }
                else if (pos.X >= mini_food.pos.X)
                {
                    if (pos.X - mini_food.pos.X < Xnear)
                    {
                        Xnear = pos.X - mini_food.pos.X;
                        nearest = mini_food.pos;
                    }
                }
            }
            return nearest;

        }
    }
}
