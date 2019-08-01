using System;
namespace emailServiceAPITemplate.Services
{
    public class BRCFormService : IBrcFormService
    {

        public BRCFormService()
        {

        }

        public Models.AccessionBasicInfo extractCareerCodeFromBRCFormInfo(Models.BRCInfo bRCInfo)
        {
            Models.AccessionBasicInfo accessionBasicInfo = new Models.AccessionBasicInfo
            {
                email = bRCInfo.emailAddress,
                firstName = bRCInfo.firstName,
                lastName = bRCInfo.lastName,
                careerCode = "AA"
            };

            return accessionBasicInfo;

        }

        internal static void extractCareerCodeFromBRCFormInfo()
        {
            throw new NotImplementedException();
        }
    }

    public interface IBrcFormService
    {
        Models.AccessionBasicInfo extractCareerCodeFromBRCFormInfo(Models.BRCInfo bRCInfo);
    }
}
