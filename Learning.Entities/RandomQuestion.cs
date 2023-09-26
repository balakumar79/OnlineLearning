using System.ComponentModel.DataAnnotations.Schema;

namespace Learning.Entities
{
    public class RandomQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        
    }
}
