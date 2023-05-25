﻿namespace DropboxLike.Domain.Models;

public class File
{
    public Stream FileStream { get; set; } 
    public string ContentType { get; set; }

    public List<string>? ObjectNames { get; set; }
}