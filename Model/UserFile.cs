using System;

namespace Model
{
    public class UserFile
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string OriginalFileName { get; set; }

        public Guid GuidName { get; set; }

        public string ContentType { get; set; }
    }
}
