public class ImageData
{
    public string id { get; set; }
    public string title { get; set; }
    public string url_viewer { get; set; }
    public string url { get; set; }
    public string display_url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int size { get; set; }
    public int time { get; set; }
    public int expiration { get; set; }
    public ResponseImage image { get; set; }
    public Thumbnail thumb { get; set; }
    public Thumbnail medium { get; set; }
    public string delete_url { get; set; }
}

public class ResponseImage
{
    public string filename { get; set; }
    public string name { get; set; }
    public string mime { get; set; }
    public string extension { get; set; }
    public string url { get; set; }
}

public class Thumbnail
{
    public string filename { get; set; }
    public string name { get; set; }
    public string mime { get; set; }
    public string extension { get; set; }
    public string url { get; set; }
}

public class ApiResponse
{
    public ImageData data { get; set; }
    public bool success { get; set; }
    public int status { get; set; }
}

public class SingFileApiResponse
{
    public string title { get; set; }
    public string link { get; set; }
}