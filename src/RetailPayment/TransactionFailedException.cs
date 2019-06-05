using System;

namespace RetailPayment
{
    /// <summary>
    ///     Исключение когда мы не можем продолжить транзакцию
    /// </summary>
    public class TransactionFailedException : Exception
    {
    }
}