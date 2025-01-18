﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using SnakeX.Models;

namespace SnakeX;

public class GameManager : Game
{
    private float _score;

    private Player player;

    private GamePlayState _gameState = GamePlayState.Play; 
    private int _nextPowerUP = 1000;
    private SpriteFont _font;
    private byte _shipDamage = 8;
    private string _shipDamageString = "DDDDD";
    private SpriteFont _damageFont;
    private Texture2D _starShipTexture;
    private Texture2D _bulletTexture;
    private Texture2D collisionTexture;
    public static readonly List<Bullet> _bullets = [];
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Vector2 _starShipPosition;
    float _starShipSpeed;
    // Configurações
    private const float _bulletSpeed = 600f; // Velocidade do projétil (pixels por segundo)
    private const float _bulletRate = 60; // _bulletRate / 60 tiros Tiros por segundo 
    private ushort _reloadSpeed = 14; // _bulletRate / 60 tiros Tiros por segundo 
    private ushort _bulletStatus = 60;
    float _starShipRotate = 0f;
    Rectangle _stickPos = new Rectangle(0, 0, 32, 32);
    short _starShipdirection = 0;  // 1 - direita, -1-esquerda
    private const float ShipRadius = 40f;
    private Vector2 shipSize = new Vector2(50, 50);
    private Vector2 bulletSize = new Vector2(20, 20);
    private bool BeastModeOne = true;

    //Asteroids
    private List<Asteroid> _asteroids = new List<Asteroid>();
    private Texture2D _asteroidTexture;
    private float AsteroidSpawnRate = 3.5f; // Asteroides por segundo
    private float _timeSinceLastAsteroid = 0f;
    private Random _random = new Random();
    private const float ShipCollisionWidth = 30f; // Largura do retângulo de colisão
    private const float ShipCollisionHeight = 50f; // Altura do retângulo de colisão

    // Enemies
    private List<Enemy> _enemies = new List<Enemy>();



    private bool RenderHitBoxes = false;

    /// FX Sounds
    /// 
    private bool IntroSound = true;
    private SoundEffect _startSound;
    private SoundEffect _explosionSound;
    private SoundEffect _bulleSound;
    private SoundEffectInstance _bulleSoundInstance;
    private SoundEffect _colisionSound;
    private SoundEffect _powerupSound;
    private Song _playerSound;
    private Song _gameoverSound;

    // Explosions
    private List<Explosion> _explosions = new List<Explosion>();

    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        Globals.WindowSize = new(1780, 980);
        _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
        _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
        _graphics.ApplyChanges();

        Globals.Content = Content;

