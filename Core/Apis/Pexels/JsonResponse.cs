public class JsonResponse
{
    public Photo[] photos {get; set; }
}

public class Photo 
{ 
    public Src src { get; set; }
}

public class Src
{
    public string small { get; set; }
}