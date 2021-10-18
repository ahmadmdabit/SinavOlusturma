using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class RandomData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }

        public RandomData()
        {
            if (Id == default(Guid))
            {
                Id = Guid.NewGuid();
            }
        }
    }
}