        _starShipPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                   _graphics.PreferredBackBufferHeight / 2);
        _starShipSpeed = 500f;

        _gameState = GamePlayState.Play;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.SpriteBatch = _spriteBatch;
        Globals.GraphicsDevice = GraphicsDevice;

        // Display:

        _font = Content.Load<SpriteFont>("Fonts/File");
        _damageFont = Content.Load<SpriteFont>("Fonts/Damage_font");

        // TODO: use this.Content to load your game content here
        _starShipTexture = Content.Load<Texture2D>("Player/spritesheet_player");
        _bulletTexture = Content.Load<Texture2D>("bullet");
        _startSound = Content.Load<SoundEffect>("intro_sound-1");
        _explosionSound = Content.Load<SoundEffect>("explosion_sound");
        _bulleSound = Content.Load<SoundEffect>("bullet_sound");
        _colisionSound = Content.Load<SoundEffect>("ship_colision_sound");
        _powerupSound = Content.Load<SoundEffect>("Sounds/level_up");
        _playerSound = Content.Load<Song>("Sounds/fase1_sound");
        _gameoverSound = Content.Load<Song>("Sounds/gameover_sound");

        MediaPlayer.IsRepeating = true; // Configurar replay automático
        MediaPlayer.Volume = 1.5f; // Configurar o volume (opcional)
        MediaPlayer.Play(_playerSound); // Reproduzir a música

        // _asteroidTexture = new Texture2D(GraphicsDevice, 20, 20);
        // Color[] asteroidData = new Color[400];
        // for (int i = 0; i < asteroidData.Length; i++) asteroidData[i] = Color.Gray;
        // _asteroidTexture.SetData(asteroidData);

        _asteroidTexture = Content.Load<Texture2D>("asteroid");

        // Criar uma textura para os projéteis
        // _bulletTexture = new Texture2D(GraphicsDevice, 5, 5);
        // Color[] data = new Color[25];
        // for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
        // _bulletTexture.SetData(data);

        //  _radiusTexture = new Texture2D(GraphicsDevice, 10, 10);
        // Color[] data2 = new Color[100];
        // for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Black;
        // _radiusTexture.SetData(data2);

        // Debug

        collisionTexture = new Texture2D(GraphicsDevice, 1, 1);
        collisionTexture.SetData(new[] { Color.Red });

    }

    protected override void Update(GameTime gameTime)
    {


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // Asteroids Logic

        // if(IntroSound){
        //     _startSound.Play();
        //     IntroSound = false;
        // }

        if(player == null){
            player = new Player(new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                   _graphics.PreferredBackBufferHeight / 2), Vector2.Zero,10f,_starShipTexture,3,3);
        }



        if(_gameState == GamePlayState.GameOver){
        
            var kstateGO = Keyboard.GetState();
            
            if(MediaPlayer.State == MediaState.Stopped) {
            MediaPlayer.Volume = 1.8f;
            MediaPlayer.Play(_gameoverSound);

            }
            

            if (kstateGO.IsKeyDown(Keys.F1))
            {
               RestartGame();
                return;
            }
            
            return;
        };

         if(_score/_nextPowerUP % 1 == 0 && _score > 0) {
            _shipDamage += 1;
            _nextPowerUP += _nextPowerUP;
            _powerupSound.Play();

            UpdateDamage();
         }
        //  if(_reloadSpeed < 7 && _score > 25000) _reloadSpeed = 7;
        //  if(_reloadSpeed < 14 && _score > 50000) _reloadSpeed = 14;



        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timeSinceLastAsteroid += (float)gameTime.ElapsedGameTime.TotalSeconds;
    

        if (_timeSinceLastAsteroid >= 1f / AsteroidSpawnRate)
        {
            SpawnAsteroid();
            SpawnEnemies();
            _timeSinceLastAsteroid = 0f;
        }

        // Inimigos Colisão
        for (int ii = _enemies.Count - 1; ii >= 0; ii--)
        {
            var enemy = _enemies[ii];
            enemy.Update(deltaTime);

            Rectangle shipCollisionRectangle = GetShipCollisionRectangle();

    
            // Verificar colisão com os projéteis
            for (int j = _bullets.Count - 1; j >= 0; j--)
            {
                // Calcular o retângulo de colisão do projetil

                 Rectangle projetilRectangle = new Rectangle(
                    (int)_bullets[j].Position.X,
                    (int)_bullets[j].Position.Y,
                    (int)(2 * 2),
                    (int)(2 * 2)
                );


                // Verificar se o projétil está colidindo com o asteroide
                if (enemy.EnemyRectangle.Contains(projetilRectangle))
                {
                    Rectangle rec = Rectangle.Intersect(projetilRectangle, enemy.EnemyRectangle);
                    _explosions.Add(new Explosion(enemy.EnemyRectangle.Location.ToVector2()));
                    _bullets.RemoveAt(j);
                    _enemies.RemoveAt(ii);
                    _explosionSound.Play();
                    _score += 50;

                    // Remover projétil e asteroide
                    // _bullets.RemoveAt(j);
                    // _asteroids.RemoveAt(i);
                    break; // Evitar erros de índice após remover o asteroide
                }
            }

            if (ii < _enemies.Count && shipCollisionRectangle.Intersects(enemy.EnemyRectangle))
            {
                _colisionSound.Play();
                _enemies.RemoveAt(ii);
                _explosions.Add(new Explosion(enemy.EnemyRectangle.Location.ToVector2()));
                // _explosionSound.Play();
                // Apply Ship damage

                _shipDamage -= 1;
                // Logic to shipDamage:

                if (_shipDamage < 1)
                {
                    _gameState = GamePlayState.GameOver;
                    MediaPlayer.Stop();// Encerrar o jogo ou tratar a colisão
                }

                UpdateDamage();

                // GraphicsDevice.Clear(Color.Black);
                // _score = 0;
                // _reloadSpeed = 1;
            }

        }

        // Atualizar asteroids
        for (int i = _asteroids.Count - 1; i >= 0; i--)
        {
            _asteroids[i].Update(deltaTime);

            // Remover asteroides fora da tela
            if (_asteroids[i].Position.X < -20 || _asteroids[i].Position.X > Globals.WindowSize.X + 20 ||
                _asteroids[i].Position.Y < -20 || _asteroids[i].Position.Y > Globals.WindowSize.Y + 20)
            {
                _asteroids.RemoveAt(i);
                continue;
            }


            // Verificar colisão com a nave usando o retângulo (já implementado antes)
            Rectangle shipCollisionRectangle = GetShipCollisionRectangle();
            // Rectangle asteroidRectangleShip = new Rectangle(
            //     (int)(_asteroids[i].Position.X - _asteroids[i].Radius),
            //     (int)(_asteroids[i].Position.Y - _asteroids[i].Radius),
            //     (int)(_asteroids[i].Radius * 4),
            //     (int)(_asteroids[i].Radius * 4)
            // );

            //  Rectangle asteroidRectangle = new Rectangle(
            //         (int)(_asteroids[i].Position.X - _asteroids[i].Radius),
            //         (int)(_asteroids[i].Position.Y - _asteroids[i].Radius),
            //         (int)(_asteroids[i].Radius * 2),
            //         (int)(_asteroids[i].Radius * 2)
            //     );

            // Atualiza Inimigos



            //  enemy in _enemies)
            // {
            //     enemy.Update(deltaTime);

            //     // Remover projéteis que saem da tela
            //     if (enemy.Position.X < 0 || enemy.Position.X > _graphics.PreferredBackBufferWidth ||
            //         enemy.Position.Y < 0 || enemy.Position.Y > _graphics.PreferredBackBufferHeight)
            //     {
            //         _enemies.RemoveAt(i);
            //     }
            // }



            // Verificar colisão com os projéteis
            for (int j = _bullets.Count - 1; j >= 0; j--)
            {
                // Calcular o retângulo de colisão do projetil

                 Rectangle projetilRectangle = new Rectangle(
                    (int)_bullets[j].Position.X,
                    (int)_bullets[j].Position.Y,
                    (int)(2 * 2),
                    (int)(2 * 2)
                );


                // Verificar se o projétil está colidindo com o asteroide
                if (_asteroids[i].AsteroidRectangle.Contains(projetilRectangle))
                {
                    Rectangle rec = Rectangle.Intersect(projetilRectangle, _asteroids[i].AsteroidRectangle);
                    _explosions.Add(new Explosion(_asteroids[i].AsteroidRectangle.Location.ToVector2()));
                    _bullets.RemoveAt(j);
                    _asteroids.RemoveAt(i);
                    _explosionSound.Play();
                    _score += 10;

                    // Remover projétil e asteroide
                    // _bullets.RemoveAt(j);
                    // _asteroids.RemoveAt(i);
                    break; // Evitar erros de índice após remover o asteroide
                }
            }

            if (i < _asteroids.Count && shipCollisionRectangle.Intersects(_asteroids[i].AsteroidRectangle))
            {
                _colisionSound.Play();
                _asteroids.RemoveAt(i);
                // Apply Ship damage

                _shipDamage -= 1;
                // Logic to shipDamage:

                if (_shipDamage < 1)
                {
                    _gameState = GamePlayState.GameOver;
                    MediaPlayer.Stop();// Encerrar o jogo ou tratar a colisão
                }

                UpdateDamage();

                // GraphicsDevice.Clear(Color.Black);
                // _score = 0;
                // _reloadSpeed = 1;
            }
        }




        // TODO: Add your update logic here

        float updatedStarShipSpeed = _starShipSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Rotate the starShip forever

        float radians = _starShipRotate;

        if (radians >= 6.28319f)
            radians = 0;

        if (radians <= -6.28319f)
            radians = 0;

        // Lógica de inversão de rotação

        // radians += 0.0174533f * 4 * _starShipdirection;

        // Atualizar projéteis
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(deltaTime);

            // Remover projéteis que saem da tela
            if (_bullets[i].Position.X < 0 || _bullets[i].Position.X > _graphics.PreferredBackBufferWidth ||
                _bullets[i].Position.Y < 0 || _bullets[i].Position.Y > _graphics.PreferredBackBufferHeight)
            {
                _bullets.RemoveAt(i);
            }
        }

        for (int i = 0; i < _explosions.Count; i++)
        {
            _explosions[i].Update(deltaTime);
            if (_explosions[i].IsExpired){
                _explosions[i].Stop();
                _explosions.RemoveAt(i);

            }
            
        }
        

        _starShipRotate = radians;

        _starShipdirection = 0;

        var kstate = Keyboard.GetState();
        // var msstate = Mouse.GetState();

        if (kstate.IsKeyDown(Keys.C))
        {
            RenderHitBoxes = true;
        }

        if (kstate.IsKeyDown(Keys.V))
        {
            RenderHitBoxes = false;
        }

        if (kstate.IsKeyDown(Keys.W))
        {
            _starShipPosition.Y -= updatedStarShipSpeed;
            player.Position = _starShipPosition;
            _starShipdirection = 0;
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            _starShipPosition.Y += updatedStarShipSpeed;
            player.Position = _starShipPosition;
            _starShipdirection = 0;
        }

        if (kstate.IsKeyDown(Keys.A))
        {
            _starShipPosition.X -= updatedStarShipSpeed;
            player.Position = _starShipPosition;
            _starShipdirection = -1;
            // radians += 0.0174533f * 4 * _starShipdirection;
            // _starShipRotate = radians;
        }
         if (kstate.IsKeyDown(Keys.R))
        {
            
        }

        if (kstate.IsKeyDown(Keys.D))
        {
            _starShipPosition.X += updatedStarShipSpeed;
            player.Position = _starShipPosition;
            _starShipdirection = 1;
            // radians += 0.0174533f * 4 * _starShipdirection;
            // _starShipRotate = radians;
        }


        if (kstate.IsKeyDown(Keys.R))
        {
            // Beast Mode
            BeastModeOne = true;
            // _reloadSpeed = 14;
        }

        if (kstate.IsKeyDown(Keys.Space))
        {
            // Controla a cadêncvia de tiro
            Vector2 firePosition = CalculateShipTip(radians);
            FireProjectile(firePosition, radians, _bulletSpeed);
            // if((int)gameTime.TotalGameTime.TotalMilliseconds % _bulletRate == 0){
            //}
        }

        if (kstate.IsKeyDown(Keys.F2))
            {
               RestartGame();
                return;
            }

        player.Update(deltaTime,_starShipdirection);

        // Recupera cadencia de tiro:
        RecuperaCadencia();

        Globals.Update(gameTime);

        base.Update(gameTime);
    }

    private void UpdateDamage()
    {
        _shipDamageString = "";

        for (int ii = 0; ii < _shipDamage; ii++)
        {
            _shipDamageString += "D";
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        
        if(_gameState == GamePlayState.Play){
        
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();

        // Exibir o score no canto superior esquerdo
        _spriteBatch.DrawString(_font, $"{_score}", new Vector2(10, 10), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);
        _spriteBatch.DrawString(_font, $"{"DAMAGE:"}", new Vector2(10, 40), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);
        _spriteBatch.DrawString(_damageFont, $"{_shipDamageString}", new Vector2(110, 42), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);

        // _spriteBatch.Draw(_starShipTexture,
        // _starShipPosition,
        // null,
        //     Color.White,
        //     _starShipRotate,
        //     new Vector2(_starShipTexture.Width / 2, _starShipTexture.Height / 2),
        //     shipSize / new Vector2(_starShipTexture.Width, _starShipTexture.Height),
        //     SpriteEffects.None,
        //     0f);

        player.Draw(_spriteBatch);


        //  _spriteBatch.Draw(_radiusTexture,
        // CalculateShipTip(_starShipRotate),
        // null,
        //     Color.White,
        //     0f,
        //     Vector2.Zero,
        //     Vector2.One,
        //     SpriteEffects.None,
        //     0f);

        // _spriteBatch.Draw(_bulletTexture,new Vector2(_starShipPosition.X, _starShipPosition.Y + _starShipTexture.Height),
        // null,Color.Transparent,_starShipRotate,
        // new Vector2(_starShipTexture.Width / 2, _starShipTexture.Height / 2),Vector2.Divide(Vector2.One,10), SpriteEffects.None,1f);


        ConstrainStarShipWithinBounds();

        foreach (var bullet in _bullets)
        {
            var _rad = bullet.radian - 1.5708f;
            // _spriteBatch.Draw(_bulletTexture, bullet.Position, Color.White);

            _spriteBatch.Draw(
            _bulletTexture,
            bullet.Position,
            null, Color.White,
            _rad,
            new Vector2(_bulletTexture.Width / 2, _bulletTexture.Height / 2),
            bulletSize / new Vector2(_bulletTexture.Width, _bulletTexture.Height),
            SpriteEffects.None,
            0f);
        }

        foreach (var asteroid in _asteroids)
        {
            _spriteBatch.Draw(
                _asteroidTexture,
                asteroid.Position,
                null,
            Color.White,
            asteroid.Radians,
            new Vector2(_asteroidTexture.Width / 2, _asteroidTexture.Height / 2),
            Vector2.Divide(Vector2.One, 15),
            SpriteEffects.None,
            0f);
        }

         foreach (var enemy in _enemies)
        {
            enemy.Draw(_spriteBatch);
        }

        foreach (var explosion in _explosions)
        {
            explosion.Draw(_spriteBatch);
        }



        // Pressione ctrl + c
        
        if(RenderHitBoxes){

            Rectangle shipCollisionRectangle = GetShipCollisionRectangle();

            Texture2D collisionTexture = new Texture2D(GraphicsDevice, 1, 1);
            collisionTexture.SetData(new[] { Color.Red });  

                _spriteBatch.Draw(
                    collisionTexture,
                    shipCollisionRectangle,
                    Color.Red * 0.5f // Cor semitransparente
                );

            // desenhando colisao asteroide e projeteis

            _asteroids.ForEach(x =>{
                _spriteBatch.Draw(
                    collisionTexture,
                    new Rectangle(
                        (int)(x.Position.X - x.Radius),
                        (int)(x.Position.Y - x.Radius),
                        (int)(x.Radius * 2),
                        (int)(x.Radius * 2)
                    ),
                    Color.Red * 0.5f // Cor semitransparente
                    );}
                    );

            _bullets.ForEach(x =>{
                _spriteBatch.Draw(
                    collisionTexture,
                    new Rectangle(
                        (int)(x.Position.X - 10),
                        (int)(x.Position.Y -10),
                        15,15
                    ),
                    Color.Red * 0.5f // Cor semitransparente
                    );}
                    );

             _enemies.ForEach(x =>{
                _spriteBatch.Draw(
                    collisionTexture,
                    x.EnemyRectangle,
                    Color.Red * 0.5f // Cor semitransparente
                    );}
                    );

        }
        
        
        

        _spriteBatch.End();

        }

        if(_gameState == GamePlayState.GameOver){


             GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            // Exibir o score no canto superior esquerdo
            _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(Globals.WindowSize.X / 2 - 80f,Globals.WindowSize.Y / 2 -40f), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);
            _spriteBatch.DrawString(_font, "THANK'S FOR PLAYING", new Vector2(Globals.WindowSize.X / 2 - 155f,Globals.WindowSize.Y / 2 +20f), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);
            _spriteBatch.DrawString(_font, $"{_score}", new Vector2(10, 10), Color.SeaGreen,0f,Vector2.Zero,Vector2.One,SpriteEffects.None,0);


             _spriteBatch.End();


            
        }



        base.Draw(gameTime);
    }

    private void FireProjectile(Vector2 startPosition, float angle, float speed)
    {

        Vector2 leftWing = shipSize / new Vector2(- _starShipTexture.Width / 2, 0); // Asa esquerda

        Vector2 rotatedLeftWing = RotatePoint(leftWing, angle);

        Vector2 center = new Vector2(startPosition.X + 10f, startPosition.Y + 25f);

        var rotated = center + rotatedLeftWing;

        // Verifica possibildiade de tiro:
        if (_bulletStatus < _bulletRate) return;
        // Calcular a velocidade com base no ângulo
        // Ajustar o ângulo para alinhar com a ponta da nave
        float adjustedAngle = angle - MathF.PI / 2;

        // Atualiza tempo para cadência:

        _bulletStatus = 0;

        // Calcular a velocidade com base no ângulo ajustado
        Vector2 velocity = new Vector2(
            speed * MathF.Cos(adjustedAngle),
            speed * MathF.Sin(adjustedAngle)
        );


        // Criar um novo projétil e adicioná-lo à lista
        _bullets.Add(new Bullet(startPosition, velocity, _starShipRotate));

        var windPosLeft = CalculateShipTWindLeft(_starShipRotate);
        var windPosRight = CalculateShipTWindRight(_starShipRotate);

        if(BeastModeOne){
            //adjustedAngle = angle - 0.7854f - MathF.PI / 2;

            // Vector2 point = RotatePointForRelative(startPosition + new Vector2(50f,25f),startPosition,_starShipRotate);

            _bullets.Add(new Bullet(windPosLeft, velocity, _starShipRotate));
            _bullets.Add(new Bullet(windPosRight, velocity, _starShipRotate));

            //adjustedAngle = angle + 0.7854f - MathF.PI / 2;
            
            // _bullets.Add(new Bullet(startPosition, new Vector2(
            // speed * MathF.Cos(adjustedAngle),
            // speed * MathF.Sin(adjustedAngle)
            // ), _starShipRotate));

        }

        _bulleSoundInstance = _bulleSound.CreateInstance();
        _bulleSoundInstance.Volume = 0.4f; 
        _bulleSoundInstance.Play();
        // _bulleSoundInstance.Dispose();

        
    }

    private Vector2 CalculateShipTip(float radian)
    {
        // Calcular a posição da ponta da nave com base na rotação
        // Ajustar para considerar a ponta para cima (90 graus / π/2 radianos)
        float adjustedRadian = radian - MathF.PI / 2;

        // Calcular a posição da ponta da nave
        return _starShipPosition + new Vector2(
            ShipRadius * MathF.Cos(adjustedRadian) - 0,
            ShipRadius * MathF.Sin(adjustedRadian)
        );

    }

    private void RecuperaCadencia()
    {
        if (_bulletStatus < 60)
            _bulletStatus += _reloadSpeed;
    }

    private void ConstrainStarShipWithinBounds()
    {
        // Calcular os limites da tela
        float halfWidth = _starShipTexture.Width / 2f / 8;
        float halfHeight = _starShipTexture.Height / 2f / 8;

        // Restringir a posição da nave dentro dos limites
        _starShipPosition.X = MathF.Max(halfWidth, MathF.Min(Globals.WindowSize.X - halfWidth, _starShipPosition.X));
        _starShipPosition.Y = MathF.Max(halfHeight, MathF.Min(Globals.WindowSize.Y - halfHeight, _starShipPosition.Y));
    }

    private void SpawnAsteroid()
{
    float velocity_asteroid = _random.Next(100) + 50;
    // Probabilidade de um Asteroid rápido
    if(_random.Next(100) < 45) velocity_asteroid *= 2;
    // Posição inicial em uma borda da tela
    Vector2 position = new Vector2(
        _random.Next(Globals.WindowSize.X), // X aleatório
        _random.Next(2) == 0 ? - 10 : Globals.WindowSize.Y + 10 // Y: calcula probabilidade 50%
    );

    // Velocidade aleatória em direção à tela

    // Caso o Asteroide venha de cima:
    Vector2 velocity;
    if(position.Y < 10){
        velocity = new Vector2(
        _random.Next(-1, 1), // X entre -1 e 1
        _random.Next(0, 2) + 0.5f // Y entre 0.5 e 1.5
    ) * velocity_asteroid; // Escala de velocidade
    }
    else if (position.Y > 10){
        velocity = new Vector2(
        _random.Next(-1, 1), // X entre -1 e 1
        _random.Next(-1, 0) + 0.5f *-1 // altera a direção do eixo Y do asteroide
    ) * velocity_asteroid; // Escala de velocidade
    }

    else {
        velocity = new Vector2(
        _random.Next(1  , 1), // X entre -1 e 1
        _random.Next(0, 0) + 0.5f // Y entre 0.5 e 1.5
    ) * velocity_asteroid; // Escala de velocidade
    }

    float radius = 20f; // Tamanho fixo para os asteroides

    _asteroids.Add(new Asteroid(position, velocity, radius));
}


