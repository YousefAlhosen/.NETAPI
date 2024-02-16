namespace DotnetAPI.Models
{
    public partial class UserJobInfo
    {
        public int UserId {get; set;}
        public string JobTitel {get; set;}
        public string Department {get; set;}

        public UserJobInfo()
        {
            if (JobTitel == null)
            {
                JobTitel ="";
            }
            if (Department == null)
            {
                Department ="";
            }
            
        }
        

    }


}