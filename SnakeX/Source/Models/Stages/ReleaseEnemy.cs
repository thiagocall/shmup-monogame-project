using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakeX.Models;

namespace SnakeX.Source.Models.Stages;
    public record ReleaseEnemy
    {
        private int TimeSeconds;
        private Enemy2 Enemy;
        private List<Vector2> _pathEnimy;
}