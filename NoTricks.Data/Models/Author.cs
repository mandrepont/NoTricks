﻿using System;

namespace NoTricks.Data.Models {
    public class Author {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PenName { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
