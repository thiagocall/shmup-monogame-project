namespace SnakeX;

public class Bullet(Vector2 startPosition, Vector2 velocity, float _radian = 0, float damage = 100)
{
        public Vector2 Position { get; set; } = startPosition;
        private Vector2 Velocity = velocity;

        public float radian {get; private set;} = _radian;

        public Rectangle HurtBox = new Rectangle(
                    (int)startPosition.X-3 ,
                    (int)startPosition.Y-10,
                    8,
                    12);
        public float Damage = damage;

        public void Update(float deltaTime)
        {
            // Atualizar a posição do projétil
            Position += Velocity * deltaTime;

            HurtBox = new Rectangle(
                    (int)Position.X-3 ,
                    (int)Position.Y-10,
                    8,
                    12);

            
        }

        public bool CollidesWith(Rectangle collistionObject)
        {

            return collistionObject.Intersects(HurtBox);
        }
}