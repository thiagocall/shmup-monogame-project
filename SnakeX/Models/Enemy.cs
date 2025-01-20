using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeX.Models
{
   public class Enemy
{
    public Vector2 Position { get; set; }
    private Random _random = new Random();
    public float Timer { get; private set; } = 0.5f; // Tempo de vida da explosão em segundos
    private static Texture2D _texture;
    private readonly Animation _anim;
    public bool IsExpired {get; private set;} = false;
    public Vector2 Velocity { get; set; }
    public float Radius { get; set; }
    public float Radians { get; set; }
    public Rectangle EnemyRectangle {get; private set;}

    public Enemy(Vector2 position, Vector2 velocity, float radius)
    {
        // _texture =  _random.Next(2) == 0 ? Globals.Content.Load<Texture2D>("Enemies/UFO") : Globals.Content.Load<Texture2D>("Enemies/Paranoid");
        _texture =  Globals.Content.Load<Texture2D>("Enemies/UFO");
        _anim = new(_texture,4, 0.1f,1,4);

         Position = position;
        Velocity = velocity;
        Radius =  10f; //radius;
        Radians = 0f;
    }

    public void Update(float deltaTime)
    {
        Timer -= deltaTime;
        int f = _anim.Update(0);
        if (f == 3){
             _anim.Stop();
             this.IsExpired = true;
            }

        Position += Velocity * deltaTime;
        Radians += 0.0174533f * 4;

        EnemyRectangle = new Rectangle(
                    (int)(Position.X + 7f),
                    (int)(Position.Y + 7f),
                    (int)(Radius * 1.8),
                    (int)(Radius * 1.8)
                );
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _anim.Draw(Position);
    }

     public void Stop()
    {
        _anim.Stop();
    }

     public bool CollidesWith(Vector2 targetPosition, float targetRadius)
    {
        float distance = Vector2.Distance(Position, targetPosition);
        return distance < (Radius + targetRadius);
    }

}
}