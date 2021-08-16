using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;

namespace HttpsCallSampleApplication
{
    public class AddClientModel
    {
        public AddClientModel()
        {
            Enabled = true;
            ClientId = ConfigurationManager.AppSettings["NewClientId"];
            ClientSecret = ConfigurationManager.AppSettings["NewClientSecret"];
            AllowedScopes = ConfigurationManager.AppSettings["NewClientAllowedScopes"].Split(";").ToList();
            Description = ConfigurationManager.AppSettings["NewClientDescription"];
            AccessTokenLifetimeInSeconds = 86400;
            Email = ConfigurationManager.AppSettings["NewClientEmail"];
            EmailTemplate = ConfigurationManager.AppSettings["NewClientEmailTemplate"];
            FirmUserBelongsTo = ConfigurationManager.AppSettings["NewClientFirmCode"];
            CorrespondentCode = ConfigurationManager.AppSettings["NewClientCorrespondentCode"];
            BoothId = ConfigurationManager.AppSettings["NewClientBoothId"];
            Office = ConfigurationManager.AppSettings["NewClientOffice"];
        }

        [Required] public bool Enabled { get; set; }

        [Required] public string ClientId { get; set; }

        [Required] public string ClientSecret { get; set; }

        [Required] public List<string> AllowedScopes { get; set; }

        public string Description { get; set; }

        [Required] public int AccessTokenLifetimeInSeconds { get; set; }

        //Extra client related data
        [Required] public string Email { get; set; }

        public string EmailTemplate { get; set; }

        [Required] public string FirmUserBelongsTo { get; set; }

        [Required] public string CorrespondentCode { get; set; }

        [Required] public string BoothId { get; set; }

        [Required] public string Office { get; set; }
    }
}
