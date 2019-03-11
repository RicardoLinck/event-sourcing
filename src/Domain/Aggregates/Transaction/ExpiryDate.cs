using System;
using EventSourcing.Base;

namespace EventSourcing.Domain.Aggregates.Transaction
{
    public sealed class ExpiryDate
    {
        private readonly DateTime _internalDateTime;
        public int Year => _internalDateTime.Year;
        public int Month => _internalDateTime.Month;

        public ExpiryDate(int year, int month)
        {
            try
            {
                _internalDateTime = new DateTime(year,month,1).AddMonths(1).AddDays(-1).Date;
            }
            catch (Exception)
            {
                throw new DomainException("INVALID_DATE");
            }
        }

        public bool IsExpiredOnDate(DateTime date)
        {
            return _internalDateTime < date.Date;
        }
    }
}