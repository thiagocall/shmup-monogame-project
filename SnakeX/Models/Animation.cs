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
        private readonly int _frames;
        private int _frame;
        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool _active = true;


        public Animation(Texture2D texture, int frameX, float frameTime)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frames = frameX;
            var frameWidth = _texture.Width / _frames;
            var frameHeigth = _texture.Height;

            for (int i = 0; i < _frames; i++)
            {
                _sourceRectangles.Add(new (i * frameWidth, 0, frameWidth, frameHeigth));
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

        public void Draw(Vector2 pos){
            Globals.SpriteBatch.Draw(
                    _texture,
                    pos - new Vector2(20f, 0f),
                    _sourceRectangles[_frame],
                    Color.White,
                    0,
                    Vector2.Zero,
                    Vector2.One,SpriteEffects.None,1);
        }



    }
}