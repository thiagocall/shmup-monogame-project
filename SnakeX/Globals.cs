using Microsoft.Xna.Framework.Content;
using SnakeX.Events;
using SnakeX.Events.Game;
using SnakeX.Models;

namespace SnakeX;

public static class Globals
{
    public static readonly PlayerEventManager<object> playerEventManager = new ();
    public static readonly GameEventManager<object> gameEventManager = new ();
    public static float Time { get; set; }
    public static float TotalSeconds { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static Point WindowSize { get; set; }
    public static Texture2D Texture { get; set; }
    public static GamePlayState GameState { get; set; }

    public static void Update(GameTime gt)
    {
        Time = (float)gt.ElapsedGameTime.TotalSeconds;
        TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
    }
}