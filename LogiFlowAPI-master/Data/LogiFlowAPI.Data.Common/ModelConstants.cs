namespace LogiFlowAPI.Data.Common
{
    public static class ModelConstants
    {
        public class Delivery
        {
            public const int SenderMaxLength = 200;
        }

        public class DeliveryStatus
        {
            public const int StatusMaxLength = 200;
        }

        public class Shipment
        {
            public const int RecipientMaxLength = 200;
            public const int ShippingAddress = 200;
        }

        public class BaseProduct
        {
            public const int NameMaxLength = 200;
        }

        public class Tag
        {
            public const int NameMaxLength = 200;
        }

        public class Team
        {
            public const int NameMaxLength = 200;
        }

        public class Warehouse
        {
            public const int NameMaxLength = 200;
        }
    }
}
