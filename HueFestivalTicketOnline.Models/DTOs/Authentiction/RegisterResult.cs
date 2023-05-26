namespace HueFestivalTicketOnline.Models.DTOs.Authentiction
{
    public class RegisterResult
    {
        public bool isExistUser { get; set; }
        public bool isExistEmail { get; set; }
        public bool validEmail { get; set; } = true;
        public bool validPhone { get; set; } = true;
        public bool CreateUserResult { get; set; }
        public string errors { get; set; }  
    }
}
