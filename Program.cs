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
using System.Runtime.InteropServices;
using SDL2.SDL_Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

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

        using Window window = new Window(SDL_CreateWindow("SDL Tutorial", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 800, 600, SDL_WindowFlags.SDL_WINDOW_SHOWN));        
        //IntPtr renderer = SDL_CreateRenderer(window.Value, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        string zipFilePath = "./assets/Recipes/Recipe-1.ora";
        string fileName = "stack.xml";
        List<KeyValuePair<string, Texture>> textureData = [];
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
                textureData = image.Stack.Layer.Select(l =>
                {
                    var fi = new FileInfo(l.Src);
                    var match = zipArchive.Entries.FirstOrDefault(e => e.Name.Equals(fi.Name, StringComparison.OrdinalIgnoreCase));
                    Texture texture = Fetch(match, window.Renderer);
                    return new KeyValuePair<string, Texture>(new FileInfo(zipFilePath).Name + '/' + fi.Name, texture);
                }).ToList();//.ToDictionary<string, Texture>(x => x.Key, x => x.Value);

            }
        }
        bool running = true;
        var color = new SDL_Color { r = 135, g = 206, b = 235, a = 255 };
        var dice = new string[] {"d6-1.png",
        "d6-2.png",
        "d6-3.png",
        "d6-4.png",
        "d6-5.png",
        "d6-6.png"};
        var loc = Assembly.GetEntryAssembly()?.Location;
        var dir = new FileInfo(loc).Directory;
        using SpriteCollection diceSprites = new(new(dice
            .Select(x => window.LoadTexture(new FileInfo($"{dir.FullName}/assets/Dice/{x}").FullName))));

        // Main loop for the program
        while (running)
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
            
            window.SetRenderDrawColor(color);
            window.RenderClear();

            //window.RenderCopyEx(background, ref r, ref r, angle, background.Center, SDL_RendererFlip.SDL_FLIP_NONE);
            int i = 0;
            textureData.ForEach(t =>
            {
                var x = new Sprite(t.Value);
                x.Loc = new SDL_Point { x = (int)(100 * i++), y = 0 };
                window.RenderSprite(x, .25);
            });
            window.RenderPresent();

        }

        textureData.ForEach(x =>
        {
            x.Value.Dispose();
        });

        return 0;
    }

    private static Texture Fetch(ZipArchiveEntry match, Renderer renderer)
    {
        byte[] bytes;
        using (var stream = match.Open())
        using (var ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            bytes = ms.ToArray();
        }
        //match.ExtractToFile(fi.FullName);
        GCHandle pinnedArray = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        IntPtr pointer = pinnedArray.AddrOfPinnedObject();
        using Surface surfacePtr = new(IMG_Load_RW(SDL_RWFromMem(pointer, bytes.Length), 0));
        Texture texture = new(SDL_CreateTextureFromSurface(renderer.Value, surfacePtr.Value));
        pinnedArray.Free();
        return texture;
    }
}
