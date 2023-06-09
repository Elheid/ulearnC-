using Microsoft.VisualBasic.FileIO;
using System;
using System.Net.Sockets;

namespace func_rocket;

public class ControlTask
{
	private static double SearchAngle(Rocket rocket, Vector target)
    {
        var requiredPathVector = target - rocket.Location;
        var angleBeetwenDirectionPath = requiredPathVector.Angle - rocket.Direction;
        var angleBeetwenVelocityPath = requiredPathVector.Angle - rocket.Velocity.Angle;
        var angleOfTurn = angleBeetwenVelocityPath + angleBeetwenDirectionPath;
        return ((angleOfTurn) < Math.PI/4) ? angleOfTurn : angleBeetwenDirectionPath;
    }

    public static Turn ControlRocket(Rocket rocket, Vector target)
	{
        var angleToGoal = SearchAngle(rocket, target);          
        return angleToGoal > 0 ? Turn.Right : angleToGoal < 0 ? Turn.Left : Turn.None;
    }
}



//return (Math.Abs(angleBeetwenVelPath) < 0.5 || Math.Abs(angleRocketDirVelPath) < 0.5) ? angleRocketDirVelPath + angleBeetwenVelPath : angleRocketDirVelPath;