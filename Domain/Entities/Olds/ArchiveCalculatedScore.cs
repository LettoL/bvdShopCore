namespace Domain.Entities.Olds
{
    public class ArchiveCalculatedScore : Entity
    {
        public int CalculatedScoreId { get; private set; }

        public ArchiveCalculatedScore(int calculatedScoreId) => CalculatedScoreId = calculatedScoreId;
    }
}