private void SpawnEnemies()
{
    float velocity_asteroid = _random.Next(100) + 100;
    // Probabilidade de um Asteroid rápido
    if(_random.Next(100) < 60) velocity_asteroid *= 3;
    // Posição inicial em uma borda da tela
    Vector2 position = new Vector2(
        _random.Next(Globals.WindowSize.X), // X aleatório
        _random.Next(2) == 0 ? - 10 : Globals.WindowSize.Y + 10 // Y: calcula probabilidade 50%
    );

    // Velocidade aleatória em direção à tela

    // Caso o Asteroide venha de cima:
    Vector2 velocity;
    if(position.Y < 10){
        velocity = new Vector2(
        _random.Next(-1, 1), // X entre -1 e 1
        _random.Next(0, 2) + 0.5f // Y entre 0.5 e 1.5
    ) * velocity_asteroid; // Escala de velocidade
    }
    else if (position.Y > 10){
        velocity = new Vector2(
        _random.Next(-1, 1), // X entre -1 e 1
        _random.Next(-1, 0) + 0.5f *-1 // altera a direção do eixo Y do asteroide
    ) * velocity_asteroid; // Escala de velocidade
    }

    else {
        velocity = new Vector2(
        _random.Next(1  , 1), // X entre -1 e 1
        _random.Next(0, 0) + 0.5f // Y entre 0.5 e 1.5
    ) * velocity_asteroid; // Escala de velocidade
    }

    float radius = 20f; // Tamanho fixo para os asteroides

    _enemies.Add(new Enemy(position, velocity, radius));
}

