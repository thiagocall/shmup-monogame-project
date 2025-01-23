
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SnakeX.Manager
{
    public static class SoundManager
    {
        private static bool IntroSound = true;
        private static SoundEffect _startSound;
        private static SoundEffect _explosionSound;
        private static SoundEffect _bulleSound;
        private static SoundEffectInstance _bulleSoundInstance;
        private static SoundEffect _colisionSound;
        private static SoundEffect _alertCollisionSound;
        private static SoundEffect _powerupSound;
        private static Song _playerSound;
        private static Song _gameoverSound;

        // Eventos


        private static readonly ContentManager Content = Globals.Content;

        public static void Load(){

        _startSound = Content.Load<SoundEffect>("intro_sound-1");
        _explosionSound = Content.Load<SoundEffect>("Sounds/explosion_4");
        _bulleSound = Content.Load<SoundEffect>("Sounds/shot_sound_2");
        _colisionSound = Content.Load<SoundEffect>("ship_colision_sound");
        _alertCollisionSound = Content.Load<SoundEffect>("ship_alarm");
        _powerupSound = Content.Load<SoundEffect>("Sounds/level_up");
        _playerSound = Content.Load<Song>("Sounds/fase1_sound");
        _gameoverSound = Content.Load<Song>("Sounds/gameover_sound");

        Globals.gameEventManager.Register(Events.Game.GameEvent.OnExplosion, (p) => OnExplosion());
        }

        public static void Update(){}

        public static void OnExplosion(){
            _explosionSound.Play();
        }


    }
}