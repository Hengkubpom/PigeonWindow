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
    internal class kid
    {
        private float speed = 1, elapsed = 0, police_time = 0;
        private int time_towalk = 3;
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero,0,1,0);
        public bool flip = false;
        public float health, max_health;
        public Vector2 pos, lastpos;
        private Vector2 target;
        private Random rnd = new Random();
        public Rectangle hitbox,bar;
        private bool on_sign = false;

        private Texture2D texture, bartexture;
        private int select;
        public bool death = false;
        public kid(Texture2D asset, Vector2 pos, int health, Texture2D bar)
        {
            this.pos = pos;
            max_health = (float)health;
            this.health = (float)health;
            texture = asset;
            anim.Load(asset, 6, 2, 10,1);
            target = pos;
            bartexture = bar;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 90,90);
        }
        public void kiddraw(SpriteBatch _batch)
        {
            bar = new Rectangle((int)pos.X+5, (int)pos.Y - 10, gethealthbar(), 10);
            hitbox = new Rectangle((int)pos.X-20, (int)pos.Y-20, 90,120);
            anim.DrawFrame(_batch, pos, flip);
            _batch.Draw(bartexture, bar, Color.Red);
        }

        public void selectbird(List<Pigeon> bird,float time,List<Car> _car,List<Sign> _sign,Rectangle nonearea)
        {
            anim.UpdateFrame(time);
            elapsed += time;
            if (death == false)
            {

                if (elapsed >= time_towalk)
                {
                    elapsed = 0;
                    select = rnd.Next(bird.Count);
                    if (bird.Count > 0)
                    {
                        target = bird[select].pos;
                    }
                    else if (bird.Count <= 0)
                    {
                        target = pos;
                    }
                }
                foreach (Sign mini_sign in _sign)
                {
                    if (hitbox.Intersects(mini_sign.hitbox))
                    {
                        if (pos.X < mini_sign.hitbox.X + (mini_sign.hitbox.Width / 2))
                        {
                            target = new Vector2(mini_sign.hitbox.X - hitbox.Width, mini_sign.hitbox.Y);
                        }
                        else if (pos.X >= mini_sign.hitbox.X + (mini_sign.hitbox.Width / 2))
                        {
                            target = new Vector2(mini_sign.hitbox.X + mini_sign.hitbox.Width, mini_sign.hitbox.Y);
                        }
                    }
                }
                if (nonearea.Intersects(hitbox))
                {
                    target = new Vector2(pos.X, pos.Y + 200);
                }
            }
            else if (death == true)
            {
                target = new Vector2(-200, 600);
            }

            
            if(health <= max_health/2 & _car.Count > 0)
            {
                target = _car[0].pos;
            }

          
        }

        public void move()
        {
            if (!hitbox.Contains(target))
            {
                if (pos.X > target.X)
                {
                    pos.X -= speed;
                    flip = true;
                }
                else if (pos.X < target.X)
                {
                    pos.X += speed;
                    flip = false;
                }
                if (pos.Y > target.Y)
                {
                    pos.Y -= speed;
                }
                else if (pos.Y < target.Y)
                {
                    pos.Y += speed;
                }
                anim.startrow = 2;
            }
            else
            {
                anim.startrow = 1;
            }

            
          
            lastpos = pos;
        }

        public bool damaged(MouseState mouse, MouseState oldms)
        {
            if(hitbox.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed && oldms.LeftButton == ButtonState.Released)
            {
                health -= 1;
                    if(health <= 0 && death == false)
                    {
                        death = true;
                        speed = speed * (float)1.5;
                    }
                return true;
            }
            return false;
        }

        public void damaged(Rectangle police_hitbox, float elapsed)
        {
            police_time += elapsed;
            if (hitbox.Intersects(police_hitbox) && police_time > 1)
            {
                health -= 5;
                health -= 0.02f*max_health;
                police_time = 0;
                if (health <= 0 && death == false)
                {
                    death = true;
                    speed = speed * (float)1.5;
                }
            }
        }

        public int gethealthbar()
        {
            return (int)(70*(health/max_health));
        }

    }
}
