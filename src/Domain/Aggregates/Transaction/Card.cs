namespace EventSourcing.Domain.Aggregates.Transaction
{
    public sealed class Card
    {
        public string Number { get; }
        public ExpiryDate ExpiryDate { get; }

        public Card(string number, ExpiryDate expiryDate)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new System.ArgumentException("INVALID_NUMBER", nameof(number));
            }

            Number = number;
            ExpiryDate = expiryDate;
        }
    }
}