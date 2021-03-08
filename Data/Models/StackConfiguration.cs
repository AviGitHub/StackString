namespace WebStack.Data.Models
{
    public class StackConfiguration
    {
        public StackConfiguration(StackDirection stackDirection)
        {
            StackDirection = stackDirection;
        }

        public int StackConfigurationId { get; set; }

        public StackDirection StackDirection { get; set; }
    }
}   