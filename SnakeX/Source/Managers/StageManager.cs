using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakeX.Source.Models;
using SnakeX.Source.Models.Stages;

namespace SnakeX;
public static class StageManager
{
    private static List<Stage> _stages;
    public static void Load(){

        _stages.Add(new Stage(Globals.SpriteBatch));

    }
    public static void Update(){}
    public static void Draw(){}
}
