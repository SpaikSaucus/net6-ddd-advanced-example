using AuthorizationOperation.Application.Shared;
using AuthorizationOperation.Domain.Authorization.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AuthorizationOperation.API.ViewModels
{
    public class AuthorizationCriteriaRequest
    {
        private enum EnumStatusRequest
        {
            [Description("DEFAULT")]
            DEFAULT,
            [Description("WAITING_FOR_SIGNERS")]
            WAITING_FOR_SIGNERS,
            [Description("AUTHORIZED")]
            AUTHORIZED,
            [Description("EXPIRED")]
            EXPIRED,
            [Description("CANCELLED")]
            CANCELLED
        }

        private List<EnumStatusRequest> listStatusIn = new();
        private List<EnumStatusRequest> ListStatusIn
        {
            get
            {
                if (this.listStatusIn.Count > 0)
                    return this.listStatusIn;

                this.listStatusIn = Utils.ConvertIntToListEnumWithBinary<EnumStatusRequest>(this.StatusIn);
                return this.listStatusIn;
            }
        }

        /// <summary>
        /// Authorization request status. DEFAULT gets AUTHORIZED and CANCELLED authorizations for the current date, along with WAITING_FOR_SIGNERS ones as well.
        /// 
        /// statusIn available values:    
        ///     <select name="statusIn">
        ///         <option>1:  DEFAULT</option>
        ///         <option>2:  WAITING_FOR_SIGNERS</option>
        ///         <option>4:  AUTHORIZED</option>
        ///         <option>6:  AUTHORIZED + WAITING_FOR_SIGNERS</option>
        ///         <option>8:  EXPIRED</option>
        ///         <option>10: EXPIRED + WAITING_FOR_SIGNERS</option>
        ///         <option>12: EXPIRED + AUTHORIZED</option>
        ///         <option>14: EXPIRED + AUTHORIZED + WAITING_FOR_SIGNERS</option>
        ///         <option>16: CANCELLED</option>
        ///         <option>18: CANCELLED + WAITING_FOR_SIGNERS</option>
        ///         <option>20: CANCELLED + AUTHORIZED</option>
        ///         <option>22: CANCELLED + AUTHORIZED + WAITING_FOR_SIGNERS</option>
        ///         <option>24: CANCELLED + EXPIRED</option>
        ///         <option>26: CANCELLED + EXPIRED + WAITING_FOR_SIGNERS</option>
        ///         <option>28: CANCELLED + EXPIRED + AUTHORIZED</option>
        ///         <option>30: CANCELLED + EXPIRED + AUTHORIZED + WAITING_FOR_SIGNERS</option>
        ///     </select>
        /// </summary>
        public int StatusIn { get; set; }


        #region Methods

        public bool StatusInDefaultSelected
        { 
            get
            {
                return this.ListStatusIn.Contains(EnumStatusRequest.DEFAULT);
            } 
        }

        public List<AuthorizationStatusEnum> ConvertToEnum()
        {
            var listStatus = new List<AuthorizationStatusEnum>();
            if (!this.StatusInDefaultSelected)
            {
                if (this.ListStatusIn.Any(x => x == EnumStatusRequest.CANCELLED))
                    listStatus.Add(AuthorizationStatusEnum.CANCELLED);

                if (this.ListStatusIn.Any(x => x == EnumStatusRequest.WAITING_FOR_SIGNERS))
                    listStatus.Add(AuthorizationStatusEnum.WAITING_FOR_SIGNERS);

                if (this.ListStatusIn.Any(x => x == EnumStatusRequest.AUTHORIZED))
                    listStatus.Add(AuthorizationStatusEnum.AUTHORIZED);

                if (this.ListStatusIn.Any(x => x == EnumStatusRequest.EXPIRED))
                    listStatus.Add(AuthorizationStatusEnum.EXPIRED);
            }

            return listStatus;
        }

        #endregion
    }
}
