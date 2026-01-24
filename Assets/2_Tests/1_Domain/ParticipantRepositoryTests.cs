using NUnit.Framework;
using System.Collections.Generic;

public class ParticipantRepositoryTests
{
    [Test]
    public void 저장소는_주입된_데이터를_정확히_반환해야_함()
    {
        // Arrange
        var sut = new ParticipantRepository();
        var expectedData = CreateTestData("펭귄갓");

        // Act
        sut.Save(Participant.Player, expectedData);
        var result = sut.Get(Participant.Player);

        // Assert
        Assert.AreEqual("펭귄갓", result.Name);
        Assert.AreEqual(expectedData.Mastery, result.Mastery);
    }

    private ParticipantData CreateTestData(string name)
    {
        var mastery = new MasteryCollection(new List<ChampionMastery>());
        return new ParticipantData(name, mastery);
    }
}