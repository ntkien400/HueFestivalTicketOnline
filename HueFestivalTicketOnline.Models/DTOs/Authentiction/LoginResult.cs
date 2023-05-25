namespace HueFestivalTicketOnline.Models.DTOs.Authentiction
{
    public class LoginResult
    {
        public bool CheckPassword { get; set; } 
        public bool CheckUserName { get; set; }
        public RefreshTokenDTO? RefreshTokenDTO { get; set; }
    }
}
