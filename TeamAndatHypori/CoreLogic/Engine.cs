using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using TeamAndatHypori.Objects.Characters.NPCs.Enemies;
using TeamAndatHypori.Objects.Characters.PlayableCharacters;
using TeamAndatHypori.Objects.Items;
using TeamAndatHypori.Objects.Projectiles;
using TeamAndatHypori.Configuration;
using TeamAndatHypori.Enums;
using TeamAndatHypori.GUI;
using TeamAndatHypori.Objects;
using TeamAndatHypori.Objects.Items.Consumables;
using TeamAndatHypori.Objects.Items.Equipment;

namespace TeamAndatHypori.CoreLogic
{
    using Microsoft.Xna.Framework.Input;

    public class Engine : Game
    {
        public readonly Item[] AllEquipments = new Item[7];
        public readonly Item[] AllPotions = new Item[3];

        private static readonly Random Rand = new Random();
        private GraphicsDeviceManager graphics;
        private Gui gui;
        private Map map;

        #region Sounds
        private SoundEffect[] Kills;
        private SoundEffect[] LevelUp;

        private SoundEffect BossPrepare;
        private SoundEffect BossKill;
        private SoundEffect PickWarrior;
        private SoundEffect PickRogue;
        private SoundEffect PickWizard;
        private SoundEffect OrcHurt;
        #endregion

        #region Textures
        public Texture2D[] PlayerMoveRight { get; private set; }
        public Texture2D[] PlayerMoveLeft { get; private set; }
        public Texture2D[] PlayerRightAttack { get; private set; }
        public Texture2D[] PlayerLeftAttack { get; private set; }
        public Texture2D[] PlayerLeftSpecial { get; private set; }
        public Texture2D[] PlayerRightSpecial { get; private set; }
        public Texture2D[] PlayerDefeat { get; private set; }

        public Texture2D[] OrcMoveRight { get; private set; }
        public Texture2D[] OrcMoveLeft { get; private set; }
        public Texture2D[] OrcRightAttack { get; private set; }
        public Texture2D[] OrcLeftAttack { get; private set; }
        public Texture2D[] OrcLeftDead { get; private set; }
        public Texture2D[] OrcRightDead { get; private set; }

        public Texture2D[] OrcArcherMoveRight { get; private set; }
        public Texture2D[] OrcArcherMoveLeft { get; private set; }
        public Texture2D[] OrcArcherRightAttack { get; private set; }
        public Texture2D[] OrcArcherLeftAttack { get; private set; }
        public Texture2D[] OrcArcherLeftDead { get; private set; }
        public Texture2D[] OrcArcherRightDead { get; private set; }

        public Texture2D[] OrcLordMoveRight { get; private set; }
        public Texture2D[] OrcLordMoveLeft { get; private set; }
        public Texture2D[] OrcLordRightAttack { get; private set; }
        public Texture2D[] OrcLordLeftAttack { get; private set; }
        public Texture2D[] OrcLordLeftDead { get; private set; }
        public Texture2D[] OrcLordRightDead { get; private set; }

        public Texture2D HealthPotion { get; private set; }
        public Texture2D DamagePotion { get; private set; }
        public Texture2D DefensePotion { get; private set; }

        public Texture2D Boots { get; private set; }
        public Texture2D Gloves { get; private set; }
        public Texture2D Helm { get; private set; }
        public Texture2D Armor { get; private set; }
        public Texture2D Sword { get; private set; }
        public Texture2D Daggers { get; private set; }
        public Texture2D Orb { get; private set; }

        public Texture2D Map { get; private set; }

        public Texture2D ArrowLeft { get; private set; }
        public Texture2D ArrowRight { get; private set; }
        public Texture2D DaggerLeft { get; private set; }
        public Texture2D DaggerRight { get; private set; }
        public Texture2D FireballLeft { get; private set; }
        public Texture2D FireballRight { get; private set; }
        public Texture2D FireboltLeft { get; private set; }
        public Texture2D FireboltRight { get; private set; }

        public Texture2D[] Explosion { get; private set; }
        #endregion

        public SpriteFont Font { get; set; }
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Projectile> Projectiles { get; private set; }
        private KeyboardState CurrentKeyboardState { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }

        public Engine()
        {
            this.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Resources";


        }

