namespace SwarajCustomer_Common.Utility
{
    public class MessageModel
    {
        public string MessageType { get; set; }
        public string Message { get; set; }

        public MessageModel()
        {

        }

        public MessageModel(string messageType, string message)
        {
            this.MessageType = messageType;
            this.Message = message;
        }

        public enum CommonActions
        {
            Unauthorized = 1,
            Error = 2,
            Successful = 3,
            UserDoesNotExists = 4,
            Failed = 5,
        }
    }
}
