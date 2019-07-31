using System;
namespace emailServiceAPITemplate.Models
{
    public class AccessionBasicInfo
    {
        public long id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string careerCode { get; set; }

        public AccessionBasicInfo()
        {
       
        }

        public AccessionBasicInfo(long id, string firstName, string lastName, string email)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;   
            this.email = email;
        }
    }
}
