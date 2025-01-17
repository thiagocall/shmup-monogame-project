using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeX.Models
{
   public class Explosion
{
    public Vector2 Position { get; set; }
    public float Timer { get; private set; } = 0.5f; // Tempo de vida da explosão em segundos
    private static Texture2D _texture;
    private readonly Animation _anim;
    public bool IsExpired {get; private set;} = false;

    public Explosion(Vector2 position)
    {
        _texture = Globals.Content.Load<Texture2D>("explosion");
        _anim = new(_texture,6, 0.1f);
        Position = position;
    }

    public void Update(float deltaTime)
    {
        Timer -= deltaTime;
        int f = _anim.Update();
        if (f == 5){
             _anim.Stop();
             this.IsExpired = true;
            }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _anim.Draw(Position);
    }

     public void Stop()
    {
        _anim.Stop();
    }

}
}