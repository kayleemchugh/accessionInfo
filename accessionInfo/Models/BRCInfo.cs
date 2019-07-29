using System;
namespace emailServiceAPITemplate.Models
{
    public class BRCInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }

        public BRCInfo()
        {
        }

        public BRCInfo(string firstName, string lastName, string gender, string streetAddress, string city, string zip, string phoneNumber, string emailAddress)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.streetAddress = streetAddress;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.phoneNumber = phoneNumber;
            this.emailAddress = emailAddress;

        }
    }
}
