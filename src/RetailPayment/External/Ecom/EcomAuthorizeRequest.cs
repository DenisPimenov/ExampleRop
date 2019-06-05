namespace RetailPayment.External.Ecom
{
    public class EcomAuthorizeRequest
    {
        public int OrderId { get; set; }

        public string CardNumber { get; set; }

        /// <summary>
        /// Месяц срока окончания действия карты 
        /// </summary>
        public int ExpMonth { get; set; }

        /// <summary>
        /// Год срока окончания действия карты
        /// </summary> 
        public int ExpYear { get; set; }

        /// <summary>
        /// CVC/CVV
        /// </summary>
        public string Cvc2 { get; set; }
    }
}