using System;

namespace BlueWarehouse.Model.Error
{
    public class ClientAuthenticationFailure : Exception
    {
        public string Reason { get; set; }

        public ClientAuthenticationFailure(string reason)
        {
            Reason = reason;
        }
    }
}
