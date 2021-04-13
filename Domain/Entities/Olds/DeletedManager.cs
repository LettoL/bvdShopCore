namespace Domain.Entities.Olds
{
    public class DeletedManager
    {
        public int Id { get; private set; }

        public DeletedManager(int id) => Id = id;
    }
}