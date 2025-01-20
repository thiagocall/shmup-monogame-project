using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeX.Models
{
   public class Asteroid
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Radius { get; set; }
    public float Radians { get; set; }
    private readonly Animation _anim;
    private Texture2D _texture;

    public Rectangle AsteroidRectangle {get; private set;}

    public Asteroid(Vector2 position, Vector2 velocity, float radius, Texture2D texture)
    {
        _anim = new(texture,7, 0.04f, 7, 48);
        Position = position;
        Velocity = velocity;
        Radius = radius;
        Radians = 0f;
        _texture = texture;
    }

    public void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        Radians += 0.0174533f * 4;

        AsteroidRectangle = new Rectangle( // aqui ele gera o retangulo do asteroide, isso
                    (int)(Position.X + 30f),
                    (int)(Position.Y + 30),
                    (int)(Radius * 3),
                    (int)(Radius * 3)
                );

        _anim.Update();
    }

    public bool CollidesWith(Vector2 targetPosition, float targetRadius)
    {
        float distance = Vector2.Distance(Position, targetPosition);
        return distance < (Radius + targetRadius);
    }

     public void Draw()
    {
        _anim.Draw(Position);
    }
}
}