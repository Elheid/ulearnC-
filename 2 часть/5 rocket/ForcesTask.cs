using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace func_rocket;

public class ForcesTask
{

	public static RocketForce GetThrustForce(double forceValue)
    {
        return r =>
        {
            var vectorOfThrust = new Vector(forceValue, 0);
            return vectorOfThrust.Rotate(r.Direction);
        };
    } 

    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) => r => gravity(spaceSize, r.Location);

    public static RocketForce Sum(params RocketForce[] forces)
    { 
        return r => 
        {
            var xCooddinate = 0.0;
            var yCoordinate = 0.0;
            foreach (var force in forces)
            {
                xCooddinate += force(r).X;
                yCoordinate += force(r).Y;
            }
            return new Vector(xCooddinate, yCoordinate);
        };
	}
}