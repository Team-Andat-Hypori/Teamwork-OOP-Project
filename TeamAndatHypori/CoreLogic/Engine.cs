using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        private GameState state;
        private int wave;
        private GraphicsDeviceManager graphics;
        private Gui gui;
        private Map map;

        #region Sounds
        private SoundEffect[] Kills;

        private SoundEffect BossPrepare;
        private SoundEffect BossKill;
        private SoundEffect PickWarrior;
        private SoundEffect PickRogue;
        private SoundEffect PickWizard;
        private SoundEffect OrcHurt;
        private SoundEffect GameOver;
        #endregion

        #region Textures
        public Texture2D[] PlayerMoveRight { get; private set; }
        public Texture2D[] PlayerMoveLeft { get; private set; }
        public Texture2D[] PlayerRightAttack { get; private set; }
        public Texture2D[] PlayerLeftAttack { get; private set; }
        public Texture2D[] PlayerLeftSpecial { get; private set; }
        public Texture2D[] PlayerRightSpecial { get; private set; }
        public Texture2D[] PlayerDefeat { get; private set; }

        public Texture2D[] WarriorMoveRight { get; private set; }
        public Texture2D[] WarriorMoveLeft { get; private set; }
        public Texture2D[] WarriorRightAttack { get; private set; }
        public Texture2D[] WarriorLeftAttack { get; private set; }
        public Texture2D[] WarriorLeftSpecial { get; private set; }
        public Texture2D[] WarriorRightSpecial { get; private set; }
        public Texture2D[] WarriorDefeat { get; private set; }

        public Texture2D[] RogueMoveRight { get; private set; }
        public Texture2D[] RogueMoveLeft { get; private set; }
        public Texture2D[] RogueRightAttack { get; private set; }
        public Texture2D[] RogueLeftAttack { get; private set; }
        public Texture2D[] RogueDefeat { get; private set; }

        public Texture2D[] WizardMoveRight { get; private set; }
        public Texture2D[] WizardMoveLeft { get; private set; }
        public Texture2D[] WizardRightAttack { get; private set; }
        public Texture2D[] WizardLeftAttack { get; private set; }
        public Texture2D[] WizardLeftSpecial { get; private set; }
        public Texture2D[] WizardRightSpecial { get; private set; }
        public Texture2D[] WizardDefeat { get; private set; }

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
        public Texture2D ChampionSelect { get; private set; }
        public Texture2D Victory { get; private set; }
        public Texture2D Defeat { get; private set; }

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

        public Engine()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.state = GameState.Pick;
            Content.RootDirectory = "Resources";
        }

        public SpriteFont Font { get; set; }

        public Player Player { get; private set; }

        public List<Explosion> Explosions { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public List<Projectile> Projectiles { get; private set; }

        protected SpriteBatch SpriteBatch { get; set; }

        private KeyboardState CurrentKeyboardState { get; set; }

        protected override void Initialize()
        {
            // Sets screen size
            this.graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            this.graphics.PreferredBackBufferHeight = Config.ScreenHeight;
            this.graphics.ApplyChanges();
            this.IsMouseVisible = false;

            this.map = new Map();

            this.gui = new Gui(this);
            this.Projectiles = new List<Projectile>();
            this.Explosions = new List<Explosion>();
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
                new Orc(1000, 300),
                new Orc(1000, 400),
                new OrcArcher(1000, 100),
                new OrcArcher(1000, 500),
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

            this.PickWarrior = Content.Load<SoundEffect>(Assets.WarriorSelect);
            this.PickRogue = Content.Load<SoundEffect>(Assets.RogueSelect);
            this.PickWizard = Content.Load<SoundEffect>(Assets.WizardSelect);
            this.BossPrepare = Content.Load<SoundEffect>(Assets.BossPrepare);
            this.BossKill = Content.Load<SoundEffect>(Assets.BossKill);
            this.OrcHurt = Content.Load<SoundEffect>(Assets.OrcHurt);
            this.GameOver = Content.Load<SoundEffect>(Assets.Defeat);
            #endregion

            // Load the visual resources
            #region TexturesLoad

            //Warrior
            this.WarriorMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                    Content.Load<Texture2D>(Assets.WarriorImages[1]),
                    Content.Load<Texture2D>(Assets.WarriorImages[2]),
                };

            this.WarriorMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                    Content.Load<Texture2D>(Assets.WarriorImages[10]),
                    Content.Load<Texture2D>(Assets.WarriorImages[11]),
                };

            this.WarriorRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[3]),
                    Content.Load<Texture2D>(Assets.WarriorImages[4]),
                    Content.Load<Texture2D>(Assets.WarriorImages[5]),
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                };

            this.WarriorLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[12]),
                    Content.Load<Texture2D>(Assets.WarriorImages[13]),
                    Content.Load<Texture2D>(Assets.WarriorImages[14]),
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                };
            this.WarriorRightSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[6]),
                    Content.Load<Texture2D>(Assets.WarriorImages[7]),
                    Content.Load<Texture2D>(Assets.WarriorImages[8]),
                    Content.Load<Texture2D>(Assets.WarriorImages[0]),
                };
            this.WarriorLeftSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WarriorImages[15]),
                    Content.Load<Texture2D>(Assets.WarriorImages[16]),
                    Content.Load<Texture2D>(Assets.WarriorImages[17]),
                    Content.Load<Texture2D>(Assets.WarriorImages[9]),
                };

            this.WarriorDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.WarriorImages[18]),
                };

            //Rogue
            this.RogueMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[0]),
                    Content.Load<Texture2D>(Assets.RogueImages[1]),
                    Content.Load<Texture2D>(Assets.RogueImages[2]),
                    Content.Load<Texture2D>(Assets.RogueImages[3]),
                };

            this.RogueMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[7]),
                    Content.Load<Texture2D>(Assets.RogueImages[8]),
                    Content.Load<Texture2D>(Assets.RogueImages[9]),
                    Content.Load<Texture2D>(Assets.RogueImages[10]),
                };

            this.RogueRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[4]),
                    Content.Load<Texture2D>(Assets.RogueImages[5]),
                    Content.Load<Texture2D>(Assets.RogueImages[6]),
                    Content.Load<Texture2D>(Assets.RogueImages[0]),
                };

            this.RogueLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.RogueImages[11]),
                    Content.Load<Texture2D>(Assets.RogueImages[12]),
                    Content.Load<Texture2D>(Assets.RogueImages[13]),
                    Content.Load<Texture2D>(Assets.RogueImages[7]),
                };
            this.RogueDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.RogueImages[14]),
                };

            //Wizard
            this.WizardMoveRight = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                    Content.Load<Texture2D>(Assets.WizardImages[1]),
                    Content.Load<Texture2D>(Assets.WizardImages[2]),
                    Content.Load<Texture2D>(Assets.WizardImages[3]),
                };

            this.WizardMoveLeft = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                    Content.Load<Texture2D>(Assets.WizardImages[11]),
                    Content.Load<Texture2D>(Assets.WizardImages[12]),
                    Content.Load<Texture2D>(Assets.WizardImages[13]),
                };

            this.WizardRightAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[4]),
                    Content.Load<Texture2D>(Assets.WizardImages[5]),
                    Content.Load<Texture2D>(Assets.WizardImages[6]),
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                };

            this.WizardLeftAttack = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[14]),
                    Content.Load<Texture2D>(Assets.WizardImages[15]),
                    Content.Load<Texture2D>(Assets.WizardImages[16]),
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                };
            this.WizardRightSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[7]),
                    Content.Load<Texture2D>(Assets.WizardImages[8]),
                    Content.Load<Texture2D>(Assets.WizardImages[9]),
                    Content.Load<Texture2D>(Assets.WizardImages[0]),
                };
            this.WizardLeftSpecial = new[]
                {
                    Content.Load<Texture2D>(Assets.WizardImages[17]),
                    Content.Load<Texture2D>(Assets.WizardImages[18]),
                    Content.Load<Texture2D>(Assets.WizardImages[19]),
                    Content.Load<Texture2D>(Assets.WizardImages[10]),
                };

            this.WizardDefeat = new[]
                {
                   Content.Load<Texture2D>(Assets.WizardImages[20]),
                };


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
            this.ChampionSelect = Content.Load<Texture2D>(Assets.Maps[1]);
            this.Victory = Content.Load<Texture2D>(Assets.Maps[2]);
            this.Defeat = Content.Load<Texture2D>(Assets.Maps[3]);

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

            this.map.LoadImage(this.Map);
            foreach (var enemy in this.Enemies)
            {
                if (enemy is Orc)
                {
                    enemy.LoadImage(this.OrcMoveLeft[0]);
                }
                else if (enemy is OrcArcher)
                {
                    enemy.LoadImage(this.OrcArcherMoveLeft[0]);
                }
                if (enemy is OrcLord)
                {
                    enemy.LoadImage(this.OrcLordMoveLeft[0]);
                }
            }

            //Item texture load
            this.AllEquipments[0].LoadImage(this.Sword);
            this.AllEquipments[1].LoadImage(this.Orb);
            this.AllEquipments[2].LoadImage(this.Daggers);
            this.AllEquipments[3].LoadImage(this.Armor);
            this.AllEquipments[4].LoadImage(this.Boots);
            this.AllEquipments[5].LoadImage(this.Helm);
            this.AllEquipments[6].LoadImage(this.Gloves);

            this.AllPotions[0].LoadImage(this.HealthPotion);
            this.AllPotions[1].LoadImage(this.DamagePotion);
            this.AllPotions[2].LoadImage(this.DefensePotion);

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

            // Exit check
            if (this.CurrentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.UnloadContent();
            }

            if (this.state == GameState.Pick)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.D1))
                {
                    this.Player = new Warrior(0, 0);
                    this.PlayerLeftAttack = WarriorLeftAttack;
                    this.PlayerRightAttack = WarriorRightAttack;
                    this.PlayerMoveLeft = WarriorMoveLeft;
                    this.PlayerMoveRight = WarriorMoveRight;
                    this.PlayerRightSpecial = WarriorRightSpecial;
                    this.PlayerLeftSpecial = WarriorLeftSpecial;
                    this.PlayerDefeat = WarriorDefeat;
                    this.Player.LoadImage(this.PlayerMoveRight[0]);
                    this.PickWarrior.Play();
                    this.state = GameState.Play;
                    this.Player.OnDeath += (sender, args) =>
                    {
                        this.GameOver.Play();
                        this.state = GameState.Defeat;
                    };
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.D2))
                {
                    this.Player = new Rogue(0, 0);
                    this.PlayerLeftAttack = RogueLeftAttack;
                    this.PlayerRightAttack = RogueRightAttack;
                    this.PlayerMoveLeft = RogueMoveLeft;
                    this.PlayerMoveRight = RogueMoveRight;
                    this.PlayerDefeat = RogueDefeat;
                    this.Player.LoadImage(this.PlayerMoveRight[0]);
                    this.PickRogue.Play();
                    this.state = GameState.Play;
                    this.Player.OnDeath += (sender, args) =>
                    {
                        this.GameOver.Play();
                        this.state = GameState.Defeat;
                    };
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.D3))
                {
                    this.Player = new Wizard(0, 0);
                    this.PlayerLeftAttack = WizardLeftAttack;
                    this.PlayerRightAttack = WizardRightAttack;
                    this.PlayerMoveLeft = WizardMoveLeft;
                    this.PlayerMoveRight = WizardMoveRight;
                    this.PlayerRightSpecial = WizardRightSpecial;
                    this.PlayerLeftSpecial = WizardLeftSpecial;
                    this.PlayerDefeat = WizardDefeat;
                    this.Player.LoadImage(this.PlayerMoveRight[0]);
                    this.PickWizard.Play();
                    this.state = GameState.Play;
                    this.Player.OnDeath += (sender, args) =>
                    {
                        this.GameOver.Play();
                        this.state = GameState.Defeat;
                    };
                }

            }
            else if (this.state == GameState.Play)
            {

                this.UpdatePlayerState();
                this.UpdatePlayerDirection();
                this.Move();
                this.PlayerAttack();
                this.Player.Update();

                this.UpdateEnemiesState();
                this.EnemiesMove();
                this.EnemiesAttack();
                this.UpdateEnemies();

                this.FlyProjectiles();
                this.CheckForProjectileHits();

                this.UpdateExplosions();

                if (this.Enemies.Count == 0)
                {
                    if (this.wave == 0)
                    {
                        this.Enemies = new List<Enemy>()
                        {
                            new Orc(1000, 300),
                            new Orc(1000, 350),
                            new Orc(1000, 400),
                            new OrcArcher(1000, 100),
                            new OrcArcher(1000, 100),
                            new OrcArcher(1000, 500),
                        };
                    }
                    else if(this.wave == 1)
                    {
                        this.BossPrepare.Play();
                        this.Enemies = new List<Enemy>()
                        {
                            new Orc(1000, 300),
                            new Orc(1000, 350),
                            new Orc(1000, 400),
                            new OrcArcher(1000, 100),
                            new OrcArcher(1000, 100),
                            new OrcArcher(1000, 500),
                            new OrcLord(1000,300)
                        };
                    }
                    else if(this.wave == 2)
                    {
                        this.BossKill.Play();
                        this.state = GameState.Win;
                    }

                    wave++;
                    foreach (var enemy in this.Enemies)
                    {
                        if (enemy is Orc)
                        {
                            enemy.LoadImage(this.OrcMoveLeft[0]);
                        }
                        else if (enemy is OrcArcher)
                        {
                            enemy.LoadImage(this.OrcArcherMoveLeft[0]);
                        }
                        if (enemy is OrcLord)
                        {
                            enemy.LoadImage(this.OrcLordMoveLeft[0]);
                        }
                    }
                }

                // Use Item
                for (int index = 0; index < Config.UseItemKeys.Length; index++)
                {
                    if (this.CurrentKeyboardState.IsKeyDown(Config.UseItemKeys[index]))
                    {
                        this.Player.UseItem(index);
                    }
                }

                // Discard item 
                for (int index = 0; index < Config.DiscardItemKeys.Length; index++)
                {
                    if (this.CurrentKeyboardState.IsKeyDown(Config.DiscardItemKeys[index]))
                    {
                        this.Player.DiscardItem(index);
                    }
                }

                // Unequip Item 
                for (int index = 0; index < Config.UnequipItemKeys.Length; index++)
                {
                    if (this.CurrentKeyboardState.IsKeyDown(Config.UnequipItemKeys[index]))
                    {
                        this.Player.UnequipItem((EquipmentSlot)index);
                    }
                }
                if (this.Player.State != State.Idle)
                {
                    this.AnimatePlayer();
                }
                this.AnimateEnemies();
                this.AnimateExplosions();

                this.DeleteDeadObjects();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (this.state == GameState.Pick)
            {
                this.GraphicsDevice.Clear(Color.Black);

                this.SpriteBatch.Begin();

                this.SpriteBatch.Draw(this.ChampionSelect, new Vector2(0, 0), Color.White);

                this.SpriteBatch.End();
            }
            else if (this.state == GameState.Defeat)
            {
                this.GraphicsDevice.Clear(Color.Black);

                this.SpriteBatch.Begin();

                this.SpriteBatch.Draw(this.Defeat, new Vector2(0, 0), Color.White);

                this.Player.Image = this.PlayerDefeat[0];

                this.SpriteBatch.Draw(this.Player.Image, new Vector2(500, 300), Color.White);

                this.SpriteBatch.End();
            }
            else if (this.state == GameState.Win)
            {
                this.GraphicsDevice.Clear(Color.Black);

                this.SpriteBatch.Begin();

                this.SpriteBatch.Draw(this.Victory, new Vector2(0, 0), Color.White);

                this.SpriteBatch.Draw(this.Player.Image, this.Player.Position, Color.White);

                this.SpriteBatch.End();
            }
            else
            {
                this.GraphicsDevice.Clear(Color.Black);

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
                foreach (var explosion in this.Explosions)
                {
                    this.SpriteBatch.Draw(explosion.Image, explosion.Position, Color.White);
                }
                this.gui.Draw(this.SpriteBatch);

                this.SpriteBatch.End();
            }

        }

        private void DeleteDeadObjects()
        {
            for (int i = 0; i < this.Projectiles.Count; i++)
            {
                if (Projectiles[i].IsAlive == false)
                {
                    Projectiles.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < this.Explosions.Count(); i++)
            {
                if (Explosions[i].IsAlive == false)
                {
                    Explosions.RemoveAt(i);
                    i--;
                }
            }
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
            }
        }

        private void LoadProjectileImage(Projectile projectile)
        {
            if (projectile.Image == null)
            {
                if (projectile is Dagger)
                {
                    projectile.LoadImage(projectile.Direction == Direction.Left ? this.DaggerLeft : this.DaggerRight);
                }
                else if (projectile is Arrow)
                {
                    projectile.LoadImage(projectile.Direction == Direction.Left ? this.ArrowLeft : this.ArrowRight);
                }
                else if (projectile is Firebolt)
                {
                    projectile.LoadImage(projectile.Direction == Direction.Left ? this.FireboltLeft : this.FireboltRight);
                }
                else if (projectile is Fireball)
                {
                    projectile.LoadImage(projectile.Direction == Direction.Left ? this.FireballLeft : this.FireballRight);
                }
            }
        }

        private void FlyProjectiles()
        {
            foreach (var projectile in this.Projectiles)
            {
                if (projectile.Direction == Direction.Left)
                {
                    projectile.Position = new Vector2(projectile.Position.X - projectile.Speed, projectile.Position.Y);
                    projectile.Bounds = new BoundingBox(new Vector3(projectile.Position.X, projectile.Position.Y, 0), new Vector3(projectile.Position.X + projectile.Width, projectile.Position.Y + projectile.Height, 0));
                }
                else if (projectile.Direction == Direction.Right)
                {
                    projectile.Position = new Vector2(projectile.Position.X + projectile.Speed, projectile.Position.Y);
                    projectile.Bounds = new BoundingBox(new Vector3(projectile.Position.X, projectile.Position.Y, 0), new Vector3(projectile.Position.X + projectile.Width, projectile.Position.Y + projectile.Height, 0));
                }
            }
        }

        private void CheckForProjectileHits()
        {
            foreach (var projectile in this.Projectiles)
            {
                if (this.Player.Intersects(projectile.Bounds))
                {
                    if (projectile is Arrow)
                    {
                        this.Player.RespondToAttack(projectile.Damage);
                        projectile.IsAlive = false;
                        continue;
                    }
                }
                foreach (var enemy in this.Enemies)
                {
                    if (enemy.Intersects(projectile.Bounds))
                    {
                        if (projectile is Fireball)
                        {
                            Explosion explosion;
                            if (projectile.Direction == Direction.Right)
                            {
                                explosion = new Explosion((int)projectile.Position.X + projectile.Width, (int)projectile.Position.Y - projectile.Height, projectile.Damage);
                            }
                            else
                            {
                                explosion = new Explosion((int)projectile.Position.X - projectile.Width , (int)projectile.Position.Y - projectile.Height, projectile.Damage);
                            }
                            
                            projectile.IsAlive = false;
                            this.Explosions.Add(explosion);
                            this.LoadExplosionImage(explosion);
                        }
                        else if (enemy.Intersects(projectile.Bounds) && projectile is Arrow == false)
                        {
                            enemy.RespondToAttack(projectile.Damage);
                            this.OrcHurt.Play();
                            projectile.IsAlive = false;
                        }
                    }
                }
            }
        }

        private void LoadExplosionImage(Explosion explosion)
        {
            if (explosion.Image == null)
            {
                explosion.LoadImage(this.Explosion[0]);
            }
        }

        private void UpdateExplosions()
        {
            foreach (var explosion in this.Explosions)
            {
                if (explosion.AnimationFrame == 1 && explosion.AnimationDelay == 0)
                {
                    foreach (var enemy in this.Enemies)
                    {
                        if (enemy.Intersects(explosion.Bounds))
                        {
                            enemy.RespondToAttack(explosion.Damage);
                            this.OrcHurt.Play();
                        }
                    }
                }
            }
        }

        private void PlayerAttack()
        {
            if (this.Player.State == State.Special && this.Player.AnimationFrame == 2 && this.Player.AnimationDelay == 0)
            {
                if (this.Player is Warrior)
                {
                    var enemiesInRange = this.Player.GetEnemiesInRange(this.Enemies);
                    if (enemiesInRange.Count > 0)
                    {
                        (this.Player as Warrior).SpecialAttack(enemiesInRange);
                        foreach (var enemy in enemiesInRange)
                        {
                            this.OrcHurt.Play();
                        }
                    }
                }
                else if (this.Player is Wizard)
                {
                    Projectile projectile = (this.Player as Wizard).SpecialAttack();
                    this.Projectiles.Add(projectile);
                    this.LoadProjectileImage(projectile);
                }
            }
            else if (this.Player.State == State.Attacking && this.Player.AnimationFrame == 2 && this.Player.AnimationDelay == 0)
            {
                if (this.Player is Warrior)
                {
                    var enemy = this.Player.GetEnemiesInRange(this.Enemies).FirstOrDefault();
                    if (enemy != null)
                    {
                        this.Player.Attack(enemy);
                        this.OrcHurt.Play();
                    }
                }
                else if (this.Player is Rogue)
                {
                    Projectile projectile = (this.Player as Rogue).ProduceProjectile();
                    this.Projectiles.Add(projectile);
                    this.LoadProjectileImage(projectile);
                }
                else if (this.Player is Wizard)
                {
                    Projectile projectile = (this.Player as Wizard).ProduceProjectile();
                    this.Projectiles.Add(projectile);
                    this.LoadProjectileImage(projectile);
                }
            }
        }

        private void EnemiesAttack()
        {
            foreach (var enemy in this.Enemies)
            {
                if (enemy.State == State.Attacking && enemy.AnimationFrame == 3 && enemy.AnimationDelay == 0)
                {
                    if (enemy is OrcArcher)
                    {
                        Projectile projectile = (enemy as OrcArcher).ProduceProjectile();
                        this.Projectiles.Add(projectile);
                        this.LoadProjectileImage(projectile);
                    }
                    else
                    {
                        enemy.Attack(this.Player);
                    }
                }
            }
        }

        private void AnimateExplosions()
        {
            foreach (var explosion in this.Explosions)
            {
                if (explosion.AnimationDelay == 0)
                {
                    explosion.Image = this.Explosion[explosion.AnimationFrame++];
                    if (explosion.AnimationFrame == this.Explosion.Length)
                    {
                        explosion.IsAlive = false;
                    }
                    explosion.AnimationDelay = 10;
                }
                explosion.AnimationDelay--;
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
                                    enemy.Image = this.OrcMoveRight[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcMoveRight.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcMoveLeft[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcMoveLeft.Length)
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
                                        enemy.IsAlive = false;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                        enemy.IsAlive = false;
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
                                    enemy.Image = this.OrcArcherMoveRight[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherMoveRight.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcArcherMoveLeft[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherMoveLeft.Length)
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
                                        enemy.IsAlive = false;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcArcherLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcArcherLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                        enemy.IsAlive = false;
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
                                    enemy.Image = this.OrcLordMoveRight[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordMoveRight.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLordMoveLeft[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordMoveLeft.Length)
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
                                        enemy.IsAlive = false;
                                    }
                                    break;
                                case Direction.Up:
                                case Direction.Left:
                                    enemy.Image = this.OrcLordLeftDead[enemy.AnimationFrame++];
                                    if (enemy.AnimationFrame == this.OrcLordLeftDead.Length)
                                    {
                                        enemy.State = State.Idle;
                                        enemy.AnimationFrame = 0;
                                        enemy.IsAlive = false;
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
                    this.Player.AnimationDelay = 30;
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
                    this.Player.AnimationDelay = 10;
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
                    this.Player.AnimationDelay = 10;
                }
            }

            this.Player.AnimationDelay--;
        }

        private void UpdatePlayerState()
        {
            if (this.Player.State == State.Idle || this.Player.State == State.Moving)
            {
                if (this.CurrentKeyboardState.IsKeyDown(Keys.Z) && this.Player is Rogue == false)
                {
                    this.Player.State = State.Special;
                    this.Player.AnimationFrame = 0;
                    this.Player.AnimationDelay = 30;
                }
                else if (this.CurrentKeyboardState.IsKeyDown(Keys.Space))
                {
                    this.Player.State = State.Attacking;
                    this.Player.AnimationFrame = 0;
                }
                else if (this.CurrentKeyboardState.IsKeyDown(Keys.Up) ||
                    this.CurrentKeyboardState.IsKeyDown(Keys.Down) ||
                    this.CurrentKeyboardState.IsKeyDown(Keys.Right) ||
                    this.CurrentKeyboardState.IsKeyDown(Keys.Left))
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
            // Update Direction
            if (this.CurrentKeyboardState.IsKeyDown(Keys.Right))
            {
                this.Player.Direction = Direction.Right;
            }
            else if (this.CurrentKeyboardState.IsKeyDown(Keys.Left))
            {
                this.Player.Direction = Direction.Left;
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
            foreach (var enemy in this.Enemies)
            {
                enemy.Update();
            }
        }

        private void UpdateEnemiesState()
        {
            foreach (var enemy in this.Enemies)
            {

                if (enemy.State == State.Idle)
                {
                    if (enemy.Health <= 0)
                    {
                        enemy.State = State.Dying;
                        enemy.AnimationFrame = 0;
                        this.Kills[Rand.Next(0, this.Kills.Length)].Play();
                        continue;
                    }
                    if (this.Player.Intersects(enemy.AttackBounds))
                    {
                        int chanceToAttack = Rand.Next(0, 7);
                        if (chanceToAttack == 0)
                        {
                            enemy.Direction = this.Player.Position.X <= enemy.Position.X ? Direction.Left : Direction.Right;
                            enemy.State = State.Attacking;
                            continue;
                        }
                    }
                    int chance = Rand.Next(0, 100);
                    if (chance < 70)
                    {
                        enemy.State = State.Idle;
                    }
                    else
                    {
                        enemy.State = State.Moving;
                        // 0 = Left, 1 = Up, 2 = Right, 3 = Down
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
            int chance = Rand.Next(0, 1);
            if (chance == 0)
            {
                this.AllPotions[0] = new HealingPotion();
                this.AllPotions[1] = new DamagePotion();
                this.AllPotions[2] = new DefensePotion();

                this.AllPotions[0].LoadImage(this.HealthPotion);
                this.AllPotions[1].LoadImage(this.DamagePotion);
                this.AllPotions[2].LoadImage(this.DefensePotion);

                return type == ItemType.Potion ? AllPotions[Rand.Next(0, this.AllPotions.Length)] : AllEquipments[Rand.Next(0, this.AllEquipments.Length)];
            }
            

            return null;
        }
    }
}