        protected override void Initialize()
        {
            // Sets screen size
            this.graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            this.graphics.PreferredBackBufferHeight = Config.ScreenHeight;
            this.graphics.ApplyChanges();
            this.IsMouseVisible = false;

            //this.Player = new Warrior();

            this.gui = new Gui(this);
            this.Player = new Rogue();
            this.Projectiles = new List<Projectile>();
            this.Font = Content.Load<SpriteFont>("font");

            this.AllEquipments[0] = new Sword();
            this.AllEquipments[1] = new Orb();
            this.AllEquipments[2] = new Daggers();
            this.AllEquipments[3] = new Armor();
            this.AllEquipments[4] = new Boots();
            this.AllEquipments[5] = new Helm();
            this.AllEquipments[6] = new Gloves();

            this.AllPotions[0] = new HealingPotion();
            this.AllPotions[1] = new DamagePotion();
            this.AllPotions[2] = new DefensePotion();

            // Load Enemies
            this.Enemies = new List<Enemy>
            {
                //new Orc(400, 300),
                //new Orc(400, 300),
                //new Orc(400, 300),
                //new Orc(400, 300),
                //new Orc(400, 300),
            };
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Sounds
            #region SoundsLoad
            this.Kills = new[]
            {
                Content.Load<SoundEffect>(Assets.KillSounds[0]),
                Content.Load<SoundEffect>(Assets.KillSounds[1]),
                Content.Load<SoundEffect>(Assets.KillSounds[2]),
                Content.Load<SoundEffect>(Assets.KillSounds[3]),
                Content.Load<SoundEffect>(Assets.KillSounds[4]),
                Content.Load<SoundEffect>(Assets.KillSounds[5]),
            };

            this.LevelUp = new[]
            {
                Content.Load<SoundEffect>(Assets.LevelUpSounds[0]),
                Content.Load<SoundEffect>(Assets.LevelUpSounds[1]),
                Content.Load<SoundEffect>(Assets.LevelUpSounds[2]),
            };

            this.PickWarrior = Content.Load<SoundEffect>(Assets.WarriorSelect);
            this.PickRogue = Content.Load<SoundEffect>(Assets.RogueSelect);
            this.PickWizard = Content.Load<SoundEffect>(Assets.WizardSelect);
            this.BossPrepare = Content.Load<SoundEffect>(Assets.BossPrepare);
            this.BossKill = Content.Load<SoundEffect>(Assets.BossKill);
            this.OrcHurt = Content.Load<SoundEffect>(Assets.OrcHurt); 
            #endregion

            // Load the visual resources
            #region TexturesLoad
            if (this.Player is Warrior)
            {
                this.PlayerMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                    Content.Load<Texture2D>(Assets.WarriorImages[1]),
                    Content.Load<Texture2D>(Assets.WarriorImages[2]),
                };

                this.PlayerMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                    Content.Load<Texture2D>(Assets.WarriorImages[10]),
                    Content.Load<Texture2D>(Assets.WarriorImages[11]),
                };

