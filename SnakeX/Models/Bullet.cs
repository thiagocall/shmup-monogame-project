namespace SnakeX;

public class Bullet(Vector2 startPosition, Vector2 velocity, float _radian = 0)
{
        public Vector2 Position { get; set; } = startPosition;
        private Vector2 Velocity = velocity;

        public float radian {get; private set;} = _radian;

        public void Update(float deltaTime)
        {
            // Atualizar a posição do projétil
            Position += Velocity * deltaTime;
        }
}