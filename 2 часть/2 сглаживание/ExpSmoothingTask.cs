using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static DataPoint RecordExpSmoothPoint(DataPoint point, double expSmoothingPoint)
    {
        return point.WithExpSmoothedY(expSmoothingPoint);
    }

    

    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        //sₜ = αxₜ + (1-α)sₜ₋₁ = sₜ₋₁ + α(xₜ - sₜ₋₁) = sₜ₋₁ + αxₜ - αsₜ₋₁ = sₜ₋₁(1 - α) + αxₜ
        //sₜ = expSmoothedY,
        //xₜ = point.OriginalY,
        //sₜ₋₁ = previousPoint.ExpSmoothedY.
        double expSmoothingPoint;
        DataPoint previousPoint = null;
        foreach (var point in data)
        {
            var previousPointUnKnown = previousPoint == null;
            var expSmoothingPointDeafultVulue = point.OriginalY;
            var expSmoothingPointNewVulue = (alpha * point.OriginalY) + ((1 - alpha) * previousPoint.ExpSmoothedY);

            expSmoothingPoint = previousPointUnKnown ? expSmoothingPointDeafultVulue : expSmoothingPointNewVulue;

            var expSmoothingPointOnDefault = expSmoothingPoint == point.OriginalY;
            var previousPointOnDefaultValue = RecordExpSmoothPoint(point, expSmoothingPoint);
            var previousPointNewValue = new DataPoint(RecordExpSmoothPoint(point, expSmoothingPoint));

            previousPoint = expSmoothingPointOnDefault ? previousPointOnDefaultValue : previousPointNewValue;

            yield return RecordExpSmoothPoint(point, expSmoothingPoint);
        }
    }
}