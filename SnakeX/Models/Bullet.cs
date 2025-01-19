namespace SnakeX;

public class Bullet(Vector2 startPosition, Vector2 velocity, float _radian = 0)
{
        public Vector2 Position { get; set; } = startPosition;
        private Vector2 Velocity = velocity;

        public float radian {get; private set;} = _radian;

        public Rectangle HurtBox {get; private set;}

        public void Update(float deltaTime)
        {
            // Atualizar a posição do projétil
            Position += Velocity * deltaTime;

            HurtBox = new Rectangle(
                    (int)Position.X ,
                    (int)Position.Y,
                    (int)(2),
                    (int)(2)
                );

            
        }



         

        public bool CollidesWith(Rectangle collistionObject)
        {

            return collistionObject.Intersects(HurtBox);
        }
}