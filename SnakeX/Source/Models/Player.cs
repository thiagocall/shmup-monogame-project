using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakeX.Events;

namespace SnakeX.Models
{
    public class Player
    {

        public int _shipDamage {get; private set;} = 3;
        public Vector2 Position { get; set; }
        public int Direction { get; set; } = 0;
        private Random _random = new Random();
        public float Timer { get; private set; } = 0.5f; // Tempo de vida da explosão em segundos
        private static Texture2D _texture;
        private readonly Animation _anim;
        public bool IsExpired { get; private set; } = false;
        public Vector2 Velocity { get; set; }
        public float Radius { get; set; }
        public float Radians { get; set; }
        public Rectangle HurtBox { get; private set; }

        // Propriedades de animação
        private readonly List<Rectangle> _sourceRectangles = new();
        private List<Rectangle> _sourceRectanglesToDraw;
        private readonly int _frames;
        private readonly int _rows;
        private int _frame;
        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool _active = true;

        private int _currentFrame;      // Frame atual
        private int _totalFrames;       // Total de frames
        private float _timer = 0f;           // Temporizador para alternar os frames

        // Estado da nave

        public bool _isDamaged {get; private set;} = false;       // Indica se a nave está danificada
        private float _blinkTimer = 2f;       // Controla o tempo do efeito de piscar
        private float _blinkDuration = 1f;    // Duração total do efeito (em segundos)
        private float _blinkInterval = 0.1f;  // Intervalo entre alternâncias de visibilidade
        private bool _isVisible = true;       // Controla a visibilidade da nave

        //

        private int _spriteRow = 0;
        private int _spriteColumn = 0;

        public Player(Vector2 position, Vector2 velocity, float radius, Texture2D texture, int spriteColumn, int spriteRow = 1, float frameTime = 0.1f, int frameX = 1, int frameY = 2, int totalFrames = 1)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frames = frameX;
            _rows = frameY;
            _totalFrames = totalFrames;
            _currentFrame = 0;
            var frameWidth = _texture.Width / _frames;
            var frameHeigth = _texture.Height / _rows;

            for (int k = 0; k < _rows; k++)
            {
                for (int i = 0; i < _frames; i++)
                {
                    _sourceRectangles.Add(new(i * frameWidth, k * frameHeigth, frameWidth, frameHeigth));
                }
            }

            _texture = texture;
            _anim = new(_texture, spriteColumn, frameTime, spriteRow, 3);
            _spriteRow = spriteRow;
            _spriteColumn = spriteColumn;

            Position = position;
            Velocity = velocity;
            Radius = radius;
            Radians = 0f;

            // Registra eventos

            Globals.playerEventManager.Register(Events.Player.PlayerEvent.OnDamage, (p) => OnDamage((int) p));
            Globals.gameEventManager.Register(Events.Game.GameEvent.OnPlayerDestroied, (p) => OnDestroied());
        }

        public void OnDamage(int damage){

            _isDamaged = true;
            _shipDamage -= damage;

            if (_shipDamage < 1)
            {
                Globals.gameEventManager.Emit(Events.Game.GameEvent.OnPlayerDestroied, this);
            }

        }

         public void OnDestroied(){

                Globals.playerEventManager.Unregister(Events.Player.PlayerEvent.OnDamage, null);

        }

         public void Repair(int damage){

            _shipDamage += damage;

        }

        public void Update(float deltaTime, Vector2 direction)
        {

            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            Vector2 normalizedDirection = magnitude > 0 ? direction / magnitude : Vector2.Zero;

            Timer -= deltaTime;
            int f = Update(direction);

            Vector2 finalVelocity = normalizedDirection * Velocity; // Velocity é a magnitude fixa

            Position += finalVelocity * deltaTime;
            Radians += 0.0174533f * 4;

            HurtBox = new Rectangle(
            (int)(Position.X + 20f),
            (int)(Position.Y + 10f),
            //   (int)(_starShipPosition.X - ShipCollisionWidth / 2),
            // (int)(_starShipPosition.Y - ShipCollisionHeight / 2),
            (int)10,
            (int)30);

            if (_isDamaged)
                {
                    _blinkTimer += deltaTime;

                    // Alternar visibilidade com base no intervalo de piscar
                    if ((_blinkTimer % _blinkInterval) < (_blinkInterval / 2))
                    {
                        _isVisible = false;
                    }
                    else
                    {
                        _isVisible = true;
                    }

                    // Verificar se o tempo total de piscada terminou
                    if (_blinkTimer >= _blinkDuration)
                    {
                        _isDamaged = false;
                        _blinkTimer = 0f;
                        _isVisible = true; // Garantir que a nave fique visível no final
                    }
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible) Draw(Position);
        }

        public bool CollidesWith(Rectangle collistionObject)
        {
            return collistionObject.Intersects(HurtBox);
        }

        private int Update(Vector2 direction)
        {

            int offset;
            _frameTimeLeft -= Globals.TotalSeconds;
            switch (direction.X)
            {
                case -1:
                    offset = 6;
                    break;

                case 0:
                    _frame = 0;
                    offset = 0;
                    break;

                case 1:
                    offset = 3;
                    break;

                default:
                    offset = 0;
                    break;
            }
            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames;
                // _frame += offset;
            }

            // UpdateRectangle to draw

            _sourceRectanglesToDraw = _sourceRectangles.GetRange(offset, _frames);


            return _frame;

        }


    private void Draw(Vector2 pos){

        _sourceRectanglesToDraw = _sourceRectanglesToDraw == null ? _sourceRectangles : _sourceRectanglesToDraw;
        Globals.SpriteBatch.Draw(
                _texture,
                pos,
                _sourceRectanglesToDraw[_frame],
                Color.White,
                0,
                Vector2.Zero,
                Vector2.One, SpriteEffects.None,1f);
    }
    }
    }