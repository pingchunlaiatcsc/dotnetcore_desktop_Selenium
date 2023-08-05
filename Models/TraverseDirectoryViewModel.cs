using System.Security.Policy;

namespace dotnetcore_desktop_app.Models
{
    public class TraverseDirectoryViewModel
    {
        public int Count { get; set; }
        public string TargetPath { get; set; }
        public List<string> ResultPaths { get; set; }
    }
}