private Rectangle GetShipCollisionRectangle()
{
    return new Rectangle(
        (int)(_starShipPosition.X - ShipCollisionWidth / 2),
        (int)(_starShipPosition.Y - ShipCollisionHeight / 2),
        (int)ShipCollisionWidth,
        (int)ShipCollisionHeight
    );
}

private Vector2 RotatePoint(Vector2 point, float rotation)
{
    return new Vector2(
        point.X * (float)Math.Cos(rotation) - point.Y * (float)Math.Sin(rotation),
        point.X * (float)Math.Sin(rotation) + point.Y * (float)Math.Cos(rotation)
    );
}

private Vector2 RotatePointForRelative(Vector2 point, Vector2 relative, float angleInRadians)
{
    // Xrot
    float Xrot = point.X * (float)Math.Cos(angleInRadians)  - point.Y * (float)Math.Sin(angleInRadians);
    float Yrot = point.X * (float)Math.Sin(angleInRadians)  - point.Y * (float)Math.Cos(angleInRadians);

    Vector2 vec = new Vector2(Xrot, Yrot);

    // Retornar à posição absoluta
    return relative + vec;
}

 private Vector2 CalculateShipTWindLeft(float radian)
    {
        // Calcular a posição da ponta da nave com base na rotação
        // Ajustar para considerar a ponta para cima (90 graus / π/2 radianos)
        float adjustedRadian = radian - MathF.PI / 1.7f;

        // Calcular a posição da ponta da nave
        return _starShipPosition + new Vector2(
            ShipRadius * MathF.Cos(adjustedRadian),
            ShipRadius * MathF.Sin(adjustedRadian)
        );

    }

     private Vector2 CalculateShipTWindRight(float radian)
    {
        // Calcular a posição da ponta da nave com base na rotação
        // Ajustar para considerar a ponta para cima (90 graus / π/2 radianos)
        float adjustedRadian = radian + MathF.PI / 0.63f;

        // Calcular a posição da ponta da nave
        return _starShipPosition + new Vector2(
            ShipRadius * MathF.Cos(adjustedRadian),
            ShipRadius * MathF.Sin(adjustedRadian)
        );

    }

    private void RestartGame(){

        //  base.Initialize();
                _gameState = GamePlayState.Play;
                _asteroids.Clear();
                _bullets.Clear();
                _enemies.Clear();
                _explosions.Clear();
                _score = 0;
                _shipDamage = 5;
                BeastModeOne = false;
                _shipDamageString = "DDDDD";
                _starShipPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                           _graphics.PreferredBackBufferHeight / 2);
                _starShipSpeed = 500f;

                MediaPlayer.Stop();
                MediaPlayer.Play(_playerSound);
                BeastModeOne = true;
    }

    

}
