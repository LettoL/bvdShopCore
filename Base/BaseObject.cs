using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Base
{
    public abstract class BaseObject
    {
        [Key]
        public int Id { get; set; }
    }
}
