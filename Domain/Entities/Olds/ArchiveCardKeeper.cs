namespace Domain.Entities.Olds
{
    public class ArchiveCardKeeper : Entity
    {
        public int CardKeeperId { get; private set; }

        public ArchiveCardKeeper(int cardKeeperId) => CardKeeperId = cardKeeperId;
    }
}