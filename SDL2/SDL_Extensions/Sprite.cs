using static SDL2.SDL;
namespace SDL2.SDL_Extensions;

public class Sprite : IDisposable
{
    public SDL_Point Loc
    {
        set
        {
            Rec.x = value.x;
            Rec.y = value.y;
        }
        get
        {
            return new SDL_Point { x = Rec.x, y = Rec.y };
        }
    }

    public int Height { get => Rec.h; set => Rec.h = value; }

    public int Width { get => Rec.w; set => Rec.w = value; }

    public int X { get => Rec.x; set => Rec.x = value; }

    public int Y { get => Rec.y; set => Rec.y = value; }

    public SDL_Point Center { get { return new SDL_Point { x = (int)(Height * .5), y = (int)(Width * .5) }; } }

    public Texture Texture { get; }

    public SDL_Rect Rec = new();


    public Sprite(Texture texture, SDL_Point? location = null)
    {
        if (location != null)
        {
            Rec.x = (int)(location?.x);
            Rec.y = (int)(location?.y);
        }
        Texture = texture;
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}

