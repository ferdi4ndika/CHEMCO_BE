namespace MiniSkeletonAPI.Presentation.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;     
        public string Issuer { get; set; } = string.Empty;  
        public string Audience { get; set; } = string.Empty; 
        public int LifeTimeDays { get; set; } = 7;

        public byte[] GetSecretKeyBytes() => System.Text.Encoding.UTF8.GetBytes(Key);
    }
}
