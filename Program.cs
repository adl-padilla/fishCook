using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using static System.Environment;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static SDL2.SDL_ttf;
using static SDL2.SDL_mixer;
using SDL2.SDL_Image_Extensions.Ora;

class Program
{
    static int Main()
    {
        if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
        {
            Console.Error.WriteLine($"SDL could not initialize! SDL_Error: {SDL_GetError()}");
            Exit(1);
            _ = IMG_Init(IMG_InitFlags.IMG_INIT_PNG);

        }

        IntPtr window = SDL_CreateWindow("SDL Tutorial", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 800, 600, SDL_WindowFlags.SDL_WINDOW_SHOWN);
        IntPtr renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        string zipFilePath = "./assets/Card-Fronts-1.ora";
        string fileName = "stack.xml";
        using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
        {
            var stack = zipArchive.Entries.FirstOrDefault(e => e.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
            if (stack != null)
            {
                Stream stream = stack.Open();

                using MemoryStream memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                XmlSerializer serializer = new XmlSerializer(typeof(Image));
                Image image = (Image)serializer.Deserialize(memoryStream);
                //Console.WriteLine(image.Stack.Layer.Count);
                var textureData = image.Stack.Layer.Select(l =>
                {

                    var fi = new FileInfo(l.Src);
                    var match = zipArchive.Entries.FirstOrDefault(e => e.Name.Equals(fi.Name, StringComparison.OrdinalIgnoreCase));
                    match.ExtractToFile(fi.FullName);
                    var texture = IMG_LoadTexture(renderer, match.Name);
                    fi.Delete();
                    return new KeyValuePair<string, IntPtr>(new FileInfo(zipFilePath).Name + '/' + fi.Name, texture);
                    // return memoryStream.ToArray();
                }).ToList();//.ToDictionary<string, IntPtr>(x=>x.Key, x=>x.Value);
                textureData.ForEach(kvp =>
                {
                    SDL_Log(kvp.Key);
                    SDL_DestroyTexture(kvp.Value);
                }
                );
            }/*
            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
            {
                Console.WriteLine(zipArchiveEntry.Name);
                //if(zipArchiveEntry.Name.Equals(fileName,StringComparison.OrdinalIgnoreCase))
                //{
                //}
            }*/
        }
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);

        return 0;
    }
}
