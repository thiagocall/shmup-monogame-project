using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace SnakeX.Models
{
    public class Animation
    {
        private readonly Texture2D _texture;
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
        private int _frameWidth;        // Largura de cada frame
        private int _frameHeight;       // Altura de cada frame


        public Animation(Texture2D texture, int frameX, float frameTime, int frameY = 1)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frames = frameX;
            _rows = frameY;
            _totalFrames = frameX;
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
        }

        public void Start(){
            _active = true;
        }

        public void Stop(){
            _active = false;
        }

        public void Reset(){

            _frame = 0;
            _frameTimeLeft = _frameTime;
        }

        public int Update(){
            if(!_active) return 0;
            _frameTimeLeft -= Globals.TotalSeconds;
            if(_frameTimeLeft <= 0){
                
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames;

            }
    
            return _frame;
        }

        public int Update(int direction){
            if(!_active) return 0;
            var offset = 0;
            _frameTimeLeft -= Globals.TotalSeconds;
            switch (direction)
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
            if(_frameTimeLeft <= 0){
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames;
                // _frame += offset;
            }

            // UpdateRectangle to draw

            _sourceRectanglesToDraw = _sourceRectangles.GetRange(offset, _frames);

    
            return _frame;
        }

         public int UpdateContinuous(){

             // Incrementar o temporizador com o tempo decorrido
            _timer += (float)Globals.TotalSeconds;
            // Alterar para o próximo frame quando o tempo do frame for alcançado
            if (_timer >= _frameTime)
            {
                _timer = 0f; // Reiniciar o temporizador
                _currentFrame = (_currentFrame + 1) % _totalFrames; // Passar para o próximo frame
            }
            _frame = _currentFrame;
            return _frame;
        }

        public void Draw(Vector2 pos){
            Globals.SpriteBatch.Draw(
                    _texture,
                    pos,
                    _sourceRectanglesToDraw[_frame],
                    Color.White,
                    0,
                    Vector2.Zero,
                    Vector2.One,SpriteEffects.None,1);
        }



    }
}