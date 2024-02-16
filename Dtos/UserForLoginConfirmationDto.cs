namespace DotnetAPI.Dtos
{
    public partial class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash {get; set;}
        public byte[] PasswordSalt {get; set;}

        UserForLoginConfirmationDto()
        {
            PasswordHash ??= Array.Empty<byte>();
            PasswordSalt ??= Array.Empty<byte>();
        }
    }
}