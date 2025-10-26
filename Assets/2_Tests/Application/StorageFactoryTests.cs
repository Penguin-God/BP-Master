using NUnit.Framework;

public class StorageFactoryTests
{
    [Test]
    public void 슬롯을_함수를_통해_변환()
    {
        SlotStorage<int> data = new SlotStorage<int>();
        data.AddSlots(Team.Red, new int[] { 1, 2 });
        data.AddSlots(Team.Blue, new int[] { 1, 2 });

        var result = StorageConverter.ConvertStorage(data, number => number.ToString());

        CollectionAssert.AreEqual(new string[] { "1", "2" }, result.GetTeam(Team.Blue));
        CollectionAssert.AreEqual(new string[] { "1", "2" }, result.GetTeam(Team.Red));
    }
}