                this.PlayerRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[3]),
                    Content.Load<Texture2D>(Assets.WarriorImages[4]),
                    Content.Load<Texture2D>(Assets.WarriorImages[5]),
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                };

                this.PlayerLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[12]),
                    Content.Load<Texture2D>(Assets.WarriorImages[13]),
                    Content.Load<Texture2D>(Assets.WarriorImages[14]),
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                };
                this.PlayerRightSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[6]),
                    Content.Load<Texture2D>(Assets.WarriorImages[7]),
                    Content.Load<Texture2D>(Assets.WarriorImages[8]),
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                };
                this.PlayerLeftSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[15]),
                    Content.Load<Texture2D>(Assets.WarriorImages[16]),
                    Content.Load<Texture2D>(Assets.WarriorImages[17]),
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                };

                this.PlayerDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.WarriorImages[18]),
                };
            }
            else if (this.Player is Rogue)
            {
                this.PlayerMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[0]),
                    Content.Load<Texture2D>(Assets.RogueImages[1]),
                    Content.Load<Texture2D>(Assets.RogueImages[2]),
                    Content.Load<Texture2D>(Assets.RogueImages[3]),
                };

                this.PlayerMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[7]),
                    Content.Load<Texture2D>(Assets.RogueImages[8]),
                    Content.Load<Texture2D>(Assets.RogueImages[9]),
                    Content.Load<Texture2D>(Assets.RogueImages[10]),
                };

                this.PlayerRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[4]),
                    Content.Load<Texture2D>(Assets.RogueImages[5]),
                    Content.Load<Texture2D>(Assets.RogueImages[6]),
                    Content.Load<Texture2D>(Assets.RogueImages[0]),
                };

                this.PlayerLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[11]),
                    Content.Load<Texture2D>(Assets.RogueImages[12]),
                    Content.Load<Texture2D>(Assets.RogueImages[13]),
                    Content.Load<Texture2D>(Assets.RogueImages[7]),
                };
                this.PlayerDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.RogueImages[14]),
                };
            }

            else if (this.Player is Wizard)
            {
                this.PlayerMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                    Content.Load<Texture2D>(Assets.WizardImages[1]),
                    Content.Load<Texture2D>(Assets.WizardImages[2]),
                    Content.Load<Texture2D>(Assets.WizardImages[3]),
                };

                this.PlayerMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                    Content.Load<Texture2D>(Assets.WizardImages[11]),
                    Content.Load<Texture2D>(Assets.WizardImages[12]),
                    Content.Load<Texture2D>(Assets.WizardImages[13]),
                };

                this.PlayerRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[4]),
                    Content.Load<Texture2D>(Assets.WizardImages[5]),
                    Content.Load<Texture2D>(Assets.WizardImages[6]),
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                };

                this.PlayerLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[14]),
                    Content.Load<Texture2D>(Assets.WizardImages[15]),
                    Content.Load<Texture2D>(Assets.WizardImages[16]),
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                };
                this.PlayerRightSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[7]),
                    Content.Load<Texture2D>(Assets.WizardImages[8]),
                    Content.Load<Texture2D>(Assets.WizardImages[9]),
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                };
                this.PlayerLeftSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[17]),
                    Content.Load<Texture2D>(Assets.WizardImages[18]),
                    Content.Load<Texture2D>(Assets.WizardImages[19]),
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                };

                this.PlayerDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.WizardImages[20]),
                };
            }

            // Load enemy resources

            //Orc

            this.OrcMoveLeft = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[0]),
                Content.Load<Texture2D>(Assets.OrcImages[1]),
                Content.Load<Texture2D>(Assets.OrcImages[2]),
                Content.Load<Texture2D>(Assets.OrcImages[3]),
            };

            this.OrcMoveRight = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[11]),
                Content.Load<Texture2D>(Assets.OrcImages[12]),
                Content.Load<Texture2D>(Assets.OrcImages[13]),
                Content.Load<Texture2D>(Assets.OrcImages[14]),
            };

            this.OrcLeftAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[4]),
                Content.Load<Texture2D>(Assets.OrcImages[5]),
                Content.Load<Texture2D>(Assets.OrcImages[6]),
                Content.Load<Texture2D>(Assets.OrcImages[7]),
            };

            this.OrcRightAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[15]),
                Content.Load<Texture2D>(Assets.OrcImages[16]),
                Content.Load<Texture2D>(Assets.OrcImages[17]),
                Content.Load<Texture2D>(Assets.OrcImages[18]),
            };

            this.OrcLeftDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[8]),
                Content.Load<Texture2D>(Assets.OrcImages[9]),
                Content.Load<Texture2D>(Assets.OrcImages[10]),
            };

            this.OrcRightDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcImages[19]),
                Content.Load<Texture2D>(Assets.OrcImages[20]),
                Content.Load<Texture2D>(Assets.OrcImages[21]),
            };

            //OrcArcher

            this.OrcArcherMoveLeft = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[0]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[1]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[2]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[3]),
            };

            this.OrcArcherMoveRight = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[11]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[12]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[13]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[14]),
            };

            this.OrcArcherLeftAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[4]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[5]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[6]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[7]),
            };

            this.OrcArcherRightAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[15]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[16]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[17]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[18]),
            };

            this.OrcArcherLeftDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[8]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[9]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[10]),
            };

            this.OrcArcherRightDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcArcherImages[19]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[20]),
                Content.Load<Texture2D>(Assets.OrcArcherImages[21]),
            };

            //OrcLord

            this.OrcLordMoveLeft = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[0]),
                Content.Load<Texture2D>(Assets.OrcLordImages[1]),
                Content.Load<Texture2D>(Assets.OrcLordImages[2]),
                Content.Load<Texture2D>(Assets.OrcLordImages[3]),
            };

            this.OrcLordMoveRight = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[11]),
                Content.Load<Texture2D>(Assets.OrcLordImages[12]),
                Content.Load<Texture2D>(Assets.OrcLordImages[13]),
                Content.Load<Texture2D>(Assets.OrcLordImages[14]),
            };

            this.OrcLordLeftAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[4]),
                Content.Load<Texture2D>(Assets.OrcLordImages[5]),
                Content.Load<Texture2D>(Assets.OrcLordImages[6]),
                Content.Load<Texture2D>(Assets.OrcLordImages[7]),
            };

            this.OrcLordRightAttack = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[15]),
                Content.Load<Texture2D>(Assets.OrcLordImages[16]),
                Content.Load<Texture2D>(Assets.OrcLordImages[17]),
                Content.Load<Texture2D>(Assets.OrcLordImages[18]),
            };

            this.OrcLordLeftDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[8]),
                Content.Load<Texture2D>(Assets.OrcLordImages[9]),
                Content.Load<Texture2D>(Assets.OrcLordImages[10]),
            };

            this.OrcLordRightDead = new[]
            {
                Content.Load<Texture2D>(Assets.OrcLordImages[19]),
                Content.Load<Texture2D>(Assets.OrcLordImages[20]),
                Content.Load<Texture2D>(Assets.OrcLordImages[21]),
            };

            //Load Items

            //Potions
            this.HealthPotion = Content.Load<Texture2D>(Assets.HealthPotion);
            this.DefensePotion = Content.Load<Texture2D>(Assets.DefensePotion);
            this.DamagePotion = Content.Load<Texture2D>(Assets.DamagePotion);

            //Equipment
            this.Armor = Content.Load<Texture2D>(Assets.Armor);
            this.Boots = Content.Load<Texture2D>(Assets.Boots);
            this.Helm = Content.Load<Texture2D>(Assets.Helmet);
            this.Gloves = Content.Load<Texture2D>(Assets.Gloves);
            this.Sword = Content.Load<Texture2D>(Assets.Sword);
            this.Orb = Content.Load<Texture2D>(Assets.Orb);
            this.Daggers = Content.Load<Texture2D>(Assets.Daggers);

            //Load Map
            this.Map = Content.Load<Texture2D>(Assets.Maps[0]);

            //Load Projectiles
            this.ArrowLeft = Content.Load<Texture2D>(Assets.ArrowImages[0]);
            this.ArrowRight = Content.Load<Texture2D>(Assets.ArrowImages[1]);
            this.DaggerLeft = Content.Load<Texture2D>(Assets.DaggerImages[0]);
            this.DaggerRight = Content.Load<Texture2D>(Assets.DaggerImages[1]);
            this.FireballLeft = Content.Load<Texture2D>(Assets.FireballImages[0]);
            this.FireballRight = Content.Load<Texture2D>(Assets.FireballImages[1]);
            this.FireboltLeft = Content.Load<Texture2D>(Assets.FireboltImages[0]);
            this.FireboltRight = Content.Load<Texture2D>(Assets.FireboltImages[1]);

            this.Explosion = new[]
            {
                Content.Load<Texture2D>(Assets.ExplosionImages[0]),
                Content.Load<Texture2D>(Assets.ExplosionImages[1]),
                Content.Load<Texture2D>(Assets.ExplosionImages[2]),
            }; 
            #endregion

            this.Player.Image = PlayerMoveRight[0];
            this.map = new Map(Map);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            this.Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            this.CurrentKeyboardState = Keyboard.GetState();

            //Exit check
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.UnloadContent();
            }

            this.UpdatePlayerState();
            this.UpdatePlayerDirection();
            this.Move();
            this.PlayerAttack();
            this.Player.Update();

            this.UpdateEnemiesState();
            this.EnemiesMove();
            this.EnemiesAttack();
            this.UpdateEnemies();

            if (this.Enemies.Count == 0)
            {
                //Implement next wave
            }

            //Use Item
            for (int index = 0; index < Config.UseItemKeys.Length; index++)
            {
                if (CurrentKeyboardState.IsKeyDown(Config.UseItemKeys[index]))
                {
                    this.Player.UseItem(index);
                }
            }

            // Discard item 
            for (int index = 0; index < Config.DiscardItemKeys.Length; index++)
            {
                if (CurrentKeyboardState.IsKeyDown(Config.DiscardItemKeys[index]))
                {
                    this.Player.DiscardItem(index);
                }
            }

            // Unequip Item 
            for (int index = 0; index < Config.UnequipItemKeys.Length; index++)
            {
                if (CurrentKeyboardState.IsKeyDown(Config.UnequipItemKeys[index]))
                {
                    this.Player.UnequipItem((EquipmentSlot)index);
                }
            }

            

            if (this.Player.State != State.Idle)
            {
                this.AnimatePlayer();
            }
            this.AnimateEnemies();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.SpriteBatch.Begin();

            this.SpriteBatch.Draw(this.map.Image, new Vector2(0, 0), Color.White);
            this.SpriteBatch.Draw(this.Player.Image, this.Player.Position, Color.White);
            foreach (var enemy in this.Enemies)
            {
                this.SpriteBatch.Draw(enemy.Image, enemy.Position, Color.White);
            }
            foreach (var projectile in this.Projectiles)
            {
                this.SpriteBatch.Draw(projectile.Image, projectile.Position, Color.White);
            }
            this.gui.Draw(this.SpriteBatch);

            this.SpriteBatch.End();
        }

        private void PlayerAttack()
        {
            // Player Attacking
            var enemiesInRange = this.Player.GetEnemiesInRange(this.Enemies);
            if (enemiesInRange.Count > 0)
            {
                if (this.Player.State == State.Special && this.Player.AnimationFrame == 2 && this.Player is Rogue == false)
                {
                    this.Player.SpecialAttack(enemiesInRange);
                }
                else if (this.Player.State == State.Attacking && this.Player.AnimationFrame == 2)
                {
                    this.Player.Attack(enemiesInRange);
                }
            }
        }

        private void EnemiesAttack()
        {
            foreach (var enemy in this.Enemies)
            {
                if (enemy.State == State.Attacking && enemy.AnimationFrame == 3)
                {
                    enemy.Attack(this.Player);
                }
            }
        }

        private void AnimateEnemies()
        {
            foreach (var enemy in this.Enemies)
            {
                if (enemy.AnimationDelay == 0)
                {
                    if (enemy is Orc)
                    {
                        if (enemy.State == State.Attacking)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Moving)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Dying)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcRightDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcRightDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (enemy is OrcArcher)
                    {
                        if (enemy.State == State.Attacking)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcArcherRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcArcherLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Moving)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcArcherRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcArcherLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Dying)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcArcherRightDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherRightDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcArcherLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (enemy is OrcLord)
                    {
                        if (enemy.State == State.Attacking)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcLordRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLordLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Moving)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcLordRightAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordRightAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLordLeftAttack[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordLeftAttack.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                        else if (enemy.State == State.Dying)
                        {
                            switch (enemy.Direction)
                            {
                                case Direction.Down:
                                case Direction.Right:
                                    enemy.Image = this.OrcLordRightDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordRightDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLordLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    enemy.AnimationDelay = 10;
                }
                enemy.AnimationDelay--;
            }
        }

        private void AnimatePlayer()
        {
            if (this.Player.AnimationDelay == 0)
            {
                if (this.Player.State == State.Special)
                {
                    switch (this.Player.Direction)
                    {
                        case Direction.Right:
                            this.Player.Image = this.PlayerRightSpecial[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerRightSpecial.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;

                        case Direction.Left:
                            this.Player.Image = this.PlayerLeftSpecial[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerLeftSpecial.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;
                    }
                }
                else if (this.Player.State == State.Attacking)
                {
                    switch (this.Player.Direction)
                    {
                        case Direction.Right:
                            this.Player.Image = this.PlayerRightAttack[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerRightAttack.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;

                        case Direction.Left:
                            this.Player.Image = this.PlayerLeftAttack[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerLeftAttack.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;
                    }
                }
                else if (this.Player.State == State.Moving)
                {
                    switch (this.Player.Direction)
                    {
                        case Direction.Right:
                            this.Player.Image = this.PlayerMoveRight[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerMoveRight.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;

                        case Direction.Left:
                            this.Player.Image = this.PlayerMoveLeft[this.Player.AnimationFrame++];
                            if (this.Player.AnimationFrame == this.PlayerMoveLeft.Length)
                            {
                                this.Player.State = State.Idle;
                                this.Player.AnimationFrame = 0;
                            }
                            break;
                    }
                }

                this.Player.AnimationDelay = 10;
            }

            this.Player.AnimationDelay--;
        }

        private void UpdatePlayerState()
        {
            if (this.Player.State == State.Idle || this.Player.State == State.Moving)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.Z) && this.Player is Rogue == false)
                {
                    this.Player.State = State.Special;
                    this.Player.AnimationFrame = 0;
                    this.Kills[Rand.Next(0, Kills.Length)].Play();
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Space))
                {
                    this.Player.State = State.Attacking;
                    this.Player.AnimationFrame = 0;
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Up) ||
                    CurrentKeyboardState.IsKeyDown(Keys.Down) ||
                    CurrentKeyboardState.IsKeyDown(Keys.Right) ||
                    CurrentKeyboardState.IsKeyDown(Keys.Left))
                {
                    this.Player.State = State.Moving;

                }
                else
                {
                    this.Player.State = State.Idle;
                }
            }     
            
        }

        private void UpdatePlayerDirection()
        {
            //Update Direction
            if (CurrentKeyboardState.IsKeyDown(Keys.Right))
            {
                this.Player.Direction = Direction.Right;
            }
            else if (CurrentKeyboardState.IsKeyDown(Keys.Left))
            {
                this.Player.Direction = Direction.Left; ;
            }

        }

        private void Move()
        {


            if (this.CurrentKeyboardState.IsKeyDown(Keys.Left))
            {
                this.Player.Position = new Vector2(this.Player.Position.X - this.Player.Speed, this.Player.Position.Y);
            }

            if (this.CurrentKeyboardState.IsKeyDown(Keys.Right))
            {
                this.Player.Position = new Vector2(this.Player.Position.X + this.Player.Speed, this.Player.Position.Y);
            }

            if (this.CurrentKeyboardState.IsKeyDown(Keys.Up))
            {
                this.Player.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y - this.Player.Speed);
            }

            if (this.CurrentKeyboardState.IsKeyDown(Keys.Down))
            {
                this.Player.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y + this.Player.Speed);
            }
        }

        private void UpdateEnemies()
        {
            // Update enemies
            if (this.Enemies.Count > 0)
            {
                for (var i = 0; i < this.Enemies.Count; i++)
                {
                    if (!this.Enemies[i].IsAlive)
                    {
                        this.Player.AddExperience(this.Enemies[i]);
                        this.Player.AddToInventory(this.LootEnemy(ItemType.Equipment));
                        this.Player.AddToInventory(this.LootEnemy(ItemType.Potion));
                        this.Enemies.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        this.Enemies[i].Update();
                    }
                }
            }
        }

        private void UpdateEnemiesState()
        {
            foreach (var enemy in this.Enemies)
            {
                if (enemy.State == State.Idle)
                {
                    if (this.Player.Intersects(enemy.AttackBounds))
                    {
                        int chanceToAttack = Rand.Next(0, 5);
                        if (chanceToAttack == 0)
                        {
                            enemy.State = State.Attacking;
                            return;
                        }
                    }
                    enemy.State = State.Moving;
                    //0 = Left, 1 = Up, 2 = Right, 3 = Down
                    int direction = Rand.Next(0, 4);
                    switch (direction)
                    {
                        case 0: enemy.Direction = Direction.Left;
                            break;
                        case 1: enemy.Direction = Direction.Up;
                            break;
                        case 2: enemy.Direction = Direction.Right;
                            break;
                        case 3: enemy.Direction = Direction.Down;
                            break;
                    }

                }

            }
        }

        private void EnemiesMove()
        {
            foreach (var enemy in this.Enemies)
            {
                if (enemy.State == State.Moving)
                {
                    switch (enemy.Direction)
                    {
                        case Direction.Left: enemy.Position = new Vector2(enemy.Position.X - enemy.Speed, enemy.Position.Y);
                            break;
                        case Direction.Up: enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y - enemy.Speed);
                            break;
                        case Direction.Right: enemy.Position = new Vector2(enemy.Position.X + enemy.Speed, enemy.Position.Y);
                            break;
                        case Direction.Down: enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + enemy.Speed);
                            break;
                    }
                }
            }
        }

        private Item LootEnemy(ItemType type)
        {
            int chance = Rand.Next(0, 5);
            if (chance == 0)
            {
                return type == ItemType.Potion ? AllPotions[Rand.Next(0, this.AllPotions.Length)] : AllEquipments[Rand.Next(0, this.AllEquipments.Length)];
            }

            return null;
        }
    }
}
