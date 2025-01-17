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

    public Rectangle AsteroidRectangle {get; private set;}

    public Asteroid(Vector2 position, Vector2 velocity, float radius)
    {
        Position = position;
        Velocity = velocity;
        Radius = radius;
        Radians = 0f;
    }

    public void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        Radians += 0.0174533f * 4;

        AsteroidRectangle = new Rectangle(
                    (int)(Position.X - Radius),
                    (int)(Position.Y - Radius),
                    (int)(Radius * 2),
                    (int)(Radius * 2)
                );
    }

    public bool CollidesWith(Vector2 targetPosition, float targetRadius)
    {
        float distance = Vector2.Distance(Position, targetPosition);
        return distance < (Radius + targetRadius);
    }
}
}