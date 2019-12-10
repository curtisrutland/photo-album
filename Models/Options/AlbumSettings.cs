namespace PhotoAlbum.Models.Options
{
  public class AlbumSettings
  {
    public string[] ImageExtensions { get; set; }
    public string[] BlacklistedPaths { get; set; }
    public string AlbumRootPath { get; set; }
  }
}