using System;

namespace WebStack.Data.Models
{
    public enum StackDirection
    {
        IN_ORDER,
        REVERTED
    }

    public class StackString
    {
        public const int LEFT_ID = -1;
        public const int RIGHT_ID = -2;
        public const int INVALID_ID = -3;

        public StackString(string content)
        {
            Content = content;
            LeftId = INVALID_ID;
            RightId = INVALID_ID;
        }

        public int StackStringId { get; set; }

        public int LeftId { get; set; }

        public int RightId { get; set; }
       
        public string Content { get; set; }
    }
}
