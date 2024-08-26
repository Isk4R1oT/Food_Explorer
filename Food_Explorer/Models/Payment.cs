namespace Food_Explorer.Models
{
    public interface IPaymentStrategy
    {
        bool Pay();
    }

    public class PayPalStrategy : IPaymentStrategy
    {
        public bool Pay()
        {
            // Логика оплаты через PayPal
            return true;
        }
    }

    public class CreditCardStrategy : IPaymentStrategy
    {
        public bool Pay()
        {
            // Логика оплаты через кредитную карту
            return true;
        }
    }

    public class BankTransferStrategy : IPaymentStrategy
    {
        public bool Pay()
        {
            // Логика оплаты через банковский перевод
            return true;
        }
    }

    public enum PaymentType
    {
        PayPalStrategy,
        CreditCardStrategy,
        BankTransferStrategy
    }
    public class PaymentContext
    {
        private readonly IPaymentStrategy _paymentStrategy;

        public PaymentContext(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public bool MakePayment()
        {
            return _paymentStrategy.Pay();
        }
    }
}


