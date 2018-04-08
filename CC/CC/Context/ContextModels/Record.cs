using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC.Context.ContextModels
{
    public class Record
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public DateTime RecordDate { get; set; }

        public string NickName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}