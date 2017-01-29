using PropertyChangedCore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore.Test
{
    public class Blog:ReactiveDbObject
    {
        [Key]
        [Reactive]
        public string Url { get; set; }

        [Required]
        [Reactive]
        public string Author { get; set; }
    }
}
