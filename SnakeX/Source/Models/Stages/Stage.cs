using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakeX.Models;

namespace SnakeX.Source.Models.Stages
{
    public class Stage
    {
        private readonly SpriteBatch _spriteBatch;
        private int startTimeSecondsFromTotalTime {get; set;}
        public Stage(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
        private string Name { get; set; }
        private int TotalSeconds {get; set;}
        private string SoundPath {get; set;}
        private string BackGroundPath {get; set;}
        private Queue<Enemy2> ActiveEnemyes {get; set;}
        private Queue<ReleaseEnemy> ReleaseEnemyes {get; set;}

    }
}