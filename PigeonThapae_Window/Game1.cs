using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1.Effects;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Transactions;

namespace PigeonThapae_Window
{
    public class Game1 : Game
    {
        //setting
        private int bound_X = 1100;
        private int bound_Y = 600; //y start 308
        private int boss_health = 40;
        private int boss_health_per_level = 40;
        private int Time_spawn_dek = 60;
        private int bar_width = 262;
        static public int money = 20;
        private Vector2 where_dekspawn = new Vector2(-150, 300);
        private int bird_price = 20;
        private int food_price = 1;
        private int price_police = 110;
        private int price_sign = 75;
        private int price_car = 175;
        private float time_police = 30;
        private float time_sign = 5;
        private float time_car = 30;
        private float cooldown_police = 35;
        private float cooldown_sign = 10;
        private float cooldown_car = 40;
        private Vector2 area_sign = new Vector2(100, 100);
        private float main_volume = 0.3f;
        private float main_volume_effect = 0.6f;

        //system
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont uid_font, cooldown_text;
        private KeyboardState _keyboardState, oldstate;
        private MouseState _mousestate, oldms;
        private Texture2D pigeon_texture, bg, kid_texure, barcolor, buy_bird, buy_car, food_texture, police_texture, sign_texture, buy_police, buy_sign;
        private Texture2D pigeon_fly, car_texture, board_ui, bar_grob, talk_texture, hit_effect, pause_1, pause_2, pause_dark, exit_texture, setting_popup, confirm, volume;
        private Texture2D over_screen, home, retry, text_highscore, text_score, scoreboard, close, menu, play, scorebutton, settingicon, mouse_1,mouse_2;
        private List<Pigeon> bird = new List<Pigeon>();
        private List<kid> dek = new List<kid>();
        private List<food> bfood = new List<food>();
        private List<Police> _police = new List<Police>();
        private List<Sign> _sign = new List<Sign>();
        private List<Car> _car = new List<Car>();
        private List<Ceffect> hit = new List<Ceffect>();
        private float elapsed, Media_elapsed = 0;        private Random rnd = new Random();
        private bool attacked = false, pause = false, Gameover = false, start = false, allow_song_gameover = true, board = false, setting = false, highscore = false, click = false;
        private float time_pick = 0;
        private Rectangle setting_exit, button_home, button_retry, button_close, button_play, button_score;
        private Rectangle bar, button_bird, button_sign, button_police, button_pause, button_car, none_area, none_area2, setting_button, button_exit;
        private Rectangle[] music_button = new Rectangle[10];
        private Rectangle[] seffect_button = new Rectangle[10];
        private string type_click = "food";
        private float c_sign, c_police, c_car;
        private string holding_text, holding_price;
        private float time_score = 0, stage = 0, bird_a = 0, money_a = 0;
        private float time_scoreb = 0, stageb = 0, bird_b = 0, money_b = 0;
        private string filepath;
        private FileStream file;
        private BinaryReader reader;
        private BinaryWriter writer;
        private Song onFight, onEnd, onLobby, onPlay;
        static public List<SoundEffect> sEffect = new List<SoundEffect>();
        ScreenState screen;
        enum ScreenState
        {
            Menu,
            Gameplay,
            Over
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            none_area = new Rectangle(0, 0, 1200, 308);
            none_area2 = new Rectangle(0, 618, 1200, 600);
            button_bird = new Rectangle(373, 739, 98, 44);
            button_police = new Rectangle(650, 710, 146, 80);
            button_sign = new Rectangle(820, 698, 170, 102);
            button_car = new Rectangle(1010, 698, 190, 98);
            button_pause = new Rectangle(600, 750, 16, 26);
            button_exit = new Rectangle(550, 750, 30, 26);
            button_home = new Rectangle(465, 670, 116, 38);
            button_retry = new Rectangle(605, 670, 140, 38);
            button_close = new Rectangle(870, 180, 30, 32);
            button_play = new Rectangle(734, 463, 280, 76);
            button_score = new Rectangle(734, 583, 280, 76);
            setting_button = new Rectangle(1135, 15, 50, 50);
            for(int i = 0; i < music_button.Length; i++)
            {
                music_button[i] = new Rectangle(555+(i*35), 318, 20, 28);
            }
            for (int i = 0; i < seffect_button.Length; i++)
            {
                seffect_button[i] = new Rectangle(555 + (i * 35), 463, 20, 28);
            }
            //_spriteBatch.Draw(pause_dark, new Rectangle(250, 125, 700, 550), Color.White);
            setting_exit = new Rectangle(525, 550, 138, 36);
            c_sign = cooldown_sign;
            c_police = cooldown_police;
            c_car = cooldown_car;
            filepath = Path.Combine("../net6.0-windows/Content/data/time.bin");
            file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            reader = new BinaryReader(file);
            time_scoreb = (float)reader.ReadInt16();
            reader.Close();
            //writer = new BinaryWriter(file);
            //writer.Write((int)40);
            //writer.Flush();
            //writer.Close();
            filepath = Path.Combine("../net6.0-windows/Content/data/stage.bin");
            file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(file);
            stageb = (float)reader.ReadInt16();
            reader.Close();
            //file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            //writer = new BinaryWriter(file);
            //writer.Write((int)30);
            //writer.Flush();
            //writer.Close();
            filepath = Path.Combine("../net6.0-windows/Content/data/bird.bin");
            file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(file);
            bird_b = (float)reader.ReadInt16();
            reader.Close();
            //file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            //writer = new BinaryWriter(file);
            //writer.Write((int)20);
            //writer.Flush();
            //writer.Close();
            filepath = Path.Combine(@"Content/data/money.bin");
            file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(file);
            money_b = (float)reader.ReadInt16();
            //file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            //writer = new BinaryWriter(file);
            //writer.Write((int)10);
            //writer.Flush();
            //writer.Close();
            reader.Close();
            screen = ScreenState.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            pigeon_texture = Content.Load<Texture2D>("bird/bird_main");
            pigeon_fly = Content.Load<Texture2D>("bird/bird_fly");
            kid_texure = Content.Load<Texture2D>("dek/dek");
            food_texture = Content.Load<Texture2D>("bird/food");
            police_texture = Content.Load<Texture2D>("police/police");
            sign_texture = Content.Load<Texture2D>("sign");
            car_texture = Content.Load<Texture2D>("car/car");
            buy_police = Content.Load<Texture2D>("UI/police_button");
            buy_sign = Content.Load<Texture2D>("UI/sign_button");
            buy_car = Content.Load<Texture2D>("UI/car_button");
            buy_bird = Content.Load<Texture2D>("UI/bird_button");
            barcolor = Content.Load<Texture2D>("bar");
            bar_grob = Content.Load<Texture2D>("UI/bar_grob");
            board_ui = Content.Load<Texture2D>("UI/board");
            uid_font = Content.Load<SpriteFont>("textfont_uid");
            cooldown_text = Content.Load<SpriteFont>("cooldown_text");
            talk_texture = Content.Load<Texture2D>("UI/talk");
            hit_effect = Content.Load<Texture2D>("effect/hit_effect");
            settingicon = Content.Load<Texture2D>("UI/settingicon");
            mouse_1 = Content.Load<Texture2D>("UI/mouse_1");
            mouse_2 = Content.Load<Texture2D>("UI/mouse_2");
            pause_1 = Content.Load<Texture2D>("UI/pause_1");
            pause_2 = Content.Load<Texture2D>("UI/pause_2");
            pause_dark = Content.Load<Texture2D>("UI/pause_dark");
            exit_texture = Content.Load<Texture2D>("UI/exit_button");
            setting_popup = Content.Load<Texture2D>("UI/Setting-popup");
            volume = Content.Load<Texture2D>("UI/volume_button");
            confirm = Content.Load<Texture2D>("UI/confrim_button");
            scoreboard = Content.Load<Texture2D>("UI/scoreboard");
            close = Content.Load<Texture2D>("UI/close_button");
            scorebutton = Content.Load<Texture2D>("UI/score");
            play = Content.Load<Texture2D>("UI/play_button");
            menu = Content.Load<Texture2D>("menu");
            bg = Content.Load<Texture2D>("bg");
            over_screen = Content.Load<Texture2D>("UI/over/over");
            home = Content.Load<Texture2D>("UI/over/home");
            retry = Content.Load<Texture2D>("UI/over/retry");
            text_highscore = Content.Load<Texture2D>("UI/over/newhighscore");
            text_score = Content.Load<Texture2D>("UI/over/score");
            onEnd = Content.Load<Song>("Sound/OnEnd");
            onFight = Content.Load<Song>("Sound/Onfight");
            onLobby = Content.Load<Song>("Sound/Onlobby");
            onPlay = Content.Load<Song>("Sound/Onplay");
            sEffect.Add(Content.Load<SoundEffect>("Sound/attacked"));     //0
            sEffect.Add(Content.Load<SoundEffect>("Sound/feed"));     //1
            sEffect.Add(Content.Load<SoundEffect>("Sound/money_down"));     //2
            sEffect.Add(Content.Load<SoundEffect>("Sound/onFly"));     //3
            sEffect.Add(Content.Load<SoundEffect>("Sound/onHit"));     //4
            sEffect.Add(Content.Load<SoundEffect>("Sound/Police"));     //5
            sEffect.Add(Content.Load<SoundEffect>("Sound/Redcar"));     //6
            sEffect.Add(Content.Load<SoundEffect>("Sound/Redcar2"));     //7
            sEffect.Add(Content.Load<SoundEffect>("Sound/Sign"));     //8
            sEffect.Add(Content.Load<SoundEffect>("Sound/onCooldown"));     //9
            sEffect.Add(Content.Load<SoundEffect>("Sound/UIopen"));     //10
            SoundEffect.MasterVolume = main_volume_effect;
            MediaPlayer.Play(onLobby);
            MediaPlayer.Volume = main_volume;
            MediaPlayer.IsRepeating = true;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            //state mouse keyboard
            _mousestate = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            //debug
            if (_keyboardState.IsKeyDown(Keys.F5) & oldstate.IsKeyUp(Keys.F5))
            {
                Console.WriteLine("=====================");
                //Console.WriteLine("Time = " + time_pick);
                Console.WriteLine("X = " + _mousestate.X + " Y =" + _mousestate.Y);
                //Console.WriteLine("Police Cooldown = " + c_police);
                //Console.WriteLine("Sign Cooldown = " + c_sign);
                //Console.WriteLine("Car Cooldown = " + c_car);
                //Console.WriteLine("Type = " + type_click);
                Console.WriteLine("timescore = " + time_score + " & " + time_scoreb);
                Console.WriteLine("money a = " + money_a + " & " + money_b);
                Console.WriteLine("bird a = " + bird_a + " & " + bird_b);
                Console.WriteLine("stage = " + stage + " & " + stageb);
                //Console.WriteLine("Screen = " + screen
                Console.WriteLine("main = " + main_volume);
                Console.WriteLine("effect = " + main_volume_effect);
            }
            //reset highest
            if(_keyboardState.IsKeyDown(Keys.F4) & oldstate.IsKeyUp(Keys.F4))
            {
                filepath = Path.Combine("../net6.0-windows/Content/data/time.bin");
                file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                writer = new BinaryWriter(file);
                writer.Write((int)0);
                writer.Flush();
                writer.Close();
                filepath = Path.Combine("../net6.0-windows/Content/data/stage.bin");
                file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                writer = new BinaryWriter(file);
                writer.Write((int)0);
                writer.Flush();
                writer.Close();
                filepath = Path.Combine("../net6.0-windows/Content/data/bird.bin");
                file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                writer = new BinaryWriter(file);
                writer.Write((int)0);
                writer.Flush();
                writer.Close();
                filepath = Path.Combine("../net6.0-windows/Content/data/money.bin");
                file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                writer = new BinaryWriter(file);
                writer.Write((int)0);
                writer.Flush();
                writer.Close();
                time_scoreb = 0;
                bird_b = 0;
                money_b = 0;
                stageb = 0;
                h_hour = 0;
                h_second = 0;
                h_minute = 0;
            }
            if(_mousestate.LeftButton == ButtonState.Pressed)
            {
                click = true;
            }
            else
            {
                click = false;
            }
            switch (screen)
            {
                case ScreenState.Menu:
                    {
                        UpdateMenu(gameTime);
                        break;
                    }
                case ScreenState.Over:
                    {
                        UpdateOver(gameTime);
                        break;
                    }
                case ScreenState.Gameplay:
                    {
                        fade("in", (float)gameTime.ElapsedGameTime.TotalSeconds, 0.01f);
                        if (!start)
                        {
                            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
                            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
                            bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
                            start = true;
                        }
                        //end game
                        if (bird.Count <= 0)
                        {
                            Gameover = true;
                            pause = true;
                            if (allow_song_gameover)
                            {
                                MediaPlayer.Volume = 0;
                                MediaPlayer.Play(onEnd);
                                allow_song_gameover = false;
                            }
                            if (time_score > time_scoreb)
                            {
                                time_scoreb = time_score;
                                stageb = stage;
                                money_b = money_a;
                                bird_b = bird_a;
                                highscore = true;
                            }
                            filepath = Path.Combine("../net6.0-windows/Content/data/time.bin");
                            file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                            writer = new BinaryWriter(file);
                            writer.Write((int)time_scoreb);
                            writer.Flush();
                            writer.Close();
                            filepath = Path.Combine("../net6.0-windows/Content/data/stage.bin");
                            file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                            writer = new BinaryWriter(file);
                            writer.Write((int)stageb);
                            writer.Flush();
                            writer.Close();
                            filepath = Path.Combine("../net6.0-windows/Content/data/bird.bin");
                            file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                            writer = new BinaryWriter(file);
                            writer.Write((int)bird_b);
                            writer.Flush();
                            writer.Close();
                            filepath = Path.Combine("../net6.0-windows/Content/data/money.bin");
                            file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                            writer = new BinaryWriter(file);
                            writer.Write((int)money_b);
                            writer.Flush();
                            writer.Close();
                            MediaPlayer.IsRepeating = false;
                            screen = ScreenState.Over;
                        }

                        //Game update all
                        if (!pause)
                        {

                            //time
                            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            time_pick += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            //cooldown skill
                            c_police -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            c_sign -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            c_car -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (c_police <= 0)
                            {
                                c_police = 0;
                            }
                            if (c_sign <= 0)
                            {
                                c_sign = 0;
                            }
                            if (c_car <= 0)
                            {
                                c_car = 0;
                            }




                            //cheat
                            if (_keyboardState.IsKeyDown(Keys.F2) & oldstate.IsKeyUp(Keys.F2))
                            {
                                money += 500;
                            }
                            if (_keyboardState.IsKeyDown(Keys.F1) & oldstate.IsKeyUp(Keys.F1))
                            {
                                c_car = 0;
                                c_sign = 0;
                                c_police = 0;
                            }





                            //spawn dek
                            if (time_pick >= Time_spawn_dek || _keyboardState.IsKeyDown(Keys.F3) & oldstate.IsKeyUp(Keys.F3))
                            {
                                dek.Add(new kid(kid_texure, where_dekspawn, boss_health, barcolor));
                                sEffect[0].CreateInstance().Play();
                                MediaPlayer.Volume = 0;
                                MediaPlayer.Play(onFight);
                                attacked = true;
                            }


                            //update bird
                            foreach (Pigeon ppbird in bird)
                            {
                                ppbird.move(none_area);
                                //randomlocation & check bird go outside
                                ppbird.updatebird(bound_X, bound_Y, elapsed, bfood);
                                if (ppbird.pos.Y <= 0 - pigeon_texture.Height)
                                {
                                    bird.Remove(ppbird);
                                    break;
                                }
                                //check bird_with_food
                                foreach (food mini_food in bfood)
                                {
                                    if (ppbird.checkfood(mini_food.hitbox))
                                    {
                                        bfood.Remove(mini_food);
                                        break;
                                    }
                                }
                            }


                            //check attacked
                            if (attacked)
                            {

                                time_pick = 0;
                                //check all dek die
                                if (dek.Count <= 0)
                                {
                                    attacked = false;
                                    boss_health += boss_health_per_level;
                                    stage += 1;
                                }
                                //update dek , location dek, move, check_with_bird
                                foreach (kid deks in dek)
                                {

                                    //select location & check if dek go out
                                    deks.selectbird(bird, elapsed, _car, _sign, none_area);
                                    if (deks.pos.X < -100 && deks.death)
                                    {
                                        MediaPlayer.Volume = 0;
                                        MediaPlayer.Play(onPlay);
                                        dek.Remove(deks);
                                        break;
                                    }
                                    deks.move();
                                    //check_with_bird
                                    foreach (Pigeon checkinterbird in bird)
                                    {
                                        checkinterbird.checkintersect(deks.hitbox);
                                    }

                                    //check_with_police & update
                                    foreach (Police mini_police in _police)
                                    {
                                        deks.damaged(mini_police.hitbox, elapsed);
                                    }

                                }
                            }
                            else
                            {
                                bar = new Rectangle(68, 670, (int)(bar_width - (bar_width * (time_pick / Time_spawn_dek))), 10);
                            }

                            //change type click
                            if (!Gameover & !setting)
                            {
                                if (type_click != "sign_select")
                                {
                                    if (button_bird.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "buybird";
                                        holding_text = "Buy Bird";
                                        holding_price = "20 Coins";
                                    }
                                    else if (button_sign.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "sign";
                                        holding_text = "Buy Sign";
                                        holding_price = "75 Coins";
                                    }
                                    else if (button_police.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "police";
                                        holding_text = "Call Police";
                                        holding_price = "110 Coins";
                                    }
                                    else if (button_car.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "car";
                                        holding_text = "Call Red Car";
                                        holding_price = "175 Coins";
                                    }
                                    else if (button_pause.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "pause";
                                        holding_text = "Pause the game";
                                        holding_price = " ";
                                    }
                                    else if (none_area.Contains(_mousestate.X, _mousestate.Y) || none_area2.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        type_click = "None";
                                        holding_text = " ";
                                        holding_price = " ";
                                    }
                                    else if (_mousestate.X >= _graphics.GraphicsDevice.Viewport.Width || _mousestate.Y > _graphics.GraphicsDevice.Viewport.Height || _mousestate.X <= 0 || _mousestate.Y <= 0)
                                    {
                                        type_click = "None";
                                        holding_text = " ";
                                        holding_price = " ";
                                    }
                                    else
                                    {
                                        type_click = "food";
                                        holding_text = " ";
                                        holding_price = " ";
                                    }
                                }
                            }
                            //click screen
                            if (_mousestate.LeftButton == ButtonState.Pressed && oldms.LeftButton == ButtonState.Released)
                            {
                                if (type_click == "food")
                                {
                                    if (attacked)
                                    {
                                        foreach (kid mini_dek in dek)
                                        {
                                            if (!mini_dek.damaged(_mousestate, oldms))
                                            {
                                                if (money >= food_price)
                                                {
                                                    var instance = sEffect[1].CreateInstance();
                                                    instance.Volume = 0.2f;
                                                    instance.Play();
                                                    bfood.Add(new food(food_texture, 1, 1, 1, new Vector2(_mousestate.X, _mousestate.Y)));
                                                    money -= food_price;
                                                }
                                                else
                                                {
                                                    sEffect[9].CreateInstance().Play();
                                                }
                                            }
                                            else //feedback
                                            {
                                                sEffect[4].CreateInstance().Play();
                                                hit.Add(new Ceffect(hit_effect, new Vector2(_mousestate.X, _mousestate.Y)));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (money >= food_price)
                                        {
                                            var instance = sEffect[1].CreateInstance();
                                            instance.Volume = 0.2f;
                                            instance.Play();
                                            bfood.Add(new food(food_texture, 1, 1, 1, new Vector2(_mousestate.X, _mousestate.Y)));
                                            money -= food_price;
                                        }
                                        else
                                        {
                                            sEffect[9].CreateInstance().Play();
                                        }
                                    }
                                }
                                else if (type_click == "buybird")
                                {
                                    if (money >= bird_price)
                                    {
                                        sEffect[2].CreateInstance().Play();
                                        bird.Add(new Pigeon(pigeon_texture, pigeon_fly, new Vector2(rnd.Next(10, bound_X), rnd.Next(310, bound_Y))));
                                        money -= bird_price;
                                    }
                                    else
                                    {
                                        sEffect[9].CreateInstance().Play();
                                    }
                                }
                                else if (type_click == "sign")
                                {
                                    if (c_sign == 0 & money >= price_sign)
                                    {
                                        sEffect[2].CreateInstance().Play();
                                        type_click = "sign_select";
                                        holding_text = "Choose the area";
                                        holding_price = " ";
                                        money -= price_sign;
                                    }
                                    else
                                    {
                                        sEffect[9].CreateInstance().Play();
                                    }
                                }
                                else if (type_click == "sign_select")
                                {
                                    if (!none_area.Contains(_mousestate.X, _mousestate.Y) & !none_area2.Contains(_mousestate.X, _mousestate.Y))
                                    {
                                        if (_mousestate.X < _graphics.GraphicsDevice.Viewport.Width & _mousestate.Y < _graphics.GraphicsDevice.Viewport.Height & _mousestate.X > 0 & _mousestate.Y > 0)
                                        {
                                            c_sign = cooldown_sign;
                                            type_click = "food";
                                            sEffect[8].CreateInstance().Play();
                                            _sign.Add(new Sign(sign_texture, new Vector2(_mousestate.X - (sign_texture.Width / 2), _mousestate.Y - (sign_texture.Height / 2)), time_sign, area_sign));
                                        }
                                    }
                                }
                                else if (type_click == "police")
                                {
                                    if (c_police == 0 & money >= price_police)
                                    {
                                        sEffect[2].CreateInstance().Play();
                                        _police.Add(new Police(police_texture, time_police));
                                        c_police = cooldown_police;
                                        money -= price_police;
                                    }
                                    else
                                    {
                                        sEffect[9].CreateInstance().Play();
                                    }

                                }
                                else if (type_click == "car")
                                {
                                    if (c_car == 0 & money >= price_car)
                                    {
                                        sEffect[2].CreateInstance().Play();
                                        _car.Add(new Car(car_texture, time_car));
                                        c_car = cooldown_car;
                                        money -= price_car;
                                    }
                                    else
                                    {
                                        sEffect[9].CreateInstance().Play();
                                    }
                                }
                                else if (type_click == "pause")
                                {
                                    pause = true;
                                    type_click = "None";
                                }
                            }
                            //pause by keyboard
                            if (_keyboardState.IsKeyDown(Keys.Escape) & oldstate.IsKeyUp(Keys.Escape))
                            {
                                pause = true;
                                type_click = "None";
                            }

                            //update car
                            foreach (Car mini_car in _car)
                            {
                                mini_car.update(elapsed);
                                if (mini_car.pos.X < -250)
                                {
                                    _car.Remove(mini_car);
                                    break;
                                }
                                foreach (kid mini_dek in dek)
                                {
                                    if (mini_dek.health <= mini_dek.max_health / 2 & mini_dek.hitbox.Intersects(mini_car.hitbox))
                                    {
                                        MediaPlayer.Volume = 0;
                                        MediaPlayer.Play(onPlay);
                                        dek.Remove(mini_dek);
                                        break;
                                    }
                                }
                            }
                            //update police

                            foreach (Police mini_police in _police)
                            {
                                mini_police.selecttarget(dek, elapsed);
                                mini_police.update();
                                if (mini_police.time_out)
                                {
                                    if (mini_police.pos.X >= _graphics.GraphicsDevice.Viewport.Width)
                                    {
                                        _police.Remove(mini_police);
                                        break;
                                    }
                                }
                            }

                            //update sign
                            foreach (Sign mini_sign in _sign)
                            {
                                if (mini_sign.time_out(elapsed))
                                {
                                    _sign.Remove(mini_sign);
                                    break;
                                }
                            }

                            //Control worth coin
                            if (bird.Count > 0 && bird.Count <= 3)
                            {
                                Coin.worth = 5;
                            }
                            else if (bird.Count > 3 && bird.Count <= 7)
                            {
                                Coin.worth = 4;
                            }
                            else if (bird.Count > 7 && bird.Count <= 10)
                            {
                                Coin.worth = 3;
                            }
                            else if (bird.Count > 10 && bird.Count <= 15)
                            {
                                Coin.worth = 2;
                            }
                            else if (bird.Count > 15)
                            {
                                Coin.worth = 1;
                            }


                            //destroy food
                            foreach (food mini_food in bfood)
                            {
                                if (mini_food.selfdestroy(elapsed))
                                {
                                    bfood.Remove(mini_food);
                                    break;
                                }
                            }

                            //update effect
                            foreach (Ceffect mini_effect in hit)
                            {
                                if (mini_effect.update(elapsed))
                                {
                                    hit.Remove(mini_effect);
                                    break;
                                }
                            }

                            //update score
                            time_score += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (bird.Count > bird_a)
                            {
                                bird_a = bird.Count;
                            }
                            if (money > money_a)
                            {
                                money_a = money;
                            }
                        }
                        else if (pause)
                        {
                            if (button_pause.Contains(_mousestate.X, _mousestate.Y) & !Gameover)
                            {
                                holding_text = "Resume";
                                if (_mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
                                {
                                    pause = false;
                                }
                            }
                            if (_keyboardState.IsKeyDown(Keys.Escape) & oldstate.IsKeyUp(Keys.Escape) & !Gameover)
                            {
                                pause = false;
                            }
                            if (button_exit.Contains(_mousestate.X, _mousestate.Y) & !Gameover)
                            {
                                holding_text = "Back to menu";
                                if (_mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
                                {
                                    MediaPlayer.Volume = 0;
                                    MediaPlayer.Play(onLobby);
                                    MediaPlayer.IsRepeating = true;
                                    screen = ScreenState.Menu;
                                }
                            }
                            if (_keyboardState.IsKeyDown(Keys.Space) & oldstate.IsKeyUp(Keys.Space))
                            {
                                if (allow_song_gameover)
                                {
                                    MediaPlayer.Volume = 0;
                                    MediaPlayer.Play(onEnd);
                                    allow_song_gameover = false;
                                    MediaPlayer.IsRepeating = false;
                                }
                                screen = ScreenState.Over;
                            }
                        }
                        break;
                    }
            }

            if (type_click != "sign_select")
            {
                if (setting_button.Contains(_mousestate.X, _mousestate.Y) & _mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
                {
                    if (!setting)
                    {
                        var instance = sEffect[10].CreateInstance();
                        instance.Volume = 0.5f;
                        instance.Play();
                        setting = true;
                    }
                }
                if (setting)
                {
                    if (setting_exit.Contains(_mousestate.X, _mousestate.Y) & _mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
                    {
                        var instance = sEffect[10].CreateInstance();
                        instance.Volume = 0.5f;
                        instance.Play();
                        setting = false;
                    }
                    //block volume music
                    for(float i=0;i<music_button.Length;i++)
                    {
                        if (music_button[(int)i].Contains(_mousestate.X, _mousestate.Y) & _mousestate.LeftButton == ButtonState.Pressed)
                        {
                            main_volume = i/10;
                            MediaPlayer.Volume = main_volume;
                        }
                    }
                    //block volume sound effect

                    for (float i = 0; i < seffect_button.Length; i++)
                    {
                        if (seffect_button[(int)i].Contains(_mousestate.X, _mousestate.Y) & _mousestate.LeftButton == ButtonState.Pressed)
                        {
                            main_volume_effect = i/10;
                            SoundEffect.MasterVolume = main_volume_effect;
                        }
                    }
                }
            }
            //old item
            oldms = _mousestate;
            oldstate = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (screen)
            {
                case ScreenState.Menu:
                    {
                        DrawMenu(_spriteBatch);
                        break;
                    }
                case ScreenState.Over:
                    {
                        DrawOver(_spriteBatch);
                        break;
                    }
                case ScreenState.Gameplay:
                    {
                        _spriteBatch.Draw(bg, _graphics.GraphicsDevice.Viewport.Bounds, Color.White);
                        //food
                        foreach (food mini_food in bfood)
                        {
                            mini_food.Draw(_spriteBatch);
                        }
                        //sign
                        foreach (Sign mini_sign in _sign)
                        {
                            mini_sign.Draw(_spriteBatch);
                        }
                        //bird
                        foreach (Pigeon ppbird in bird)
                        {
                            ppbird.Pigeondraw(_spriteBatch, uid_font);
                        }
                        //police
                        foreach (Police mini_police in _police)
                        {
                            mini_police.Draw(_spriteBatch);
                        }
                        //kid
                        foreach (kid deks in dek)
                        {
                            deks.kiddraw(_spriteBatch);
                        }
                        //car
                        foreach (Car mini_car in _car)
                        {
                            mini_car.Draw(_spriteBatch);
                        }
                        if (type_click == "sign_select")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text, new Vector2(105, 590), Color.Black);
                            if (!none_area.Contains(_mousestate.X, _mousestate.Y) & !none_area2.Contains(_mousestate.X, _mousestate.Y) & _mousestate.X < _graphics.GraphicsDevice.Viewport.Width & _mousestate.Y < _graphics.GraphicsDevice.Viewport.Height & _mousestate.X > 0 & _mousestate.Y > 0)
                            {
                                _spriteBatch.Draw(sign_texture, new Vector2(_mousestate.X - (sign_texture.Width / 2), _mousestate.Y - (sign_texture.Height / 2)), Color.White);
                            }
                        }
                        _spriteBatch.Draw(board_ui, new Vector2(0, 642), Color.White);
                        //bar
                        if (!attacked)
                        {
                            _spriteBatch.Draw(barcolor, bar, Color.LimeGreen);

                        }
                        //feedback
                        foreach (Ceffect mini_effect in hit)
                        {
                            mini_effect.Draw(_spriteBatch);
                        }
                        //ui
                        //_spriteBatch.Draw(barcolor, none_area2, Color.White);
                        if (type_click == "buybird")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(100, 590), Color.Black);
                        }
                        else if (type_click == "police")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(100, 590), Color.Black);
                        }
                        else if (type_click == "sign")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(100, 590), Color.Black);
                        }
                        else if (type_click == "car")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text + " " + holding_price, new Vector2(85, 590), Color.Black);
                        }
                        else if (type_click == "pause")
                        {
                            _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                            _spriteBatch.DrawString(uid_font, holding_text, new Vector2(120, 590), Color.Black);
                        }
                        _spriteBatch.Draw(bar_grob, new Vector2(68, 666), Color.White);
                        _spriteBatch.Draw(buy_bird, button_bird, Color.White);
                        if (c_sign == 0)
                        {
                            _spriteBatch.Draw(buy_sign, button_sign, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(buy_sign, button_sign, Color.Gray);
                            _spriteBatch.DrawString(cooldown_text, "" + (int)c_sign, new Vector2(button_sign.X + 80, button_sign.Y + 34), Color.White);
                        }
                        if (c_car == 0)
                        {
                            _spriteBatch.Draw(buy_car, button_car, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(buy_car, button_car, Color.Gray);
                            _spriteBatch.DrawString(cooldown_text, "" + (int)c_car, new Vector2(button_car.X + 80, button_car.Y + 34), Color.White);
                        }

                        if (c_police == 0)
                        {
                            _spriteBatch.Draw(buy_police, button_police, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(buy_police, button_police, Color.Gray);
                            _spriteBatch.DrawString(cooldown_text, "" + (int)c_police, new Vector2(button_police.X + 60, button_police.Y + 22), Color.White);
                        }
                        _spriteBatch.DrawString(uid_font, Convert.ToString(money), new Vector2(77, 746), Color.Black);
                        _spriteBatch.DrawString(uid_font, Convert.ToString(bird.Count), new Vector2(77, 710), Color.Black);

                        if (!pause)
                        {
                            _spriteBatch.Draw(pause_1, button_pause, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(pause_dark, Vector2.Zero, Color.White);
                            _spriteBatch.Draw(pause_2, button_pause, Color.White);
                            _spriteBatch.Draw(exit_texture, button_exit, Color.White);
                            if (button_pause.Contains(_mousestate.X, _mousestate.Y) & !Gameover)
                            {
                                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                                _spriteBatch.DrawString(uid_font, holding_text, new Vector2(140, 590), Color.Black);
                            }
                            if (button_exit.Contains(_mousestate.X, _mousestate.Y) & !Gameover)
                            {
                                _spriteBatch.Draw(talk_texture, new Vector2(21, 561), Color.White);
                                _spriteBatch.DrawString(uid_font, holding_text, new Vector2(130, 590), Color.Black);
                            }
                        }
                        break;
                    }
            }
            _spriteBatch.Draw(settingicon, setting_button, Color.White);   //setting button
            if (setting)
            {
                _spriteBatch.Draw(setting_popup, new Rectangle(250, 125, 700, 550), Color.White);
                _spriteBatch.Draw(confirm, setting_exit, Color.White);
                //music
                for (float i = 0; i < music_button.Length; i++)
                {
                    if (main_volume == i/10)
                    {
                        _spriteBatch.Draw(volume, music_button[(int)i], Color.White);
                    }
                }
                //sound effect
                for (float i = 0; i < seffect_button.Length; i++)
                {
                    if (main_volume_effect == (i / 10))
                    {
                        _spriteBatch.Draw(volume, seffect_button[(int)i], Color.White);
                    }
                }
            }
            if (!click)
            {
                _spriteBatch.Draw(mouse_1, new Vector2(_mousestate.X - 5, _mousestate.Y), Color.White); //cursor
            }
            else
            {
                _spriteBatch.Draw(mouse_2, new Vector2(_mousestate.X - 5, _mousestate.Y), Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        private int h_hour = 0,h_minute = 0, h_second = 0;
        private void UpdateMenu(GameTime gameTime)
        {
            float highest = time_scoreb;
            fade("in", (float)gameTime.ElapsedGameTime.TotalSeconds, 0.01f);
            if (start)
            {
                ResetGame();
            }
            if (_mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released & !setting & !board)
            {
                if (button_play.Contains(_mousestate.X, _mousestate.Y))
                {
                    var instance = sEffect[10].CreateInstance();
                    instance.Volume = 0.5f;
                    instance.Play();
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Play(onPlay);
                    screen = ScreenState.Gameplay;
                }
                if (button_score.Contains(_mousestate.X, _mousestate.Y))
                {
                    var instance = sEffect[10].CreateInstance();
                    instance.Volume = 0.5f;
                    instance.Play();
                    board = true;
                    if (highest >= 3600)
                    {
                        h_hour = (int)(highest / 3600);
                        highest -= (h_hour * 3600);
                    }
                    if (highest >= 60)
                    {
                        h_minute = (int)(highest / 60);
                        highest -= (h_minute * 60);
                        h_second = (int)highest;
                    }
                    else
                    {
                        h_second = (int)highest;
                    }
                }
            }
            if (board)
            {
                if (button_close.Contains(_mousestate.X, _mousestate.Y) & _mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
                {
                    var instance = sEffect[10].CreateInstance();
                    instance.Volume = 0.5f;
                    instance.Play();
                    board = false;
                }
            }
        }
        private void DrawMenu(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(menu, Vector2.Zero, Color.White);
            _spriteBatch.Draw(play, button_play, Color.White);
            _spriteBatch.Draw(scorebutton, button_score, Color.White);
            if (board)
            {
                _spriteBatch.Draw(scoreboard, new Rectangle(250, 125, 700, 550), Color.White);
                _spriteBatch.Draw(close, button_close, Color.White);
                _spriteBatch.DrawString(cooldown_text, h_hour+"h "+h_minute+"m "+h_second+"s ", new Vector2(670, 285), Color.MidnightBlue);
                _spriteBatch.DrawString(cooldown_text, Convert.ToString(bird_b), new Vector2(670, 365), Color.MidnightBlue);
                _spriteBatch.DrawString(cooldown_text, Convert.ToString(money_b), new Vector2(670, 450), Color.MidnightBlue);
                _spriteBatch.DrawString(cooldown_text, Convert.ToString(stageb), new Vector2(670, 540), Color.MidnightBlue);
            }
        }



        //over value
        private float over_time_text = 0;
        private int hour = 0, minute = 0, second = 0, over_bird = 0, over_money = 0, over_stage = 0;
        private bool over_time_success = false, overnum = false;
        private void UpdateOver(GameTime gameTime)
        {
            fade("in", (float)gameTime.ElapsedGameTime.TotalSeconds, 0.01f);
            if (over_time_text < 15000 & !over_time_success)
            {
                over_time_text += 123;
            }
            else
            {
                over_time_success = true;
                over_time_text = time_score;
                over_time_text = (int)(over_time_text * 100);
                over_time_text = over_time_text / 100;
                if (!overnum)
                {
                    if(over_time_text >= 3600)
                    {
                        hour = (int)(over_time_text / 3600);
                        over_time_text = over_time_text - (hour * 3600);
                    }
                    if(over_time_text >= 60)
                    {
                        minute = (int)(over_time_text / 60);
                        second = (int)(over_time_text - (minute * 60));
                    }
                    else
                    {
                        second = (int)over_time_text;
                    }
                    overnum = true;
                }
            }
            if(over_bird < bird_a)
            {
                over_bird += 1;
            }
            if (over_stage < stage)
            {
                over_stage += 1;
            }
            if(over_money < 2460 & over_money != money_a)
            {
                over_money += 123;
            }
            else
            {
                over_money = (int)money_a;
            }

            


            if (_mousestate.LeftButton == ButtonState.Pressed & oldms.LeftButton == ButtonState.Released)
            {
                if (button_home.Contains(_mousestate.X, _mousestate.Y))
                {
                    var instance = sEffect[10].CreateInstance();
                    instance.Volume = 0.5f;
                    instance.Play();
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Play(onLobby);
                    screen = ScreenState.Menu;
                }
                if (button_retry.Contains(_mousestate.X, _mousestate.Y))
                {
                    var instance = sEffect[10].CreateInstance();
                    instance.Volume = 0.5f;
                    instance.Play();
                    ResetGame();
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Play(onPlay);
                    screen = ScreenState.Gameplay;
                }
            }
        }
        private void DrawOver(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(over_screen, Vector2.Zero, Color.White);
            if (highscore)
            {
                _spriteBatch.Draw(text_highscore, new Vector2(400, 370), Color.White);
            }
            else
            {
                _spriteBatch.Draw(text_score, new Vector2(450, 370), Color.White);
            }
            _spriteBatch.Draw(home, button_home, Color.White);
            _spriteBatch.Draw(retry, button_retry, Color.White);
            if (!over_time_success)
            {
                _spriteBatch.DrawString(uid_font, Convert.ToString(over_time_text), new Vector2(630, 440), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(uid_font, hour + "h " + minute + "m " + second + "s ", new Vector2(610, 440), Color.White);
            }
            _spriteBatch.DrawString(uid_font, Convert.ToString(over_bird), new Vector2(630, 492), Color.White);
            _spriteBatch.DrawString(uid_font, Convert.ToString(over_money), new Vector2(630, 555), Color.White);
            _spriteBatch.DrawString(uid_font, Convert.ToString(over_stage), new Vector2(630, 610), Color.White);
        }


        private void ResetGame()
        {
            start = false;
            money = 20;
            stage = 0;
            time_score = 0;
            money_a = 0;
            bird_a = 0;
            bird.Clear();
            dek.Clear();
            _car.Clear();
            bfood.Clear();
            _police.Clear();
            _sign.Clear();
            c_police = cooldown_police;
            c_sign = cooldown_sign;
            c_car = cooldown_car;
            Gameover = false;
            time_pick = 0;
            boss_health = 40;
            attacked = false;
            pause = false;
            Coin.worth = 5;
            highscore = false;
            allow_song_gameover = true;
            overnum = false;
            over_time_success = false;
            MediaPlayer.IsRepeating = true;
        }

        protected void fade(string type, float elapsed, float rate)
        {
            Media_elapsed += elapsed;
            if (type == "out")
            {
                if (Media_elapsed > 0.5 && MediaPlayer.Volume > 0)
                {
                    MediaPlayer.Volume -= rate;
                }
            }
            if (type == "in")
            {
                if (Media_elapsed > 0.5 && MediaPlayer.Volume < main_volume)
                {
                    MediaPlayer.Volume += rate;
                }
            }
        }
    }
}