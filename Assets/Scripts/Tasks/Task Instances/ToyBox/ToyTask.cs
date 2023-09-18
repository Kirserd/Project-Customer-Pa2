public class ToyTask : Task
{
    private const byte PREDICATE = 7;
    private byte _toyCounter = 0;

    public void AddToy()
    {
        _toyCounter++;
        if (_toyCounter >= PREDICATE)
            Stop(_caller, TaskStarter.Availability.Done);
    }

    public override void Reset()
    {
        base.Reset();
        _toyCounter = 0;
    }
}
