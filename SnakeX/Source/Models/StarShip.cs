
namespace SnakeX.Models;


public class StarShip(Texture2D _texture) {

    private readonly Texture2D _texture = _texture;
    public Vector2 _starShipPosition = new Vector2(Globals.WindowSize.X / 2,
                                   Globals.WindowSize.Y / 2);
    public float _starShipSpeed = 500f;
    private float _starShipRotate = 0f;
    private Rectangle _stickPos = new Rectangle(0,0,32,32);
    private short _starShipdirection = 0;  // 1 - direita, -1-esquerda

    private void UpdateControls()
    {
        // _direction = Vector2.Zero;

        // if (InputManager.IsKeyDown(Keys.Left)) _direction.X = -1;
        // if (InputManager.IsKeyDown(Keys.Right)) _direction.X = 1;
        // if (InputManager.IsKeyDown(Keys.Up)) _direction.Y = -1;
        // if (InputManager.IsKeyDown(Keys.Down)) _direction.Y = 1;

        // if (_direction != Vector2.Zero) _direction.Normalize();
    }



}