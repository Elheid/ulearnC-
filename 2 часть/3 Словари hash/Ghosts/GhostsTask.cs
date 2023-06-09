using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
	private byte[] arrayForDocument = { 10, 30, 40, 10, 50 };
    private Vector vector = new Vector(1, 2);
	private Segment segment = new Segment(new Vector(0,0), new Vector(1, 10));
	private Cat cat = new Cat("Barsik", "Ely", new DateTime(21, 2, 1));
	private Robot robot = new Robot("id1398");
	public void DoMagic()
	{
        var newVector = new Vector(-1, -4);
        arrayForDocument[0] = 66;
		vector.Add(newVector);
		segment.Start.Add(newVector);
		cat.Rename("Tuzik");
		Robot.BatteryCapacity *= 1287959;
	}

	public Document Create()
	{
		return new Document("Text", Encoding.UTF8, arrayForDocument);
	}

	Vector IFactory<Vector>.Create(	)
	{
		return vector;
	}

	Segment IFactory<Segment>.Create()
	{
		return segment;
	}

	Cat IFactory<Cat>.Create()
	{
		return cat;
	}

	Robot IFactory<Robot>.Create()
	{
		return robot;
	}
}