using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeX.Models
{
   public class Player
{
    public Vector2 Position { get; set; }
    public int Direction { get; set; } = 0;
    private Random _random = new Random();
    public float Timer { get; private set; } = 0.5f; // Tempo de vida da explosão em segundos
    private static Texture2D _texture;
    private readonly Animation _anim;
    public bool IsExpired {get; private set;} = false;
    public Vector2 Velocity { get; set; }
    public float Radius { get; set; }
    public float Radians { get; set; }
    public Rectangle HurtBox {get; private set;}

    private int _spriteRow = 0;
    private int _spriteColumn = 0;

    public Player(Vector2 position, Vector2 velocity, float radius, Texture2D texture, int spriteColumn, int spriteRow = 1, float frameTime = 0.1f)
    {
        _texture = texture;
        _anim = new(_texture,spriteColumn,frameTime,spriteRow,3);
        _spriteRow = spriteRow;
        _spriteColumn = spriteColumn;

        Position = position;
        Velocity = velocity;
        Radius =  radius;
        Radians = 0f;
    }

    public void Update(float deltaTime, int direction = 0)
    {
        Timer -= deltaTime;
        int f = _anim.Update(direction);
        if (f == 3){
             _anim.Stop();
             this.IsExpired = true;
            }

        Position += Velocity * deltaTime;
        Radians += 0.0174533f * 4;

        HurtBox = new Rectangle(
        (int)(Position.X + 20f),
        (int)(Position.Y + 10f),
        //   (int)(_starShipPosition.X - ShipCollisionWidth / 2),
        // (int)(_starShipPosition.Y - ShipCollisionHeight / 2),
        (int)10,
        (int)30);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _anim.Draw(Position);
    }

     public void Stop()
    {
        _anim.Stop();
    }

     public bool CollidesWith(Rectangle collistionObject)
    {
        return collistionObject.Intersects(HurtBox);
    }

}
}