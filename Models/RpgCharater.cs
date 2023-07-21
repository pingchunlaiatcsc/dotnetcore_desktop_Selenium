namespace dotnetcore_desktop_app.Models
{
    public class RpgCharater
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string Rpgclass { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 100;
    }
}
